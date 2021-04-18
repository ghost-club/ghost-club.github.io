module View.Content

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open Fable.Core
open Fable.Core.JsInterop
open Properties
open Model
open View
open Wrappers.Rewrapped

let [<Literal>] __FILE__ = __SOURCE_FILE__

let view (model: Model) dispatch =
  div [Key.Src(__FILE__, __LINE__)] [
    Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
      p [Key.Src(__FILE__,__LINE__)] [str LoremIpsum]
    ]
    div [Key.Src(__FILE__,__LINE__)] (PhotoGallery.view model dispatch)
    Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
      p [Key.Src(__FILE__,__LINE__)] [str LoremIpsumJp]
    ]
    Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
      Block.block [] [Heading.h2 [Props [Style [Color "white"]]] [str "DJ Mix"]]
      MixCloud.mixCloudList {
        options = [MixCloud.HideCover true]
        onLoad = None
        items = [
          { user = "cannorin"; mixName = "20210402-gc-birthday-mix" }
          { user = "cannorin"; mixName = "20210402-gc-birthday-mix" }
          { user = "cannorin"; mixName = "20210402-gc-birthday-mix" }
        ]
      }
    ]
    ofList [
      for i = 1 to 10 do
        Section.section [Props [Key (sprintf "lorem-ipsum-%d" i)]] [
          p [Key.Src(__FILE__,__LINE__)] [str (if i % 2 = 0 then LoremIpsum else LoremIpsumJp)]
        ]
    ]
  ]