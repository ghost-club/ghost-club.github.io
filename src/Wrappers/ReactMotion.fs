(*
Fable binding for react-motion

Copyright 2021 cannorin

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*)

module ReactMotion
open Fable.Core
open Fable.Core.JsInterop

open Fable.React

type SpringHelperConfig =
  | Stiffness of int
  | Damping of int
  | Precision of float

type OpaqueConfig = {| ``val``: float; stiffness: int; damping: int; precision: float |}

let inline opaqueConfig value stiffness damping precision =
  {| ``val`` = value; stiffness = stiffness; damping = damping; precision = precision |}

[<Import("spring", "react-motion")>]
let spring (value: float) (config: SpringHelperConfig list) : OpaqueConfig = jsNative

type Presets =
  abstract noWobble: OpaqueConfig with get
  abstract gentle: OpaqueConfig with get
  abstract wobbly: OpaqueConfig with get
  abstract stiff: OpaqueConfig with get

[<Import("presets", "react-motion")>]
let presets : Presets = jsNative

type [<RequireQualifiedAccess>] MotionProp<'Style, 'PlainStyle> =
  | DefaultStyle of 'PlainStyle
  | Style of 'Style
  | OnRest of (unit -> unit)

let inline motion (props: MotionProp<'Style, 'PlainStyle> list) (children: 'PlainStyle -> ReactElement) : ReactElement =
  let props = keyValueList CaseRules.LowerFirst (!!("children", children) :: (box props :?> obj list))
  ofImport "Motion" "react-motion" props []

type [<RequireQualifiedAccess>] TransitionStyle<'Data, 'PlainStyle> =
  | Key of string
  | Data of 'Data
  | Style of 'PlainStyle

type TransitionPlainStyle<'Data, 'PlainStyle> = {|
  key: string
  data: 'Data
  style: 'PlainStyle
|}

type InterpolateFunction<'Data, 'PlainStyle> =
  TransitionPlainStyle<'Data, 'PlainStyle>[] -> TransitionStyle<'Data, 'PlainStyle>[]

type [<RequireQualifiedAccess>] TransitionProp<'Data, 'Style, 'PlainStyle> =
  | DefaultStyles of TransitionPlainStyle<'Data, 'PlainStyle>
  | Styles of TransitionStyle<'Data, 'PlainStyle>[]
  | [<CompiledName("styles")>] StylesFunc of InterpolateFunction<'Data, 'PlainStyle>
  | WillEnter of (TransitionStyle<'Data, 'PlainStyle> -> 'PlainStyle)
  | WillLeave of (TransitionStyle<'Data, 'PlainStyle> -> 'Style)
  | DidLeave of (TransitionStyle<'Data, 'PlainStyle> -> unit)

let inline transitionMotion (props: TransitionProp<'Data, 'Style, 'PlainStyle> list) (children: TransitionPlainStyle<'Data, 'PlainStyle>[] -> ReactElement) : ReactElement =
  ofImport "TransitionMotion" "react-motion" (keyValueList CaseRules.LowerFirst (!!("children", children) :: (box props :?> obj list))) []

type [<RequireQualifiedAccess>] StaggeredMotionProp<'Style, 'PlainStyle> =
  | DefaultStyles of 'PlainStyle[]
  | Styles of ('PlainStyle[] -> 'Style[])

let inline staggeredMotion (props: StaggeredMotionProp<'Style, 'PlainStyle> list) (children: 'PlainStyle[] -> ReactElement) =
  ofImport "StaggeredMotion" "react-motion" (keyValueList CaseRules.LowerFirst (!!("children", children) :: (box props :?> obj list))) []