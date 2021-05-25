module View.Transition

open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
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
            if transitionCompleted.current then 1.0, DisplayOptions.Inherit
            else if transitionInProgress.current then
              min 1.0 scrollAmount.current, DisplayOptions.Inherit
            else 0.0, DisplayOptions.None
          [Opacity opacity; Display display]
        )] []
      inViewPlain [
        !^Class("transition-checker")
        !^Key.Src(__FILE__, __LINE__)
        OnChange (fun inView _ ->
          props.dispatch (SetFlag (MenuIsVisible, not inView))
          transitionCompleted.update (not inView))
        ] nothing
      inViewPlain [
        !^Class("transition-scroll")
        !^Key.Src(__FILE__,__LINE__)
        Thresholds [| for i = 0 to 10 do yield 0.1 * float i |]
        OnChange (fun _ entry ->
          if not transitionCompleted.current then
            scrollAmount.update (entry.intersectionRatio * 1.75))] nothing
      inViewPlain [
        !^Class("transition")
        !^Key.Src(__FILE__,__LINE__)
        OnChange (fun inView _ ->
          transitionInProgress.update inView
          props.dispatch (SetFlag (PlayButtonIsShown, not inView)))] <|
          picture [Key.Src(__FILE__,__LINE__)] [
            source [SrcSet Assets.WebP.GCBuilding1; Type "image/webp"; Class "transition-building"]
            source [SrcSet Assets.WebPAlt.GCBuilding1; Type "image/png"; Class "transition-building"]
            img [Src Assets.WebPAlt.GCBuilding1; Alt ""; Class "transition-building"; HTMLAttr.Custom("loading", "lazy")]
          ]
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> __FILE__ + ":" + __LINE__)) props
