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
      DangerouslySetInnerHTML { __html = Properties.Assets.InlineSVG.LeftButton }
    ] []
  ), withKey=(fun _ -> __SOURCE_FILE__ + ":" + __LINE__))

let private nextArrow =
  FunctionComponent.Of((fun (props: ReactSlick.CustomArrowProps) ->
    div [
      Class (props.className |> Option.defaultValue "")
      OnClick (props.onClick |> Option.map (box >> unbox) |> Option.defaultValue ignore)
      HTMLAttr.Custom ("style", props.style |> Option.map box |> Option.defaultValue (box ""))
      DangerouslySetInnerHTML { __html = Properties.Assets.InlineSVG.RightButton }
    ] []
  ), withKey=(fun _ -> __SOURCE_FILE__ + ":" + __LINE__))

let view =
  FunctionComponent.Of((fun (prop: {| images: Api.IMediaInfo[]; lang: Language; dispatch: (Msg -> unit) |}) ->
    let state = Hooks.useState {| isOpen = false; index = 0 |}
    let sliderRef = Hooks.useRef null
    let lightboxRef = Hooks.useState null

    (*
    Hooks.useEffectDisposable((fun () ->
      if lightboxRef.current <> null then
        if state.current.isOpen then
          ScrollLock.lock lightboxRef.current
          //ScrollLock.lockWithOptions lightboxRef.current !!{| reserveScrollBarGap = true |}
        else
          ScrollLock.unlock lightboxRef.current
      { new IDisposable with member __.Dispose() = ScrollLock.clearAll() }
    ), [| state; lightboxRef |])
    *)

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
          it.variableWidth <- Some true
          it.ref <- Some (U2.Case2 sliderRef)
          ()) [
        for i, mi in prop.images |> Seq.indexed do
          yield
            img [
              Src (Api.IMediaInfo.getOrigUrl mi)
              HTMLAttr.Custom ("loading", "lazy")
              HTMLAttr.Width  mi.width
              HTMLAttr.Height mi.height
              Alt ""
              OnDoubleClick (fun _e ->
                sliderRef.current.slickGoTo(i)
                state.update {| isOpen = true; index = i |})
            ]
      ]
      if state.current.isOpen then
        lightbox [
          !^Key.Src(__SOURCE_FILE__,__LINE__)
          ReactModalProps [ModalId "lightbox"; OverlayRef (fun el -> lightboxRef.update el)]
          MainSrc (prop.images.[state.current.index] |> Api.IMediaInfo.getOrigUrl)
          //NextSrc (album.[(state.current.index + 1) % album.Length] |> Api.IMediaInfo.getOrigUrl)
          //PrevSrc (album.[(state.current.index + album.Length - 1) % album.Length] |> Api.IMediaInfo.getOrigUrl)
          OnAfterOpen (fun () ->
            disableScroll.on())
          OnCloseRequest (fun () ->
            disableScroll.off()
            sliderRef.current.slickPlay()
            state.update {| state.current with isOpen = false |})
          OnMoveNextRequest (fun () ->
            let newIndex = (state.current.index + 1) % prop.images.Length
            sliderRef.current.slickGoTo(newIndex)
            state.update {| state.current with index = newIndex |})
          OnMovePrevRequest (fun () ->
            let newIndex = (state.current.index + prop.images.Length - 1) % prop.images.Length
            sliderRef.current.slickGoTo(newIndex)
            state.update {| state.current with index = newIndex |})
        ]
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> __SOURCE_FILE__ + ":" + __LINE__))
