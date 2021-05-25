module App

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open Fable.Core
open Fable.Core.JsInterop
open Browser.Types
open Browser.Dom
open Properties
open Model
open View

let [<Literal>] __FILE__ = __SOURCE_FILE__
let [<ImportDefault("./locales/en/translation.json")>] enTranslation : obj = jsNative
let [<ImportDefault("./locales/ja/translation.json")>] jaTranslation : obj = jsNative

type ISmoothScrollPolyfill =
  abstract polyfill: unit -> unit

let [<ImportDefault("smoothscroll-polyfill")>] smoothscroll : ISmoothScrollPolyfill = jsNative

let initReactModalTask =
  promise {
    ReactModal.ReactModal.setAppElement(!^document.getElementById("#root"))
    return Ignore
  }

let initSmoothScrollPolyfillTask =
  promise {
    smoothscroll.polyfill()
    return Ignore
  }

let initI18nTask =
  let options =
    jsOptions<I18next.InitOptions>(fun it ->
      it.supportedLngs <- Some (ResizeArray ["ja"; "en"])
      it.ns <- Some (!^(ResizeArray ["translation"]))
      it.fallbackLng <- Some !^"en"
      it.returnEmptyString <- Some false
      it.resources <-
        Some (jsOptions<I18next.Resource>(fun it ->
          it.["ja"] <- !!{| translation = jaTranslation |}
          it.["en"] <- !!{| translation = enTranslation |}
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
        //.``use``(!^ReactI18next.initReactI18next)
        .``use``(!^ReactI18nextBrowserLanguageDetector.languageDetector)
        .init(options)
    let lang =
      match I18next.i18next.language with
      | "ja" -> Ja
      | _ -> En
    return SwitchLanguage lang
  }

let changeLanguageTask (langCode: string) =
  I18next.i18next.changeLanguage(langCode).``then``(fun _ ->
    let langAttr = document.createAttribute("lang")
    langAttr.value <- langCode
    document.documentElement.attributes.setNamedItem(langAttr) |> ignore
    Ignore
  )

let mutable private initApiCompleted = false
let mutable private initApiRetried = false

let initApiTask () =
  let timeout ms =
    promise {
      do! Promise.sleep ms
      if not initApiCompleted && not initApiRetried then
        initApiRetried <- true
        return TryLoadApiAgain
      else if initApiRetried then
        if Screen.check Screen.Mobile then
          return LoadApiResponse (Api.IResult.Error "API timed out")
        else
          window.location.reload(true)
          return Ignore
      else
        return Ignore
    }

  Promise.race [
    if not initApiRetried then timeout 5000
    Api.getAll ()
    |> Promise.tap (fun _ -> initApiCompleted <- true)
    |> Promise.map LoadApiResponse
    |> Promise.catch (fun e ->
      LoadApiResponse (Api.IResult.Error e.Message)
    )
  ]


let initCmd =
  Cmd.batch [
    Cmd.OfPromise.result (initReactModalTask |> Promise.catch InitError)
    Cmd.OfPromise.result (initSmoothScrollPolyfillTask |> Promise.catch InitError)
    Cmd.OfPromise.result (initI18nTask |> Promise.catch InitError)
    Cmd.OfPromise.result (initApiTask () |> Promise.catch InitError)
  ]

let delayCmd ms msg : Cmd<Msg> =
  Cmd.OfPromise.result <|
    promise {
      let! _ = Promise.sleep ms
      return msg
    }

let private validHash =
  ["about"; "how-to-join"; "dj-mix"; "gallery"; "contact"; "credits"]

let scrollToAnchorCmd () : Cmd<Msg> =
  Cmd.OfPromise.result <|
    promise {
      if isNull window.location.hash then return Ignore
      else
        let hash = window.location.hash.TrimStart('#')
        if validHash |> List.contains hash then
          let target = document.getElementById(hash)
          if isNull target then return Ignore
          else
            target.scrollIntoView(!!{| behavior = ScrollIntoViewOptionsBehavior.Smooth |} :> ScrollIntoViewOptions)
            return Ignore
        else return Ignore
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
  | CheckAnchorAndJump -> model, scrollToAnchorCmd ()
  | CheckInitTaskDone ->
    let model, cmd =
      match model.state with
      | ModelState.Loading ->
        if   model.lang = Unspecified
          || model.api |> Api.IResult.isLoading
          || model.completed |> Set.contains LogoShown |> not
        then model, Cmd.none
        else { model with state = ModelState.Loaded }, Cmd.ofMsg (TriggerAfter (1000, CheckAnchorAndJump))
      | _ -> model, Cmd.none
    model, cmd
  | Completed x -> { model with completed = Set.add x model.completed }, Cmd.ofMsg CheckInitTaskDone
  | SwitchLanguage lang ->
    let cmd =
      match lang with
      | Unspecified -> Cmd.none
      | En -> Cmd.OfPromise.perform changeLanguageTask "en" id
      | Ja -> Cmd.OfPromise.perform changeLanguageTask "ja" id
    { model with lang = lang }, Cmd.batch [cmd; Cmd.ofMsg CheckInitTaskDone]
  | TryLoadApiAgain -> model, Cmd.OfPromise.result (initApiTask ())
  | LoadApiResponse x -> { model with api = x }, Cmd.ofMsg CheckInitTaskDone
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

let private viewMain model dispatch =
  ofList [
    Transition.viewTransition {| dispatch = dispatch |}
    Menu.viewMenu {| lang = model.lang; flags = model.flags; dispatch = dispatch |}
    Content.view {| lang = model.lang; api = model.api; dispatch = dispatch |}
    Footer.view
  ]

let private view model dispatch =
  ofList [
    Header.view {| state = model.state; completed = model.completed; flags = model.flags; dispatch = dispatch |}
    match model.state with
    | ModelState.Loaded -> viewMain model dispatch
    | _ -> null
  ]

open Elmish.Debug
open Elmish.HMR

Program.mkProgram init update view
|> Program.withReactSynchronous "root"
#if DEBUG
|> Program.withDebugger
|> Program.withConsoleTrace
#endif
|> Program.run
