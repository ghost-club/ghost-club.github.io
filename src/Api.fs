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

let inline private parseJson txt =
  JS.JSON.parse(txt, (fun key value ->
    if (key :?> string) = "created" then
      JS.Constructors.Date.Create(value :?> string) |> box
    else value
  ))

let inline private getImpl action : JS.Promise<IResult<'a>> =
  promise {
    let! resp =
      Fetch.fetch
        (Properties.GoogleAppUrl + "?action=" + action)
        [Method HttpMethod.GET; Mode RequestMode.Cors]
    let! txt = resp.text()
    return parseJson txt :?> IResult<'a>
  }

type IMediaInfo = {|
  id: string
  baseUrl: string
  mimeType: string
  created: DateTimeOffset
  width: int
  height: int
  thumbnailUrl: string
  origUrl: string
  srcSet: string
  thumbnailUrlWebP: string
  origUrlWebP: string
  srcSetWebP: string
|}


module IMediaInfo =
  let inline getUrlWithSize (width: int) (height: int) (x: IMediaInfo) =
    sprintf "%s=w%d-h%d" x.baseUrl width height
  let inline getThumbUrl (x: IMediaInfo) = x.thumbnailUrl ^?? x.baseUrl
  let inline getOrigUrl (x: IMediaInfo) = x.origUrl ^?? sprintf "%s=w%d-h%d" x.baseUrl x.width x.height
  let inline getSrcSet (x: IMediaInfo) = x.srcSet ^?? x.origUrl
  let inline getSrcSetWebP (x: IMediaInfo) =
    if isNullOrUndefined x.srcSetWebP then None
    else Some x.srcSetWebP

// let getImages () : JS.Promise<IResult<IMediaInfo[]>> = getImpl "images"

type [<AllowNullLiteral>] IPoem =
  [<Emit("$0[0]")>]
  abstract Japanese: string
  [<Emit("$0[1]")>]
  abstract English: string

module IPoem =
  let inline create (ja: string) (en: string) : IPoem = !!(ja, en)

// let getPoems () : JS.Promise<IResult<IPoem[]>> = getImpl "poems"

type All = {| images: IMediaInfo[]; poems: IPoem[] |}

let getAll () : JS.Promise<IResult<All>> =
  promise {
    let! resp =
      Fetch.fetch Properties.DataUrl [Method HttpMethod.GET; Mode RequestMode.Cors]
    let! txt = resp.text()
    try
      let result = parseJson txt :?> IResult<All>
      if IResult.isOk result then
        return result
      else
        return! getImpl "all"
    with
      | _ -> return! getImpl "all"
  }