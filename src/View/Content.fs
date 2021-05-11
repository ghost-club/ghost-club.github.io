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

let inline private pictureWebpOrPNG key webp webpAlt =
  let style = [Width "100%"; ObjectFit "contain"]
  picture [Key key] [
    source [Style style; SrcSet webp; Type "image/webp"]
    source [Style style; SrcSet webpAlt; Type "image/png"]
    img [Style style; Src webpAlt; Alt ""]
  ]

let private viewAbout =
  FunctionComponent.Of ((fun (_props: {| lang: Language |}) ->
    Section.section [CustomClass "has-text-left"; Props [Key.Src(__FILE__,__LINE__)]] [
      div [Class "is-hidden-mobile"; Key.Src(__FILE__,__LINE__)] [
        Columns.columns [Columns.IsVCentered; Props [Key.Src(__FILE__,__LINE__)]] [
          Column.column [Props [Key.Src(__FILE__,__LINE__)]] [
            Heading.h2 [Props [Style [Color "white"]]] [str "About"]
            p [Key.Src(__FILE__,__LINE__)] [str !@"loremipsum"]
          ]
          Column.column [Props [Key.Src(__FILE__,__LINE__)]] [
            picture [Key.Src(__FILE__,__LINE__)] [
              source [SrcSet Assets.WebP.About; Type "image/webp"]
              source [SrcSet Assets.WebPAlt.About; Type "image/png"]
              img [Src Assets.WebPAlt.About; Alt ""]
            ]
          ]
        ]
        pictureWebpOrPNG (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoPC1 Assets.WebPAlt.GCPhotoPC1
        pictureWebpOrPNG (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoPC2 Assets.WebPAlt.GCPhotoPC2
        pictureWebpOrPNG (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoPC3 Assets.WebPAlt.GCPhotoPC3
      ]

      div [Class "is-hidden-tablet"; Key.Src(__FILE__,__LINE__)] [
        picture [Key.Src(__FILE__,__LINE__); Style [Position PositionOptions.Absolute; Width "100%"]] [
          source [Class "content-about-picture-mobile"; SrcSet Assets.WebP.About; Type "image/webp"]
          source [Class "content-about-picture-mobile"; SrcSet Assets.WebPAlt.About; Type "image/png"]
          img [Class "content-about-picture-mobile"; Src Assets.WebPAlt.About; Alt ""]
        ]
        div [Style [PaddingTop "50%"]; Key.Src(__FILE__,__LINE__)] []
        Heading.h2 [Props [Style [Color "white"]]] [str "About"]
        p [Key.Src(__FILE__,__LINE__)] [str !@"loremipsum"]
        pictureWebpOrPNG (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoMobile1 Assets.WebPAlt.GCPhotoMobile1
        pictureWebpOrPNG (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoMobile2 Assets.WebPAlt.GCPhotoMobile2
        pictureWebpOrPNG (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoMobile3 Assets.WebPAlt.GCPhotoMobile3
      ]
    ]
  ))

let private viewHowToJoin =
  FunctionComponent.Of ((fun (_props: {| lang: Language |}) ->
    Section.section [CustomClass "has-text-left"; Props [Key.Src(__FILE__,__LINE__)]] [
      Columns.columns [Columns.IsVCentered; Props [Key.Src(__FILE__,__LINE__)]] [
        Column.column [Props [Key.Src(__FILE__,__LINE__)]] [
          Heading.h2 [Props [Style [Color "white"]]] [str "How to join"]
          Block.block [Props [Key.Src(__FILE__,__LINE__)]] [str !@"loremipsum"]
          Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
            img [Src Assets.SVG.Obake; Class "is-hidden-tablet"; Style [Width "100%"; ObjectFit "contain"]]
          ]
          Block.block [Props [Style [Width "100%"; Height "70px"; Display DisplayOptions.InlineBlock]]] [
            button [Class "shadowed"; Key.Src(__FILE__,__LINE__); OnTouchStart ignore] [
              div [Class "shadowed-inner"; Style [FontSize "1.2rem"]; Key.Src(__FILE__,__LINE__); OnTouchStart ignore] [
                str "Send a friend request"
              ]
            ]
          ]
        ]
        Column.column [Props [Style [Padding "5% 10%"]; Key.Src(__FILE__,__LINE__)]] [
          img [Src Assets.SVG.Obake; Class "is-hidden-mobile"; Style [Width "100%"; ObjectFit "contain"]]
        ]
      ]
    ]
  ))

let view (model: Model) dispatch =
  div [Class "content has-text-centered"; Key "content"] [
    picture [Key.Src(__FILE__,__LINE__)] [
      source [Class "content-building"; SrcSet Assets.WebP.GCBuilding2; Type "image/webp"]
      source [Class "content-building"; SrcSet Assets.WebPAlt.GCBuilding2; Type "image/png"]
      img [Class "content-building"; Src Assets.WebPAlt.GCBuilding2; Alt ""]
    ]

    div [Class "content-foreground limited-width"; Key.Src(__FILE__,__LINE__)] [
      a [Class "anchor"; Id "about"; Key.Src(__FILE__,__LINE__)] []
      viewAbout {| lang = model.lang |}

      a [Class "anchor"; Id "how-to-join"; Key.Src(__FILE__,__LINE__)] []
      viewHowToJoin {| lang = model.lang |}

      a [Class "anchor"; Id "dj-mix"; Key.Src(__FILE__,__LINE__)] []
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

      a [Class "anchor"; Id "gallery"; Key.Src(__FILE__,__LINE__)] []
      Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
        Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
          Heading.h2 [Props [Style [Color "white"]]] [str "Gallery"]
        ]
        ofList (PhotoGallery.view model dispatch)
      ]

      a [Class "anchor"; Id "contact"; Key.Src(__FILE__,__LINE__)] []
      Section.section [Props [Key.Src(__FILE__,__LINE__); Style [Height "40vmax"]]] [
        div [Key.Src(__FILE__,__LINE__); Class "content-contact"] [
          div [Key.Src(__FILE__,__LINE__); Class "content-contact-head"] [
            Heading.h2 [Props [Style [Color "white"]]] [str "Contact"]
          ]
          div [Key.Src(__FILE__,__LINE__); Class "content-contact-body"] [
            div [Style [Width "240px"; Height "70px"; Display DisplayOptions.InlineBlock]] [
              button [Class "shadowed"; Key.Src(__FILE__,__LINE__); OnTouchStart ignore] [
                div [Class "shadowed-inner"; Key.Src(__FILE__,__LINE__); OnTouchStart ignore] [
                  str "Contact"
                ]
              ]
            ]
          ]
        ]
      ]
      Section.section [Props [Key.Src(__FILE__,__LINE__); Style [Height "10vmax"]]] [
        span [Key.Src(__FILE__,__LINE__); Style [FontSize "0.75rem"]] [
          str "Copyright © GHOSTCLUB All Rights Reserved."
        ]
      ]
    ]
  ]