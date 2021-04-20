module View.Header

open Fable.React
open Fable.React.Props
open Model
open ReactTransitionGroup
open Properties

let [<Literal>] __FILE__ = __SOURCE_FILE__

let view : {| state: ModelState; completed: Set<Completed>; dispatch: (Msg -> unit) |} -> ReactElement =
  FunctionComponent.Of ((fun props ->
    let logoLoaded = Hooks.useState false
    let firstViewShown = Hooks.useState false
    let playButtonOnHover = Hooks.useState false

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
        HTMLAttr.Custom ("playsInline", true); AutoPlay true; Muted true; Loop true; Poster "assets/video/bg.jpg"
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
            (*
            cssTransition [
                CSSTransitionProp.ClassNamesForAll "slide"
                TimeoutForAll 2000.0
                In firstViewShown.current] <|
              div [Key.Src(__FILE__,__LINE__); Class "slide-init"] [
                p [Class "header-top-text"; Key.Src(__FILE__, __LINE__)] [
                  str "“Just a whisper...\nI hear it in my ghost.”"
                ]
              ]
            *)
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
          div [Class "header-bottom-playbutton-container"] [
            div [
              Class "header-bottom-playbutton"
              DangerouslySetInnerHTML { __html = Assets.InlineSVG.PlayMovie }
              OnClick (fun _ -> printfn "play!")
            ] []
          ]
        ]
      ]
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> sprintf "%s:%s" __FILE__ __LINE__))
