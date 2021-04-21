module MixCloud
open Fable.React
open Fable.React.Props

type MixCloudOption =
  | HideCover of bool
  | HideArtwork of bool
  | Mini of bool

let private buildMixCloudUrl (opts: MixCloudOption list) user mix =
  let rec go acc = function
    | [] ->
      sprintf "https://www.mixcloud.com/widget/iframe/?%sfeed=%%2F%s%%2F%s%%2F" acc user mix
    | x :: xs ->
      let acc =
        match x with
        | HideCover true -> acc + "hide_cover=1&"
        | HideArtwork true -> acc + "hide_artwork=1&"
        | Mini true -> acc + "mini=1&"
        | _ -> ""
      go acc xs
  go "" opts

open Fulma

let mixCloud (options: MixCloudOption list) (user: string) (mix: string) =
  Block.block [] [
    iframe [
      Style [
        Width "100%"
        Height 120
      ]
      Src (buildMixCloudUrl options user mix)
      FrameBorder 0
    ] []
  ]

type MixCloudListProp = { user: string; mixName: string }
type MixCloudListProps = {
  items: MixCloudListProp list
  options: MixCloudOption list
  onLoad: (unit -> unit) option
}

let mixCloudList : MixCloudListProps -> ReactElement =
  FunctionComponent.Of ((fun props ->
    let propsCount = List.length props.items
    let loadedPropsCount = Hooks.useState 0
    Hooks.useEffect(fun () ->
      if loadedPropsCount.current = propsCount then
        loadedPropsCount.update(-1)
        match props.onLoad with None -> () | Some f -> f ()
    )
    ofList [
      for i, item in Seq.indexed props.items do
        Block.block [
          Props [
            Key (sprintf "%d-mixcloud-%s-%s" i item.user item.mixName)
          ]] [
          iframe [
            Style [
              Width "100%"
              Height 120
            ]
            Src (buildMixCloudUrl props.options item.user item.mixName)
            FrameBorder 0
            OnLoad (fun _ -> loadedPropsCount.update(loadedPropsCount.current+1))
          ] []
        ]
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun prop -> sprintf "%A" prop.items))
