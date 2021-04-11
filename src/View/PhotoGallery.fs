module View.PhotoGallery

open System
open Fable
open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Wrappers.Rewrapped

open Model

let imgDelayed =
  FunctionComponent.Of

let viewPhotoGallery (model: Model) (dispatch: Msg -> unit) =
  let render (album: Album.IMediaInfo[]) =
    ReactSlick.slider
      (fun it ->
        it.className <- Some "center"
        it.centerMode <- Some true
        it.infinite <- Some true
        it.centerPadding <- Some "50px"
        it.slidesToShow <- Some 1.0
        it.dots <- Some true
        it.autoplay <- Some true
        it.arrows <- Some false
        it.lazyLoad <- Some ReactSlick.LazyLoadTypes.Progressive
        ()) [
        for i, mi in Seq.indexed album do
          yield
            div [Key (sprintf "gallery-img%d" i)] [
              img [Src (Album.IMediaInfo.getOrigUrl mi); OnLoad (fun e -> ())]
            ]
      ]

  match model.state with
  | AlbumLoaded album -> [render album]
  | _ -> []
