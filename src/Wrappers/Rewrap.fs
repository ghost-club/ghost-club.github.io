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
