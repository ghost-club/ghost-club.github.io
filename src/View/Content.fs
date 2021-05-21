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
    let videoModalShown = Hooks.useState false

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

        div [Key.Src(__FILE__,__LINE__); Style [
          Position PositionOptions.Relative
          Display DisplayOptions.Flex
          AlignItems AlignItemsOptions.Center
          JustifyContent "center"
          Margin "20px 0px"]] [
          let style = [Width "100%"; ObjectFit "contain"]
          picture [Key.Src(__FILE__,__LINE__)] [
            source [Style style; SrcSet Assets.WebP.VideoThumbnail; Type "image/webp"]
            source [Style style; SrcSet Assets.WebPAlt.VideoThumbnail; Type "image/jpeg"]
            img [Style style; Src Assets.WebPAlt.VideoThumbnail; Alt ""]
          ]
          let style = [Position PositionOptions.Absolute; Width "30%"; Height "30%"]
          div [
            Class "content-mobile-playbutton"
            DangerouslySetInnerHTML { __html = Assets.InlineSVG.PlayMovieMini }
            Style style
            OnClick (fun _ -> videoModalShown.update true)
          ] []
        ]
        Header.viewVideoModal
          {| isOpen = videoModalShown.current
             key = __LINE__ + ":" + __FILE__
             onClose = (fun () -> videoModalShown.update false) |}
        pictureWebpOrPNG (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoMobile1 Assets.WebPAlt.GCPhotoMobile1
        pictureWebpOrPNG (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoMobile2 Assets.WebPAlt.GCPhotoMobile2
        pictureWebpOrPNG (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoMobile3 Assets.WebPAlt.GCPhotoMobile3
      ]
    ]
  ), memoizeWith=memoEqualsButFunctions)

let inline private viewObake customClass =
  div [
    Key.Src(__FILE__,__LINE__)
    Class customClass
    DangerouslySetInnerHTML { __html = Assets.InlineSVG.Obake }
  ] []

let private viewHowToJoin =
  FunctionComponent.Of ((fun (_props: {| lang: Language |}) ->
    Section.section [CustomClass "has-text-left"; Props [Key.Src(__FILE__,__LINE__)]] [
      Columns.columns [Columns.IsVCentered; Props [Key.Src(__FILE__,__LINE__)]] [
        Column.column [Props [Key.Src(__FILE__,__LINE__)]] [
          Heading.h2 [Props [Style [Color "white"]]] [str "How to join"]
          Block.block [Props [Key.Src(__FILE__,__LINE__)]] [str !@"loremipsum"]
          Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
            viewObake "is-hidden-tablet"
          ]
          Block.block [Props [Style [Width "100%"; Height "70px"; Display DisplayOptions.InlineBlock]]] [
            button [
              Class "shadowed"; Key.Src(__FILE__,__LINE__); OnTouchStart ignore;
              OnClick (fun _e -> Browser.Dom.window.``open``("https://vrchat.com/home/user/usr_7e0bc356-da1f-44da-be54-72b6e4216c15", "_blank", "noopener") |> ignore)] [
              div [Class "shadowed-inner"; Style [FontSize "1.2rem"]; Key.Src(__FILE__,__LINE__); OnTouchStart ignore] [
                str "Send a friend request"
              ]
            ]
          ]
        ]
        Column.column [Props [Key.Src(__FILE__,__LINE__)]] [
          viewObake "is-hidden-mobile"
        ]
      ]
    ]
  ), memoizeWith=memoEqualsButFunctions)

let viewCredits =
  Section.section [CustomClass "credits"; Props [Id "credits";  Key "credits"]] [
    Heading.h6 [CustomClass "credits-head has-text-centered"; Props [Style [Color "white"; FontWeight "500"]]] [str "Staff & Credits"]
    div [Class "credits-body is-hidden-mobile"; Key.Src(__FILE__,__LINE__)] [
      for i, group in Credits.Entries |> List.indexed do
        div [Class "credits-section"; Key (sprintf "credits-group-%d" i)] [
          for entry in group do
            match Credits.pp entry with
            | Some text -> p [Class "credits-item"] [str text]
            | None -> p [Class "credits-item-dummy"] [str "end"]
        ]
    ]
    div [Class "credits-body is-hidden-tablet"; Key.Src(__FILE__,__LINE__)] [
      div [Class "credits-section"; Key.Src(__FILE__,__LINE__)] [
        for group in Credits.Entries do
          for entry in group do
            match Credits.pp entry with
            | Some text -> p [Class "credits-item"] [str text]
            | None -> null
      ]
    ]
  ]

let view (prop: {| lang: Language; albumState: AlbumState; dispatch: Msg -> unit |}) =
  div [Id "content"; Class "content has-text-centered"; Key "content"] [
    picture [Key.Src(__FILE__,__LINE__)] [
      source [Class "content-building"; SrcSet Assets.WebP.GCBuilding2; Type "image/webp"]
      source [Class "content-building"; SrcSet Assets.WebPAlt.GCBuilding2; Type "image/jpg"]
      img [Class "content-building"; Src Assets.WebPAlt.GCBuilding2; Alt ""]
    ]

    div [Class "content-foreground limited-width"; Key.Src(__FILE__,__LINE__)] [
      a [Class "anchor"; Id "about"; Href "#about"; Key.Src(__FILE__,__LINE__)] []
      viewAbout {| lang = prop.lang |}

      a [Class "anchor"; Id "how-to-join"; Href "#how-to-join"; Key.Src(__FILE__,__LINE__)] []
      viewHowToJoin {| lang = prop.lang |}

      a [Class "anchor"; Id "dj-mix"; Href "dj-mix"; Key.Src(__FILE__,__LINE__)] []
      Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
        Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
          Heading.h2 [Props [Style [Color "white"]]] [str "DJ Mix"]
        ]
        iframe [
          Title "GHOSTCLUB Mixcloud"
          Src "https://www.mixcloud.com/widget/iframe/?hide_cover=1&feed=%2F0bake%2Fplaylists%2Fghostclub%2F"
          Style [Width "100%"; Height "180px"]
          FrameBorder 0] []
      ]

      a [Class "anchor"; Id "gallery"; Href "gallery"; Key.Src(__FILE__,__LINE__)] []
      Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
        Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
          Heading.h2 [Props [Style [Color "white"]]] [str "Gallery"]
        ]
        PhotoGallery.view prop
      ]

      a [Class "anchor"; Id "contact"; Href "contact"; Key.Src(__FILE__,__LINE__)] []
      Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
        div [Key.Src(__FILE__,__LINE__); Class "content-contact"] [
          Block.block [CustomClass "content-contact-head"; Props [Key.Src(__FILE__,__LINE__)]] [
            Heading.h2 [Props [Style [Color "white"]]] [str "Contact"]
          ]
          Block.block [CustomClass "content-contact-body"; Props [Key.Src(__FILE__,__LINE__)]] [
            div [Style [Width "240px"; Height "70px"; Display DisplayOptions.InlineBlock]] [
              button [
                Class "shadowed"
                Key.Src(__FILE__,__LINE__)
                OnTouchStart ignore
                OnClick (fun _e ->
                  Browser.Dom.window.``open``("https://docs.google.com/forms/d/e/1FAIpQLSdKU2PixQJ1TMyWtZuukNiB39vVnstvA_vKV5PxULDKGMO4wg/viewform", "_blank", "noopener") |> ignore)] [
                div [Class "shadowed-inner"; Key.Src(__FILE__,__LINE__); OnTouchStart ignore] [
                  str "Contact"
                ]
              ]
            ]
          ]
        ]
      ]

      viewCredits
    ]
  ]
