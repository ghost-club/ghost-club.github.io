module View.Menu

open Elmish
open Fable.React
open Fable.React.Props
open Fable.Core
open Fable.Core.JsInterop
open Properties
open Model
open View
open Fulma
open Wrappers.Rewrapped

let [<Literal>] __FILE__ = __SOURCE_FILE__

let viewPC (model: Model) dispatch =
  let langSwitchText = !@UITexts.ChangeToAnotherLanguage
  div [
    Class "menu"
    Style [
      Position (
        if model.flags |> Set.contains MenuIsSticky then PositionOptions.Fixed
        else PositionOptions.Absolute
      )
    ]
    Key.Src(__FILE__,__LINE__)] [
    Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
      p [Key "hello-world"] [str !@"Hello, world!"]
      p [Key "album-state"] [str (sprintf "album: %s" model.albumState.AsString)]
    ]
    Block.block [Props [Key.Src(__FILE__, __LINE__)]] [
      Button.button [
        Props [Key.Src(__FILE__, __LINE__)]
        Button.OnClick (fun _ -> dispatch (SwitchLanguage (Language.Flip model.lang))) ] [
          str langSwitchText
      ]
    ]
  ]
