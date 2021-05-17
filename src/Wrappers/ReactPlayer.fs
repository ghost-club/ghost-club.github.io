module ReactPlayer

open System
open Fable.Core
open Fable.Core.JsInterop
open Browser.Types
open Fable.React
open System.ComponentModel

type [<StringEnum; RequireQualifiedAccess>] SeekType = Seconds | Fraction

type ReactPlayer =
  abstract seekTo: amount:float * ?``type``:SeekType -> unit
  abstract getCurrentTime: unit -> float
  abstract getSecondsLoaded: unit -> float
  abstract getDuration: unit -> float
  abstract getInternalPlayer: ?key:string -> obj
  abstract showPreview: unit -> unit

type IPlatformSpecificReactPlayerProp<'PlatformConfig> = interface end

type [<StringEnum; RequireQualifiedAccess>] YouTubeColor = Red | White

type [<RequireQualifiedAccess>] YouTubePlayerVar =
  | Start of int | End of int
  | [<CompiledName("cc_lang_pref")>] CcLangPref of lang:string
  | Color of YouTubeColor
  | [<CompiledName("hl")>] Lang of lang:string
  | [<CompiledName("widget_referrer")>] WidgetReferrer of domain:string
  | [<Erase; EditorBrowsable(EditorBrowsableState.Never)>] Flag of string * int
  static member inline CcShownByDefault flag = Flag ("cc_load_policy", if flag then 1 else 0)
  static member inline DisableKeyboard flag = Flag ("disablekb", if flag then 1 else 0)
  static member inline AnnotationShownByDefault flag = Flag ("iv_load_policy", if flag then 1 else 3)
  static member inline ShowFullScreenButton flag = Flag ("fs", if flag then 1 else 0)
  static member inline ModestBranding flag = Flag ("modestbranding", if flag then 1 else 0)
  static member inline ShowRelated flag = Flag ("rel", if flag then 1 else 0)

type YouTubeConfig =
  | EmbedOptions of obj
  | OnUnstarted of (unit -> unit)
  | [<Erase; EditorBrowsable(EditorBrowsableState.Never)>] ErasedKVP of string * obj
  static member inline PlayerVars (vars: YouTubePlayerVar list) =
    ErasedKVP ("playerVars", keyValueList CaseRules.LowerFirst vars)
  [<EditorBrowsable(EditorBrowsableState.Never)>]
  static member inline Player (props: YouTubeReactPlayerProp list) =
    ofImport "default" "react-player/youtube" (keyValueList CaseRules.LowerFirst props) []
and YouTubeReactPlayerProp = IPlatformSpecificReactPlayerProp<YouTubeConfig>
let inline PlayerVars vars = YouTubeConfig.PlayerVars vars

type [<RequireQualifiedAccess; StringEnum>] VimeoQuality =
  | Auto
  | [<CompiledName("360p")>] Q_360p
  | [<CompiledName("540p")>] Q_540p
  | [<CompiledName("720p")>] Q_720p
  | [<CompiledName("1080p")>] Q_1080p
  | [<CompiledName("2k")>] Q_2k
  | [<CompiledName("4k")>] Q_4k

type [<RequireQualifiedAccess>] VimeoOption =
  | Autopause of bool
  | Background of bool
  | Byline of bool
  | Color of hex:string
  | Dnt of bool
  | Portrait of bool
  | Quality of VimeoQuality
  | Responsive of bool
  | Speed of bool
  | TextTrack of lang:string
  | Title of bool
  | Transparent of bool

type VimeoConfig =
  | [<Erase; EditorBrowsable(EditorBrowsableState.Never)>] ErasedKVP of string * obj
  static member inline PlayerOptions (options: VimeoOption list) =
    ErasedKVP ("playerOptions", keyValueList CaseRules.LowerFirst options)
  [<EditorBrowsable(EditorBrowsableState.Never)>]
  static member inline Player (props: VimeoReactPlayerProp list) =
    ofImport "default" "react-player/vimeo" (keyValueList CaseRules.LowerFirst props) []
and VimeoReactPlayerProp = IPlatformSpecificReactPlayerProp<VimeoConfig>
let inline PlayerOptions options = VimeoConfig.PlayerOptions options

type Config =
  | [<Erase; EditorBrowsable(EditorBrowsableState.Never)>] ErasedKVP of string * obj
  static member inline YouTube (config: YouTubeConfig list) =
    ErasedKVP ("youtube", keyValueList CaseRules.LowerFirst config)
  static member inline Vimeo (config: VimeoOption list) =
    ErasedKVP ("vimeo", keyValueList CaseRules.LowerFirst config)
  static member inline CustomPlayer (key: string, config: 'a list) = ErasedKVP (key, keyValueList CaseRules.LowerFirst config)
  [<EditorBrowsable(EditorBrowsableState.Never)>]
  static member inline Player (props: BaseReactPlayerProp list) =
    ofImport "default" "react-player" (keyValueList CaseRules.LowerFirst props) []
and BaseReactPlayerProp = IPlatformSpecificReactPlayerProp<Config>

[<RequireQualifiedAccess>]
module ReactPlayerProp =
  type Common<'a> =
    | Url of string
    | [<CompiledName("url")>] UrlProps of {| src: string; ``type``: string |}[]
    | [<CompiledName("url")>] UrlArray of string[]
    | [<CompiledName("url")>] UrlStream of MediaStream
    | Playing of bool
    | Loop of bool
    | Controls of bool
    | Light of bool
    | [<CompiledName("light")>] LightThumbnail of url:string
    | Volume of float
    | Muted of bool
    | PlaybackRate of float
    | ProgressInterval of ms:int
    | Playsinline of bool
    | Pip of bool
    | StopOnUnmount of bool
    | Fallback of ReactElement
    | Wrapper of string
    | PlayIcon of obj
    | PreviewTabIndex of int
    | OnReady of (ReactPlayer -> unit)
    | OnStart of (unit -> unit)
    | OnPlay of (unit -> unit)
    | OnPause of (unit -> unit)
    | OnBuffer of (unit -> unit)
    | OnBufferEnd of (unit -> unit)
    | OnEnded of (unit -> unit)
    | OnClickPreview of (obj -> unit)
    | OnEnablePIP of (unit -> unit)
    | OnDisablePIP of (unit -> unit)
    | OnError of (obj -> unit)
    | OnDuration of (float -> unit)
    | OnSeek of (float -> unit)
    | OnProgress of ({| played: float; playedSeconds: float; loaded: float; loadedSeconds: float |} -> unit)
    | Key of string
    | Ref of (Element -> unit)
    | RefValue of IRefValue<Element option>
    | [<Erase; EditorBrowsable(EditorBrowsableState.Never)>] Custom of string * obj
    interface IPlatformSpecificReactPlayerProp<'a>

  let inline Width (width: obj) = Custom ("width", width)
  let inline Height (height: obj) = Custom ("height", height)
  let inline Style (cssProps: Props.CSSProp list) = Custom ("style", keyValueList CaseRules.LowerFirst cssProps)

  let inline Config (config: 'PlatformConfig list) : IPlatformSpecificReactPlayerProp<'PlatformConfig> =
    Custom ("config", keyValueList CaseRules.LowerFirst config) |> box :?> _

module ReactPlayer =
  let inline optimized (props: IPlatformSpecificReactPlayerProp< ^Platform > list) : ReactElement =
    (^Platform: (static member Player: _ -> _) props)

  let inline player (props: BaseReactPlayerProp list) = Config.Player props

  let inline lazy' (props: BaseReactPlayerProp list) =
    ofImport "default" "react-player/lazy" (keyValueList CaseRules.LowerFirst props) []

  let inline youtube (props: YouTubeReactPlayerProp list) = YouTubeConfig.Player props

  let inline vimeo (props: VimeoReactPlayerProp list) = VimeoConfig.Player props
