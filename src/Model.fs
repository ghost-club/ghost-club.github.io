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

[<StringEnum>]
type Flag =
  | MenuIsVisible
  | PlayButtonIsShown

type Model = {
  state: ModelState
  albumState: AlbumState
  lang: Language
  completed: Set<Completed>
  flags: Set<Flag>
}

type Msg =
  | Ignore
  | CheckInitTaskDone
  | InitError of exn
  | LoadAlbumResponse of Album.IResult
  | SwitchLanguage of Language
  | Completed of Completed
  | SetFlag of Flag * bool
  | TriggerAfter of ms:int * Msg

let initModel arg = {
  state = ModelState.Loading
  albumState = AlbumState.Loading
  lang = Unspecified
  completed = Set.empty
  flags = Set.ofList [PlayButtonIsShown]
}