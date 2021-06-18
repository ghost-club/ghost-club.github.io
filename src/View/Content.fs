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

let inline private pictureWebPOrPngChild class_ style (webp: Img) (webpAlt: Img) =
  [
    source [Class class_; Style style; SrcSet webp.srcSet; Width webp.width; Height webp.height; Sizes "100vw"; Type "image/webp"]
    img [Class class_; Style style; Src webpAlt.src; SrcSet webpAlt.srcSet; Width webpAlt.width; Height webpAlt.height; Sizes "100vw"; Alt ""; HTMLAttr.Custom("loading", "lazy")]
  ]


let inline private pictureWebpOrPNGFadeIn key (webp: Img) (webpAlt: Img) =
  let style = [Width "100%"; ObjectFit "contain"; UserSelect UserSelectOptions.None]
  fadeIn
    {|
      children =
        picture [Key key] <| pictureWebPOrPngChild null style webp webpAlt
      key = key+"fade-container"
    |}

let inline private pictureWebPOrPngWithClass key class_ (webp: Img) (webpAlt: Img) =
  picture [Key key] <| pictureWebPOrPngChild class_ [] webp webpAlt

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
      h1 [Class "hidden"] [str "About"]
      div [Class "is-hidden-mobile"; Key.Src(__FILE__,__LINE__)] [
        Columns.columns [Columns.IsVCentered; Props [Key.Src(__FILE__,__LINE__)]] [
          Column.column [Props [Key.Src(__FILE__,__LINE__)]] [
            fadeIn {| children = Heading.h1 [CustomClass "content-about-title"] [str "About"]; key = __FILE__+":"+__LINE__ |}
            fadeIn {| children = p [Class "text"; Key.Src(__FILE__,__LINE__)] [str aboutText]; key = __FILE__+":"+__LINE__ |}
          ]
          Column.column [Props [Style [AlignSelf AlignSelfOptions.FlexEnd]; Key.Src(__FILE__,__LINE__)]] [
            fadeIn
              {|
                children =
                  pictureWebPOrPngWithClass (__FILE__+":"+__LINE__) "content-about-picture" Assets.WebP.About Assets.WebPAlt.About
                key = __FILE__+":"+__LINE__
              |}
          ]
        ]
        pictureWebpOrPNGFadeIn (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoPC1 Assets.WebPAlt.GCPhotoPC1
        pictureWebpOrPNGFadeIn (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoPC2 Assets.WebPAlt.GCPhotoPC2
        pictureWebpOrPNGFadeIn (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoPC3 Assets.WebPAlt.GCPhotoPC3
      ]

      div [Class "is-hidden-tablet"; Key.Src(__FILE__,__LINE__)] [
        fadeIn
          {|
            children =
              picture [Key.Src(__FILE__,__LINE__); Style [Position PositionOptions.Absolute; Width "100%"]] <|
                pictureWebPOrPngChild "content-about-picture-mobile" [] Assets.WebP.About Assets.WebPAlt.About
            key = __FILE__+":"+__LINE__
          |}
        div [Style [PaddingTop "50%"]; Key.Src(__FILE__,__LINE__)] []

        fadeIn {| children = Heading.h1 [CustomClass "content-about-title"] [str "About"]; key = __FILE__+":"+__LINE__ |}
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
                picture [Key.Src(__FILE__,__LINE__)] <|
                  pictureWebPOrPngChild null style Assets.WebP.VideoThumbnail Assets.WebPAlt.VideoThumbnail
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
        pictureWebpOrPNGFadeIn (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoMobile1 Assets.WebPAlt.GCPhotoMobile1
        pictureWebpOrPNGFadeIn (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoMobile2 Assets.WebPAlt.GCPhotoMobile2
        pictureWebpOrPNGFadeIn (__FILE__+":"+__LINE__) Assets.WebP.GCPhotoMobile3 Assets.WebPAlt.GCPhotoMobile3
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
      h1 [Class "hidden"] [str "How to join"]
      Columns.columns [Columns.IsVCentered; Props [Key.Src(__FILE__,__LINE__)]] [
        Column.column [Props [Key.Src(__FILE__,__LINE__)]] [
          Heading.h1 [] [str "How to join"]
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

open ReactIntersectionObserver

type EmptyProps = interface end

let private viewDJMix =
  FunctionComponent.Of((fun (_: EmptyProps) ->
    let visible = Hooks.useState false
    let shown = Hooks.useState false
    let iframeRef = Hooks.useRef None

    Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
      Heading.h1 [] [str "DJ Mix"]
      div [
        Key.Src(__FILE__,__LINE__)
        Style [
          Position PositionOptions.Relative
          Display (if shown.current then DisplayOptions.None else DisplayOptions.Inherit)
        ]] [
        div [
          Key.Src(__FILE__,__LINE__)
          Style [
            Position PositionOptions.Absolute
            Width "100%"
            Height "180px"
            BackgroundColor "#25292c"
            PointerEvents "none"
          ]
        ] []
      ]
      inViewPlain [
        !^Key.Src(__FILE__,__LINE__)
        RootMargin "100px"
        TriggerOnce true
        OnChange (fun inView _ -> visible.update inView)
        !^Style([Position PositionOptions.Relative; ZIndex 1])
      ] <|
        if visible.current then
          iframe [
            Title "GHOSTCLUB Mixcloud"
            Src Links.MixCloudWidget
            Style [Width "100%"; Height "180px"]
            FrameBorder 0
            RefValue iframeRef
            OnLoad (fun _ -> shown.update true)
            HTMLAttr.Custom("loading", "lazy")] []
        else
          div [
            Key "mixcloud-iframe-placeholder"
            Style [
              Width "100%"
              Height "180px"
            ]
          ] []
    ]
  ), memoizeWith=memoEqualsButFunctions, withKey=(fun _ -> "dj-mix"))

let private viewCredits =
  Section.section [CustomClass "credits"; Props [Id "credits";  Key "credits"]] [
    h1 [Class "credits-head has-text-centered"] [str "Staff & Credits"]
    div [Class "credits-body is-hidden-mobile"; Key.Src(__FILE__,__LINE__)] [
      for i, group in Credits.Entries |> List.indexed do
        ul [Class "credits-section"; Key (sprintf "credits-group-%d" i)] [
          for entry in group do
            match Credits.pp entry with
            | Some text -> li [Class "credits-item"] [str text]
            | None -> li [Class "credits-item-dummy"] [str "end"]
        ]
    ]
    div [Class "credits-body is-hidden-tablet"; Key.Src(__FILE__,__LINE__)] [
      ul [Class "credits-section"; Key.Src(__FILE__,__LINE__)] [
        for group in Credits.Entries do
          for entry in group do
            match Credits.pp entry with
            | Some text -> li [Class "credits-item"] [str text]
            | None -> null
      ]
    ]
  ]

let view (prop: {| lang: Language; api: Api.IResult<Api.All>; canUseWebP: bool; dispatch: Msg -> unit |}) =
  inViewPlain [
    !^Id("content")
    !^Class("content has-text-centered")
    !^Key("content")
    As "main"
    OnChange (fun inView _ ->
      prop.dispatch (SetFlag (MenuIsVisible, inView)))
  ] <| ofList [
    pictureWebPOrPngWithClass (__FILE__+__LINE__) "content-building" Assets.WebP.GCBuilding2 Assets.WebPAlt.GCBuilding2

    div [Class "content-foreground limited-width"; Key.Src(__FILE__,__LINE__)] [
      a [Class "anchor"; Id "about"; Href "#about"; Key.Src(__FILE__,__LINE__)] []
      viewAbout {| lang = prop.lang; api = prop.api |}

      a [Class "anchor"; Id "how-to-join"; Href "#how-to-join"; Key.Src(__FILE__,__LINE__)] []
      viewHowToJoin {| lang = prop.lang |}

      a [Class "anchor"; Id "dj-mix"; Href "dj-mix"; Key.Src(__FILE__,__LINE__)] []
      viewDJMix !!{||}

      a [Class "anchor"; Id "gallery"; Href "gallery"; Key.Src(__FILE__,__LINE__)] []
      Section.section [Props [Key.Src(__FILE__,__LINE__)]] [
        Heading.h1 [] [str "Gallery"]
        match prop.api with
        | Api.IResult.Ok all -> PhotoGallery.view {| images = Some all.images; canUseWebP = prop.canUseWebP; dispatch = prop.dispatch |}
        | _ -> PhotoGallery.view {| images = None; canUseWebP = prop.canUseWebP; dispatch = prop.dispatch |}
      ]

      a [Class "anchor"; Id "contact"; Href "contact"; Key.Src(__FILE__,__LINE__)] []
      Section.section [CustomClass "content-contact"; Props [Key.Src(__FILE__,__LINE__)]] [
        Heading.h1 [CustomClass "content-contact-head"] [str "Contact"]
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

      viewCredits
    ]
  ]
