module View.Transition

open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Wrappers.Rewrapped
open ReactIntersectionObserver

open Properties
open Model

let [<Literal>] __FILE__ = __SOURCE_FILE__

let viewTransition (props: {| dispatch: Msg -> unit |}) =
  FunctionComponent.Of((fun (props: {| dispatch: Msg -> unit |}) ->
    let transitionCompleted = Hooks.useState false
    let transitionInProgress = Hooks.useState false

    let scrollAmount = Hooks.useState 0.0

    ofList [
      div [
        Class "transition-background"
        Key.Src(__FILE__, __LINE__)
        Style (
          let opacity, display =
            if transitionInProgress.current then
              min 1.0 scrollAmount.current, DisplayOptions.Inherit
            else if transitionCompleted.current then 1.0, DisplayOptions.Inherit
            else 0.0, DisplayOptions.None
          [Opacity opacity; Display display]
        )] []
      inViewPlain [
        !^Class("transition-checker")
        !^Key.Src(__FILE__, __LINE__)
        OnChange (fun inView _ ->
          props.dispatch (SetFlag (MenuIsSticky, not inView))
          transitionCompleted.update (not inView))
        ] nothing
      inViewPlain [
        !^Class("transition-scroll")
        !^Key.Src(__FILE__,__LINE__)
        Thresholds [| for i = 0 to 5 do yield 0.2 * float i |]
        OnChange (fun _ entry ->
          if not transitionCompleted.current then
            scrollAmount.update (entry.intersectionRatio * 2.1))] nothing
      inViewPlain [
        !^Class("transition")
        !^Key.Src(__FILE__,__LINE__)
        OnChange (fun inView _ ->
          transitionInProgress.update inView
          props.dispatch (SetFlag (PlayButtonIsShown, not inView)))] <|
        picture [] [
          source [SrcSet Assets.WebP.GCBuilding1; Type "image/webp"; Class "transition-building"]
          source [SrcSet Assets.WebPAlt.GCBuilding1; Type "image/png"; Class "transition-building"]
          img [Src Assets.WebPAlt.GCBuilding1; Alt ""; Class "transition-building"]
        ]

    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> __FILE__ + ":" + __LINE__)) props
