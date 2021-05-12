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

let private viewLanguageSwitch =
  FunctionComponent.Of ((fun (props: {| dispatch: Msg -> unit; lang: Language; enabled: bool |}) ->
    let containerClass =
      match props.lang with
      | Unspecified | En -> "menu-item language-button-container language-en"
      | Ja -> "menu-item language-button-container language-jp"
    div [
      Class containerClass
      DangerouslySetInnerHTML { __html = Assets.InlineSVG.LanguageButton }
      OnClick (fun _e -> props.dispatch (SwitchLanguage (Language.Flip props.lang)))] []
  ), memoizeWith=memoEqualsButFunctions)

let private viewBody =
  FunctionComponent.Of ((fun (props: {| dispatch: Msg -> unit; lang: Language; enabled: bool |}) ->
    let menuModalIsShown = Hooks.useState false
    let mobileMenuRef = Hooks.useRef None

    Browser.Dom.window.onorientationchange <- (fun _e -> menuModalIsShown.update false)

    (*
    Hooks.useEffectDisposable((fun () ->
      match mobileMenuRef.current with
      | None ->
        ScrollLock.clearAll()
      | Some menu ->
        if menuModalIsShown.current then
          ScrollLock.lock(menu)
        else
          ScrollLock.unlock(menu)
      { new System.IDisposable with member __.Dispose() = ScrollLock.clearAll() }
    ), [| menuModalIsShown; mobileMenuRef |])
    *)

    Hooks.useEffect((fun () ->
      if props.enabled && menuModalIsShown.current && Screen.check Screen.Mobile then
        disableScroll.on()
      else
        disableScroll.off()
    ), [| menuModalIsShown; props.enabled |])

    let inline menuItem href title =
      li [Class "menu-item"] [
        a [
          Href ("#" + href)
          OnClick (fun _ ->
            let target = Browser.Dom.document.getElementById(href)
            if target <> null then
              target.scrollIntoView(!!{| behavior = Browser.Types.ScrollIntoViewOptionsBehavior.Smooth |} :> Browser.Types.ScrollIntoViewOptions)
            menuModalIsShown.update false)
        ] [str title]]

    let inline menuModal str =
      if menuModalIsShown.current then str + " is-active"
      else str

    div [Class "menu limited-width"; Key.Src(__FILE__,__LINE__)] [
      img [Class "menu-logo"; Key.Src(__FILE__,__LINE__); Src Assets.SVG.LogoSmall]
      div [Class "menu-desktop is-hidden-touch"; Key.Src(__FILE__,__LINE__)] [
        div [Class "shadowed"; Key.Src(__FILE__,__LINE__)] [
          div [Class "shadowed-inner"; Key.Src(__FILE__,__LINE__)] [
            ul [Class "menu-item menu-links"; Key.Src(__FILE__,__LINE__)] [
              menuItem "about" "About"
              menuItem "how-to-join" "How to join"
              menuItem "dj-mix" "DJ Mix"
              menuItem "gallery" "Gallery"
              menuItem "contact" "Contact"
            ]
            a [Class "menu-item"; Target "_blank"; Href "https://twitter.com/6h057clu8"; Key.Src(__FILE__,__LINE__); DangerouslySetInnerHTML { __html = Assets.InlineSVG.TwitterButton2 }] []
            viewLanguageSwitch props
          ]
        ]
      ]
      div [Class (menuModal "menu-mobile-button is-hidden-desktop"); Key.Src(__FILE__,__LINE__)] [
        button [
          Class (menuModal "hamburger hamburger--squeeze")
          Type "button"
          OnClick (fun _e ->
            menuModalIsShown.update(not menuModalIsShown.current))] [
          span [Class "hamburger-box"] [
            span [Class "hamburger-inner"] []
          ]
        ]
      ]
      div [
        Class (menuModal "menu-mobile is-hidden-desktop")
        Key.Src(__FILE__,__LINE__)] [
        div [Class "shadowed"; Key.Src(__FILE__,__LINE__); RefValue mobileMenuRef] [
          div [Class "shadowed-inner"; Key.Src(__FILE__,__LINE__)] [
            ul [Class "menu-item menu-links"; Key.Src(__FILE__,__LINE__)] [
              li [Class "menu-item"] []
              menuItem "about" "About"
              menuItem "how-to-join" "How to join"
              menuItem "dj-mix" "DJ Mix"
              menuItem "gallery" "Gallery"
              menuItem "contact" "Contact"
            ]
            a [Class "menu-item align-center"; Target "_blank"; Href "https://twitter.com/6h057clu8"; Key.Src(__FILE__,__LINE__); DangerouslySetInnerHTML { __html = Assets.InlineSVG.TwitterButton }] []
            viewLanguageSwitch props
          ]
        ]
      ]
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> __FILE__ + ":" + __LINE__))

let viewMenu (model: Model) dispatch =
  let className, enabled =
    let baseClass = "menu-container"
    if model.flags |> Set.contains MenuIsVisible then baseClass, true
    else if model.flags |> Set.contains PlayButtonIsShown then baseClass + " disable", false
    else baseClass + " hidden", false
  div [Class className; Key.Src(__FILE__,__LINE__)] [
    viewBody {| dispatch = dispatch; lang = model.lang; enabled = enabled|}
  ]
