module React

open Fable.React
open Fable.Core

type Context<'T> = IContext<'T>
type ReactElement = Fable.React.ReactElement
type Component<'P, 'S> = Fable.React.Component<'P, 'S>
type Component<'P> = Component<'P, obj>

type FunctionComponent<'Props> = 'Props -> Fable.React.ReactElement

type ComponentType<'Props> =
    inherit Fable.React.ReactElementType<'Props>

type [<AllowNullLiteral>] HTMLProps<'T> = interface end

type [<AllowNullLiteral>] FunctionComponentElement<'T> =
    inherit Fable.React.ReactElement

type [<AllowNullLiteral>] ReactPortal =
    inherit Fable.React.ReactElement
    abstract children: Fable.React.ReactElement with get

type [<AllowNullLiteral>] ReactNode =
    inherit Fable.React.ReactElement

type ComponentClass<'T> =
    inherit ComponentType<'T>

type CSSProperties = interface end

module CSSProperties =
    open Fable.Core.JsInterop
    let inline ofCSSProps (css: Props.CSSProp list) : CSSProperties =
        keyValueList CaseRules.LowerFirst css :?> _