module Model

open Fable.Core
open Properties

type State =
  | Init
  | AlbumLoading
  | AlbumLoaded of Album.MediaInfo[]
  | AlbumLoadFailed of string
  member this.AsString =
    match this with
    | Init -> "Init"
    | AlbumLoading -> "Album Loading"
    | AlbumLoaded _ -> "Album Loaded"
    | AlbumLoadFailed msg -> sprintf "Album Load Failed (%s)" msg


type Model = {
  state: State
  initCompleted: bool
  lang: Language
}

type Msg =
  | Ignore
  | InitCompleted
  | InitFailed of exn
  | LoadAlbum
  | LoadAlbumResponse of Album.Response
  | SwitchLanguage of Language
