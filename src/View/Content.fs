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
open FadeIn

let [<Literal>] __FILE__ = __SOURCE_FILE__

let inline private pictureWebpOrPNG key webp webpAlt =
  let style = [Width "100%"; ObjectFit "contain"; UserSelect UserSelectOptions.None]
  fadeIn
    {|
      children =
        picture [Key key] [
          source [Style style; SrcSet webp; Type "image/webp"]
          source [Style style; SrcSet webpAlt; Type "image/png"]
          img [Style style; Src webpAlt; Alt ""; HTMLAttr.Custom("loading", "lazy")]
        ]
      key = key+"fade-container"
    |}

let private viewAbout =
  FunctionComponent.Of ((fun (prop: {| lang: Language; api: Api.IResult<Api.All> |}) ->
    let videoModalShown = Hooks.useState false
    let poemIndex = Hooks.useState 0
    Hooks.useEffect((fun () ->
      let rand = System.Random()
      match prop.api with
      | Api.IResult.Ok all when all.poems.Length > 0 ->
        poemIndex.update(rand.Next(all.poems.Length))
      | _ -> ()
    ), [|prop.api|])

    let aboutText =
      match prop.api with
      | Api.IResult.Ok all when all.poems.Length > 0 ->
        match prop.lang with
        | Ja -> all.poems.[poemIndex.current].Japanese
        | _ -> all.poems.[poemIndex.current].English
      | _ -> !@Texts.About

    Section.section [CustomClass "has-text-left"; Props [Style [PaddingTop "0"]; Key.Src(__FILE__,__LINE__)]] [
      div [Class "is-hidden-mobile"; Key.Src(__FILE__,__LINE__)] [
        Columns.columns [Columns.IsVCentered; Props [Key.Src(__FILE__,__LINE__)]] [
          Column.column [Props [Key.Src(__FILE__,__LINE__)]] [
            fadeIn {| children = Heading.h2 [CustomClass "content-about-title"] [str "About"]; key = __FILE__+":"+__LINE__ |}
            fadeIn {| children = p [Class "text"; Key.Src(__FILE__,__LINE__)] [str aboutText]; key = __FILE__+":"+__LINE__ |}
          ]
          Column.column [Props [Style [AlignSelf AlignSelfOptions.FlexEnd]; Key.Src(__FILE__,__LINE__)]] [
            fadeIn
              {|
                children =
                  picture [Key.Src(__FILE__,__LINE__)] [
                    source [Class "content-about-picture"; SrcSet Assets.WebP.About; Type "image/webp"]
                    source [Class "content-about-picture"; SrcSet Assets.WebPAlt.About; Type "image/png"]
                    img [Class "content-about-picture"; Src Assets.WebPAlt.About; Alt ""; HTMLAttr.Custom("loading", "lazy")]
                  ]
                key = __FILE__+":"+__LINE__
              |}
          ]
        ]
        pictureWebpOrPNG (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoPC1 Assets.WebPAlt.GCPhotoPC1
        pictureWebpOrPNG (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoPC2 Assets.WebPAlt.GCPhotoPC2
        pictureWebpOrPNG (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoPC3 Assets.WebPAlt.GCPhotoPC3
      ]

      div [Class "is-hidden-tablet"; Key.Src(__FILE__,__LINE__)] [
        fadeIn
          {|
            children =
              picture [Key.Src(__FILE__,__LINE__); Style [Position PositionOptions.Absolute; Width "100%"]] [
                source [Class "content-about-picture-mobile"; SrcSet Assets.WebP.About; Type "image/webp"]
                source [Class "content-about-picture-mobile"; SrcSet Assets.WebPAlt.About; Type "image/png"]
                img [Class "content-about-picture-mobile"; Src Assets.WebPAlt.About; Alt ""; HTMLAttr.Custom("loading", "lazy")]
              ]
            key = __FILE__+":"+__LINE__
          |}
        div [Style [PaddingTop "50%"]; Key.Src(__FILE__,__LINE__)] []

        fadeIn {| children = Heading.h2 [CustomClass "content-about-title"] [str "About"]; key = __FILE__+":"+__LINE__ |}
        fadeIn {| children = p [Class "text"; Key.Src(__FILE__,__LINE__)] [str aboutText]; key = __FILE__+":"+__LINE__ |}

        fadeIn
          {|
            children =
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
                  img [Style style; Src Assets.WebPAlt.VideoThumbnail; Alt ""; HTMLAttr.Custom("loading", "lazy")]
                ]
                div [
                  Class "content-mobile-playbutton"
                  DangerouslySetInnerHTML { __html = Assets.InlineSVG.PlayMovieMini }
                  OnClick (fun _ -> videoModalShown.update true)
                ] []
              ]
            key = __FILE__+":"+__LINE__
          |}

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
          Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
            p [Class "text"] [str !@Texts.HowToJoin]
          ]
          Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
            viewObake "is-hidden-tablet"
          ]
          Block.block [Props [Style [Width "100%"; Height "70px"; Display DisplayOptions.InlineBlock]]] [
            button [
              Class "shadowed"; Key.Src(__FILE__,__LINE__); OnTouchStart ignore;
              OnClick (fun _e -> Browser.Dom.window.``open``(Links.Discord, "_blank", "noopener") |> ignore)] [
              div [Class "shadowed-inner"; Style [FontSize "1.2rem"]; Key.Src(__FILE__,__LINE__); OnTouchStart ignore] [
                str "Join our Discord server"
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
    h6 [Class "credits-head has-text-centered"] [str "Staff & Credits"]
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

open ReactIntersectionObserver

let view (prop: {| lang: Language; api: Api.IResult<Api.All>; dispatch: Msg -> unit |}) =
  inViewPlain [
    !^Id("content")
    !^Class("content has-text-centered")
    !^Key("content")
    OnChange (fun inView _ ->
      prop.dispatch (SetFlag (MenuIsVisible, inView)))
  ] <| ofList [
    picture [Key.Src(__FILE__,__LINE__)] [
      source [Class "content-building"; SrcSet Assets.WebP.GCBuilding2; Type "image/webp"]
      source [Class "content-building"; SrcSet Assets.WebPAlt.GCBuilding2; Type "image/jpg"]
      img [Class "content-building"; Src Assets.WebPAlt.GCBuilding2; Alt ""; HTMLAttr.Custom("loading", "lazy")]
    ]

    div [Class "content-foreground limited-width"; Key.Src(__FILE__,__LINE__)] [
      a [Class "anchor"; Id "about"; Href "#about"; Key.Src(__FILE__,__LINE__)] []
      viewAbout {| lang = prop.lang; api = prop.api |}

      a [Class "anchor"; Id "how-to-join"; Href "#how-to-join"; Key.Src(__FILE__,__LINE__)] []
      viewHowToJoin {| lang = prop.lang |}

      a [Class "anchor"; Id "dj-mix"; Href "dj-mix"; Key.Src(__FILE__,__LINE__)] []
      Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
        Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
          Heading.h2 [Props [Style [Color "white"]]] [str "DJ Mix"]
        ]
        iframe [
          Title "GHOSTCLUB Mixcloud"
          Src Links.MixCloudWidget
          Style [Width "100%"; Height "180px"]
          FrameBorder 0
          HTMLAttr.Custom("loading", "lazy")] []
      ]

      a [Class "anchor"; Id "gallery"; Href "gallery"; Key.Src(__FILE__,__LINE__)] []
      Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
        Block.block [Props [Key.Src(__FILE__,__LINE__)]] [
          Heading.h2 [Props [Style [Color "white"]]] [str "Gallery"]
        ]
        match prop.api with
        //| Api.IResult.Ok all -> PhotoGallery.view {| lang = prop.lang; images = Some all.images; dispatch = prop.dispatch |}
        | _ -> PhotoGallery.view {| lang = prop.lang; images = None; dispatch = prop.dispatch |}
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
                  Browser.Dom.window.``open``(Links.Contact, "_blank", "noopener") |> ignore)] [
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
