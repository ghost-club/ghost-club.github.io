[<AutoOpen>]
module Misc

open Fable.Core
open Fable.Core.JsInterop

open Browser.Types

type IDisableScroll =
  abstract ``on``: ?elem:Element -> unit
  abstract off: unit -> unit

[<ImportDefault("disable-scroll")>]
let disableScroll : IDisableScroll = jsNative

module Hooks =
  [<Import("useReduceMotion", "react-reduce-motion")>]
  let useReduceMotion: unit -> bool = jsNative