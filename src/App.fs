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
open Wrappers.Rewrapped

let [<ImportDefault("./locales/en/translation.json")>] enTranslation : obj = jsNative
let [<ImportDefault("./locales/en/ui.json")>] enUi : obj = jsNative
let [<ImportDefault("./locales/ja/translation.json")>] jaTranslation : obj = jsNative
let [<ImportDefault("./locales/ja/ui.json")>] jaUi : obj = jsNative

let initI18n =
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
  I18next.i18next
    .``use``(!^ReactI18nextBrowserLanguageDetector.languageDetector)
    .init(options)
  |> Promise.map ignore

let initTasks = [
  initI18n
]

let initCmd : Cmd<Msg> =
  initTasks
  |> Promise.all
  |> Promise.map (fun _ -> InitCompleted)
  |> Promise.catch InitFailed
  |> Cmd.OfPromise.result

let init _ = { state = Init; initCompleted = false; lang = Unspecified }, initCmd

let internal update msg model =
  match msg, model.state with
  | Ignore, _ -> model, Cmd.none
  | InitCompleted, _ ->
    let lang =
      match I18next.i18next.language with
      | "ja" -> Ja
      | _ -> En
    { model with initCompleted = true; lang = lang }, Cmd.none
  | InitFailed e, _ ->
    eprintfn "failed to initialize: %s" e.Message
    model, Cmd.none
  | SwitchLanguage lang, _ ->
    let task () =
      match lang with
      | Unspecified
      | En -> I18next.i18next.changeLanguage("en")
      | Ja -> I18next.i18next.changeLanguage("ja")
    { model with lang = lang }, Cmd.OfPromise.perform task () (fun _ -> Ignore)
  | LoadAlbum, (Init | AlbumLoadFailed _) ->
    { model with state = AlbumLoading },
    Cmd.OfPromise.perform Album.get () LoadAlbumResponse
  | LoadAlbum, _ -> model, Cmd.none
  | LoadAlbumResponse (Ok album), _ ->
    { model with state = AlbumLoaded album }, Cmd.none
  | LoadAlbumResponse (Error msg), _ ->
    { model with state = AlbumLoadFailed msg }, Cmd.none

let private viewLoading model dispatch =
  Hero.hero [ Hero.IsFullHeight ] [
    Hero.body [] [
      p [Key "loading"] [str "Loading..."]
    ]
  ]

let private viewMain model dispatch =
  Hero.hero [ Hero.IsFullHeight ] [
    Hero.body [] [
      Container.container [] [
        Columns.columns [ Columns.CustomClass "has-text-centered" ] [
          Column.column [ Column.Width(Screen.All, Column.IsHalf); Column.Offset(Screen.All, Column.IsOneQuarter) ] [
            yield
              Content.content [] [
                p [Key "hello-world"] [str !~"Hello, world!"]
                p [Key "state"] [str (sprintf "state: %s" model.state.AsString)]
              ]
            match model.state with
            | Init | AlbumLoadFailed _ ->
              yield
                Button.button [ Button.OnClick (fun _ -> dispatch LoadAlbum) ] [
                  str !~"load"
                ]
            | AlbumLoading -> ()
            | AlbumLoaded album -> ()

            let langSwitchText =
              match model.lang with
              | Unspecified
              | En -> !~UITexts.ChangeToAnotherLanguage
              | Ja -> !~UITexts.ChangeToAnotherLanguage
            yield
              Button.button [ Button.OnClick (fun _ -> dispatch (SwitchLanguage (Language.Flip model.lang))) ] [
                str langSwitchText
              ]
          ]
        ]
      ]
    ]
  ]

let private view model dispatch =
  div [Key "div-main"] [
    yield Misc.viewGoogleFontLoader model dispatch
    if model.initCompleted then
      yield viewMain model dispatch
    else
      yield viewLoading model dispatch
  ]

open Elmish.Debug
open Elmish.HMR

Program.mkProgram init update view
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
|> Program.withDebugger
|> Program.withConsoleTrace
#endif
|> Program.run
