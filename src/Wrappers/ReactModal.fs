module ReactModal
open Fable.Core
open Browser.Types

type Styles =
  | Content of Fable.React.Props.CSSProp list
  | Overlay of Fable.React.Props.CSSProp list

type Classes = {| ``base``: string; afterOpen: string; beforeClose: string |}

type Aria =
  | Labelledby of string
  | Describedby of string
  | Modal of bool

type OnAfterOpenCallbackOptions = {| overlayEl: Element; contentEl: HTMLDivElement |}
type OnAfterOpenCallback = OnAfterOpenCallbackOptions -> unit

type Prop =
  | IsOpen of bool
  | Styles of Styles
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
  | Id of string

type [<AllowNullLiteral>] ReactModalStatic =
  abstract defaultStyles: {| content: React.CSSProperties option; overlay: React.CSSProperties option |}

  /// Call this to properly hide your application from assistive screenreaders
  /// and other assistive technologies while the modal is open.
  abstract setAppElement: appElement: U2<string, HTMLElement> -> unit

let [<Import("ReactModal","react-modal")>] ReactModal: ReactModalStatic = jsNative

open Fable.Core.JsInterop
open Fable.React

let modal (props: Prop seq) children =
  ofImport "ReactModal" "react-modal" (keyValueList CaseRules.LowerFirst props) children