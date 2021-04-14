module View.Misc

open Model
open Fable
open Fable.Core
open Fable.Core.JsInterop

open ReactGoogleFontLoader

let inline private font (font: string) (weights: U2<string, float> list) : Font =
  jsOptions (fun (it: Font) ->
    it.font <- font
    it.weights <- Some (ResizeArray(weights))
  )

let inline private googleFontLoader props =
  React.FunctionComponent.Of(
    GoogleFontLoader,
    withKey=(fun _ -> "google-font-loader")
  ) props

let viewGoogleFontLoader (model: Model) (dispatch: Msg -> unit) =
  googleFontLoader
    (jsOptions (fun (it: GoogleFontLoaderProps) ->
      it.fonts <- ResizeArray([|
        font "Roboto" [ !^400.0; !^"400i" ]
      |])
    ))
