module View.Header

open Fable.React
open Fable.React.Props
open Fable.Core.JsInterop
open Model
open Wrappers.Rewrapped
open Properties

let [<Literal>] __FILE__ = __SOURCE_FILE__

let view : {| state: ModelState; dispatch: (Msg -> unit) |} -> ReactElement =
  FunctionComponent.Of ((fun props ->
    let logoLoaded = Hooks.useState false

    ReactIntersectionObserver.inViewPlain
      (fun it ->
        it.onChange <- (fun inView _ -> props.dispatch (SetMenuIsSticky (not inView)))) [
      div [Class "header"; Key.Src(__FILE__,__LINE__)] [
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
            it.timeout <- Some (!!{| appear = 0; enter = 0; exit = 1000 |})
            ()) [
          div [Class "header-loading-screen"; Key.Src(__FILE__, __LINE__)] []
        ]
        div [Class "header-container"] [
          div [Class "header-top"; Key.Src(__FILE__, __LINE__)] [
            p [Class "header-top-text"; Key.Src(__FILE__, __LINE__)] [str "“Just a whisper..."]
            p [Class "header-top-text"; Key.Src(__FILE__, __LINE__)] [str "I hear it in my ghost.”"]
          ]
          div [Class "header-logo-container"; Key.Src(__FILE__, __LINE__)] [
            ReactTransitionGroup.cssTransition
              (fun it ->
                it.classNames <- !^"fade"
                it.appear <- Some true
                it.timeout <- Some (!!{| appear = 500; enter = 500; exit = 0 |})
                it.``in`` <- Some logoLoaded.current
              ) [
                img [Class "header-logo"; Src Assets.Image.Logo; OnLoad (fun _ -> logoLoaded.update true)]
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
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> sprintf "%s::%s" __FILE__ __LINE__))
