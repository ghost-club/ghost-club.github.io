module Wrappers.Rewrapped

open Fable.Core.JsInterop
open Properties

/// `i18next.t`
let inline (!~) (text: 'a) : string =
  I18next.i18next.t.Invoke(!^text)