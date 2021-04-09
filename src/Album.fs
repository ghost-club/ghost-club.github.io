module Album

open System
open Fable.Core
open Thoth.Json
open Thoth.Fetch
open Fetch.Types

[<Literal>]
let private googleAppUrl = "https://script.google.com/macros/s/AKfycbwjFhMb6D76FNfvTClDQ6zgMBWQ4p0WdUJxVXzYq8F1OVszjGz0cY6FJ7Kto1WDPi_J/exec"

type MediaInfo = {
  baseUrl: string
  mimeType: string
  created: DateTimeOffset
  width: int64
  height: int64
}
let private MediaInfoDecoder =
  Decode.object (fun get -> {
    baseUrl = get.Required.Field "baseUrl" Decode.string
    mimeType = get.Required.Field "mimeType" Decode.string
    created = get.Required.Field "created" Decode.datetimeOffset
    width = get.Required.Field "width" (Decode.string |> Decode.map int64)
    height = get.Required.Field "height" (Decode.string |> Decode.map int64)
  })

type Response = Result<MediaInfo [], string>
let private ResponseDecoder =
  Decode.object (fun get ->
    let status = get.Required.Field "status" Decode.string
    match status with
    | "ok" -> Ok (get.Required.Field "value" (Decode.array MediaInfoDecoder))
    | "error" -> Error (get.Required.Field "message" Decode.string)
    | _ -> Error "invalid response from API server"
  )

let get () : JS.Promise<Response> =
  promise {
    let! response =
      Fetch.tryGet(
        googleAppUrl + "?action=images",
        decoder = ResponseDecoder,
        properties = [Method HttpMethod.GET; Mode RequestMode.Cors])
    match response with
    | Ok resp -> return resp
    | Error err -> return Error (Helper.message err)
  }
