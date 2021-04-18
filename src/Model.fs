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

[<StringEnum>]
type Completed =
  | BackgroundVideoLoaded
  | LogoShown
  | FirstViewShown

type Model = {
  state: ModelState
  albumState: AlbumState
  lang: Language
  menuIsSticky: bool
  completed: Set<Completed>
}

type Msg =
  | Ignore
  | InitTaskCompleted
  | InitError of exn
  | LoadAlbumResponse of Album.IResult
  | SwitchLanguage of Language
  | SetMenuIsSticky of bool
  | Completed of Completed
  | TriggerAfter of ms:int * Msg

let initModel arg = {
  state = ModelState.Loading
  albumState = AlbumState.Loading
  lang = Unspecified
  menuIsSticky = false
  completed = Set.empty
}