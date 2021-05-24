module Api

open System
open Fable.Core
open Fable.Core.JsInterop
open Fetch.Types

type [<AllowNullLiteral>] IResult<'T> =
  abstract status: string with get, set


[<RequireQualifiedAccess>]
module IResult =
  type [<AllowNullLiteral>] Ok<'T> =
    inherit IResult<'T>
    abstract value: 'T with get, set

  type [<AllowNullLiteral>] Error<'T> =
    inherit IResult<'T>
    abstract message: string with get, set

  let inline Loading<'a> : IResult<'a> = !!{| status = "loading" |}
  let inline Ok (x: 'a) : IResult<'a> = !!{| status = "ok"; value = x |}
  let inline Error (msg: string)  : IResult<'a> = !!{| status = "error"; message = msg |}

  let inline isOk (x: IResult<_>) = x.status = "ok"
  let inline isError (x: IResult<_>) = x.status <> "ok" && x.status <> "loading"
  let inline isLoading (x: IResult<_>) = x.status = "loading"

  let inline (|Loading|Ok|Error|) (x: IResult<'T>) =
    match x.status with
    | "ok" ->
      Ok (x :?> Ok<'T>).value
    | "loading" ->
      Loading
    | _ ->
      Error (x :?> Error<'T>).message

let private getImpl action : JS.Promise<IResult<'a>> =
  promise {
    let! resp =
      Fetch.fetch
        (Properties.GoogleAppUrl + "?action=" + action)
        [Method HttpMethod.GET; Mode RequestMode.Cors]
    let! txt = resp.text()
    return
      JS.JSON.parse(txt, (fun key value ->
        if (key :?> string) = "created" then
          JS.Constructors.Date.Create(value :?> string) |> box
        else value
      )) :?> IResult<'a>
  }

type [<AllowNullLiteral>] IMediaInfo =
  abstract baseUrl: string with get, set
  abstract mimeType: string with get, set
  abstract created: DateTimeOffset with get, set
  abstract width: int with get, set
  abstract height: int with get, set

module IMediaInfo =
  let getUrlWithSize (width: int) (height: int) (x: IMediaInfo) =
    sprintf "%s=w%d-h%d" x.baseUrl width height
  let getOrigUrl (x: IMediaInfo) = sprintf "%s=w%d-h%d" x.baseUrl x.width x.height

let getImages () : JS.Promise<IResult<IMediaInfo[]>> = getImpl "images"

type [<AllowNullLiteral>] IPoem =
  [<Emit("$0[0]")>]
  abstract Japanese: string
  [<Emit("$0[1]")>]
  abstract English: string

module IPoem =
  let inline create (ja: string) (en: string) : IPoem = !!(ja, en)

let getPoems () : JS.Promise<IResult<IPoem[]>> = getImpl "poems"

type All = {| images: IMediaInfo[]; poems: IPoem[] |}

let getAll () : JS.Promise<IResult<All>> =
  promise {
    let! resp =
      Fetch.fetch Properties.DataUrl [Method HttpMethod.GET; Mode RequestMode.Cors]
    let! txt = resp.text()
    let result =
      JS.JSON.parse(txt, (fun key value ->
        if (key :?> string) = "created" then
          JS.Constructors.Date.Create(value :?> string) |> box
        else value
      )) :?> IResult<All>
    if IResult.isOk result then
      return result
    else
      return! getImpl "all"
  }