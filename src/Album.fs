module Album

open System
open Fable.Core
open Fetch.Types

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

type [<AllowNullLiteral>] IResult =
  abstract status: string with get, set

type [<AllowNullLiteral>] IResultOK =
  inherit IResult
  abstract value: IMediaInfo[] with get, set

type [<AllowNullLiteral>] IResultError =
  inherit IResult
  abstract message: string with get, set

let private err msg =
  JsInterop.jsOptions<IResultError>(fun it ->
    it.status <- "error"
    it.message <- msg
  ) :> IResult

let get () : JS.Promise<IResult> =
  promise {
    let! resp =
      Fetch.fetch
        (Properties.GoogleAppUrl + "?action=images")
        [Method HttpMethod.GET; Mode RequestMode.Cors]
    let! txt = resp.text()
    return
      JS.JSON.parse(txt, (fun key value ->
        if (key :?> string) = "created" then
          JS.Constructors.Date.Create(value :?> string) |> box
        else value
      )) :?> IResult
  }

[<RequireQualifiedAccess>]
module IResult =
  let inline (|Ok|Error|) (x: IResult) =
    if x.status = "ok" then
      Ok (x :?> IResultOK)
    else
      Error (x :?> IResultError)
