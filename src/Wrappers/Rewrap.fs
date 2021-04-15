module Wrappers.Rewrapped

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

  let inViewPlain (optionsSet: Props -> unit) (children: ReactElement list) =
    let options = jsOptions optionsSet |> box :?> InViewProps
    options.children <- !^(ofList children)
    ofImport "InView" "react-intersection-observer" options []
