// ts2fable 0.7.1
module rec ReactI18nextBrowserLanguageDetector
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types

type Array<'T> = System.Collections.Generic.IList<'T>


type [<AllowNullLiteral>] IExports =
    abstract I18nextBrowserLanguageDetector: I18nextBrowserLanguageDetectorStatic

type [<AllowNullLiteral>] DetectorOptions =
    /// order and from where user language should be detected
    abstract order: Array<U2<string, string>> option with get, set
    /// keys or params to lookup language from
    abstract lookupQuerystring: string option with get, set
    abstract lookupCookie: string option with get, set
    abstract lookupSessionStorage: string option with get, set
    abstract lookupLocalStorage: string option with get, set
    abstract lookupFromPathIndex: float option with get, set
    abstract lookupFromSubdomainIndex: float option with get, set
    /// cache user language on
    abstract caches: ResizeArray<string> option with get, set
    /// languages to not persist (cookie, localStorage)
    abstract excludeCacheFor: ResizeArray<string> option with get, set
    /// optional expire and domain for set cookie
    abstract cookieMinutes: float option with get, set
    abstract cookieDomain: string option with get, set
    /// optional htmlTag with lang attribute
    abstract htmlTag: HTMLElement option with get, set

type [<AllowNullLiteral>] CustomDetector =
    abstract name: string with get, set
    abstract cacheUserLanguage: lng: string * options: DetectorOptions -> unit
    abstract lookup: options: DetectorOptions -> string option

type [<AllowNullLiteral>] I18nextBrowserLanguageDetector =
    inherit I18next.LanguageDetectorModule
    /// Adds detector.
    abstract addDetector: detector: CustomDetector -> I18nextBrowserLanguageDetector
    /// Initializes detector.
    abstract init: ?services: obj * ?options: DetectorOptions -> unit
    abstract detect: ?detectionOrder: DetectorOptions -> string option
    abstract cacheUserLanguage: lng: string * ?caches: ResizeArray<string> -> unit
    abstract ``type``: string with get, set
    abstract detectors: I18nextBrowserLanguageDetectorDetectors with get, set
    abstract services: obj option with get, set
    abstract i18nOptions: obj option with get, set

type [<AllowNullLiteral>] I18nextBrowserLanguageDetectorStatic =
    [<Emit "new $0($1...)">] abstract Create: ?services: obj * ?options: DetectorOptions -> I18nextBrowserLanguageDetector

module I18next =

    type [<AllowNullLiteral>] PluginOptions =
        abstract detection: DetectorOptions option with get, set

type [<AllowNullLiteral>] I18nextBrowserLanguageDetectorDetectors =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set
