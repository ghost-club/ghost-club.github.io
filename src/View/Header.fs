module View.Header

open Fable.React
open Fable.React.Props
open Model
open ReactTransitionGroup
open Properties

let [<Literal>] __FILE__ = __SOURCE_FILE__

let view : {| state: ModelState; completed: Set<Completed>; flags: Set<Flag>; dispatch: (Msg -> unit) |} -> ReactElement =
  FunctionComponent.Of ((fun props ->
    let logoLoaded = Hooks.useState false
    let firstViewShown = Hooks.useState false
    let videoRef : IRefValue<option<Browser.Types.Element>> = Hooks.useRef(None)

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
          video.setAttribute("muted", "")
          Fable.Core.JS.setTimeout (fun () -> video.play()) 1 |> ignore
      ), [| videoRef |])

    Hooks.useEffect(
      (fun () ->
        if Set.contains FirstViewShown props.completed then
          firstViewShown.update true
        else if Set.contains LogoShown props.completed then
          props.dispatch (TriggerAfter (1000, Completed FirstViewShown))),
      [|props.completed|])

    div [Class (if props.state = ModelState.Loaded then "header header-loaded" else "header"); Key.Src(__FILE__,__LINE__)] [
      video [
        Class "background-video"
        Key "background-video";
        RefValue videoRef;
        HTMLAttr.Custom ("playsInline", true); Muted true; AutoPlay true; Loop true; Poster "assets/video/bg.jpg"
        OnLoadedData (fun _ -> props.dispatch (Completed BackgroundVideoLoaded))] [
        source [Src "assets/video/bg.webm"; Type "video/webm"]
        source [Src "assets/video/bg.mp4";  Type "video/mp4"]
        img    [Src "assets/video/bg.jpg";  Title "HTML5 not supported"]
      ]
      div [Class "header-container"] [
        cssTransition [
            CSSTransitionProp.ClassNamesForAll "fade"
            UnmountOnExit true
            In (props.completed |> Set.contains LogoShown |> not)
            TimeoutForAll 1000.0] <|
          div [Class "header-loading-screen"; Key.Src(__FILE__, __LINE__)] []

        div [Class "header-top"; Key.Src(__FILE__, __LINE__)] [
          div [Key.Src(__FILE__,__LINE__); Class "slide-parent"] [
            p [Class "header-top-text"; Key.Src(__FILE__, __LINE__)] [
              if firstViewShown.current then str "“" else str ""
              AnimatedText.slot
                {| targetText = "Just a whisper...\nI hear it in my ghost"
                   period = "."
                   visible = firstViewShown.current |}
              if firstViewShown.current then str "”" else str ""
            ]
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
              OnLoad (fun _ ->
                logoLoaded.update true
                props.dispatch (TriggerAfter(1000, Completed LogoShown)))
            ]
        ]
        div [Class "header-bottom"; Key.Src(__FILE__, __LINE__)] [
          div [Class "header-bottom-playbutton-container"; Key.Src(__FILE__,__LINE__)] [
            div [
              Class (
                if props.flags |> Set.contains PlayButtonIsShown then "header-bottom-playbutton"
                else "header-bottom-playbutton hidden"
              )
              DangerouslySetInnerHTML { __html = Assets.InlineSVG.PlayMovie }
              OnClick (fun _ -> printfn "play!")
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
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> sprintf "%s:%s" __FILE__ __LINE__))
