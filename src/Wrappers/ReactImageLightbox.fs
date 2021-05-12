module ReactImageLightbox
open Fable.Core
open Fable.Core.JsInterop

open Browser.Types
open Fable.React

open System.ComponentModel

type Prop =
  | MainSrc of string
  | PrevSrc of string
  | NextSrc of string
  | MainSrcThumbnail of string
  | PrevSrcThumbnail of string
  | NextSrcThumbnail of string
  | OnCloseRequest of (unit -> unit)
  | OnMovePrevRequest of (unit -> unit)
  | OnMoveNextRequest of (unit -> unit)
  | OnImageLoad of (unit -> unit)
  | OnImageLoadError of (unit -> unit)
  | ImageLoadErrorMessage of ReactElement
  | OnAfterOpen of (unit -> unit)
  | DiscourageDownloads of bool
  | AnimationDisabled of bool
  | AnimationOnKeyInput of bool
  | AnimationDuration of float
  | KeyRepeatLimit of float
  | KeyRepeatKeyupBonus of float
  | ImageTitle of ReactElement
  | [<CompiledName("imageTitle")>] ImageTitleStr of string
  | ImageCaption of ReactElement
  | [<CompiledName("imageCaption")>] ImageCaptionStr of string
  | ImageCrossOrigin of string
  | ToolbarButtons of ReactElement list
  | ImagePadding of float
  | ClickOutsideToClose of bool
  | EnableZoom of bool
  | WrapperClassName of string
  | NextLabel of string
  | PrevLabel of string
  | ZoomInLabel of string
  | ZoomOutLabel of string
  | CloseLabel of string
  | [<EditorBrowsable(EditorBrowsableState.Never)>] ReactModalStyle of obj
  | [<EditorBrowsable(EditorBrowsableState.Never)>] ReactModalProps of obj
  | [<Erase; EditorBrowsable(EditorBrowsableState.Never)>] WrappedProp of obj
  static member inline op_ErasedCast (x: Props.IHTMLProp) : Prop = WrappedProp x

let inline ReactModalStyle (styles: Props.CSSProp list) : Prop =
  ReactModalStyle (keyValueList CaseRules.LowerFirst styles)

let inline ReactModalProps (props: ReactModal.Prop list) : Prop =
  ReactModalProps (keyValueList CaseRules.LowerFirst props)

let inline lightbox (props: Prop list) =
  ofImport "default" "react-image-lightbox" (keyValueList CaseRules.LowerFirst props) []
