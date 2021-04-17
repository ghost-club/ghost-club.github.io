module View.Misc

open Model
open Fable
open Fable.Core
open Fable.Core.JsInterop

open ReactGoogleFontLoader

let inline font (font: string) (weights: U2<string, float> list) : Font =
  jsOptions (fun (it: Font) ->
    it.font <- font
    it.weights <- Some (ResizeArray(weights))
  )

let inline googleFontLoader props =
  React.FunctionComponent.Of(
    GoogleFontLoader,
    withKey=(fun _ -> "google-font-loader")
  ) props

let viewGoogleFontLoader (model: Model) (dispatch: Msg -> unit) =
  googleFontLoader
    (jsOptions (fun (it: GoogleFontLoaderProps) ->
      it.fonts <- ResizeArray([|
        font "IBM Plex Serif" [!^"400"; !^"600"]
        font "Noto Serif JP" [!^"400"; !^"600"]
      |])
    ))

// <iframe width="100%" height="120" src="https://www.mixcloud.com/widget/iframe/?hide_cover=1&feed=%2Fcannorin%2F20210402-gc-birthday-mix%2F" frameborder="0" ></iframe>
// <iframe width="100%" height="60" src="https://www.mixcloud.com/widget/iframe/?hide_cover=1&mini=1&feed=%2Fcannorin%2F20210402-gc-birthday-mix%2F" frameborder="0" ></iframe>
// <iframe width="100%" height="120" src="https://www.mixcloud.com/widget/iframe/?hide_cover=1&hide_artwork=1&feed=%2Fcannorin%2F20210402-gc-birthday-mix%2F" frameborder="0" ></iframe>
// <iframe width="100%" height="400" src="https://www.mixcloud.com/widget/iframe/?feed=%2Fcannorin%2F20210402-gc-birthday-mix%2F" frameborder="0" ></iframe>
open Fable.React
open Fable.React.Props
