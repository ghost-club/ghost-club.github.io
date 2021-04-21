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
  ofList [
    picture [Class "content-building"] [
      source [SrcSet Assets.WebP.GCBuilding2; Type "image/webp"]
      source [SrcSet Assets.WebPAlt.GCBuilding2; Type "image/png"]
      img [Src Assets.WebPAlt.GCBuilding2; Alt ""]
    ]

    div [Class "content-foreground"; Key.Src(__FILE__,__LINE__)] [
      Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
        p [Key.Src(__FILE__,__LINE__)] [str LoremIpsum]
      ]

      Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
        Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
          Heading.h2 [Props [Style [Color "white"]]] [str "DJ Mix"]
        ]
        MixCloud.mixCloudList {
          options = [MixCloud.HideCover true]
          onLoad = None
          items = [
            { user = "cannorin"; mixName = "20210402-gc-birthday-mix" }
          ]
        }
      ]

      Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
        Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
          Heading.h2 [Props [Style [Color "white"]]] [str "Gallery"]
        ]
        ofList (PhotoGallery.view model dispatch)
      ]

      Section.section [Props [Key.Src(__FILE__,__LINE__); Style [Height "50vmax"]]] [
        Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
          Heading.h2 [Props [Style [Color "white"]]] [str "Contact"]
        ]
      ]
    ]
  ]