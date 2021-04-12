module rec ReactIntersectionObserver
open System
open Browser.Types
open Fable.Core
open Fable.Core.JS
open IntersectionObserver

type [<AllowNullLiteral>] ObserverInstanceCallback =
    [<Emit "$0($1...)">] abstract Invoke: inView: bool * entry: IntersectionObserverEntry -> unit

type [<AllowNullLiteral>] ObserverInstance =
    abstract inView: bool with get, set
    abstract callback: ObserverInstanceCallback
    abstract element: Element
    abstract observerId: string
    abstract observer: IntersectionObserver
    abstract thresholds: ReadonlyArray<float>

type [<AllowNullLiteral>] RenderProps =
    inherit State
    abstract ref: (Element -> unit) with get, set

type [<AllowNullLiteral>] IntersectionOptions =
    inherit IntersectionObserverInit
    /// Only trigger the inView callback once
    abstract triggerOnce: bool with set
    /// Skip assigning the observer to the `ref`
    abstract skip: bool with set
    abstract initialInView: bool with set
    /// IntersectionObserver v2 - Track the actual visibility of the element
    abstract trackVisibility: bool with set
    /// IntersectionObserver v2 - Set a minimum delay between notifications
    abstract delay: float with set

type [<AllowNullLiteral>] IntersectionObserverProps =
    inherit IntersectionOptions
    /// Children expects a function that receives an object
    /// contain an `inView` boolean and `ref` that should be
    /// assigned to the element root.
    abstract children: (RenderProps -> React.ReactNode) with set
    /// Call this function whenever the in view state changes
    abstract onChange: (bool -> IntersectionObserverEntry -> unit) with set

type InViewHookResponse = RenderProps

type [<AllowNullLiteral>] State =
    abstract inView: bool with get, set
    abstract entry: IntersectionObserverEntry option with get, set

type [<AllowNullLiteral>] InViewProps =
    inherit IntersectionOptions
    abstract ``as``: string with get, set
    /// Children expects a function that receives an object
    /// contain an `inView` boolean and `ref` that should be
    /// assigned to the element root.
    abstract children: U2<(RenderProps -> React.ReactNode), React.ReactNode> with get, set
    abstract onChange: (bool -> IntersectionObserverEntry -> unit) option with get, set

/// Monitors scroll, and triggers the children function with updated props
type [<AbstractClass; Erase>] InView =
    inherit React.Component<InViewProps, State>
    abstract componentDidUpdate: prevProps: IntersectionObserverProps -> unit
    abstract node: Element option with get, set
    abstract _unobserveCb: (unit -> unit) option with get, set
    abstract observeNode: unit -> unit
    abstract unobserve: unit -> unit
    abstract handleNode: (Element -> unit) with get, set
    abstract handleChange: (bool -> IntersectionObserverEntry -> unit) with get, set

/// Monitors scroll, and triggers the children function with updated props
type [<AllowNullLiteral>] InViewStatic =
    abstract displayName: string with get, set
    abstract defaultProps: InViewStaticDefaultProps with get, set
    [<Emit "new $0($1...)">] abstract Create: props: InViewProps -> InView

type [<AllowNullLiteral>] InViewStaticDefaultProps =
    abstract threshold: float with get, set
    abstract triggerOnce: bool with get, set
    abstract initialInView: bool with get, set

type [<AllowNullLiteral>] IExports =
    abstract InView: InViewStatic
    abstract useInView: ?p0: IntersectionOptions -> InViewHookResponse

let [<ImportDefault("react-intersection-observer")>] InView: InViewStatic = jsNative
let [<Import("*", "react-intersection-observer")>] reactIntersectionObserver: IExports = jsNative