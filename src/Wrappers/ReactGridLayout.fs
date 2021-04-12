// ts2fable 0.7.1
module rec ReactGridLayout
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types

let [<Import("*","react-grid-layout")>] reactGridLayout: ReactGridLayout.IExports = jsNative

type [<AllowNullLiteral>] IExports =
    abstract ReactGridLayout: ReactGridLayoutStatic

type [<AbstractClass; Erase>] ReactGridLayout =
    inherit React.Component<ReactGridLayout.ReactGridLayoutProps>

type [<AllowNullLiteral>] ReactGridLayoutStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> ReactGridLayout

type [<StringEnum>] [<RequireQualifiedAccess>] ResizeHandle =
    | S
    | W
    | E
    | N
    | Sw
    | Nw
    | Se
    | Ne

module ReactGridLayout =

    type [<AllowNullLiteral>] IExports =
        abstract Responsive: ResponsiveStatic
        abstract WidthProvider: ``component``: U2<React.ComponentClass<'P>, React.FunctionComponent<'P>> -> React.ComponentClass<obj>

    type [<AllowNullLiteral>] Layout =
        /// A string corresponding to the component key.
        /// Uses the index of components instead if not provided.
        abstract i: string with get, set
        /// X position in grid units.
        abstract x: float with get, set
        /// Y position in grid units.
        abstract y: float with get, set
        /// Width in grid units.
        abstract w: float with get, set
        /// Height in grid units.
        abstract h: float with get, set
        /// Minimum width in grid units.
        abstract minW: float option with get, set
        /// Maximum width in grid units.
        abstract maxW: float option with get, set
        /// Minimum height in grid units.
        abstract minH: float option with get, set
        /// Maximum height in grid units.
        abstract maxH: float option with get, set
        /// set by DragEvents (onDragStart, onDrag, onDragStop) and ResizeEvents (onResizeStart, onResize, onResizeStop)
        abstract moved: bool option with get, set
        /// If true, equal to `isDraggable: false` and `isResizable: false`.
        abstract ``static``: bool option with get, set
        /// If false, will not be draggable. Overrides `static`.
        abstract isDraggable: bool option with get, set
        /// If false, will not be resizable. Overrides `static`.
        abstract isResizable: bool option with get, set
        /// By default, a handle is only shown on the bottom-right (southeast) corner.
        /// Note that resizing from the top or left is generally not intuitive.
        abstract resizeHandles: ResizeArray<ResizeHandle> option with get, set
        /// If true and draggable, item will be moved only within grid.
        abstract isBounded: bool option with get, set

    type [<AllowNullLiteral>] Layouts =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: P: string -> ResizeArray<Layout> with get, set

    type [<AllowNullLiteral>] ItemCallback =
        [<Emit "$0($1...)">] abstract Invoke: layout: ResizeArray<Layout> * oldItem: Layout * newItem: Layout * placeholder: Layout * ``event``: MouseEvent * element: HTMLElement -> unit

    type [<AllowNullLiteral>] CoreProps =
        /// The classname to add to the root element.
        abstract className: string option with get, set
        /// Inline-style object to pass to the root element.
        abstract style: React.CSSProperties option with get, set
        /// If true, the container height swells and contracts to fit contents.
        abstract autoSize: bool option with get, set
        /// A CSS selector for tags that will not be draggable.
        ///
        /// For example: `draggableCancel: '.MyNonDraggableAreaClassName'`
        ///
        /// If you forget the leading. it will not work.
        abstract draggableCancel: string option with get, set
        /// A CSS selector for tags that will act as the draggable handle.
        ///
        /// For example: `draggableHandle: '.MyDragHandleClassName'`
        ///
        /// If you forget the leading . it will not work.
        abstract draggableHandle: string option with get, set
        /// If true, the layout will compact vertically.
        abstract verticalCompact: bool option with get, set
        /// Compaction type.
        abstract compactType: CorePropsCompactType option with get, set
        /// This allows setting the initial width on the server side.
        /// This is required unless using the HOC <WidthProvider> or similar.
        abstract width: float option with get, set
        /// Rows have a static height, but you can change this based on breakpoints if you like.
        abstract rowHeight: float option with get, set
        /// Configuration of a dropping element. Dropping element is a "virtual" element
        /// which appears when you drag over some element from outside.
        abstract droppingItem: CorePropsDroppingItem option with get, set
        /// If set to false it will disable dragging on all children.
        abstract isDraggable: bool option with get, set
        /// If set to false it will disable resizing on all children.
        abstract isResizable: bool option with get, set
        /// Defines which resize handles should be rendered
        /// Allows for any combination of:
        /// 's' - South handle (bottom-center)
        /// 'w' - West handle (left-center)
        /// 'e' - East handle (right-center)
        /// 'n' - North handle (top-center)
        /// 'sw' - Southwest handle (bottom-left)
        /// 'nw' - Northwest handle (top-left)
        /// 'se' - Southeast handle (bottom-right)
        /// 'ne' - Northeast handle (top-right)
        abstract resizeHandles: ResizeArray<ResizeHandle> option with get, set
        /// Defines custom component for resize handle
        abstract resizeHandle: U2<React.ReactNode, (ResizeHandle -> React.ReactNode)> option with get, set
        /// If set to false it will not call `onDrop()` callback.
        abstract isDroppable: bool option with get, set
        /// If true and draggable, all items will be moved only within grid.
        abstract isBounded: bool option with get, set
        /// If true, grid items won't change position when being dragged over.
        abstract preventCollision: bool option with get, set
        /// Uses CSS3 `translate()` instead of position top/left.
        /// This makes about 6x faster paint performance.
        abstract useCSSTransforms: bool option with get, set
        /// Default Infinity, but you can specify a max here if you like.
        /// Note that this isn't fully fleshed out and won't error if you specify a layout that
        /// extends beyond the row capacity. It will, however, not allow users to drag/resize
        /// an item past the barrier. They can push items beyond the barrier, though.
        /// Intentionally not documented for this reason.
        abstract maxRows: float option with get, set
        /// Scale coefficient for CSS3 `transform: scale()`
        abstract transformScale: float option with get, set
        /// Calls when drag starts.
        abstract onDragStart: ItemCallback option with get, set
        /// Calls on each drag movement.
        abstract onDrag: ItemCallback option with get, set
        /// Calls when drag is complete.
        abstract onDragStop: ItemCallback option with get, set
        /// Calls when resize starts.
        abstract onResizeStart: ItemCallback option with get, set
        /// Calls when resize movement happens.
        abstract onResize: ItemCallback option with get, set
        /// Calls when resize is complete.
        abstract onResizeStop: ItemCallback option with get, set
        /// Calls when some element has been dropped
        abstract onDrop: layout: ResizeArray<Layout> * item: Layout * e: Event -> unit

    type [<AllowNullLiteral>] ReactGridLayoutProps =
        inherit CoreProps
        /// Number of columns in this layout.
        abstract cols: float option with get, set
        /// Margin between items `[x, y]` in px.
        abstract margin: float * float option with get, set
        /// Padding inside the container `[x, y]` in px.
        abstract containerPadding: float * float option with get, set
        /// Layout is an array of object with the format:
        ///
        /// `{x: number, y: number, w: number, h: number}`
        ///
        /// The index into the layout must match the key used on each item component.
        /// If you choose to use custom keys, you can specify that key in the layout
        /// array objects like so:
        ///
        /// `{i: string, x: number, y: number, w: number, h: number}`
        ///
        /// If not provided, use data-grid props on children.
        abstract layout: ResizeArray<Layout> option with get, set
        /// Callback so you can save the layout.
        /// Calls back with (currentLayout) after every drag or resize stop.
        abstract onLayoutChange: layout: ResizeArray<Layout> -> unit

    type [<AllowNullLiteral>] ResponsiveProps =
        inherit CoreProps
        /// `{name: pxVal}, e.g. {lg: 1200, md: 996, sm: 768, xs: 480}`
        ///
        /// Breakpoint names are arbitrary but must match in the cols and layouts objects.
        abstract breakpoints: ResponsivePropsBreakpoints option with get, set
        /// Number of cols. This is a breakpoint -> cols map, e.g. `{lg: 12, md: 10, ...}`.
        abstract cols: ResponsivePropsBreakpoints option with get, set
        /// Margin between items in px and formatt [x, y] or { breakpoint: [x, y] }.
        abstract margin: U2<float * float, ResponsivePropsMargin> option with get, set
        /// Padding inside the container in px and formatt [x, y] or { breakpoint: [x, y] }.
        abstract containerPadding: U2<float * float, ResponsivePropsMargin> option with get, set
        /// layouts is an object mapping breakpoints to layouts.
        ///
        /// e.g. `{lg: Layout[], md: Layout[], ...}`
        abstract layouts: Layouts option with get, set
        /// Calls back with breakpoint and new number pf cols.
        abstract onBreakpointChange: newBreakpoint: string * newCols: float -> unit
        /// Callback so you can save the layout.
        abstract onLayoutChange: currentLayout: ResizeArray<Layout> * allLayouts: Layouts -> unit
        /// Callback when the width changes, so you can modify the layout as needed.
        abstract onWidthChange: containerWidth: float * margin: float * float * cols: float * containerPadding: float * float -> unit

    type [<AbstractClass; Erase>] Responsive =
        inherit React.Component<ResponsiveProps>

    type [<AllowNullLiteral>] ResponsiveStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Responsive

    type [<AllowNullLiteral>] WidthProviderProps =
        /// If true, WidthProvider will measure the container's width before mounting children.
        /// Use this if you'd like to completely eliminate any resizing animation on
        /// application/component mount.
        abstract measureBeforeMount: bool option with get, set

    type [<StringEnum>] [<RequireQualifiedAccess>] CorePropsCompactType =
        | Vertical
        | Horizontal

    type [<AllowNullLiteral>] CorePropsDroppingItem =
        abstract i: string with get, set
        abstract w: float with get, set
        abstract h: float with get, set

    type [<AllowNullLiteral>] ResponsivePropsBreakpoints =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: P: string -> float with get, set

    type [<AllowNullLiteral>] ResponsivePropsMargin =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: P: string -> float * float with get, set
