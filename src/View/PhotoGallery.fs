module View.PhotoGallery

open System
open Fable
open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Wrappers.Rewrapped

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

let view (model: Model) (dispatch: Msg -> unit) =
  let render (album: Album.IMediaInfo[]) =
    div [Key.Src(__SOURCE_FILE__, __LINE__)] [
      ReactSlick.slider
        (fun it ->
          it.centerMode <- Some true
          it.infinite <- Some true
          it.centerPadding <- Some "0"
          it.slidesToShow <- Some 1.0
          it.dots <- Some true
          it.dotsClass <- Some "slick-dots is-hidden-mobile"
          it.autoplay <- Some true
          it.prevArrow <- Some (prevArrow !!{||})
          it.nextArrow <- Some (nextArrow !!{||})
          it.variableWidth <- Some true
          ()) [
        for mi in album do
          yield
            img [
              Src (Album.IMediaInfo.getOrigUrl mi)
              HTMLAttr.Custom ("loading", "lazy")
              HTMLAttr.Width  mi.width
              HTMLAttr.Height mi.height
              Alt ""
            ]
      ]
    ]


  match model.albumState with
  | AlbumState.Loaded album -> [render album]
  | _ -> []
