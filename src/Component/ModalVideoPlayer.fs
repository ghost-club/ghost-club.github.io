module ModalVideoPlayer

open System
open Fable.Core
open Fable.Core.JsInterop
open Browser.Types
open Browser.Dom
open Fable.React
open Fable.React.Props
open System.ComponentModel
open ReactPlayer

type Animation =
  | Disabled
  | Enabled of durationMS: int

type Prop<'Platform> = {|
  isOpen: bool
  useKey: string
  url: string
  config: 'Platform list
  onAfterOpen: (ReactModal.OnAfterOpenCallbackOptions -> unit) option
  onCloseRequest: (U2<MouseEvent, KeyboardEvent> -> unit) option
|}

module P = ReactPlayer
module PP = ReactPlayerProp
module M = ReactModal

let inline modalVideoPlayer< ^Platform when ^Platform: (static member Player: IPlatformSpecificReactPlayerProp< ^Platform > list -> ReactElement ) > =
  FunctionComponent.Of((fun (prop: Prop< ^Platform >) ->
    let state = Hooks.useState {| isClosing = false |}

    let afterOpen e =
      disableScroll.on()
      match prop.onAfterOpen with
      | None -> ()
      | Some f -> f e

    let requestClose e =
      match prop.onCloseRequest with
      | None -> ()
      | Some onClose ->
        state.update(fun s -> {| s with isClosing = true |})
        disableScroll.off()
        JS.setTimeout (fun () -> onClose e; state.update (fun s -> {| s with isClosing = false |})) 300 |> ignore

    let closeIfClickInner (e: MouseEvent) =
      if (!!e.target : Element).className.Contains("ril-inner") then
        requestClose !^e

    let handleKeyInput (e: KeyboardEvent) =
      if e.code = "Escape" then
        e.preventDefault()
        requestClose !^e

    M.modal [
      M.IsOpen prop.isOpen
      M.OnAfterOpen afterOpen
      M.OnRequestClose (fun e -> requestClose !!e)
      M.Prop.Style [
        M.Style.Overlay [ ZIndex 1000; BackgroundColor "transparent" ]
        M.Style.Content [
          BackgroundColor "transparent"
          OverflowStyle OverflowOptions.Hidden
          Border "none"
          BorderRadius 0
          Padding 0
          Top 0; Left 0; Right 0; Bottom 0
        ]
      ]
      M.ContentLabel "Movie"
      M.AppElement (if not (isNullOrUndefined document) then document.body else JS.undefined)
    ] [
      div [
        Class <|
          "ril-outer ril__outer ril__outerAnimating "
          + if state.current.isClosing then "ril-closing ril__outerClosing" else ""
        Style [
          Transition "opacity 300ms"
          AnimationDuration "300ms"
          AnimationDirection (if state.current.isClosing then SingleAnimationDirection.Normal else SingleAnimationDirection.Reverse)
        ]
        OnKeyDown handleKeyInput; OnKeyUp handleKeyInput
      ] [
        div [
          Class "ril-inner ril__inner"
          OnClick closeIfClickInner
          Style [
            Display DisplayOptions.Flex
            AlignItems AlignItemsOptions.Center
            JustifyContent "center"
            Padding "50px 0px"
          ]
          Key (prop.useKey + "-inner")] [

          div [
            Key (prop.useKey + "-inner-video-wrapper")
            Style [
              Position PositionOptions.Relative
              Width "calc(min(100vw, 177.77vh))"
              Height "calc(min(56.25vw, 100vh))"
            ]
          ] [
            (^Platform: (static member Player: IPlatformSpecificReactPlayerProp< ^Platform > list -> ReactElement) [
              PP.Config prop.config
              PP.Url prop.url
              PP.Playing prop.isOpen
              PP.StopOnUnmount true
              PP.Loop true
              PP.Width "100%"
              PP.Height "100%"
              PP.Playsinline true
              PP.Pip false
            ])
          ]
        ]

        div [Class "ril-toolbar ril__toolbar"; Key (prop.useKey + "-toolbar")] [
          ul [Class "ril-toolbar-left ril__toolbarSide ril__toolbarLeftSide"] []
          ul [Class "ril-toolbar-right ril__toolbarSide ril__toolbarRightSide"] [
            li [Class "ril-toolbal__item ril__toolbarItem"] [
              button [
                Type "button"; Key "close"; AriaLabel !@"Close Movie"
                Class "ril-close ril-toolbar__item__child ril__toolbarItemChild ril__builtinButton ril__closeButton"
                OnClick (fun e -> if not state.current.isClosing then requestClose !^e)
              ] []
            ]
          ]
        ]
      ]
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun p -> p.useKey))
