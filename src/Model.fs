module Model

open Fable.Core
open Properties

[<RequireQualifiedAccess>]
type AlbumState =
  | Init
  | Loading
  | Loaded of Album.IMediaInfo[]
  | LoadFailed of string
  member this.AsString =
    match this with
    | Init -> "Init"
    | Loading -> "Album Loading"
    | Loaded _ -> "Album Loaded"
    | LoadFailed msg -> sprintf "Album Load Failed (%s)" msg

type Model = {
  initCompleted: bool
  albumState: AlbumState
  lang: Language
}

type Msg =
  | Ignore
  | InitCompleted
  | InitFailed of exn
  | LoadAlbum
  | LoadAlbumResponse of Album.IResult
  | SwitchLanguage of Language

let initModel arg = {
  initCompleted = false
  albumState = AlbumState.Init
  lang = Unspecified
}