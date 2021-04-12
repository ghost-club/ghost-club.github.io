module React

open Fable.React
open Fable.Core
open Browser.Types

type Context<'T> = IContext<'T>
type ReactElement = Fable.React.ReactElement
type Component<'P, 'S> = Fable.React.Component<'P, 'S>
type Component<'P> = Component<'P, obj>
type PureComponent<'P, 'S> = Fable.React.PureComponent<'P, 'S>
type PureComponent<'P> = PureComponent<'P, obj>

type FunctionComponent<'Props> = 'Props -> Fable.React.ReactElement
type StatelessComponent<'P> = FunctionComponent<'P>

type ReactType<'T> = Fable.React.ReactElementType<'T>

type ComponentType<'Props> = Fable.React.ReactElementType<'Props>

type [<AllowNullLiteral>] HTMLProps<'T> = interface end

type [<AllowNullLiteral>] FunctionComponentElement<'T> =
    inherit Fable.React.ReactElement

type [<AllowNullLiteral>] ReactPortal =
    inherit Fable.React.ReactElement
    abstract children: Fable.React.ReactElement with get

type ReactNode = Fable.React.ReactElement

type ComponentClass<'T> = ComponentType<'T>

type CSSProperties = interface end
module CSSProperties =
    open Fable.Core.JsInterop
    let inline ofCSSProps (css: Props.CSSProp list) : CSSProperties =
        keyValueList CaseRules.LowerFirst css :?> _

type [<AllowNullLiteral>] SyntheticEventBase = interface end

type [<AllowNullLiteral>] SyntheticEvent<'T, 'E when 'E :> Event and 'T :> EventTarget> =
    inherit SyntheticEventBase
    abstract bubbles: bool with get, set
    abstract currentTarget: 'C with get, set
    abstract cancelable: bool with get, set
    abstract defaultPrevented: bool with get, set
    abstract eventPhase: float with get, set
    abstract isTrusted: bool with get, set
    abstract nativeEvent: 'E with get, set
    abstract preventDefault: unit -> unit
    abstract isDefaultPrevented: unit -> bool
    abstract stopPropagation: unit -> unit
    abstract isPropagationStopped: unit -> bool
    abstract persist: unit -> unit
    abstract target: 'T with get, set
    abstract timeStamp: float with get, set
    abstract ``type``: string with get, set
type SyntheticEvent<'T when 'T :> EventTarget> = SyntheticEvent<'T, Event>

type [<AllowNullLiteral>] MouseEvent<'T when 'T :> EventTarget> =
    inherit SyntheticEvent<'T, MouseEvent>
    abstract altKey: bool with get, set
    abstract button: float with get, set
    abstract buttons: float with get, set
    abstract clientX: float with get, set
    abstract clientY: float with get, set
    abstract ctrlKey: bool with get, set
    abstract getModifierState: key: string -> bool
    abstract metaKey: bool with get, set
    abstract pageX: float with get, set
    abstract pageY: float with get, set
    abstract relatedTarget: EventTarget option with get, set
    abstract screenX: float with get, set
    abstract screenY: float with get, set
    abstract shiftKey: bool with get, set

type [<AllowNullLiteral>] TouchList =
    [<EmitIndexer>] abstract Item: index: float -> Touch with get, set
    abstract length: float with get, set
    abstract item: index: float -> Touch
    abstract identifiedTouch: identifier: float -> Touch

type [<AllowNullLiteral>] TouchEvent<'T when 'T :> EventTarget> =
    inherit SyntheticEvent<'T, TouchEvent>
    abstract altKey: bool with get, set
    abstract changedTouches: TouchList with get, set
    abstract ctrlKey: bool with get, set
    abstract getModifierState: key: string -> bool
    abstract metaKey: bool with get, set
    abstract shiftKey: bool with get, set
    abstract targetTouches: TouchList with get, set
    abstract touches: TouchList with get, set

type [<AllowNullLiteral>] EventHandler<'E when 'E :> SyntheticEventBase> =
    [<Emit "$0($1...)">] abstract Invoke: ``event``: 'E -> unit

module EventHandler =
    let inline create (func: 'E -> unit) : EventHandler<'E> when 'E :> SyntheticEventBase = box func :?> _

type ReactEventHandler<'T when 'T :> EventTarget> = EventHandler<SyntheticEvent<'T>>
type MouseEventHandler<'T when 'T :> EventTarget> = EventHandler<MouseEvent<'T>>
type TouchEventHandler<'T when 'T :> EventTarget> = EventHandler<TouchEvent<'T>>

type RefObject<'T> = IRefValue<'T>