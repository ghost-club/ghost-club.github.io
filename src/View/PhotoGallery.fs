module View.PhotoGallery

open System
open Fable
open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Wrappers.Rewrapped

open Model

let viewPhotoGallery (model: Model) (dispatch: Msg -> unit) =
  let render (album: Album.IMediaInfo[]) =
    div [Key "photo-gallery"] [
      ReactSlick.slider
        (fun it ->
          it.className <- Some "center"
          it.centerMode <- Some true
          it.infinite <- Some true
          it.centerPadding <- Some "0"
          it.slidesToShow <- Some 1.0
          it.dots <- Some true
          it.autoplay <- Some true
          it.arrows <- Some false
          it.lazyLoad <- Some ReactSlick.LazyLoadTypes.Progressive
          ()) [
        for i, mi in Seq.indexed album do
          yield
            div [Key (sprintf "photo-gallery-img%d" i)] [
              img [Src (Album.IMediaInfo.getOrigUrl mi)]
            ]
      ]
    ]

  match model.albumState with
  | AlbumState.Loaded album -> [render album]
  | _ -> []
