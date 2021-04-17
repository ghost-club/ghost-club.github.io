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
    Cmd.OfPromise.result (initI18nTask  |> Promise.catch InitError)
    Cmd.OfPromise.result (initAlbumTask |> Promise.catch InitError)
  ]

let init arg = initModel arg, initCmd

let internal update msg model =
  match msg with
  | Ignore -> model, Cmd.none
  | InitError e ->
    eprintfn "error on initialization:\n%s" (e.ToString())
    let state =
      match model.state with
      | ModelState.Error es -> ModelState.Error (e :: es)
      | _ -> ModelState.Error [e]
    { model with state = state }, Cmd.none
  | InitTaskCompleted ->
    let model =
      match model.state with
      | ModelState.Loading ->
        if model.lang = Unspecified then model
        else if model.albumState = AlbumState.Loading then model
        else if not model.backgroundVideoIsLoaded then model
        else { model with state = ModelState.Loaded }
      | _ -> model
    model, Cmd.none
  | BackgroundVideoLoaded -> { model with backgroundVideoIsLoaded = true }, Cmd.ofMsg InitTaskCompleted
  | SwitchLanguage lang ->
    let cmd =
      match lang with
      | Unspecified -> Cmd.none
      | En -> Cmd.OfPromise.perform (fun lang -> I18next.i18next.changeLanguage(lang)) "en" (fun _ -> Ignore)
      | Ja -> Cmd.OfPromise.perform (fun lang -> I18next.i18next.changeLanguage(lang)) "ja" (fun _ -> Ignore)
    { model with lang = lang }, Cmd.batch [cmd; Cmd.ofMsg InitTaskCompleted]
  | LoadAlbumResponse (Album.IResult.Ok x) ->
    { model with albumState = AlbumState.Loaded x.value }, Cmd.ofMsg InitTaskCompleted
  | LoadAlbumResponse (Album.IResult.Error x) ->
    { model with albumState = AlbumState.LoadFailed x.message }, Cmd.ofMsg InitTaskCompleted

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

let private viewHeaderMemo : {| state: ModelState; dispatch: (Msg -> unit) |} -> ReactElement =
  FunctionComponent.Of ((fun props ->
    div [Class "fullscreen-header"; Key.Src(__FILE__,__LINE__)] [
      video [
        Class "background-video"
        Key "background-video";
        HTMLAttr.Custom ("playsInline", true); AutoPlay true; Muted true; Loop true; Poster "assets/video/bg.jpg"
        OnLoadedData (fun _ -> props.dispatch BackgroundVideoLoaded)] [
        source [Src "assets/video/bg.webm"; Type "video/webm"]
        source [Src "assets/video/bg.mp4";  Type "video/mp4"]
        img    [Src "assets/video/bg.jpg";  Title "HTML5 not supported"]
      ]
      ReactTransitionGroup.cssTransition
        (fun it ->
          it.classNames <- !^"fade"
          it.unmountOnExit <- Some true
          it.``in`` <- Some (props.state = ModelState.Loading)
          it.timeout <- Some (!^1000.0)
          ()) [
        div [
          Class "loading-screen"
          Key.Src(__FILE__, __LINE__)] [
          Hero.hero [Hero.IsFullHeight; Props [Key.Src(__FILE__, __LINE__)]] [
            Hero.body [] [
              Container.container [Container.IsFluid; Modifiers [Modifier.TextAlignment (Screen.All, TextAlignment.Centered)]] [
                Heading.h1 [Props [Style [Color "#FFFFFF"]]] [str "Loading..."]
              ]
            ]
          ]
        ]
      ]
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> sprintf "%s-%s" __FILE__ __LINE__))

let private viewMenu (model: Model) dispatch =
  let langSwitchText =
    match model.lang with
    | Unspecified
    | En -> !@UITexts.ChangeToAnotherLanguage
    | Ja -> !@UITexts.ChangeToAnotherLanguage
  div [
    Class "menu"
    Key.Src(__FILE__,__LINE__)
    Style [Width "100%"; Height "100vh"]] [
    Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
      p [Key "hello-world"] [str !@"Hello, world!"]
      p [Key "album-state"] [str (sprintf "album: %s" model.albumState.AsString)]
    ]
    Block.block [Props [Key.Src(__FILE__, __LINE__)]] [
      Button.button [
        Props [Key.Src(__FILE__, __LINE__)]
        Button.OnClick (fun _ -> dispatch (SwitchLanguage (Language.Flip model.lang))) ] [
          str langSwitchText
      ]
    ]
  ]

let private viewContent (model: Model) dispatch =
  div [Key.Src(__FILE__, __LINE__)] [
    Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
      p [Key.Src(__FILE__,__LINE__)] [str LoremIpsum]
    ]
    div [Key.Src(__FILE__,__LINE__)] (PhotoGallery.viewPhotoGallery model dispatch)
    Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
      p [Key.Src(__FILE__,__LINE__)] [str LoremIpsumJp]
    ]
    Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
      Block.block [] [Heading.h2 [Props [Style [Color "white"]]] [str "DJ Mix"]]
      MixCloud.mixCloudList
        {
          options = [MixCloud.HideCover true]
          onLoad  = None
          items = [
            { user = "cannorin"; mixName = "20210402-gc-birthday-mix" }
            { user = "cannorin"; mixName = "20210402-gc-birthday-mix" }
            { user = "cannorin"; mixName = "20210402-gc-birthday-mix" }
          ]
        }
    ]
    ofList [
      for i = 1 to 10 do
        Section.section [Props [Key (sprintf "lorem-ipsum-%d" i)]] [
          p [Key.Src(__FILE__,__LINE__)] [str (if i % 2 = 0 then LoremIpsum else LoremIpsumJp)]
        ]
    ]
  ]

let private viewMain (model: Model) dispatch =
  Columns.columns [
    CustomClass "has-text-centered"
    Props [Key.Src(__FILE__,__LINE__)]] [
    Column.column [
      Modifiers [Modifier.IsHidden(Screen.Mobile, true)]
      Column.Width(Screen.Tablet, Column.Is2)
      Column.Width(Screen.Desktop, Column.Is2)
      Column.Width(Screen.WideScreen, Column.Is2)
      Column.Width(Screen.FullHD, Column.Is2)
    ] [
      viewMenu model dispatch
    ]

    Column.column [
      Column.Width(Screen.Mobile, Column.IsFull)
      Column.Width(Screen.Tablet, Column.Is10)
      Column.Width(Screen.Desktop, Column.Is10)
      Column.Width(Screen.WideScreen, Column.Is10)
      Column.Width(Screen.FullHD, Column.Is10)
      CustomClass "main"; Props [Key "main"]] [

      viewContent model dispatch
    ]
  ]

let private view model dispatch =
  div [Key.Src (__FILE__, __LINE__)] [
    //viewHeader model dispatch
    viewHeaderMemo {| state = model.state; dispatch = dispatch |}
    //Misc.viewGoogleFontLoader model dispatch
    div [Key.Src(__FILE__, __LINE__)] [
      ofOption <|
        match model.state with
        | ModelState.Loaded -> Some (viewMain model dispatch)
        | _ -> None
    ]
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
