module View.Footer

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open Fable.Core
open Fable.Core.JsInterop
open Properties
open Model

let [<Literal>] __FILE__ = __SOURCE_FILE__

let view =
  div [Class "footer"; Id "footer";  Key "footer"] [
    span [Key.Src(__FILE__,__LINE__); Class "footer-copyright-text"] [
      str "Copyright\u00A0©\u00A0GHOSTCLUB All\u00A0Rights\u00A0Reserved."
    ]
  ]