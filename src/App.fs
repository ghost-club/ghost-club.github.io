module App.View

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open Fable.FontAwesome
open Fable.Core.JsInterop

type State =
  | Init
  | AlbumLoading
  | AlbumLoaded of Album.MediaInfo[]
  | AlbumLoadFailed of string
  member this.AsString =
    match this with
    | Init -> "Init"
    | AlbumLoading -> "Album Loading"
    | AlbumLoaded _ -> "Album Loaded"
    | AlbumLoadFailed msg -> sprintf "Album Load Failed (%s)" msg

type Model = {
  state: State
}

type Msg =
  | LoadAlbum
  | LoadAlbumResponse of Album.Response

let init _ = { state = Init }, Cmd.none

let private update msg model =
  match msg, model.state with
  | LoadAlbum, (Init | AlbumLoadFailed _) ->
    { model with state = AlbumLoading },
    Cmd.OfPromise.perform Album.get () LoadAlbumResponse
  | LoadAlbum, _ -> model, Cmd.none
  | LoadAlbumResponse (Ok album), _ ->
    { model with state = AlbumLoaded album }, Cmd.none
  | LoadAlbumResponse (Error msg), _ ->
    { model with state = AlbumLoadFailed msg }, Cmd.none

let private view model dispatch =
  Hero.hero [ Hero.IsFullHeight ] [
    Hero.body [] [
      Container.container [] [
        Columns.columns [ Columns.CustomClass "has-text-centered" ] [
          Column.column [ Column.Width(Screen.All, Column.IsHalf); Column.Offset(Screen.All, Column.IsOneQuarter) ] [
            yield
              Content.content [] [
                str (sprintf "state: %s" model.state.AsString)
              ]
            match model.state with
            | Init | AlbumLoadFailed _ ->
              yield
                Button.button [ Button.OnClick (fun _ -> dispatch LoadAlbum) ] [
                  str "load"
                ]
            | AlbumLoading -> ()
            | AlbumLoaded album -> ()
          ]
        ]
      ]
    ]
  ]

open Elmish.Debug
open Elmish.HMR

Program.mkProgram init update view
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
