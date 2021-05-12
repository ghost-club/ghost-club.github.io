module ReactTransitionGroup
open Fable.Core
open Fable.Core.JsInterop

open Browser.Types
open Fable.React
open System.ComponentModel

type ReactTransitionElement =
  inherit ReactElement
type ITransitionProp = interface end
type ICSSTransitionProp = interface end
type ITransitionGroupProp = interface end

type CommonProp =
  /// Normally a component is not transitioned if it is shown when the
  /// `<Transition>` component mounts. If you want to transition on the first
  /// mount set  appear to true, and the component will transition in as soon
  /// as the `<Transition>` mounts. Note: there are no specific "appear" states.
  /// appear only adds an additional enter transition.
  | Appear of bool
  /// Enable or disable enter transitions.
  | Enter of bool
  /// Enable or disable exit transitions.
  | Exit of bool
  | [<Erase; EditorBrowsable(EditorBrowsableState.Never)>] CustomProp of Props.IHTMLProp
  interface ITransitionProp
  interface ICSSTransitionProp
  interface ITransitionGroupProp
  static member inline op_ErasedCast (x: Props.IHTMLProp) : CommonProp = CustomProp x

type RefHandler<'ImplicitRefHandler, 'ExplicitRefHandler> =
  {| implicit: 'ImplicitRefHandler; explicit: 'ExplicitRefHandler |}

type EndHandler = RefHandler<(HTMLElement -> (unit -> unit) -> unit), ((unit -> unit) -> unit)>
type EnterHandler = RefHandler<(HTMLElement -> bool -> unit), (bool -> unit)>
type ExitHandler = RefHandler<(HTMLElement -> unit), (unit -> unit)>

type CommonTransitionProp<'RefElement> =
  /// The duration of the transition, in milliseconds. Required unless addEndListener is provided.
  | [<CompiledName("timeout")>] TimeoutForAll of float
  /// Add a custom transition end trigger. Called with the transitioning DOM
  /// node and a done callback. Allows for more fine grained transition end
  /// logic. Note: Timeouts are still used as a fallback if provided.
  | [<CompiledName("addEndListener")>] EndListener of EndHandler
  /// Show the component; triggers the enter or exit states
  | In of bool
  /// By default the child component is mounted immediately along with the
  /// parent Transition component. If you want to "lazy mount" the component on
  /// the first `in={true}` you can set `mountOnEnter`. After the first enter
  /// transition the component will stay mounted, even on "exited", unless you
  /// also specify `unmountOnExit`.
  | MountOnEnter of bool
  /// By default the child component stays mounted after it reaches the
  /// 'exited' state. Set `unmountOnExit` if you'd prefer to unmount the
  /// component after it finishes exiting.
  | UnmountOnExit of bool
  /// A React reference to DOM element that need to transition: https://stackoverflow.com/a/51127130/4671932
  /// When `nodeRef` prop is used, node is not passed to callback functions (e.g. onEnter) because user already has direct access to the node.
  /// When changing `key` prop of `Transition` in a `TransitionGroup` a new `nodeRef` need to be provided to `Transition` with changed `key`
  | NodeRef of React.Ref<'RefElement>
  interface ITransitionProp
  interface ICSSTransitionProp

type [<RequireQualifiedAccess>] TransitionProp =
  /// Callback fired before the "entering" status is applied. An extra
  /// parameter `isAppearing` is supplied to indicate if the enter stage is
  /// occurring on the initial mount
  | OnEnter of EnterHandler
  /// Callback fired after the "entering" status is applied. An extra parameter
  /// isAppearing is supplied to indicate if the enter stage is occurring on
  /// the initial mount
  | OnEntering of EnterHandler
  /// Callback fired after the "entered" status is applied. An extra parameter
  /// isAppearing is supplied to indicate if the enter stage is occurring on
  /// the initial mount
  | OnEntered of EnterHandler
  /// Callback fired before the "exiting" status is applied.
  | OnExit of ExitHandler
  /// Callback fired after the "exiting" status is applied.
  | OnExiting of ExitHandler
  /// Callback fired after the "exited" status is applied.
  | OnExited of ExitHandler
  interface ITransitionProp

type [<RequireQualifiedAccess>] Class =
  | Appear of string | AppearActive of string | AppearDone of string
  | Enter of string | EnterActive of string | EnterDone of string
  | Exit of string | ExitActive of string | ExitDone of string

type [<RequireQualifiedAccess>] CSSTransitionProp =
  | [<CompiledName("classNames")>] ClassNamesForAll of string
  ///     (node: HTMLElement, isAppearing: bool)
  | OnEnter of (HTMLElement * bool -> unit)
  ///     (node: HTMLElement, isAppearing: bool)
  | OnEntering of (HTMLElement * bool -> unit)
  ///     (node: HTMLElement, isAppearing: bool)
  | OnExit of (HTMLElement * bool -> unit)
  ///     (node: HTMLElement, isAppearing: bool)
  | OnExiting of (HTMLElement * bool -> unit)
  interface ICSSTransitionProp
  static member inline ClassNames (classNames: Class list) : ICSSTransitionProp =
    !!("classNames", keyValueList CaseRules.LowerFirst classNames)

type ICommonProp =
  inherit ITransitionProp
  inherit ICSSTransitionProp
type [<RequireQualifiedAccess>] Timeout = Appear of float | Enter of float | Exit of float
/// The duration of the transition, in milliseconds. Required unless addEndListener is provided.
let inline Timeout (timeouts: {| appear: int; enter: int; exit: int |}) : ICommonProp = !!("timeout", timeouts)

type ICustomProp =
  inherit ICommonProp
  inherit ITransitionGroupProp

type [<StringEnum; RequireQualifiedAccess>] TransitionStatus =
  | [<CompiledName("unmounted")>] Unmounted
  | [<CompiledName("exiting")>] Exiting
  | [<CompiledName("exited")>] Exited
  | [<CompiledName("entering")>] Entering
  | [<CompiledName("entered")>] Entered
type private TransitionChildrenFunc = Children of (TransitionStatus -> ReactElement)

/// The Transition component lets you describe a transition from one component
/// state to another _over time_ with a simple declarative API. Most commonly
/// It's used to animate the mounting and unmounting of Component, but can also
/// be used to describe in-place transition states as well.
///
/// By default the `Transition` component does not alter the behavior of the
/// component it renders, it only tracks "enter" and "exit" states for the components.
/// It's up to you to give meaning and effect to those states.
let inline transition (props: ITransitionProp list) children =
  let props = keyValueList CaseRules.LowerFirst props
  box (ofImport "Transition" "react-transition-group" props [children]) :?> ReactTransitionElement

/// The Transition component lets you describe a transition from one component
/// state to another _over time_ with a simple declarative API. Most commonly
/// It's used to animate the mounting and unmounting of Component, but can also
/// be used to describe in-place transition states as well.
///
/// By default the `Transition` component does not alter the behavior of the
/// component it renders, it only tracks "enter" and "exit" states for the components.
/// It's up to you to give meaning and effect to those states.
///
/// A function child can be used instead of a React element. This function is
/// called with the current transition status ('entering', 'entered',
/// 'exiting',  'exited', 'unmounted'), which can be used to apply context.
/// ```jsx
///     <Transition in={this.state.in} timeout={150}>
///         {state => (
///             <MyComponent className={`fade fade-${state}`} />
///         )}
///     </Transition>
/// ```
let inline transitionFunc (props: ITransitionProp list) (children: TransitionStatus -> ReactElement) =
  let props = keyValueList CaseRules.LowerFirst (box (Children children) :: (box props :?> obj list))
  box (ofImport "Transition" "react-transition-group" props []) :?> ReactTransitionElement

let inline cssTransition (props: ICSSTransitionProp list) children =
  let props = keyValueList CaseRules.LowerFirst props
  box (ofImport "CSSTransition" "react-transition-group" props [children]) :?> ReactTransitionElement

/// A function child can be used instead of a React element. This function is
/// called with the current transition status ('entering', 'entered',
/// 'exiting',  'exited', 'unmounted'), which can be used to apply context.
let inline cssTransitionFunc (props: ICSSTransitionProp list) (children: TransitionStatus -> ReactElement) =
  let props = keyValueList CaseRules.LowerFirst (box (Children children) :: (box props :?> obj list))
  box (ofImport "CSSTransition" "react-transition-group" props []) :?> ReactTransitionElement

type [<RequireQualifiedAccess>] TransitionGroupProp<'T> =
  | Component of 'T
  | ChildFactory of (ReactElement -> ReactElement)
  interface ITransitionGroupProp

/// The `<TransitionGroup>` component manages a set of `<Transition>` components
/// in a list. Like with the `<Transition>` component, `<TransitionGroup>`, is a
/// state machine for managing the mounting and unmounting of components over
/// time.
///
/// Note that `<TransitionGroup>`  does not define any animation behavior!
/// Exactly _how_ a list item animates is up to the individual `<Transition>`
/// components. This means you can mix and match animations across different
/// list items.
let inline transitionGroup (props: ITransitionGroupProp list) (children: ReactTransitionElement seq) =
  let option = keyValueList CaseRules.LowerFirst props
  ofImport "TransitionGroup" "react-transition-group" option (box children :?> _)

type [<StringEnum>] [<RequireQualifiedAccess>] SwitchTransitionMode =
  | [<CompiledName "out-in">] OutIn
  | [<CompiledName "in-out">] InOut

/// A transition component inspired by the [vue transition modes](https://vuejs.org/v2/guide/transitions.html#Transition-Modes).
/// You can use it when you want to control the render between state transitions.
/// Based on the selected mode and the child's key which is the `Transition` or `CSSTransition` component, the `SwitchTransition` makes a consistent transition between them.
///
/// If the `out-in` mode is selected, the `SwitchTransition` waits until the old child leaves and then inserts a new child.
/// If the `in-out` mode is selected, the `SwitchTransition` inserts a new child first, waits for the new child to enter and then removes the old child
let inline switchTransition (mode: SwitchTransitionMode) (children: ReactTransitionElement) =
  ofImport "SwitchTransition" "react-transition-group" {| mode = mode |} (box children :?> _)
