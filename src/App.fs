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

let [<Literal>] __FILE__ = __SOURCE_FILE__
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

let init arg = initModel arg, initCmd

let internal update msg model =
  match msg with
  | Ignore -> model, Cmd.none
  | InitCompleted ->
    let lang =
      match I18next.i18next.language with
      | "ja" -> Ja
      | _ -> En
    { model with initCompleted = true; lang = lang }, Cmd.none
  | InitFailed e ->
    eprintfn "failed to initialize: %s" e.Message
    model, Cmd.none
  | SwitchLanguage lang ->
    let task () =
      match lang with
      | Unspecified
      | En -> I18next.i18next.changeLanguage("en")
      | Ja -> I18next.i18next.changeLanguage("ja")
    { model with lang = lang }, Cmd.OfPromise.perform task () (fun _ -> Ignore)
  | LoadAlbum ->
    match model.albumState with
    | AlbumState.Loaded _ | AlbumState.Loading -> model, Cmd.none
    | _ ->
      { model with albumState = AlbumState.Loading },
      Cmd.OfPromise.perform Album.get () LoadAlbumResponse
  | LoadAlbumResponse (Album.IResult.Ok x) ->
    { model with albumState = AlbumState.Loaded x.value }, Cmd.none
  | LoadAlbumResponse (Album.IResult.Error x) ->
    { model with albumState = AlbumState.LoadFailed x.message }, Cmd.none

let private viewLoading model dispatch =
  Hero.hero [ Props [Key.Src(__FILE__, __LINE__)]; Hero.IsFullHeight ] [
    Hero.body [ Props [Key.Src(__FILE__,__LINE__)] ] [
      p [Key.Src(__FILE__,__LINE__)] [str "Loading..."]
    ]
  ]

let private viewVideo model dispatch =
  div [Class "background"; Key.Src(__FILE__,__LINE__)] [
    video [Key "background-video"; HTMLAttr.Custom ("playsInline", true); AutoPlay true; Muted true; Loop true; Poster "assets/video/bg.jpg"] [
      source [Src "assets/video/bg.webm"; Type "video/webm"]
      source [Src "assets/video/bg.mp4";  Type "video/mp4"]
      img    [Src "assets/video/bg.jpg";  Title "HTML5 not supported"]
    ]
  ]

let private viewMain (model: Model) dispatch =
  let langSwitchText =
    match model.lang with
    | Unspecified
    | En -> !@UITexts.ChangeToAnotherLanguage
    | Ja -> !@UITexts.ChangeToAnotherLanguage

  Columns.columns [CustomClass "has-text-centered"; Props [Key.Src(__FILE__,__LINE__)]] [
    Column.column [Column.Width(Screen.All, Column.Is2); Props [Key "desktop-sidebar"]] [
      Block.block [CustomClass "sticky menu full-height"; Props [Key.Src(__FILE__,__LINE__); Style [MarginBottom 0]]] [
        Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
          p [Key "hello-world"] [str !@"Hello, world!"]
          p [Key "album-state"] [str (sprintf "album: %s" model.albumState.AsString)]
        ]
        Block.block [Props [Key.Src(__FILE__, __LINE__)]] [
          ofOption (
            match model.albumState with
            | AlbumState.Init | AlbumState.LoadFailed _ ->
              Button.button [
                Props [Key.Src(__FILE__,__LINE__)]
                Button.OnClick (fun _ -> dispatch LoadAlbum) ] [
                str !@"load"
              ] |> Some
            | _ -> None
          )
        ]
        Block.block [Props [Key.Src(__FILE__, __LINE__)]] [
          Button.button [
            Props [Key.Src(__FILE__, __LINE__)]
            Button.OnClick (fun _ -> dispatch (SwitchLanguage (Language.Flip model.lang))) ] [
              str langSwitchText
          ]
        ]
      ]
      div [Key.Src(__FILE__,__LINE__)] []
    ]

    Column.column [Column.Width(Screen.All, Column.Is10); CustomClass "content"; Props [Key "content"]] [
      Block.block [Props [Key.Src(__FILE__, __LINE__)]] [
        Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
          p [Key.Src(__FILE__,__LINE__)] [str LoremIpsum]
        ]
        div [Key.Src(__FILE__,__LINE__)] (PhotoGallery.viewPhotoGallery model dispatch)
        Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
          p [Key.Src(__FILE__,__LINE__)] [str LoremIpsum]
        ]
        ofList [
          for i = 1 to 10 do
            Section.section [Props [Key (sprintf "lorem-ipsum-%d" i)]] [
              p [Key.Src(__FILE__,__LINE__)] [str LoremIpsum]
            ]
        ]
      ]
    ]
  ]

let private view model dispatch =
  div [Key.Src (__FILE__, __LINE__)] [
    viewVideo model dispatch
    Misc.viewGoogleFontLoader model dispatch
    if model.initCompleted then viewMain model dispatch else viewLoading model dispatch
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
