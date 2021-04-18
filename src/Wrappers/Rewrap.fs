module Wrappers.Rewrapped

open Fable.Core
open Fable.Core.JsInterop
open Properties

/// `i18next.t`
let inline (!@) (text: 'a) : string =
  I18next.i18next.t.Invoke(!^text)

open Fable.React

module ReactSlick =
  open ReactSlick

  let slider (setProps: Settings -> unit) children =
    ofImport
      "default" "react-slick"
      (jsOptions setProps)
      children

module ReactIntersectionObserver =
  open IntersectionObserver
  open ReactIntersectionObserver

  type [<AllowNullLiteral>] Props =
    inherit IntersectionOptions
    abstract ``as``: string with set
    /// Children expects a function that receives an object
    /// contain an `inView` boolean and `ref` that should be
    /// assigned to the element root.
    abstract onChange: (bool -> IntersectionObserverEntry -> unit) with set

  let inView (optionsSet: Props -> unit) (children: RenderProps -> ReactElement list) =
    let options = jsOptions optionsSet |> box :?> InViewProps
    options.children <- !^(fun props -> ofList (children props))
    ofImport "InView" "react-intersection-observer" options []

  let inViewPlain (optionsSet: Props -> unit) children =
    let options = jsOptions optionsSet |> box :?> InViewProps
    ofImport "InView" "react-intersection-observer" options children

module ReactTransitionGroup =
  open ReactTransitionGroup

  let transition (optionsSet: TransitionProps<'RefElement> -> unit) children =
    ofImport "Transition" "react-transition-group" (jsOptions optionsSet) children

  let cssTransition (optionsSet: CSSTransitionProps<'RefElement> -> unit) children =
    ofImport "CSSTransition" "react-transition-group" (jsOptions optionsSet) children

  type TransitionGroupProp<'T> =
    | Component of 'T
    | ChildFactory of (ReactElement -> ReactElement)

  let transitionGroup (props: U2<Props.IHTMLProp, TransitionGroupProp<'T>> list) children =
    let option = keyValueList CaseRules.LowerFirst props :?> TransitionGroupProps<'T>
    ofImport "TransitionGroup" "react-transition-group" option children

  let switchTransition (mode: SwitchTransitionPropsMode option) children =
    let options =
      jsOptions<SwitchTransitionProps>(fun it ->
        match mode with
        | None -> ()
        | Some m -> it.mode <- Some m
      )
    ofImport "SwitchTransition" "react-transition-group" options children
