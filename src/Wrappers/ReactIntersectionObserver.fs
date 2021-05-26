(*
Fable binding for react-intersection-observer

Copyright 2021 cannorin

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*)

module rec ReactIntersectionObserver
open IntersectionObserver
open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
open Fable.React

type [<AllowNullLiteral>] RenderProps =
  abstract inView: bool with get, set
  abstract entry: IntersectionObserverEntry option with get, set
  abstract ref: (Element -> unit) with get, set

type [<AllowNullLiteral>] private InViewProps =
  inherit IntersectionObserverInit
  abstract triggerOnce: bool with set
  abstract skip: bool with set
  abstract initialInView: bool with set
  abstract trackVisibility: bool with set
  abstract delay: float with set
  abstract ``as``: string with set
  abstract children: U2<(RenderProps -> React.ReactNode), React.ReactNode> with set
  abstract onChange: (bool -> IntersectionObserverEntry -> unit) option with set

type Prop =
  | [<CompiledName("root")>] RootElement of Browser.Types.Element
  | [<CompiledName("root")>] RootDocument of Browser.Types.Document
  | Threshold of float
  | [<CompiledName("threshold")>] Thresholds of float[]
  /// Only trigger the inView callback once
  | TriggerOnce of bool
  /// Skip assigning the observer to the `ref`
  | Skip of bool
  | InitialInView of bool
  /// IntersectionObserver v2 - Track the actual visibility of the element
  | TrackVisibility of bool
  /// IntersectionObserver v2 - Set a minimum delay between notifications
  | Delay of float
  | As of string
  | OnChange of (bool -> IntersectionObserverEntry -> unit)
  | [<Erase>] CustomProp of Props.IHTMLProp
  static member inline op_ErasedCast (x: Props.IHTMLProp) : Prop = CustomProp x

let inline inView (props: Prop list) (children: RenderProps -> ReactElement) =
  let options = keyValueList CaseRules.LowerFirst props |> box :?> InViewProps
  options.children <- !^(fun props -> children props)
  ofImport "InView" "react-intersection-observer" options []

let inline inViewPlain (props: Prop list) children =
  let options = keyValueList CaseRules.LowerFirst props |> box :?> InViewProps
  ofImport "InView" "react-intersection-observer" options [children]
