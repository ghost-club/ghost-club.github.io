// ts2fable 0.7.1
module rec Masonry
open System
open Fable
open Fable.Core
open Fable.Core.JS
open Browser.Types

type ComponentClass<'T> = React.ComponentClass<'T>
let [<Import("Masonry","react-masonry-component")>] Masonry: ComponentClass<MasonryPropTypes> = jsNative

type [<AllowNullLiteral>] MasonryOptions =
    abstract columnWidth: U3<float, string, HTMLElement> option with get, set
    abstract itemSelector: string option with get, set
    abstract gutter: U2<float, string> option with get, set
    abstract percentPosition: bool option with get, set
    abstract horizontalOrder: bool option with get, set
    abstract stamp: string option with get, set
    abstract fitWidth: bool option with get, set
    abstract originLeft: bool option with get, set
    abstract originTop: bool option with get, set
    abstract containerStyle: Object option with get, set
    abstract transitionDuration: U2<float, string> option with get, set
    abstract resize: bool option with get, set
    abstract initLayout: bool option with get, set

type [<AllowNullLiteral>] MasonryPropTypes =
    abstract enableResizableChildren: bool option with get, set
    abstract disableImagesLoaded: bool option with get, set
    abstract updateOnEachImageLoad: bool option with get, set
    abstract onImagesLoaded: (obj option -> unit) option with get, set
    abstract options: MasonryOptions option with get, set
    abstract className: string option with get, set
    abstract elementType: string option with get, set
    abstract style: Object option with get, set
    abstract onLayoutComplete: (obj option -> unit) option with get, set
    abstract onRemoveComplete: (obj option -> unit) option with get, set
