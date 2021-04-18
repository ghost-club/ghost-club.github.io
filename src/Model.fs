module Model

open Fable.Core
open Properties

[<RequireQualifiedAccess>]
type AlbumState =
  | Loading
  | Loaded of Album.IMediaInfo[]
  | LoadFailed of string
  member this.AsString =
    match this with
    | Loading -> "Album Loading"
    | Loaded _ -> "Album Loaded"
    | LoadFailed msg -> sprintf "Album Load Failed (%s)" msg

[<RequireQualifiedAccess>]
type ModelState =
  | Loading
  | Loaded
  | Error of exn list

type Model = {
  state: ModelState
  albumState: AlbumState
  backgroundVideoIsLoaded: bool
  lang: Language
  menuIsSticky: bool
}

type Msg =
  | Ignore
  | InitTaskCompleted
  | InitError of exn
  | BackgroundVideoLoaded
  | LoadAlbumResponse of Album.IResult
  | SwitchLanguage of Language
  | SetMenuIsSticky of bool

let initModel arg = {
  state = ModelState.Loading
  albumState = AlbumState.Loading
  backgroundVideoIsLoaded = false
  lang = Unspecified
  menuIsSticky = false
}