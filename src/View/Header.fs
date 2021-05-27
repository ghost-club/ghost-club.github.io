module View.Header

open Fable.React
open Fable.React.Props
open Model
open ReactTransitionGroup
open ReactPlayer
open ModalVideoPlayer
open Properties

let [<Literal>] __FILE__ = __SOURCE_FILE__

let viewVideoModal =
  FunctionComponent.Of((fun (prop: {| isOpen: bool; onClose: unit -> unit; key: string |}) ->
    modalVideoPlayer
      {| isOpen = prop.isOpen
         useKey = __LINE__ + ":" + __FILE__
         url = Links.VimeoMovie
         config = [
           VimeoConfig.PlayerOptions [
             VimeoOption.Byline false
             VimeoOption.Dnt true
             VimeoOption.Responsive true
             VimeoOption.Title false
           ]
         ]
         onAfterOpen = None
         onCloseRequest = Some (fun _ -> prop.onClose ()) |}
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun p -> p.key))

let view : {| state: ModelState; completed: Set<Completed>; flags: Set<Flag>; dispatch: (Msg -> unit) |} -> ReactElement =
  FunctionComponent.Of ((fun props ->
    let logoLoaded = Hooks.useState false
    let firstViewShown = Hooks.useState false
    let videoRef : IRefValue<option<Browser.Types.Element>> = Hooks.useRef(None)
    let videoModalShown = Hooks.useState false

    // workaround for React bug: https://github.com/facebook/react/issues/10389
    Hooks.useEffect(
      (fun () ->
        match videoRef.current with
        | None -> ()
        | Some video ->
          let video = video :?> Browser.Types.HTMLVideoElement
          video.volume <- 0.0
          video.defaultMuted <- true
          video.muted <- true
          video.autoplay <- true
          video.setAttribute("playsinline", "")
          video.setAttribute("disablepictureinpicture", "")
          video.setAttribute("preload", "true")
          Fable.Core.JS.setTimeout (fun () -> video.play()) 1000 |> ignore
      ), [| videoRef |])

    Hooks.useEffect(
      (fun () ->
        if Set.contains FirstViewShown props.completed then
          firstViewShown.update true
        else if Set.contains LogoShown props.completed then
          props.dispatch (TriggerAfter (1000, Completed FirstViewShown))),
      [|props.completed|])

    div [
      Id "header"
      Class (if props.state = ModelState.Loaded then "header header-loaded" else "header")
      HTMLAttr.Custom("data-nosnippet","")
      Key.Src(__FILE__,__LINE__)] [
      video [
        Class "background-video"
        Key "background-video";
        RefValue videoRef;
         Muted true; AutoPlay true; HTMLAttr.Custom ("playsInline", true); Loop true; Poster Assets.Movie.JPG
        OnLoadedData (fun _ -> props.dispatch (Completed BackgroundVideoLoaded))] [
        source [Src Assets.Movie.WebM; Type "video/webm"]
        source [Src Assets.Movie.MP4;  Type "video/mp4"]
        img    [Src Assets.Movie.JPG; HTMLAttr.Custom("decoding", "async"); Title "HTML5 not supported"; HTMLAttr.Custom("loading", "lazy")]
      ]
      div [Class "header-container"] [
        cssTransition [
            CSSTransitionProp.ClassNamesForAll "fade"
            UnmountOnExit true
            In (props.completed |> Set.contains LogoShown |> not || props.state = ModelState.Loading)
            TimeoutForAll 1000.0] <|
          div [Class "header-loading-screen"; Key.Src(__FILE__, __LINE__)] []

        div [Class "header-top"; Key.Src(__FILE__, __LINE__)] [
          p [Class "header-top-text"; Key.Src(__FILE__, __LINE__)] [
            if firstViewShown.current then str "“" else str ""
            AnimatedText.slot
              {| targetText = "Just a whisper...\nI hear it in my ghost"
                 period = "."
                 visible = firstViewShown.current |}
            if firstViewShown.current then str "”" else str ""
          ]
        ]
        div [Class "header-logo-container"; Key.Src(__FILE__, __LINE__)] [
          cssTransition [
              CSSTransitionProp.ClassNamesForAll "fade"
              Appear true
              Timeout {| appear=1000; enter=1000; exit=0 |}
              In logoLoaded.current] <|
            img [
              Class "header-logo fade-init-hidden"
              Src Assets.SVG.Logo
              Alt "ghostclub logo"
              OnLoad (fun _ ->
                logoLoaded.update true
                props.dispatch (TriggerAfter(1000, Completed LogoShown)))
            ]
        ]
        div [Class "header-bottom"; Key.Src(__FILE__, __LINE__)] [
          div [Class "header-bottom-playbutton-container is-hidden-mobile"; Key.Src(__FILE__,__LINE__)] [
            div [
              Class (
                if props.flags |> Set.contains PlayButtonIsShown then "header-bottom-playbutton"
                else "header-bottom-playbutton hidden"
              )
              DangerouslySetInnerHTML { __html = Assets.InlineSVG.PlayMovieMini }
              OnClick (fun _ -> videoModalShown.update true)
            ] []
          ]
          div [Class "header-bottom-scroll-indicator is-hidden-mobile"; Key.Src(__FILE__,__LINE__)] [
            p [Class "header-bottom-scroll-indicator-text"; Key.Src(__FILE__,__LINE__)] [
              span [Style [MarginRight "1em"]] [str "Scroll Down"]
              span [Style [FontSize "1.75vh"]] [str "━━━━━━━━"]
            ]
          ]
        ]
      ]
      viewVideoModal
        {| isOpen = videoModalShown.current
           key = __LINE__ + ":" + __FILE__
           onClose = (fun () -> videoModalShown.update false) |}
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> sprintf "%s:%s" __FILE__ __LINE__))
