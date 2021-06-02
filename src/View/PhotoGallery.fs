module View.PhotoGallery

open System
open Fable
open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Properties
open ReactModal
open ReactImageLightbox

open Model

let private prevArrow =
  FunctionComponent.Of((fun (props: ReactSlick.CustomArrowProps) ->
    div [
      Class (props.className |> Option.defaultValue "")
      OnClick (props.onClick |> Option.map (box >> unbox) |> Option.defaultValue ignore)
      HTMLAttr.Custom ("style", props.style |> Option.map box |> Option.defaultValue (box ""))
      DangerouslySetInnerHTML { __html = Assets.InlineSVG.LeftButton }
    ] []
  ), withKey=(fun _ -> __SOURCE_FILE__ + ":" + __LINE__))

let private nextArrow =
  FunctionComponent.Of((fun (props: ReactSlick.CustomArrowProps) ->
    div [
      Class (props.className |> Option.defaultValue "")
      OnClick (props.onClick |> Option.map (box >> unbox) |> Option.defaultValue ignore)
      HTMLAttr.Custom ("style", props.style |> Option.map box |> Option.defaultValue (box ""))
      DangerouslySetInnerHTML { __html = Assets.InlineSVG.RightButton }
    ] []
  ), withKey=(fun _ -> __SOURCE_FILE__ + ":" + __LINE__))

let view =
  FunctionComponent.Of((fun (prop: {| images: Api.IMediaInfo[] option; canUseWebP: bool; dispatch: (Msg -> unit) |}) ->
    let state = Hooks.useState {| isOpen = false; index = 0 |}
    let sliderRef = Hooks.useRef null
    let lightboxRef = Hooks.useState null

    ofList [
      ReactSlick.slider
        (fun it ->
          it.key <- __SOURCE_FILE__ + ":" + __LINE__
          it.centerMode <- Some true
          it.infinite <- Some true
          it.centerPadding <- Some "0"
          it.slidesToShow <- Some 1.0
          it.dots <- Some true
          it.dotsClass <- Some "slick-dots is-hidden-mobile"
          it.autoplay <- Some (not state.current.isOpen)
          it.prevArrow <- Some (prevArrow !!{||})
          it.nextArrow <- Some (nextArrow !!{||})
          // it.variableWidth <- Some true
          it.ref <- Some (U2.Case2 sliderRef)
          ()) [
        match prop.images with
        | None ->
          for i, x in Assets.StaticGallery.Photos |> Seq.indexed do
            yield
              img [
                Src x
                HTMLAttr.Custom ("loading", "lazy")
                Alt ""
                OnDoubleClick (fun _e ->
                  sliderRef.current.slickGoTo(i)
                  state.update {| isOpen = true; index = i |})
              ]
        | Some imgs ->
          for i, mi in imgs |> Seq.indexed do
            let src, srcSet =
              match Api.IMediaInfo.getSrcSetWebP mi, not (isNullOrUndefined mi.origUrlWebP) with
              | Some srcSet, true ->
                mi.origUrlWebP, srcSet
              | _ ->
                mi.origUrl, Api.IMediaInfo.getSrcSet mi
            yield
              img [
                Src src
                SrcSet srcSet
                HTMLAttr.Custom ("loading", "lazy")
                Alt ""
                Width mi.width
                Height mi.height
                OnDoubleClick (fun _e ->
                  sliderRef.current.slickGoTo(i)
                  state.update {| isOpen = true; index = i |})
              ]
      ]
      if state.current.isOpen then
        lightbox [
          !^Key.Src(__SOURCE_FILE__,__LINE__)
          ReactModalProps [ModalId "lightbox"; OverlayRef (fun el -> lightboxRef.update el)]
          match prop.images with
          | Some imgs ->
            MainSrc (imgs.[state.current.index] |> Api.IMediaInfo.getOrigUrl)
          | None ->
            MainSrc (Assets.StaticGallery.Photos.[state.current.index])
          OnAfterOpen (fun () ->
            disableScroll.on())
          OnCloseRequest (fun () ->
            disableScroll.off()
            sliderRef.current.slickPlay()
            state.update {| state.current with isOpen = false |})
        ]
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> __SOURCE_FILE__ + ":" + __LINE__))
