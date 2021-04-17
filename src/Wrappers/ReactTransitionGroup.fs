// ts2fable 0.7.1
module rec ReactTransitionGroup
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types

let [<Import("Transition","react-transition-group")>] Transition: TransitionStatic = jsNative
let [<Import("CSSTransition","react-transition-group")>] CSSTransition: CSSTransitionStatic = jsNative
let [<Import("TransitionGroup","react-transition-group")>] TransitionGroup : TransitionGroupStatic = jsNative
let [<Import("SwitchTransition","react-transition-group")>] SwitchTransition: SwitchTransitionStatic = jsNative
let [<Import("config","react-transition-group")>] config: Config = jsNative

type [<AllowNullLiteral>] Config =
    abstract disabled: bool with get, set
type Component<'T> = React.Component<'T>

type [<AllowNullLiteral>] IExports_CSSTransition =
    abstract CSSTransition: CSSTransitionStatic

type [<AllowNullLiteral>] CSSTransitionClassNames =
    abstract appear: string option with get, set
    abstract appearActive: string option with get, set
    abstract appearDone: string option with get, set
    abstract enter: string option with get, set
    abstract enterActive: string option with get, set
    abstract enterDone: string option with get, set
    abstract exit: string option with get, set
    abstract exitActive: string option with get, set
    abstract exitDone: string option with get, set

type CSSTransitionProps =
    CSSTransitionProps<obj>

type [<AllowNullLiteral>] CSSTransitionProps<'Ref> =
    inherit EndListenerProps<'Ref>
    abstract classNames: U2<string, CSSTransitionClassNames> with get,set
    abstract onEnter: node:HTMLElement * isAppearing:bool -> unit
    abstract onEntering: node:HTMLElement * isAppearing:bool -> unit
    abstract onExit: node:HTMLElement -> unit
    abstract onExiting: node:HTMLElement -> unit
    abstract onExited: node:HTMLElement -> unit

type [<AbstractClass; Erase>] CSSTransition<'Ref> =
    inherit Component<CSSTransitionProps<'Ref>>

type [<AllowNullLiteral>] CSSTransitionStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> CSSTransition<'Ref>

type [<AllowNullLiteral>] IExports_SwitchTransition =
    abstract SwitchTransition: SwitchTransitionStatic

type [<StringEnum>] [<RequireQualifiedAccess>] modes =
    | [<CompiledName "out-in">] Out
    | [<CompiledName "in-out">] In

type [<AllowNullLiteral>] SwitchTransitionProps =
    /// Transition modes.
    /// `out-in`: Current element transitions out first, then when complete, the new element transitions in.
    /// `in-out`: New element transitions in first, then when complete, the current element transitions out.
    abstract mode: SwitchTransitionPropsMode option with get, set
    /// Any `Transition` or `CSSTransition` component
    abstract children: ReactElement with get, set

/// A transition component inspired by the [vue transition modes](https://vuejs.org/v2/guide/transitions.html#Transition-Modes).
/// You can use it when you want to control the render between state transitions.
/// Based on the selected mode and the child's key which is the `Transition` or `CSSTransition` component, the `SwitchTransition` makes a consistent transition between them.
///
/// If the `out-in` mode is selected, the `SwitchTransition` waits until the old child leaves and then inserts a new child.
/// If the `in-out` mode is selected, the `SwitchTransition` inserts a new child first, waits for the new child to enter and then removes the old child
///
/// ```jsx
/// function App() {
///   const [state, setState] = useState(false);
///   return (
///     <SwitchTransition>
///       <FadeTransition key={state ? "Goodbye, world!" : "Hello, world!"}
///         addEndListener={(node, done) => node.addEventListener("transitionend", done, false)}
///         classNames='fade' >
///         <button onClick={() => setState(state => !state)}>
///           {state ? "Goodbye, world!" : "Hello, world!"}
///         </button>
///       </FadeTransition>
///     </SwitchTransition>
///   )
/// }
/// ```
type [<AbstractClass; Erase>] SwitchTransition =
    inherit Component<SwitchTransitionProps>

/// A transition component inspired by the [vue transition modes](https://vuejs.org/v2/guide/transitions.html#Transition-Modes).
/// You can use it when you want to control the render between state transitions.
/// Based on the selected mode and the child's key which is the `Transition` or `CSSTransition` component, the `SwitchTransition` makes a consistent transition between them.
///
/// If the `out-in` mode is selected, the `SwitchTransition` waits until the old child leaves and then inserts a new child.
/// If the `in-out` mode is selected, the `SwitchTransition` inserts a new child first, waits for the new child to enter and then removes the old child
///
/// ```jsx
/// function App() {
///   const [state, setState] = useState(false);
///   return (
///     <SwitchTransition>
///       <FadeTransition key={state ? "Goodbye, world!" : "Hello, world!"}
///         addEndListener={(node, done) => node.addEventListener("transitionend", done, false)}
///         classNames='fade' >
///         <button onClick={() => setState(state => !state)}>
///           {state ? "Goodbye, world!" : "Hello, world!"}
///         </button>
///       </FadeTransition>
///     </SwitchTransition>
///   )
/// }
/// ```
type [<AllowNullLiteral>] SwitchTransitionStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> SwitchTransition

type [<StringEnum>] [<RequireQualifiedAccess>] SwitchTransitionPropsMode =
    | [<CompiledName "out-in">] OutIn
    | [<CompiledName "in-out">] InOut
type ReactNode = React.ReactNode
let [<Import("UNMOUNTED","react-transition-group/Transition")>] UNMOUNTED: obj = jsNative
let [<Import("EXITED","react-transition-group/Transition")>] EXITED: obj = jsNative
let [<Import("ENTERING","react-transition-group/Transition")>] ENTERING: obj = jsNative
let [<Import("ENTERED","react-transition-group/Transition")>] ENTERED: obj = jsNative
let [<Import("EXITING","react-transition-group/Transition")>] EXITING: obj = jsNative

type [<AllowNullLiteral>] IExports =
    abstract Transition: TransitionStatic

type [<AllowNullLiteral>] RefHandler<'RefElement, 'ImplicitRefHandler, 'ExplicitRefHandler> =
    abstract implicit: 'ImplicitRefHandler with get, set
    abstract explicit: 'ExplicitRefHandler with get, set

type EndHandler<'RefElement> =
    RefHandler<'RefElement, (HTMLElement -> (unit -> unit) -> unit), ((unit -> unit) -> unit)>

type EnterHandler<'RefElement> =
    RefHandler<'RefElement, (HTMLElement -> bool -> unit), (bool -> unit)>

type ExitHandler<'E> =
    RefHandler<'E, (HTMLElement -> unit), (unit -> unit)>

type [<AllowNullLiteral>] TransitionActions =
    /// Normally a component is not transitioned if it is shown when the
    /// `<Transition>` component mounts. If you want to transition on the first
    /// mount set  appear to true, and the component will transition in as soon
    /// as the `<Transition>` mounts. Note: there are no specific "appear" states.
    /// appear only adds an additional enter transition.
    abstract appear: bool option with get, set
    /// Enable or disable enter transitions.
    abstract enter: bool option with get, set
    /// Enable or disable exit transitions.
    abstract exit: bool option with get, set

type [<AllowNullLiteral>] BaseTransitionProps<'RefElement> =
    /// Show the component; triggers the enter or exit states
    abstract ``in``: bool option with get, set
    /// By default the child component is mounted immediately along with the
    /// parent Transition component. If you want to "lazy mount" the component on
    /// the first `in={true}` you can set `mountOnEnter`. After the first enter
    /// transition the component will stay mounted, even on "exited", unless you
    /// also specify `unmountOnExit`.
    abstract mountOnEnter: bool option with get, set
    /// By default the child component stays mounted after it reaches the
    /// 'exited' state. Set `unmountOnExit` if you'd prefer to unmount the
    /// component after it finishes exiting.
    abstract unmountOnExit: bool option with get, set
    /// Callback fired before the "entering" status is applied. An extra
    /// parameter `isAppearing` is supplied to indicate if the enter stage is
    /// occurring on the initial mount
    abstract onEnter: EnterHandler<'RefElement> option with get, set
    /// Callback fired after the "entering" status is applied. An extra parameter
    /// isAppearing is supplied to indicate if the enter stage is occurring on
    /// the initial mount
    abstract onEntering: EnterHandler<'RefElement> option with get, set
    /// Callback fired after the "entered" status is applied. An extra parameter
    /// isAppearing is supplied to indicate if the enter stage is occurring on
    /// the initial mount
    abstract onEntered: EnterHandler<'RefElement> option with get, set
    /// Callback fired before the "exiting" status is applied.
    abstract onExit: ExitHandler<'RefElement> option with get, set
    /// Callback fired after the "exiting" status is applied.
    abstract onExiting: ExitHandler<'RefElement> option with get, set
    /// Callback fired after the "exited" status is applied.
    abstract onExited: ExitHandler<'RefElement> option with get, set
    /// A function child can be used instead of a React element. This function is
    /// called with the current transition status ('entering', 'entered',
    /// 'exiting',  'exited', 'unmounted'), which can be used to apply context
    /// specific props to a component.
    /// ```jsx
    ///     <Transition in={this.state.in} timeout={150}>
    ///         {state => (
    ///             <MyComponent className={`fade fade-${state}`} />
    ///         )}
    ///     </Transition>
    /// ```
    abstract children: TransitionChildren option with get, set
    /// A React reference to DOM element that need to transition: https://stackoverflow.com/a/51127130/4671932
    /// When `nodeRef` prop is used, node is not passed to callback functions (e.g. onEnter) because user already has direct access to the node.
    /// When changing `key` prop of `Transition` in a `TransitionGroup` a new `nodeRef` need to be provided to `Transition` with changed `key`
    /// prop (@see https://github.com/reactjs/react-transition-group/blob/master/test/Transition-test.js).
    abstract nodeRef: React.Ref<'RefElement> option with get, set
    [<Emit "$0[$1]{{=$2}}">] abstract Item: prop: string -> obj option with get, set

type TransitionStatus =
    obj

type TransitionChildren =
    U2<ReactNode, (TransitionStatus -> ReactNode)>

type [<AllowNullLiteral>] TimeoutProps<'RefElement> =
    inherit BaseTransitionProps<'RefElement>
    /// The duration of the transition, in milliseconds. Required unless addEndListener is provided.
    ///
    /// You may specify a single timeout for all transitions:
    /// ```js
    ///    timeout={500}
    /// ```
    /// or individually:
    /// ```js
    /// timeout={{
    ///   appear: 500,
    ///   enter: 300,
    ///   exit: 500,
    /// }}
    /// ```
    /// - appear defaults to the value of `enter`
    /// - enter defaults to `0`
    /// - exit defaults to `0`
    abstract timeout: U2<float, TimeoutPropsTimeout> with get, set
    /// Add a custom transition end trigger. Called with the transitioning DOM
    /// node and a done callback. Allows for more fine grained transition end
    /// logic. Note: Timeouts are still used as a fallback if provided.
    abstract addEndListener: EndHandler<'RefElement> option with get, set

type [<AllowNullLiteral>] EndListenerProps<'Ref> =
    inherit BaseTransitionProps<'Ref>
    /// The duration of the transition, in milliseconds. Required unless addEndListener is provided.
    ///
    /// You may specify a single timeout for all transitions:
    /// ```js
    ///    timeout={500}
    /// ```
    /// or individually:
    /// ```js
    /// timeout={{
    ///   appear: 500,
    ///   enter: 300,
    ///   exit: 500,
    /// }}
    /// ```
    /// - appear defaults to the value of `enter`
    /// - enter defaults to `0`
    /// - exit defaults to `0`
    abstract timeout: U2<float, TimeoutPropsTimeout> option with get, set
    /// Add a custom transition end trigger. Called with the transitioning DOM
    /// node and a done callback. Allows for more fine grained transition end
    /// logic. Note: Timeouts are still used as a fallback if provided.
    abstract addEndListener: EndHandler<'Ref> with get, set

type TransitionProps =
    TransitionProps<obj>

type TransitionProps<'RefElement> =
    U2<TimeoutProps<'RefElement>, EndListenerProps<'RefElement>>

/// The Transition component lets you describe a transition from one component
/// state to another _over time_ with a simple declarative API. Most commonly
/// It's used to animate the mounting and unmounting of Component, but can also
/// be used to describe in-place transition states as well.
///
/// By default the `Transition` component does not alter the behavior of the
/// component it renders, it only tracks "enter" and "exit" states for the components.
/// It's up to you to give meaning and effect to those states. For example we can
/// add styles to a component when it enters or exits:
///
/// ```jsx
/// import Transition from 'react-transition-group/Transition';
///
/// const duration = 300;
///
/// const defaultStyle = {
///    transition: `opacity ${duration}ms ease-in-out`,
///    opacity: 0,
/// }
///
/// const transitionStyles = {
///    entering: { opacity: 1 },
///    entered:  { opacity: 1 },
/// };
///
/// const Fade = ({ in: inProp }) => (
///    <Transition in={inProp} timeout={duration}>
///      {(state) => (
///        <div style={{
///          ...defaultStyle,
///          ...transitionStyles[state]
///        }}>
///          I'm A fade Transition!
///        </div>
///      )}
///    </Transition>
/// );
/// ```
type [<AbstractClass; Erase>] Transition<'RefElement> =
    inherit Component<TransitionProps<'RefElement>>

/// The Transition component lets you describe a transition from one component
/// state to another _over time_ with a simple declarative API. Most commonly
/// It's used to animate the mounting and unmounting of Component, but can also
/// be used to describe in-place transition states as well.
///
/// By default the `Transition` component does not alter the behavior of the
/// component it renders, it only tracks "enter" and "exit" states for the components.
/// It's up to you to give meaning and effect to those states. For example we can
/// add styles to a component when it enters or exits:
///
/// ```jsx
/// import Transition from 'react-transition-group/Transition';
///
/// const duration = 300;
///
/// const defaultStyle = {
///    transition: `opacity ${duration}ms ease-in-out`,
///    opacity: 0,
/// }
///
/// const transitionStyles = {
///    entering: { opacity: 1 },
///    entered:  { opacity: 1 },
/// };
///
/// const Fade = ({ in: inProp }) => (
///    <Transition in={inProp} timeout={duration}>
///      {(state) => (
///        <div style={{
///          ...defaultStyle,
///          ...transitionStyles[state]
///        }}>
///          I'm A fade Transition!
///        </div>
///      )}
///    </Transition>
/// );
/// ```
type [<AllowNullLiteral>] TransitionStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> Transition<'RefElement>

type [<AllowNullLiteral>] TimeoutPropsTimeout =
    abstract appear: float option with get, set
    abstract enter: float option with get, set
    abstract exit: float option with get, set
type ReactType<'T> = React.ReactType<'T>
type ReactElement = React.ReactElement

type [<AllowNullLiteral>] IExports_TransitionGroup =
    abstract TransitionGroup: TransitionGroupStatic

type [<AllowNullLiteral>] TransitionGroupProps<'T> =
    inherit TransitionActions
    abstract ``component``: 'T with get, set
    abstract children: U2<ReactElement, ResizeArray<ReactElement>> option with get,set
    abstract childFactory: option<ReactElement -> ReactElement> with get,set
    [<Emit("$0[$1]{{=$s}}")>]
    abstract Item: string -> obj with get,set

/// The `<TransitionGroup>` component manages a set of `<Transition>` components
/// in a list. Like with the `<Transition>` component, `<TransitionGroup>`, is a
/// state machine for managing the mounting and unmounting of components over
/// time.
///
/// Consider the example below using the `Fade` CSS transition from before.
/// As items are removed or added to the TodoList the `in` prop is toggled
/// automatically by the `<TransitionGroup>`. You can use _any_ `<Transition>`
/// component in a `<TransitionGroup>`, not just css.
///
/// ```jsx
/// import TransitionGroup from 'react-transition-group/TransitionGroup';
///
/// class TodoList extends React.Component {
///    constructor(props) {
///      super(props)
///      this.state = {items: ['hello', 'world', 'click', 'me']}
///    }
///    handleAdd() {
///      const newItems = this.state.items.concat([
///        prompt('Enter some text')
///      ]);
///      this.setState({ items: newItems });
///    }
///    handleRemove(i) {
///      let newItems = this.state.items.slice();
///      newItems.splice(i, 1);
///      this.setState({items: newItems});
///    }
///    render() {
///      return (
///        <div>
///          <button onClick={() => this.handleAdd()}>Add Item</button>
///          <TransitionGroup>
///            {this.state.items.map((item, i) => (
///              <FadeTransition key={item}>
///                <div>
///                  {item}{' '}
///                  <button onClick={() => this.handleRemove(i)}>
///                    remove
///                  </button>
///                </div>
///              </FadeTransition>
///            ))}
///          </TransitionGroup>
///        </div>
///      );
///    }
/// }
/// ```
///
/// Note that `<TransitionGroup>`  does not define any animation behavior!
/// Exactly _how_ a list item animates is up to the individual `<Transition>`
/// components. This means you can mix and match animations across different
/// list items.
type [<AbstractClass; Erase>] TransitionGroup<'T> =
    inherit Component<TransitionGroupProps<'T>>

/// The `<TransitionGroup>` component manages a set of `<Transition>` components
/// in a list. Like with the `<Transition>` component, `<TransitionGroup>`, is a
/// state machine for managing the mounting and unmounting of components over
/// time.
///
/// Consider the example below using the `Fade` CSS transition from before.
/// As items are removed or added to the TodoList the `in` prop is toggled
/// automatically by the `<TransitionGroup>`. You can use _any_ `<Transition>`
/// component in a `<TransitionGroup>`, not just css.
///
/// ```jsx
/// import TransitionGroup from 'react-transition-group/TransitionGroup';
///
/// class TodoList extends React.Component {
///    constructor(props) {
///      super(props)
///      this.state = {items: ['hello', 'world', 'click', 'me']}
///    }
///    handleAdd() {
///      const newItems = this.state.items.concat([
///        prompt('Enter some text')
///      ]);
///      this.setState({ items: newItems });
///    }
///    handleRemove(i) {
///      let newItems = this.state.items.slice();
///      newItems.splice(i, 1);
///      this.setState({items: newItems});
///    }
///    render() {
///      return (
///        <div>
///          <button onClick={() => this.handleAdd()}>Add Item</button>
///          <TransitionGroup>
///            {this.state.items.map((item, i) => (
///              <FadeTransition key={item}>
///                <div>
///                  {item}{' '}
///                  <button onClick={() => this.handleRemove(i)}>
///                    remove
///                  </button>
///                </div>
///              </FadeTransition>
///            ))}
///          </TransitionGroup>
///        </div>
///      );
///    }
/// }
/// ```
///
/// Note that `<TransitionGroup>`  does not define any animation behavior!
/// Exactly _how_ a list item animates is up to the individual `<Transition>`
/// components. This means you can mix and match animations across different
/// list items.
type [<AllowNullLiteral>] TransitionGroupStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> TransitionGroup<'T>
