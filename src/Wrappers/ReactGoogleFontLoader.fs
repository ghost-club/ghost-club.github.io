// ts2fable 0.7.1
module rec ReactGoogleFontLoader
open System
open Fable
open Fable.Core
open Fable.Core.JS

let [<Import("GoogleFontLoader","react-google-font-loader")>] GoogleFontLoader: React.FunctionComponent'<GoogleFontLoaderProps> = jsNative

type [<AllowNullLiteral>] Font =
    abstract font: string with get, set
    abstract weights: ResizeArray<U2<string, float>> option with get, set

type [<AllowNullLiteral>] GoogleFontLoaderProps =
    abstract fonts: ResizeArray<Font> with get, set
    abstract subsets: ResizeArray<string> option with get, set
    abstract display: GoogleFontLoaderPropsDisplay option with get, set

type [<StringEnum>] [<RequireQualifiedAccess>] GoogleFontLoaderPropsDisplay =
    | Auto
    | Block
    | Swap
    | Fallback
    | Optional
