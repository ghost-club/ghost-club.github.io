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