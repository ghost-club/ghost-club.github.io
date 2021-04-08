// ts2fable 0.7.1
module rec ReactI18next
open System
open Fable
open Fable.Core
open Fable.Core.JS

type ReactOptions = I18next.ReactOptions
type i18n = I18next.i18n
type ThirdPartyModule = I18next.ThirdPartyModule
type WithT = I18next.WithT
type TFunction = I18next.TFunction
type Resource = I18next.Resource

let [<Import("initReactI18next","react-i18next")>] initReactI18next: ThirdPartyModule = jsNative
let [<Import("I18nextProvider","react-i18next")>] I18nextProvider: React.FunctionComponent'<I18nextProviderProps> = jsNative
let [<Import("I18nContext","react-i18next")>] I18nContext: React.IContext<I18nContextReactContext> = jsNative

type [<AllowNullLiteral>] IExports =
    abstract setDefaults: options: ReactOptions -> unit
    abstract getDefaults: unit -> ReactOptions
    abstract setI18n: instance: i18n -> unit
    abstract getI18n: unit -> i18n
    abstract composeInitialProps: ForComponent: obj option -> (obj -> Promise<obj option>)
    abstract getInitialProps: unit -> GetInitialPropsReturn
    abstract Trans: props: TransProps<'E> -> React.ReactElement
    abstract useSSR: initialI18nStore: Resource * initialLanguage: string -> unit
    abstract useTranslation: ?ns: Namespace * ?options: UseTranslationOptions -> UseTranslationResponse
    abstract withSSR: unit -> (React.ComponentType<'Props> -> IExportsWithSSR<'Props>)
    abstract withTranslation: ?ns: Namespace * ?options: WithTranslationOptions -> ('C -> React.ComponentType<obj>)
    abstract Translation: props: TranslationProps -> obj option

type [<AllowNullLiteral>] GetInitialPropsReturn =
    abstract initialI18nStore: GetInitialPropsReturnInitialI18nStore with get, set
    abstract initialLanguage: string with get, set

type [<AllowNullLiteral>] WithTranslationOptions =
    abstract withRef: bool option with get, set

type Namespace =
    U2<string, ResizeArray<string>>

type [<AllowNullLiteral>] ReportNamespaces =
    abstract addUsedNamespaces: namespaces: ResizeArray<Namespace> -> unit
    abstract getUsedNamespaces: unit -> ResizeArray<string>

module I18next =

    type [<AllowNullLiteral>] i18n =
        abstract reportNamespaces: ReportNamespaces with get, set

type TransProps =
    TransProps<obj>

type [<AllowNullLiteral>] TransProps<'E> =
    inherit React.HTMLProps<'E>
    abstract children: React.ReactNode option with get, set
    abstract components: U2<ResizeArray<React.ReactNode>, TransPropsComponents> option with get, set
    abstract count: float option with get, set
    abstract defaults: string option with get, set
    abstract i18n: i18n option with get, set
    abstract i18nKey: string option with get, set
    abstract ns: Namespace option with get, set
    abstract parent: U2<string, React.ComponentType<obj option>> option with get, set
    abstract tOptions: GetInitialPropsReturnInitialI18nStoreItem option with get, set
    abstract values: GetInitialPropsReturnInitialI18nStoreItem option with get, set
    abstract t: TFunction option with get, set

type [<AllowNullLiteral>] UseTranslationOptions =
    abstract i18n: i18n option with get, set
    abstract useSuspense: bool option with get, set

type [<AllowNullLiteral>] UseTranslationResponse =
    interface end

type [<AllowNullLiteral>] WithTranslation =
    inherit WithT
    abstract i18n: i18n with get, set
    abstract tReady: bool with get, set

type [<AllowNullLiteral>] WithTranslationProps =
    abstract i18n: i18n option with get, set
    abstract useSuspense: bool option with get, set

type [<AllowNullLiteral>] I18nextProviderProps =
    abstract i18n: i18n with get, set
    abstract defaultNS: string option with get, set

type [<AllowNullLiteral>] TranslationProps =
    abstract children: (TFunction -> TranslationPropsChildren -> bool -> React.ReactNode) with get, set
    abstract ns: Namespace option with get, set
    abstract i18n: i18n option with get, set
    abstract useSuspense: bool option with get, set

type [<AllowNullLiteral>] I18nContextReactContext =
    abstract i18n: i18n with get, set

type [<AllowNullLiteral>] IExportsWithSSR<'Props> =
    [<Emit "$0($1...)">] abstract Invoke: p0: obj -> React.FunctionComponentElement<'Props>
    abstract getInitialProps: (obj -> Promise<obj option>) with get, set

type [<AllowNullLiteral>] GetInitialPropsReturnInitialI18nStoreItem =
    interface end

type [<AllowNullLiteral>] GetInitialPropsReturnInitialI18nStore =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: ns: string -> GetInitialPropsReturnInitialI18nStoreItem with get, set

type [<AllowNullLiteral>] TransPropsComponents =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: tagName: string -> React.ReactNode

type [<AllowNullLiteral>] TranslationPropsChildren =
    abstract i18n: i18n with get, set
    abstract lng: string with get, set
