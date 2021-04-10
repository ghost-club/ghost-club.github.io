// ts2fable 0.7.1
module rec I18next
open System
open Fable.Core
open Fable.Core.JS

type Array<'T> = System.Collections.Generic.IList<'T>
type Error = System.Exception
type TemplateStringsArray = System.Collections.Generic.IReadOnlyList<string>

let [<Import("default","i18next")>] i18next: i18n = jsNative

type [<AllowNullLiteral>] IExports =
    abstract ResourceStore: ResourceStoreStatic

type [<AllowNullLiteral>] MergeBy<'T, 'K> =
    interface end

type [<AllowNullLiteral>] FallbackLngObjList =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: language: string -> ResizeArray<string> with get, set

type FallbackLng =
    U4<string, ResizeArray<string>, FallbackLngObjList, (string -> U3<string, ResizeArray<string>, FallbackLngObjList>)>

type [<AllowNullLiteral>] FormatFunction =
    [<Emit "$0($1...)">] abstract Invoke: value: obj option * ?format: string * ?lng: string * ?options: obj -> string

type [<AllowNullLiteral>] InterpolationOptions =
    /// Format function see formatting for details
    abstract format: FormatFunction option with get, set
    /// Used to separate format from interpolation value
    abstract formatSeparator: string option with get, set
    /// Escape function
    abstract escape: str: string -> string
    /// Always format interpolated values.
    abstract alwaysFormat: bool option with get, set
    /// Escape passed in values to avoid xss injection
    abstract escapeValue: bool option with get, set
    /// If true, then value passed into escape function is not casted to string, use with custom escape function that does its own type check
    abstract useRawValueToEscape: bool option with get, set
    /// Prefix for interpolation
    abstract prefix: string option with get, set
    /// Suffix for interpolation
    abstract suffix: string option with get, set
    /// Escaped prefix for interpolation (regexSafe)
    abstract prefixEscaped: string option with get, set
    /// Escaped suffix for interpolation (regexSafe)
    abstract suffixEscaped: string option with get, set
    /// Suffix to unescaped mode
    abstract unescapeSuffix: string option with get, set
    /// Prefix to unescaped mode
    abstract unescapePrefix: string option with get, set
    /// Prefix for nesting
    abstract nestingPrefix: string option with get, set
    /// Suffix for nesting
    abstract nestingSuffix: string option with get, set
    /// Escaped prefix for nesting (regexSafe)
    abstract nestingPrefixEscaped: string option with get, set
    /// Escaped suffix for nesting (regexSafe)
    abstract nestingSuffixEscaped: string option with get, set
    /// Separates options from key
    abstract nestingOptionsSeparator: string option with get, set
    /// Global variables to use in interpolation replacements
    abstract defaultVariables: InterpolationOptionsDefaultVariables option with get, set
    /// After how many interpolation runs to break out before throwing a stack overflow
    abstract maxReplaces: float option with get, set
    /// If true, it will skip to interpolate the variables
    abstract skipOnVariables: bool option with get, set

type [<AllowNullLiteral>] ReactOptions =
    /// Set to true if you like to wait for loaded in every translated hoc
    abstract wait: bool option with get, set
    /// Set it to fallback to let passed namespaces to translated hoc act as fallbacks
    abstract nsMode: ReactOptionsNsMode option with get, set
    /// Set it to the default parent element created by the Trans component.
    abstract defaultTransParent: string option with get, set
    /// Set which events trigger a re-render, can be set to false or string of events
    abstract bindI18n: string option with get, set
    /// Set which events on store trigger a re-render, can be set to false or string of events
    abstract bindI18nStore: string option with get, set
    /// Set fallback value for Trans components without children
    abstract transEmptyNodeValue: string option with get, set
    /// Set it to false if you do not want to use Suspense
    abstract useSuspense: bool option with get, set
    /// Function to generate an i18nKey from the defaultValue (or Trans children)
    /// when no key is provided.
    /// By default, the defaultValue (Trans text) itself is used as the key.
    /// If you want to require keys for all translations, supply a function
    /// that always throws an error.
    abstract hashTransKey: defaultValue: TOptionsBase -> TOptionsBase
    /// Convert eg. <br/> found in translations to a react component of type br
    abstract transSupportBasicHtmlNodes: bool option with get, set
    /// Which nodes not to convert in defaultValue generation in the Trans component.
    abstract transKeepBasicHtmlNodesFor: ResizeArray<string> option with get, set

/// This interface can be augmented by users to add types to `i18next` default PluginOptions.
type [<AllowNullLiteral>] PluginOptions =
    interface end

type [<AllowNullLiteral>] DefaultPluginOptions =
    /// Options for language detection - check documentation of plugin
    abstract detection: obj option with get, set
    /// Options for backend - check documentation of plugin
    abstract backend: obj option with get, set
    /// Options for cache layer - check documentation of plugin
    abstract cache: obj option with get, set
    /// Options for i18n message format - check documentation of plugin
    abstract i18nFormat: obj option with get, set

type [<AllowNullLiteral>] InitOptions =
    inherit DefaultPluginOptions
    inherit PluginOptions
    /// Logs info level to console output. Helps finding issues with loading not working.
    abstract debug: bool option with get, set
    /// Resources to initialize with (if not using loading or not appending using addResourceBundle)
    abstract resources: Resource option with get, set
    /// Allow initializing with bundled resources while using a backend to load non bundled ones.
    abstract partialBundledLanguages: bool option with get, set
    /// Language to use (overrides language detection)
    abstract lng: string option with get, set
    /// Language to use if translations in user language are not available.
    abstract fallbackLng: FallbackLng option with get, set
    /// DEPRECATED use supportedLngs
    abstract whitelist: ResizeArray<string> option with get, set
    /// DEPRECTADED use nonExplicitSupportedLngs
    abstract nonExplicitWhitelist: bool option with get, set
    /// Array of allowed languages
    abstract supportedLngs: ResizeArray<string> option with get, set
    /// If true will pass eg. en-US if finding en in supportedLngs
    abstract nonExplicitSupportedLngs: bool option with get, set
    /// Language codes to lookup, given set language is
    /// 'en-US': 'all' --> ['en-US', 'en', 'dev'],
    /// 'currentOnly' --> 'en-US',
    /// 'languageOnly' --> 'en'
    abstract load: InitOptionsLoad option with get, set
    /// Array of languages to preload. Important on server-side to assert translations are loaded before rendering views.
    abstract preload: ResizeArray<string> option with get, set
    /// Language will be lowercased eg. en-US --> en-us
    abstract lowerCaseLng: bool option with get, set
    /// Language will be lowercased EN --> en while leaving full locales like en-US
    abstract cleanCode: bool option with get, set
    /// String or array of namespaces to load
    abstract ns: U2<string, ResizeArray<string>> option with get, set
    /// Default namespace used if not passed to translation function
    abstract defaultNS: string option with get, set
    /// String or array of namespaces to lookup key if not found in given namespace.
    abstract fallbackNS: U2<string, ResizeArray<string>> option with get, set
    /// Calls save missing key function on backend if key not found
    abstract saveMissing: bool option with get, set
    /// Experimental: enable to update default values using the saveMissing
    /// (Works only if defaultValue different from translated value.
    /// Only useful on initial development or when keeping code as source of truth not changing values outside of code.
    /// Only supported if backend supports it already)
    abstract updateMissing: bool option with get, set
    abstract saveMissingTo: InitOptionsSaveMissingTo option with get, set
    /// Used for custom missing key handling (needs saveMissing set to true!)
    abstract missingKeyHandler: (ResizeArray<string> -> string -> string -> string -> unit) option with get, set
    /// Receives a key that was not found in `t()` and returns a value, that will be returned by `t()`
    abstract parseMissingKeyHandler: key: string -> obj option
    /// Appends namespace to missing key
    abstract appendNamespaceToMissingKey: bool option with get, set
    /// Gets called in case a interpolation value is undefined. This method will not be called if the value is empty string or null
    abstract missingInterpolationHandler: (string -> obj option -> InitOptions -> obj option) option with get, set
    /// Will use 'plural' as suffix for languages only having 1 plural form, setting it to false will suffix all with numbers
    abstract simplifyPluralSuffix: bool option with get, set
    /// String or array of postProcessors to apply per default
    abstract postProcess: U2<string, ResizeArray<string>> option with get, set
    /// passthrough the resolved object including 'usedNS', 'usedLang' etc into options object of postprocessors as 'i18nResolved' property
    abstract postProcessPassResolved: bool option with get, set
    /// Allows null values as valid translation
    abstract returnNull: bool option with get, set
    /// Allows empty string as valid translation
    abstract returnEmptyString: bool option with get, set
    /// Allows objects as valid translation result
    abstract returnObjects: bool option with get, set
    /// Gets called if object was passed in as key but returnObjects was set to false
    abstract returnedObjectHandler: key: string * value: string * options: obj option -> unit
    /// Char, eg. '\n' that arrays will be joined by
    abstract joinArrays: string option with get, set
    /// Sets defaultValue
    abstract overloadTranslationOptionHandler: args: ResizeArray<string> -> TOptions
    abstract interpolation: InterpolationOptions option with get, set
    /// Options for react - check documentation of plugin
    abstract react: ReactOptions option with get, set
    /// Triggers resource loading in init function inside a setTimeout (default async behaviour).
    /// Set it to false if your backend loads resources sync - that way calling i18next.t after
    /// init is possible without relaying on the init callback.
    abstract initImmediate: bool option with get, set
    /// Char to separate keys
    abstract keySeparator: string option with get, set
    /// Char to split namespace from key
    abstract nsSeparator: string option with get, set
    /// Char to split plural from key
    abstract pluralSeparator: string option with get, set
    /// Char to split context from key
    abstract contextSeparator: string option with get, set
    /// Prefixes the namespace to the returned key when using `cimode`
    abstract appendNamespaceToCIMode: bool option with get, set
    /// Compatibility JSON version
    abstract compatibilityJSON: InitOptionsCompatibilityJSON option with get, set
    /// Options for https://github.com/locize/locize-editor
    abstract editor: InitOptionsEditor option with get, set
    /// Options for https://github.com/locize/locize-lastused
    abstract locizeLastUsed: InitOptionsLocizeLastUsed option with get, set
    /// Automatically lookup for a flat key if a nested key is not found an vice-versa
    abstract ignoreJSONStructure: bool option with get, set

type [<AllowNullLiteral>] TOptionsBase =
    /// Default value to return if a translation was not found
    abstract defaultValue: obj option with get, set
    /// Count value used for plurals
    abstract count: float option with get, set
    /// Used for contexts (eg. male\female)
    abstract context: obj option with get, set
    /// Object with vars for interpolation - or put them directly in options
    abstract replace: obj option with get, set
    /// Override language to use
    abstract lng: string option with get, set
    /// Override languages to use
    abstract lngs: ResizeArray<string> option with get, set
    /// Override language to lookup key if not found see fallbacks for details
    abstract fallbackLng: FallbackLng option with get, set
    /// Override namespaces (string or array)
    abstract ns: U2<string, ResizeArray<string>> option with get, set
    /// Override char to separate keys
    abstract keySeparator: string option with get, set
    /// Override char to split namespace from key
    abstract nsSeparator: string option with get, set
    /// Accessing an object not a translation string (can be set globally too)
    abstract returnObjects: bool option with get, set
    /// Char, eg. '\n' that arrays will be joined by (can be set globally too)
    abstract joinArrays: string option with get, set
    /// String or array of postProcessors to apply see interval plurals as a sample
    abstract postProcess: U2<string, ResizeArray<string>> option with get, set
    /// Override interpolation options
    abstract interpolation: InterpolationOptions option with get, set

type [<AllowNullLiteral>] StringMap =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set

type TOptions =
    TOptions<obj>

type [<AllowNullLiteral>] TOptions<'TInterpolationMap> =
    interface end

type [<AllowNullLiteral>] Callback =
    [<Emit "$0($1...)">] abstract Invoke: error: obj option * t: TFunction -> unit

type ExistsFunction<'TInterpolationMap> =
    ExistsFunction<obj, 'TInterpolationMap>

type ExistsFunction =
    ExistsFunction<obj, obj>

/// Uses similar args as the t function and returns true if a key exists.
type [<AllowNullLiteral>] ExistsFunction<'TKeys, 'TInterpolationMap> =
    [<Emit "$0($1...)">] abstract Invoke: key: U2<'TKeys, ResizeArray<'TKeys>> * ?options: TOptions<'TInterpolationMap> -> bool

type [<AllowNullLiteral>] WithT =
    abstract t: TFunction with get, set

type TFunctionResult =
    U3<string, obj, Array<U2<string, obj>>> option

type TFunctionKeys =
    U2<string, TemplateStringsArray>

type [<AllowNullLiteral>] TFunction =
    [<Emit "$0($1...)">] abstract Invoke: key: U2<'TKeys, ResizeArray<'TKeys>> * ?defaultValue: string * ?options: U2<TOptions<'TInterpolationMap>, string> -> 'TResult

type [<AllowNullLiteral>] Resource =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: language: string -> ResourceLanguage with get, set

type [<AllowNullLiteral>] ResourceLanguage =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: ``namespace``: string -> ResourceKey with get, set

type ResourceKey =
    U2<string, obj option>

type [<AllowNullLiteral>] Interpolator =
    abstract init: options: InterpolationOptions * reset: bool -> obj
    abstract reset: unit -> obj
    abstract resetRegExp: unit -> obj
    abstract interpolate: str: string * data: obj * lng: string * options: InterpolationOptions -> string
    abstract nest: str: string * fc: (ResizeArray<obj option> -> obj option) * options: InterpolationOptions -> string

type [<AllowNullLiteral>] ResourceStore =
    abstract data: Resource with get, set
    abstract options: InitOptions with get, set
    /// Gets fired when resources got added or removed
    abstract on: ``event``: ResourceStoreOnEvent * callback: (string -> string -> unit) -> unit
    /// Remove event listener
    /// removes all callback when callback not specified
    abstract off: ``event``: ResourceStoreOffEvent * ?callback: (string -> string -> unit) -> unit

type [<StringEnum>] [<RequireQualifiedAccess>] ResourceStoreOnEvent =
    | Added
    | Removed

type [<StringEnum>] [<RequireQualifiedAccess>] ResourceStoreOffEvent =
    | Added
    | Removed

type [<AllowNullLiteral>] ResourceStoreStatic =
    [<Emit "new $0($1...)">] abstract Create: data: Resource * options: InitOptions -> ResourceStore

type [<AllowNullLiteral>] Services =
    abstract backendConnector: obj option with get, set
    abstract i18nFormat: obj option with get, set
    abstract interpolator: Interpolator with get, set
    abstract languageDetector: obj option with get, set
    abstract languageUtils: obj option with get, set
    abstract logger: obj option with get, set
    abstract pluralResolver: obj option with get, set
    abstract resourceStore: ResourceStore with get, set

type [<AllowNullLiteral>] Module =
    abstract ``type``: ModuleType with get, set

type CallbackError =
    Error option

type [<AllowNullLiteral>] ReadCallback =
    [<Emit "$0($1...)">] abstract Invoke: err: CallbackError * data: U2<ResourceKey, bool> option -> unit

type [<AllowNullLiteral>] MultiReadCallback =
    [<Emit "$0($1...)">] abstract Invoke: err: CallbackError * data: Resource option -> unit

type BackendModule =
    BackendModule<obj>

/// Used to load data for i18next.
/// Can be provided as a singleton or as a prototype constructor (preferred for supporting multiple instances of i18next).
/// For singleton set property `type` to `'backend'` For a prototype constructor set static property.
type [<AllowNullLiteral>] BackendModule<'TOptions> =
    inherit Module
    abstract ``type``: string with get, set
    abstract init: services: Services * backendOptions: 'TOptions * i18nextOptions: InitOptions -> unit
    abstract read: language: string * ``namespace``: string * callback: ReadCallback -> unit
    /// Save the missing translation
    abstract create: languages: ResizeArray<string> * ``namespace``: string * key: string * fallbackValue: string -> unit
    /// Load multiple languages and namespaces. For backends supporting multiple resources loading
    abstract readMulti: languages: ResizeArray<string> * namespaces: ResizeArray<string> * callback: MultiReadCallback -> unit
    /// Store the translation. For backends acting as cache layer
    abstract save: language: string * ``namespace``: string * data: ResourceLanguage -> unit

/// Used to detect language in user land.
/// Can be provided as a singleton or as a prototype constructor (preferred for supporting multiple instances of i18next).
/// For singleton set property `type` to `'languageDetector'` For a prototype constructor set static property.
type [<AllowNullLiteral>] LanguageDetectorModule =
    inherit Module
    abstract ``type``: string with get, set
    abstract init: services: Services * detectorOptions: obj * i18nextOptions: InitOptions -> unit
    /// Must return detected language
    abstract detect: unit -> string option
    abstract cacheUserLanguage: lng: string -> unit

/// Used to detect language in user land.
/// Can be provided as a singleton or as a prototype constructor (preferred for supporting multiple instances of i18next).
/// For singleton set property `type` to `'languageDetector'` For a prototype constructor set static property.
type [<AllowNullLiteral>] LanguageDetectorAsyncModule =
    inherit Module
    abstract ``type``: string with get, set
    /// Set to true to enable async detection
    abstract async: obj with get, set
    abstract init: services: Services * detectorOptions: obj * i18nextOptions: InitOptions -> unit
    /// Must call callback passing detected language
    abstract detect: callback: (string -> unit) -> unit
    abstract cacheUserLanguage: lng: string -> unit

/// Used to extend or manipulate the translated values before returning them in `t` function.
/// Need to be a singleton object.
type [<AllowNullLiteral>] PostProcessorModule =
    inherit Module
    /// Unique name
    abstract name: string with get, set
    abstract ``type``: string with get, set
    abstract ``process``: value: string * key: string * options: TOptions * translator: obj option -> string

/// Override the built-in console logger.
/// Do not need to be a prototype function.
type [<AllowNullLiteral>] LoggerModule =
    inherit Module
    abstract ``type``: string with get, set
    abstract log: [<ParamArray>] args: ResizeArray<obj option> -> unit
    abstract warn: [<ParamArray>] args: ResizeArray<obj option> -> unit
    abstract error: [<ParamArray>] args: ResizeArray<obj option> -> unit

type [<AllowNullLiteral>] I18nFormatModule =
    inherit Module
    abstract ``type``: string with get, set

type [<AllowNullLiteral>] ThirdPartyModule =
    inherit Module
    abstract ``type``: string with get, set
    abstract init: i18next: i18n -> unit

type [<AllowNullLiteral>] Modules =
    abstract backend: BackendModule option with get, set
    abstract logger: LoggerModule option with get, set
    abstract languageDetector: U2<LanguageDetectorModule, LanguageDetectorAsyncModule> option with get, set
    abstract i18nFormat: I18nFormatModule option with get, set
    abstract ``external``: ResizeArray<ThirdPartyModule> with get, set

type [<AllowNullLiteral>] Newable<'T> =
    [<Emit "new $0($1...)">] abstract Create: [<ParamArray>] args: ResizeArray<obj option> -> obj

type [<AllowNullLiteral>] i18n =
    abstract t: TFunction with get, set
    /// <summary>The default of the i18next module is an i18next instance ready to be initialized by calling init.
    /// You can create additional instances using the createInstance function.</summary>
    /// <param name="callback">- will be called after all translations were loaded or with an error when failed (in case of using a backend).</param>
    abstract init: ?callback: Callback -> Promise<TFunction>
    abstract init: options: InitOptions * ?callback: Callback -> Promise<TFunction>
    abstract loadResources: ?callback: (obj option -> unit) -> unit
    /// The use function is there to load additional plugins to i18next.
    /// For available module see the plugins page and don't forget to read the documentation of the plugin.
    ///
    /// Accepts a class or object
    abstract ``use``: ``module``: U4<'T, Newable<'T>, ResizeArray<ThirdPartyModule>, ResizeArray<Newable<ThirdPartyModule>>> -> i18n
    /// List of modules used
    abstract modules: Modules with get, set
    /// Internal container for all used plugins and implementation details like languageUtils, pluralResolvers, etc.
    abstract services: Services with get, set
    /// Internal container for translation resources
    abstract store: ResourceStore with get, set
    /// Uses similar args as the t function and returns true if a key exists.
    abstract exists: ExistsFunction with get, set
    /// Returns a resource data by language.
    abstract getDataByLanguage: lng: string -> I18nGetDataByLanguage option
    /// Returns a t function that defaults to given language or namespace.
    /// Both params could be arrays of languages or namespaces and will be treated as fallbacks in that case.
    /// On the returned function you can like in the t function override the languages or namespaces by passing them in options or by prepending namespace.
    abstract getFixedT: lng: U2<string, ResizeArray<string>> * ?ns: U2<string, ResizeArray<string>> -> TFunction
    abstract getFixedT: lng: obj * ns: U2<string, ResizeArray<string>> -> TFunction
    /// Changes the language. The callback will be called as soon translations were loaded or an error occurs while loading.
    /// HINT: For easy testing - setting lng to 'cimode' will set t function to always return the key.
    abstract changeLanguage: ?lng: string * ?callback: Callback -> Promise<TFunction>
    /// Is set to the current detected or set language.
    /// If you need the primary used language depending on your configuration (supportedLngs, load) you will prefer using i18next.languages[0].
    abstract language: string with get, set
    /// Is set to an array of language-codes that will be used it order to lookup the translation value.
    abstract languages: ResizeArray<string> with get, set
    /// Loads additional namespaces not defined in init options.
    abstract loadNamespaces: ns: U2<string, ResizeArray<string>> * ?callback: Callback -> Promise<unit>
    /// Loads additional languages not defined in init options (preload).
    abstract loadLanguages: lngs: U2<string, ResizeArray<string>> * ?callback: Callback -> Promise<unit>
    /// Reloads resources on given state. Optionally you can pass an array of languages and namespaces as params if you don't want to reload all.
    abstract reloadResources: ?lngs: U2<string, ResizeArray<string>> * ?ns: U2<string, ResizeArray<string>> * ?callback: (unit -> unit) -> Promise<unit>
    abstract reloadResources: lngs: obj * ns: U2<string, ResizeArray<string>> * ?callback: (unit -> unit) -> Promise<unit>
    /// Changes the default namespace.
    abstract setDefaultNamespace: ns: string -> unit
    /// Returns rtl or ltr depending on languages read direction.
    abstract dir: ?lng: string -> I18nDirReturn
    /// Exposes interpolation.format function added on init.
    abstract format: FormatFunction with get, set
    /// Will return a new i18next instance.
    /// Please read the options page for details on configuration options.
    /// Providing a callback will automatically call init.
    /// The callback will be called after all translations were loaded or with an error when failed (in case of using a backend).
    abstract createInstance: ?options: InitOptions * ?callback: Callback -> i18n
    /// Creates a clone of the current instance. Shares store, plugins and initial configuration.
    /// Can be used to create an instance sharing storage but being independent on set language or namespaces.
    abstract cloneInstance: ?options: InitOptions * ?callback: Callback -> i18n
    /// Gets fired after initialization.
    [<Emit "$0.on('initialized',$1)">] abstract on_initialized: callback: (InitOptions -> unit) -> unit
    /// Gets fired on loaded resources.
    [<Emit "$0.on('loaded',$1)">] abstract on_loaded: callback: (bool -> unit) -> unit
    /// Gets fired if loading resources failed.
    [<Emit "$0.on('failedLoading',$1)">] abstract on_failedLoading: callback: (string -> string -> string -> unit) -> unit
    /// Gets fired on accessing a key not existing.
    [<Emit "$0.on('missingKey',$1)">] abstract on_missingKey: callback: (ResizeArray<string> -> string -> string -> string -> unit) -> unit
    /// Gets fired when resources got added or removed.
    abstract on: ``event``: I18nOnEvent * callback: (string -> string -> unit) -> unit
    /// Gets fired when changeLanguage got called.
    [<Emit "$0.on('languageChanged',$1)">] abstract on_languageChanged: callback: (string -> unit) -> unit
    /// Event listener
    abstract on: ``event``: string * listener: (ResizeArray<obj option> -> unit) -> unit
    /// Remove event listener
    /// removes all callback when callback not specified
    abstract off: ``event``: string * ?listener: (ResizeArray<obj option> -> unit) -> unit
    /// Gets one value by given key.
    abstract getResource: lng: string * ns: string * key: string * ?options: obj -> obj option
    /// Adds one key/value.
    abstract addResource: lng: string * ns: string * key: string * value: string * ?options: I18nAddResourceOptions -> i18n
    /// Adds multiple key/values.
    abstract addResources: lng: string * ns: string * resources: obj option -> i18n
    /// Adds a complete bundle.
    /// Setting deep param to true will extend existing translations in that file.
    /// Setting overwrite to true it will overwrite existing translations in that file.
    abstract addResourceBundle: lng: string * ns: string * resources: obj option * ?deep: bool * ?overwrite: bool -> i18n
    /// Checks if a resource bundle exists.
    abstract hasResourceBundle: lng: string * ns: string -> bool
    /// Returns a resource bundle.
    abstract getResourceBundle: lng: string * ns: string -> obj option
    /// Removes an existing bundle.
    abstract removeResourceBundle: lng: string * ns: string -> i18n
    /// Current options
    abstract options: InitOptions with get, set
    /// Is initialized
    abstract isInitialized: bool with get, set
    /// Emit event
    abstract emit: eventName: string -> unit

type [<StringEnum>] [<RequireQualifiedAccess>] I18nDirReturn =
    | Ltr
    | Rtl

type [<StringEnum>] [<RequireQualifiedAccess>] I18nOnEvent =
    | Added
    | Removed

type [<AllowNullLiteral>] I18nAddResourceOptions =
    abstract keySeparator: string option with get, set
    abstract silent: bool option with get, set

type [<AllowNullLiteral>] InterpolationOptionsDefaultVariables =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: index: string -> obj option with get, set

type [<StringEnum>] [<RequireQualifiedAccess>] ReactOptionsNsMode =
    | Default
    | Fallback

type [<StringEnum>] [<RequireQualifiedAccess>] InitOptionsLoad =
    | All
    | CurrentOnly
    | LanguageOnly

type [<StringEnum>] [<RequireQualifiedAccess>] InitOptionsSaveMissingTo =
    | Current
    | All
    | Fallback

type [<StringEnum>] [<RequireQualifiedAccess>] InitOptionsCompatibilityJSON =
    | V1
    | V2
    | V3

type [<StringEnum>] [<RequireQualifiedAccess>] InitOptionsEditorToggleKeyModifier =
    | CtrlKey
    | MetaKey
    | AltKey
    | ShiftKey

type [<StringEnum>] [<RequireQualifiedAccess>] InitOptionsEditorMode =
    | Iframe
    | Window

type [<AllowNullLiteral>] InitOptionsEditor =
    /// Enable on init without the need of adding querystring locize=true
    abstract enabled: bool option with get, set
    /// If set to false you will need to open the editor via API
    abstract autoOpen: bool option with get, set
    /// Enable by adding querystring locize=true; can be set to another value or turned off by setting to false
    abstract enableByQS: string option with get, set
    /// Turn on/off by pressing key combination. Combine this with `toggleKeyCode`
    abstract toggleKeyModifier: InitOptionsEditorToggleKeyModifier option with get, set
    /// Turn on/off by pressing key combination. Combine this with `toggleKeyModifier`
    abstract toggleKeyCode: float option with get, set
    /// Use lng in editor taken from query string, eg. if running with lng=cimode (i18next, locize)
    abstract lngOverrideQS: string option with get, set
    /// Use lng in editor, eg. if running with lng=cimode (i18next, locize)
    abstract lngOverride: string option with get, set
    /// How the editor will open.
    /// Setting to window will open a new window/tab instead
    abstract mode: InitOptionsEditorMode option with get, set
    /// Styles to adapt layout in iframe mode to your website layout.
    /// This will add a style to the `<iframe>`
    abstract iframeContainerStyle: string option with get, set
    /// Styles to adapt layout in iframe mode to your website layout.
    /// This will add a style to the parent of `<iframe>`
    abstract iframeStyle: string option with get, set
    /// Styles to adapt layout in iframe mode to your website layout.
    /// This will add a style to `<body>`
    abstract bodyStyle: string option with get, set
    /// Handle when locize saved the edited translations, eg. reload website
    abstract onEditorSaved: (obj -> U2<string, ResizeArray<string>> -> unit) option with get, set

type [<AllowNullLiteral>] InitOptionsLocizeLastUsed =
    /// The id of your locize project
    abstract projectId: string with get, set
    /// An api key if you want to send missing keys
    abstract apiKey: string option with get, set
    /// The reference language of your project
    abstract referenceLng: string option with get, set
    /// Version
    abstract version: string option with get, set
    /// Debounce interval to send data in milliseconds
    abstract debounceSubmit: float option with get, set
    /// Hostnames that are allowed to send last used data.
    /// Please keep those to your local system, staging, test servers (not production)
    abstract allowedHosts: ResizeArray<string> option with get, set

type [<StringEnum>] [<RequireQualifiedAccess>] ModuleType =
    | Backend
    | Logger
    | LanguageDetector
    | PostProcessor
    | I18nFormat
    | [<CompiledName "3rdParty">] N3rdParty

type [<AllowNullLiteral>] I18nGetDataByLanguageTranslation =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string with get, set

type [<AllowNullLiteral>] I18nGetDataByLanguage =
    abstract translation: I18nGetDataByLanguageTranslation with get, set
