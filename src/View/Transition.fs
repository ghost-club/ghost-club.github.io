module View.Transition

open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open ReactIntersectionObserver

open Properties
open Model

let [<Literal>] __FILE__ = __SOURCE_FILE__

let view (props: {| dispatch: Msg -> unit |}) =
  FunctionComponent.Of((fun (props: {| dispatch: Msg -> unit |}) ->
    let transitionInProgress = Hooks.useState false
    let scrollAmount = Hooks.useState 0.0

    ofList [
      div [
        Class "transition-background"
        Key.Src(__FILE__, __LINE__)
        HTMLAttr.Custom("data-nosnippet","")
        Style (
          let opacity = min 1.0 scrollAmount.current
          [Opacity opacity]
        )] []
          //props.dispatch (SetFlag (MenuIsVisible, not inView))
      inViewPlain [
        !^Class("transition-scroll")
        !^Key.Src(__FILE__,__LINE__)
        !^HTMLAttr.Custom("data-nosnippet","")
        Thresholds [| for i = 0 to 10 do yield 0.1 * float i |]
        OnChange (fun inView entry ->
          props.dispatch (SetFlag (TransitionCompleted, not inView))
          scrollAmount.update (1.0 - entry.intersectionRatio))] nothing
      inViewPlain [
        !^Class("transition")
        !^Key.Src(__FILE__,__LINE__)
        !^HTMLAttr.Custom("data-nosnippet","")
        OnChange (fun inView _ ->
          transitionInProgress.update inView
          props.dispatch (SetFlag (PlayButtonIsShown, not inView)))] <|
          picture [Key.Src(__FILE__,__LINE__)] [
            source [SrcSet Assets.WebP.GCBuilding1.srcSet; Width Assets.WebP.GCBuilding1.width; Height Assets.WebP.GCBuilding1.height; Sizes "100vw"; Type "image/webp"; Class "transition-building"]
            img [Src Assets.WebPAlt.GCBuilding1.src; SrcSet Assets.WebPAlt.GCBuilding1.srcSet; Width Assets.WebPAlt.GCBuilding1.width; Height Assets.WebPAlt.GCBuilding1.height; Sizes "100vw"; Alt ""; Class "transition-building"; HTMLAttr.Custom("loading", "lazy")]
          ]
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> __FILE__ + ":" + __LINE__)) props
