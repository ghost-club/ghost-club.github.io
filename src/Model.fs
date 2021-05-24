module Model

open Fable.Core
open Properties

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
  api: Api.IResult<Api.All>
  lang: Language
  completed: Set<Completed>
  flags: Set<Flag>
}

type Msg =
  | Ignore
  | CheckInitTaskDone
  | CheckAnchorAndJump
  | InitError of exn
  | LoadApiResponse of Api.IResult<Api.All>
  | SwitchLanguage of Language
  | Completed of Completed
  | SetFlag of Flag * bool
  | TriggerAfter of ms:int * Msg

let initModel arg = {
  state = ModelState.Loading
  api = Api.IResult.Loading
  lang = Unspecified
  completed = Set.empty
  flags = Set.ofList [PlayButtonIsShown]
}