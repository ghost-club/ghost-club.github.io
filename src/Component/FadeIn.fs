module FadeIn

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open ReactIntersectionObserver

let fadeIn =
  FunctionComponent.Of((fun (prop: {| key: string; children: ReactElement |}) ->
    let isShown = Hooks.useState false
    inViewPlain [
      !^Class(if isShown.current then "fade-in active" else "fade-in")
      OnChange (fun inView _ ->
        isShown.update inView
      )
    ] prop.children
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun p -> p.key))