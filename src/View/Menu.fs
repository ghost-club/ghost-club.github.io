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

let view_ (model: Model) dispatch =
  let sticky = model.flags |> Set.contains MenuIsVisible
  let langSwitchText = !@UITexts.ChangeToAnotherLanguage
  div [
    Class (if sticky then "menu-desktop menu-desktop-sticky" else "menu-desktop")
    Key.Src(__FILE__,__LINE__)] [
    div [
      Class "menu"
      Key.Src(__FILE__,__LINE__)
    ] [
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
  ]

let viewPC =
  FunctionComponent.Of ((fun (props: {| dispatch: Msg -> unit; lang: Language |}) ->
    div [Class "menu-desktop is-hidden-touch"; Key.Src(__FILE__,__LINE__)] [
      div [Class "menu-desktop-logo"; Key.Src(__FILE__,__LINE__)] []
      div [Class "menu-desktop-body"; Key.Src(__FILE__,__LINE__)] [
        div [Class "shadowed"; Key.Src(__FILE__,__LINE__)] [
          str "Foo"
        ]
      ]
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> __FILE__ + ":" + __LINE__))

let viewMobile =
  FunctionComponent.Of ((fun (props: {| dispatch: Msg -> unit; lang: Language |}) ->
    div [Class "menu-mobile is-hidden-desktop"; Key.Src(__FILE__,__LINE__)] [

    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> __FILE__ + ":" + __LINE__))

let viewMenu (model: Model) dispatch =
  let className =
    let baseClass = "menu-container"
    if model.flags |> Set.contains MenuIsVisible then baseClass
    else baseClass + " hidden"
  div [Class className; Key.Src(__FILE__,__LINE__)] [
    viewPC     {| dispatch = dispatch; lang = model.lang |}
    viewMobile {| dispatch = dispatch; lang = model.lang |}
  ]
