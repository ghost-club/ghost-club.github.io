module App

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open Fable.Core
open Fable.Core.JsInterop
open Properties
open Model
open View

let [<Literal>] __FILE__ = __SOURCE_FILE__
let [<ImportDefault("./locales/en/translation.json")>] enTranslation : obj = jsNative
let [<ImportDefault("./locales/en/ui.json")>] enUi : obj = jsNative
let [<ImportDefault("./locales/ja/translation.json")>] jaTranslation : obj = jsNative
let [<ImportDefault("./locales/ja/ui.json")>] jaUi : obj = jsNative

let initI18nTask =
  let options =
    jsOptions<I18next.InitOptions>(fun it ->
      it.supportedLngs <- Some (ResizeArray ["ja"; "en"])
      it.ns <- Some (!^(ResizeArray ["translation"; "ui"]))
      it.fallbackLng <- Some !^"en"
      it.returnEmptyString <- Some false
      it.resources <-
        Some (jsOptions<I18next.Resource>(fun it ->
          it.["ja"] <- !!{| translation = jaTranslation; ui = jaUi |}
          it.["en"] <- !!{| translation = enTranslation; ui = enUi |}
        ))
      it.detection <-
        Some (jsOptions<ReactI18nextBrowserLanguageDetector.DetectorOptions>(fun it ->
          it.cookieDomain <- Some DomainNameInPunyCode
          it.cookieMinutes <- Some 10.0
          ()
        ) |> box)
      #if DEBUG
      it.debug <- Some true
      #endif
    )
  promise {
    let! _ =
      I18next.i18next
        .``use``(!^ReactI18nextBrowserLanguageDetector.languageDetector)
        .init(options)
    let lang =
      match I18next.i18next.language with
      | "ja" -> Ja
      | _ -> En
    return SwitchLanguage lang
  }

let initAlbumTask =
  Album.get () |> Promise.map LoadAlbumResponse

let initCmd =
  Cmd.batch [
    Cmd.OfPromise.result (initI18nTask |> Promise.catch InitError)
    Cmd.OfPromise.result (initAlbumTask |> Promise.catch InitError)
  ]

let delayCmd ms msg : Cmd<Msg> =
  Cmd.OfPromise.result <|
    promise {
      let! _ = Promise.sleep ms
      return msg
    }

let init arg = initModel arg, initCmd

let internal update msg model =
  match msg with
  | Ignore -> model, Cmd.none
  | TriggerAfter (ms, msg) -> model, delayCmd ms msg
  | InitError e ->
    eprintfn "error on initialization:\n%s" (e.ToString())
    let state =
      match model.state with
      | ModelState.Error es -> ModelState.Error (e :: es)
      | _ -> ModelState.Error [e]
    { model with state = state }, Cmd.none
  | CheckInitTaskDone ->
    let model, cmd =
      match model.state with
      | ModelState.Loading ->
        if   model.lang = Unspecified
          || model.albumState = AlbumState.Loading
          || model.completed |> Set.contains BackgroundVideoLoaded |> not
          || model.completed |> Set.contains LogoShown |> not
        then model, Cmd.none
        else { model with state = ModelState.Loaded }, Cmd.none
      | _ -> model, Cmd.none
    model, cmd
  | Completed x -> { model with completed = Set.add x model.completed }, Cmd.ofMsg CheckInitTaskDone
  | SwitchLanguage lang ->
    let cmd =
      match lang with
      | Unspecified -> Cmd.none
      | En -> Cmd.OfPromise.perform (fun lang -> I18next.i18next.changeLanguage(lang)) "en" (fun _ -> Ignore)
      | Ja -> Cmd.OfPromise.perform (fun lang -> I18next.i18next.changeLanguage(lang)) "ja" (fun _ -> Ignore)
    { model with lang = lang }, Cmd.batch [cmd; Cmd.ofMsg CheckInitTaskDone]
  | LoadAlbumResponse (Album.IResult.Ok x) ->
    { model with albumState = AlbumState.Loaded x.value }, Cmd.ofMsg CheckInitTaskDone
  | LoadAlbumResponse (Album.IResult.Error x) ->
    { model with albumState = AlbumState.LoadFailed x.message }, Cmd.ofMsg CheckInitTaskDone
  | SetFlag (flag, true)  -> { model with flags = Set.add flag model.flags }, Cmd.none
  | SetFlag (flag, false) -> { model with flags = Set.remove flag model.flags }, Cmd.none

let private viewError model (exns: exn list) dispatch =
  Hero.hero [ Props [Key.Src(__FILE__, __LINE__)]; Hero.IsFullHeight ] [
    Hero.body [ Props [Key.Src(__FILE__,__LINE__)] ] [
      p [Key.Src(__FILE__,__LINE__)] [str "Failed to initialize the web application."]
      p [Key.Src(__FILE__,__LINE__)] [str "Errors:"]
      ofList [
        for i, e in List.indexed exns do
          code [Key (sprintf "error-%d" i)] [
            str (e.ToString())
          ]
      ]
    ]
  ]

let private viewMain (model: Model) dispatch =
  div [Class "main has-text-centered"; Key.Src(__FILE__,__LINE__)] [
    Columns.columns [
      Props [Key.Src(__FILE__,__LINE__)]] [
      Column.column [
        Modifiers [Modifier.IsHidden(Screen.Mobile, true)]
        Column.Width(Screen.Tablet, Column.Is2)
        Column.Width(Screen.Desktop, Column.Is2)
        Column.Width(Screen.WideScreen, Column.Is2)
        Column.Width(Screen.FullHD, Column.Is2)
      ] [
        Menu.viewPC model dispatch
      ]

      Column.column [
        Column.Width(Screen.Mobile, Column.IsFull)
        Column.Width(Screen.Tablet, Column.Is10)
        Column.Width(Screen.Desktop, Column.Is10)
        Column.Width(Screen.WideScreen, Column.Is10)
        Column.Width(Screen.FullHD, Column.Is10)
        CustomClass "content"
        Props [Key "content"]] [

        Content.view model dispatch
      ]
    ]
  ]

let private view model dispatch =
  ofList [
    Header.view {| state = model.state; completed = model.completed; flags = model.flags; dispatch = dispatch |}
    ofList <|
      match model.state with
      | ModelState.Loaded ->
        [ Transition.viewTransition {| dispatch = dispatch |}
          viewMain model dispatch ]
      | _ -> []
  ]

open Elmish.Debug
open Elmish.HMR

Program.mkProgram init update view
|> Program.withReactSynchronous "app"
#if DEBUG
|> Program.withDebugger
|> Program.withConsoleTrace
#endif
|> Program.run
