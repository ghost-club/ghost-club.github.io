// ts2fable 0.7.1
module rec ReactSlick
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types

module JSX =
    type Element = React.ReactElement

type [<AllowNullLiteral>] IExports =
    abstract Slider: SliderStatic

type ComponentConstructor<'TProps> =
    U2<React.ComponentClass<'TProps>, React.StatelessComponent<'TProps>>

type [<AllowNullLiteral>] CustomArrowProps =
    abstract className: string option with get, set
    abstract style: React.CSSProperties option with get, set
    abstract onClick: React.MouseEventHandler<EventTarget> option with get, set
    abstract currentSlide: float option with get, set
    abstract slideCount: float option with get, set

type [<AllowNullLiteral>] ResponsiveObject =
    abstract breakpoint: float with get, set
    abstract settings: U2<Settings, string> with get, set

type SwipeDirection =
    U2<string, string>

type [<StringEnum>] [<RequireQualifiedAccess>] LazyLoadTypes =
    | Ondemand
    | Progressive

type [<AllowNullLiteral>] Settings =
    abstract accessibility: bool option with get, set
    abstract adaptiveHeight: bool option with get, set
    abstract afterChange: currentSlide: float -> unit
    abstract appendDots: dots: React.ReactNode -> JSX.Element
    abstract arrows: bool option with get, set
    abstract asNavFor: Slider option with get, set
    abstract autoplaySpeed: float option with get, set
    abstract autoplay: bool option with get, set
    abstract beforeChange: currentSlide: float * nextSlide: float -> unit
    abstract centerMode: bool option with get, set
    abstract centerPadding: string option with get, set
    abstract className: string option with get, set
    abstract cssEase: string option with get, set
    abstract customPaging: index: float -> JSX.Element
    abstract dotsClass: string option with get, set
    abstract dots: bool option with get, set
    abstract draggable: bool option with get, set
    abstract easing: string option with get, set
    abstract edgeFriction: float option with get, set
    abstract fade: bool option with get, set
    abstract focusOnSelect: bool option with get, set
    abstract infinite: bool option with get, set
    abstract initialSlide: float option with get, set
    abstract lazyLoad: LazyLoadTypes option with get, set
    abstract nextArrow: JSX.Element option with get, set
    abstract onEdge: swipeDirection: SwipeDirection -> unit
    abstract onInit: unit -> unit
    abstract onLazyLoad: slidesToLoad: ResizeArray<float> -> unit
    abstract onReInit: unit -> unit
    abstract onSwipe: swipeDirection: SwipeDirection -> unit
    abstract pauseOnDotsHover: bool option with get, set
    abstract pauseOnFocus: bool option with get, set
    abstract pauseOnHover: bool option with get, set
    abstract prevArrow: JSX.Element option with get, set
    abstract responsive: ResizeArray<ResponsiveObject> option with get, set
    abstract rows: float option with get, set
    abstract rtl: bool option with get, set
    abstract slide: string option with get, set
    abstract slidesPerRow: float option with get, set
    abstract slidesToScroll: float option with get, set
    abstract slidesToShow: float option with get, set
    abstract speed: float option with get, set
    abstract swipeToSlide: bool option with get, set
    abstract swipe: bool option with get, set
    abstract swipeEvent: swipeDirection: SwipeDirection -> unit
    abstract touchMove: bool option with get, set
    abstract touchThreshold: float option with get, set
    abstract useCSS: bool option with get, set
    abstract useTransform: bool option with get, set
    abstract variableWidth: bool option with get, set
    abstract vertical: bool option with get, set
    abstract verticalSwiping: bool option with get, set
    abstract waitForAnimate: bool option with get, set
    abstract key: string with get, set
    abstract ref: React.Ref<Slider> with get, set

and [<AllowNullLiteralAttribute>] Slider =
    abstract slickNext: unit -> unit
    abstract slickPause: unit -> unit
    abstract slickPlay: unit -> unit
    abstract slickPrev: unit -> unit
    abstract slickGoTo: slideNumber: int * ?dontAnimate: bool -> unit

type [<AllowNullLiteral>] SliderStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> Slider
