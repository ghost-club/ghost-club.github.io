module ReactGridSystem
open System
open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open React

type [<StringEnum>] [<RequireQualifiedAccess>] Align =
  | Normal
  | Start
  | Center
  | End
  | Stretch

type [<StringEnum>] [<RequireQualifiedAccess>] Justify =
  | Start
  | Center
  | End
  | Between
  | Around
  | Initial
  | Inherit

type [<StringEnum>] [<RequireQualifiedAccess>] ScreenClass =
  | Xs
  | Sm
  | Md
  | Lg
  | Xl
  | Xxl

type ScreenClassItem<'T> =
  | Xs of 'T
  | Sm of 'T
  | Md of 'T
  | Lg of 'T
  | Xl of 'T
  | Xxl of 'T

type ScreenClassMap<'T> = ScreenClassItem<'T> seq

type Offsets = ScreenClassMap<float>
type Push = ScreenClassMap<float>
type Pull = ScreenClassMap<float>

type [<RequireQualifiedAccess>] ColProps =
  | Debug of bool
  | [<CompiledName("width")>] Width of int
  | [<CompiledName("width")>] WidthStr of string
  | Offset of Offsets
  | Push of Push
  | Pull of Pull
  | [<CompiledName("component")>] ComponentLazy of (unit -> string)
  | [<CompiledName("component")>] Component of string
  | Style of obj
  interface Fable.React.Props.IHTMLProp

type [<RequireQualifiedAccess>] ContainerProps =
  | Fluid of bool
  | [<CompiledName("component")>] ComponentLazy of (unit -> string)
  | [<CompiledName("component")>] Component of string
  | Style of obj
  interface Fable.React.Props.IHTMLProp

type [<RequireQualifiedAccess>] RowProps =
  | Debug of bool
  | Align of Align
  | Justify of Justify
  | Nogutter of bool
  | Nowrap of bool
  | GutterWidth of float
  | [<CompiledName("component")>] ComponentLazy of (unit -> string)
  | [<CompiledName("component")>] Component of string
  | Style of obj
  interface Fable.React.Props.IHTMLProp

type ClearFixProps = ScreenClassMap<bool>

type HiddenProps = ScreenClassMap<bool>

type [<RequireQualifiedAccess>] ScreenClassRenderProps =
  | Render of (obj -> ReactElement)

type VisibleProps = ScreenClassMap<bool>

type [<RequireQualifiedAccess>] Configuration =
  | Breakpoints of float[]
  | ContainerWidths of float[]
  | GutterWidth of float
  | GridColumns of float

type [<RequireQualifiedAccess>] ScreenClassProviderProps =
  | Children of ReactNode
  | FallbackScreenClass of ScreenClass
  | UseOwnWidth of bool

module private Internal =
  type [<AllowNullLiteral>] IExports =
    abstract setConfiguration: obj -> unit
    abstract useScreenClass: RefObject<'a> option -> string

  let [<ImportAll("react-grid-system")>] react_grid_system : IExports = jsNative

let setConfiguration (configuration: Configuration list) =
  Internal.react_grid_system.setConfiguration (keyValueList CaseRules.LowerFirst configuration)

let useScreenClass (elementRef: RefObject<'a> option) =
  Internal.react_grid_system.useScreenClass elementRef

open Fable.React

let col (props: Props.IHTMLProp list) children = ofImport "Col" "react-grid-system" (keyValueList CaseRules.LowerFirst props) children

let container (props: Props.IHTMLProp list) children = ofImport "Container" "react-grid-system" (keyValueList CaseRules.LowerFirst props) children

let row (props: Props.IHTMLProp list) children = ofImport "Row" "react-grid-system" (keyValueList CaseRules.LowerFirst props) children

// let clearFix (props: ClearFixProps list) children = ofImport "ClearFix" "react-grid-system" (keyValueList CaseRules.LowerFirst props) children

let hidden (props: HiddenProps list) children = ofImport "Hidden" "react-grid-system" (keyValueList CaseRules.LowerFirst props) children

let visible (props: VisibleProps list) children = ofImport "Visible" "react-grid-system" (keyValueList CaseRules.LowerFirst props) children

let screenClassRender (props: ScreenClassRenderProps list) children = ofImport "ScreenClassRender" "react-grid-system" (keyValueList CaseRules.LowerFirst props) children

let screenClassProvider (props: ScreenClassProviderProps list) children = ofImport "ScreenClassProvider" "react-grid-system" (keyValueList CaseRules.LowerFirst props) children
