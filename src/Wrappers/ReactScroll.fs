module ReactScroll

open Fable.Core
open Fable.Core.JsInterop

open Fable.React

type Prop =
  | ActiveClass of string
  | To of string
  | Spy of bool
  | Smooth of bool
  | HashSpy of bool
  | Offset of px:int
  | Duration of ms:float
  | [<CompiledName("duration")>] DurationFunc of (float -> float)
  | Delay of ms:float
  | IsDynamic of bool
  | OnSetActive of (string -> unit)
  | OnSetInactive of (unit -> unit)
  | IgnoreCancelEvents of bool
  | [<Erase>] CustomProp of Props.IHTMLProp
  static member inline op_ErasedCast (x: Props.IHTMLProp) : Prop = CustomProp x

type [<RequireQualifiedAccess>] ElementProp =
  | Name of string
  | Id of string

module ReactScroll =
  let inline link (props: Prop seq) children =
    ofImport "Link" "react-scroll" (keyValueList CaseRules.LowerFirst props) children

  let inline button (props: Prop seq) children =
    ofImport "Button" "react-scroll" (keyValueList CaseRules.LowerFirst props) children

  let inline element (props: ElementProp seq) children =
    ofImport "Element" "react-scroll" (keyValueList CaseRules.LowerFirst props) children