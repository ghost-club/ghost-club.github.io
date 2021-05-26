(*
Fable binding for react-modal

Copyright 2021 cannorin

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*)

module ReactModal
open Fable.Core
open Fable.Core.JsInterop
open Browser.Types
open System.ComponentModel

type Style = interface end
module Style =
  open Fable.React.Props
  let inline Overlay (props: CSSProp list) : Style = !!("overlay", keyValueList CaseRules.LowerFirst props)
  let inline Content (props: CSSProp list) : Style = !!("content", keyValueList CaseRules.LowerFirst props)

type Classes = {| ``base``: string; afterOpen: string; beforeClose: string |}

type Aria =
  | Labelledby of string
  | Describedby of string
  | Modal of bool

type OnAfterOpenCallbackOptions = {| overlayEl: Element; contentEl: HTMLDivElement |}
type OnAfterOpenCallback = OnAfterOpenCallbackOptions -> unit

type Prop =
  | IsOpen of bool
  // | [<CompiledName("styles")>] ModalStyles of Styles
  | PortalClassName of string
  | BodyOpenClassName of string
  | HtmlOpenClassName of string
  | ClassName of string
  | [<CompiledName("className")>] ClassNameDetailed of Classes
  | OverlayClassName of string
  | [<CompiledName("overlayClassName")>] OverlayClassNameDetailed of Classes
  | AppElement of HTMLElement
  | OnAfterOpen of OnAfterOpenCallback
  | OnAfterClose of (unit -> unit)
  | OnRequestClose of (U2<React.MouseEvent, React.KeyboardEvent> -> unit)
  | CloseTimeoutMS of int
  | AriaHideApp of bool
  | ShouldFocusAfterRender of bool
  | ShouldCloseOnOverlayClick of bool
  | ShouldCloseOnEsc of bool
  | ShouldReturnFocusAfterClose of bool
  | PreventScroll of bool
  | ParentSelector of (unit -> HTMLElement)
  | Aria of Aria
  | Data of obj
  | Role of string
  | ContentLabel of string
  | ContentRef of (HTMLDivElement -> unit)
  | OverlayRef of (HTMLDivElement -> unit)
  | OverlayElement of ((Fable.React.Props.IHTMLProp seq * React.ReactElement) -> React.ReactElement)
  | ContentElement of ((Fable.React.Props.IHTMLProp seq * React.ReactElement) -> React.ReactElement)
  | TestId of string
  | [<CompiledName("id")>] ModalId of string
  | [<Erase; EditorBrowsable(EditorBrowsableState.Never)>] CustomItem of string * obj
  static member inline Style (styles: Style list) : Prop =
    CustomItem ("style", keyValueList CaseRules.LowerFirst styles)

type [<AllowNullLiteral>] ReactModalStatic =
  abstract defaultStyles: {| content: React.CSSProperties option; overlay: React.CSSProperties option |}

  /// Call this to properly hide your application from assistive screenreaders
  /// and other assistive technologies while the modal is open.
  abstract setAppElement: appElement: U2<string, HTMLElement> -> unit

let [<ImportDefault("react-modal")>] ReactModal: ReactModalStatic = jsNative

open Fable.Core.JsInterop
open Fable.React

let inline modal (props: Prop seq) children =
  ofImport "default" "react-modal" (keyValueList CaseRules.LowerFirst props) children