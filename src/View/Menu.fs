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

let private viewBody =
  FunctionComponent.Of ((fun (props: {| dispatch: Msg -> unit; lang: Language |}) ->
    let menuModalIsShown = Hooks.useState false

    let inline menuModal str =
      if menuModalIsShown.current then str + " is-active"
      else str

    div [Class "menu"; Key.Src(__FILE__,__LINE__)] [
      img [Class "menu-logo"; Key.Src(__FILE__,__LINE__); Src Assets.SVG.LogoSmall]
      div [Class "menu-desktop-body is-hidden-touch"; Key.Src(__FILE__,__LINE__)] [
        div [Class "shadowed"; Key.Src(__FILE__,__LINE__)] [
          str "Foo"
        ]
      ]
      div [Class (menuModal "menu-mobile-button is-hidden-desktop"); Key.Src(__FILE__,__LINE__)] [
        button [
          Class (menuModal "hamburger hamburger--squeeze")
          Type "button"
          OnClick (fun _e -> menuModalIsShown.update(not menuModalIsShown.current))] [
          span [Class "hamburger-box"] [
            span [Class "hamburger-inner"] []
          ]
        ]
      ]
      div [Class (menuModal "menu-mobile-modal is-hidden-desktop"); Key.Src(__FILE__,__LINE__)] [
        div [Class "shadowed"; Key.Src(__FILE__,__LINE__)] [
          div [Class "shadowed-inner"; Key.Src(__FILE__,__LINE__)] [

          ]
        ]
      ]
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> __FILE__ + ":" + __LINE__))

let viewMenu (model: Model) dispatch =
  let className =
    let baseClass = "menu-container"
    if model.flags |> Set.contains MenuIsVisible then baseClass
    else baseClass + " hidden"
  div [Class className; Key.Src(__FILE__,__LINE__)] [
    viewBody {| dispatch = dispatch; lang = model.lang |}
  ]
