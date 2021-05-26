module Properties

open Fable.Core

let [<Literal>] DomainNameInPunyCode = "xn--pckjp4dudxftf.xn--tckwe"

let [<Literal>] DataUrl = "https://xn--pckjp4dudxftf.xn--tckwe/data/index.json"

let [<Literal>] GoogleAppUrl = "https://script.google.com/macros/s/AKfycbxkGeSRx3QMBSyCLIZCMlAjy_iqxz7e_1B3eyrhmDifjUyFUR5U53gRF_9oF78sLlgI/exec"

module Assets =
  module SVG =
    let [<Literal>] Logo = "assets/image/logo.svg"
    let [<Literal>] LogoSmall = "assets/image/logo_small.svg"
    let [<Literal>] Obake = "assets/image/obake.svg"
    let [<Literal>] WatchMovie = "assets/image/watch_movie.svg"

  module WebPAlt =
    let [<Literal>] GCPhotoMobile1 = "assets/image/gc_sp_1.png"
    let [<Literal>] GCPhotoMobile2 = "assets/image/gc_sp_2.png"
    let [<Literal>] GCPhotoMobile3 = "assets/image/gc_sp_3.png"
    let [<Literal>] GCPhotoPC1 = "assets/image/gc_pc_1.png"
    let [<Literal>] GCPhotoPC2 = "assets/image/gc_pc_2.png"
    let [<Literal>] GCPhotoPC3 = "assets/image/gc_pc_3.png"
    let [<Literal>] GCBuilding1 = "assets/image/gc_mansion_1.png"
    let [<Literal>] GCBuilding2 = "assets/image/gc_mansion_2.jpg"
    let [<Literal>] About = "assets/image/about.jpg"
    let [<Literal>] VideoThumbnail = "assets/image/video_thumb.jpg"

  module WebP =
    let [<Literal>] GCPhotoMobile1 = "assets/image/gc_sp_1.webp"
    let [<Literal>] GCPhotoMobile2 = "assets/image/gc_sp_2.webp"
    let [<Literal>] GCPhotoMobile3 = "assets/image/gc_sp_3.webp"
    let [<Literal>] GCPhotoPC1 = "assets/image/gc_pc_1.webp"
    let [<Literal>] GCPhotoPC2 = "assets/image/gc_pc_2.webp"
    let [<Literal>] GCPhotoPC3 = "assets/image/gc_pc_3.webp"
    let [<Literal>] GCBuilding1 = "assets/image/gc_mansion_1.webp"
    let [<Literal>] GCBuilding2 = "assets/image/gc_mansion_2.webp"
    let [<Literal>] About = "assets/image/about.webp"
    let [<Literal>] VideoThumbnail = "assets/image/video_thumb.webp"

  module Movie =
    let [<Literal>] MP4 = "assets/video/website_background_movie_1080.mp4"
    let [<Literal>] WebM = "assets/video/website_background_movie_1080.webm"
    let [<Literal>] JPG = "assets/video/website_background_movie_1080.jpg"

  module InlineSVG =
    let LeftButton =
      """<svg class="left-button" viewBox="0 0 67 67" fill="none" xmlns="http://www.w3.org/2000/svg">
<path class="circle" vector-effect="non-scaling-stroke" d="M33.4 65.8C51.294 65.8 65.8 51.294 65.8 33.4C65.8 15.506 51.294 1 33.4 1C15.506 1 1 15.506 1 33.4C1 51.294 15.506 65.8 33.4 65.8Z" stroke="white" stroke-width="2" stroke-miterlimit="10"/>
<path class="arrow"  vector-effect="non-scaling-stroke" d="M41.1 18.6992L19.5 33.3992L41.1 48.0992" stroke="white" stroke-width="2" stroke-miterlimit="10"/>
</svg>"""

    let RightButton =
      """<svg class="right-button" viewBox="0 0 67 67" fill="none" xmlns="http://www.w3.org/2000/svg">
<path class="circle" vector-effect="non-scaling-stroke" d="M33.3998 1.00078C15.5058 1.00077 0.999806 15.5067 0.999804 33.4008C0.999803 51.2948 15.5058 65.8008 33.3998 65.8008C51.2938 65.8008 65.7998 51.2948 65.7998 33.4008C65.7998 15.5068 51.2938 1.00078 33.3998 1.00078Z" stroke="white" stroke-width="2" stroke-miterlimit="10"/>
<path class="arrow"  vector-effect="non-scaling-stroke" d="M25.6998 48.1016L47.2998 33.4016L25.6998 18.7016" stroke="white" stroke-width="2" stroke-miterlimit="10"/>
</svg>"""

    let TwitterButton =
      """<svg class="twitter-logo" viewBox="0 0 50 50" fill="none" xmlns="http://www.w3.org/2000/svg">
<path d="M25 50C38.8071 50 50 38.8071 50 25C50 11.1929 38.8071 0 25 0C11.1929 0 0 11.1929 0 25C0 38.8071 11.1929 50 25 50Z" fill="white"/>
<path d="M20.5 38.1673C31.6667 38.1673 37.6667 29.0007 37.6667 21.0007C37.6667 20.6673 37.6667 20.5007 37.6667 20.1673C38.8333 19.334 39.8333 18.334 40.6667 17.0007C39.6667 17.5007 38.5 17.834 37.1667 18.0007C38.5 17.334 39.3333 16.0007 39.8333 14.6673C38.6667 15.334 37.3333 15.834 36 16.1673C34.8333 15.0007 33.3333 14.334 31.5 14.334C28.1667 14.334 25.5 17.0007 25.5 20.334C25.5 20.834 25.5 21.334 25.6667 21.6673C20.6667 21.334 16.1667 19.0007 13.1667 15.334C12.6667 16.1673 12.3333 17.334 12.3333 18.334C12.3333 20.5007 13.3333 22.334 15 23.334C14 23.334 13.1667 23.0007 12.3333 22.6673C12.3333 25.6673 14.3333 28.0007 17.1667 28.5007C16.6667 28.6673 16.1667 28.6673 15.5 28.6673C15.1667 28.6673 14.6667 28.6673 14.3333 28.5007C15.1667 30.834 17.3333 32.6673 20 32.6673C18 34.334 15.3333 35.1673 12.5 35.1673C12 35.1673 11.5 35.1673 11 35.0007C13.8333 37.1673 17 38.1673 20.5 38.1673Z" fill="black"/>
</svg>"""

    let TwitterButton2 =
      """<svg class="twitter-logo" viewBox="0 0 27 21" fill="none" xmlns="http://www.w3.org/2000/svg">
<path d="M9.19034 21C18.9769 21 24.2353 12.9231 24.2353 5.87413C24.2353 5.58042 24.2353 5.43357 24.2353 5.13986C25.2578 4.40559 26.1342 3.52447 26.8645 2.34965C25.9881 2.79021 24.9656 3.08392 23.7971 3.23077C24.9656 2.64336 25.696 1.46853 26.1342 0.293706C25.1117 0.881119 23.9432 1.32168 22.7746 1.61538C21.7521 0.587412 20.4375 0 18.8308 0C15.9094 0 13.5724 2.34965 13.5724 5.28671C13.5724 5.72727 13.5724 6.16783 13.7184 6.46154C9.33641 6.16783 5.39259 4.11189 2.76338 0.881118C2.32518 1.61538 2.03304 2.64336 2.03304 3.52448C2.03304 5.43357 2.90945 7.04895 4.37012 7.93007C3.49372 7.93007 2.76338 7.63636 2.03304 7.34266C2.03304 9.98601 3.78585 12.042 6.269 12.4825C5.83079 12.6294 5.39259 12.6294 4.80832 12.6294C4.51619 12.6294 4.07799 12.6294 3.78585 12.4825C4.51619 14.5385 6.41506 16.1538 8.75214 16.1538C6.99933 17.6224 4.66226 18.3566 2.17911 18.3566C1.74091 18.3566 1.3027 18.3566 0.864502 18.2098C3.34765 20.1189 6.12293 21 9.19034 21Z" fill="white"/>
</svg>"""

    let PlayMovieMini =
      """<svg class="watch-movie" viewBox="0 0 67 67" fill="none" xmlns="http://www.w3.org/2000/svg">
<path class="circle" d="M33.4 65.8C51.294 65.8 65.8 51.294 65.8 33.4C65.8 15.506 51.294 1 33.4 1C15.506 1 1 15.506 1 33.4C1 51.294 15.506 65.8 33.4 65.8Z" stroke="white" stroke-width="2" stroke-miterlimit="10"/>
<path class="play" d="M28.0996 44.8L44.8996 33.4L28.0996 22V44.8Z" fill="white"/>
</svg>"""

    let LanguageButton =
      """<svg class="language-button" viewBox="0 0 130 30" fill="none" xmlns="http://www.w3.org/2000/svg">
<rect vector-effect="non-scaling-stroke" x="1" y="1" width="128" height="28" stroke="white" stroke-width="2"/>
<rect class="slider" vector-effect="non-scaling-stroke" x="0" y="1" width="65" height="28" fill="white" stroke-width="2"/>
<path class="label-jp" d="M24.916 22.216C27.4 22.216 28.606 20.956 28.606 18.778V10.408H29.92V9.436H24.79V10.408H26.464V19.282C26.464 20.686 25.924 21.28 24.898 21.28C24.574 21.28 24.286 21.244 24.07 21.19V21.118C24.34 21.01 24.592 20.668 24.592 20.2C24.592 19.552 24.142 19.084 23.476 19.084C22.702 19.084 22.252 19.66 22.252 20.344C22.252 21.442 23.134 22.216 24.916 22.216ZM31.548 22H36.714V21.028H35.004V16.366H37.794C40.206 16.366 41.592 15.07 41.592 12.838C41.592 10.624 40.296 9.436 37.884 9.436H31.548V10.408H32.862V21.028H31.548V22ZM35.004 10.498H37.398C38.658 10.498 39.378 11.29 39.378 12.478V13.324C39.378 14.512 38.658 15.304 37.398 15.304H35.004V10.498Z" fill="black"/>
<path class="label-en" d="M83.882 22H93.674V18.598H92.036V20.938H87.338V16.024H90.398V17.302H91.586V13.684H90.398V14.962H87.338V10.498H91.856V12.604H93.494V9.436H83.882V10.408H85.196V21.028H83.882V22ZM95.5715 22H99.3695V21.028H98.0555V11.326H98.1455L104.895 22H106.713V10.408H108.027V9.436H104.229V10.408H105.543V19.03H105.453L99.3875 9.436H95.5715V10.408H96.8855V21.028H95.5715V22Z" fill="white"/>
</svg>
"""
    let Obake =
      """<svg class="obake" viewBox="0 0 347 214" fill="none" xmlns="http://www.w3.org/2000/svg">
<path class="frame" d="M2 165.145V2H298.051L344.865 48.8554V212H48.8133L2 165.145Z" stroke="white" stroke-width="3.2744" stroke-miterlimit="10"/>
<g class="body">
  <path d="M284.476 185.625V165.544L268.406 149.459V128.279H183.962H99.5185V149.459L83.4482 165.544V185.625" stroke="white" stroke-width="3.7037" stroke-miterlimit="10"/>
  <g class="tie">
    <path d="M199.234 187.824L194.742 141.068H173.282L168.79 187.824H199.234Z" stroke="white" stroke-width="3.7037" stroke-miterlimit="10"/>
    <path d="M171.085 164.447H196.937" stroke="white" stroke-width="3.7037" stroke-miterlimit="10"/>
  </g>
</g>
<g class="head">
  <path d="M239.759 117.79C240.857 113.394 241.356 108.899 241.356 104.203C241.356 72.4334 215.603 46.7578 183.962 46.7578C152.321 46.7578 126.568 72.5333 126.568 104.203C126.568 108.899 127.167 113.394 128.165 117.79" stroke="white" stroke-width="3.7037" stroke-miterlimit="10"/>
  <path class="eye-l" d="M146.631 93.1133H167.991" stroke="white" stroke-width="3.7037" stroke-miterlimit="10"/>
  <path class="eye-r" d="M199.933 93.1133H221.293" stroke="white" stroke-width="3.7037" stroke-miterlimit="10"/>
</g>
<g class="icon">
  <path d="M41.5274 64.3418C54.0962 64.3418 64.2852 54.1436 64.2852 41.5635C64.2852 28.9834 54.0962 18.7852 41.5274 18.7852C28.9586 18.7852 18.7695 28.9834 18.7695 41.5635C18.7695 54.1436 28.9586 64.3418 41.5274 64.3418Z" stroke="white" stroke-width="2.4653" stroke-miterlimit="10"/>
  <path d="M41.5264 40.564C44.9443 40.564 47.715 37.7908 47.715 34.3699C47.715 30.949 44.9443 28.1758 41.5264 28.1758C38.1086 28.1758 35.3379 30.949 35.3379 34.3699C35.3379 37.7908 38.1086 40.564 41.5264 40.564Z" fill="white"/>
  <path d="M41.5267 51.6525C45.551 51.6525 48.8132 48.3873 48.8132 44.3595C48.8132 40.3316 45.551 37.0664 41.5267 37.0664C37.5025 37.0664 34.2402 40.3316 34.2402 44.3595C34.2402 48.3873 37.5025 51.6525 41.5267 51.6525Z" fill="white"/>
  <path d="M48.913 44.3594H34.2402V63.2414H48.913V44.3594Z" fill="white"/>
</g>
</svg>"""

  module StaticGallery =
    let Photos = [|
      for i = 0 to 13 do
        sprintf "assets/gallery_static/%d.jpg" i
    |]

[<StringEnum>]
type Language = Unspecified | En | Ja with
  static member Flip = function Unspecified -> Unspecified | En -> Ja | Ja -> En
  member this.AsLangCode =
    match this with
    | Unspecified | En -> "en"
    | Ja -> "ja"

module Credits =
  let Entries =
    [ [ Some ("Director", ["0b4k3"; "Rintaro"])
        Some ("Environment Artist", ["rakurai"])
        Some ("GI Architect", ["phi16"])
        Some ("VJ Architect", ["fotfla"])
        Some ("Generalist", ["Reflex"])
        Some ("Wiring Artist", ["tanitta"])
        Some ("Graphics Designer", ["Daiya Tanabe"; "k0nest"]) ]
      [ Some ("Gardener", ["amanek"])
        Some ("Builder", ["free458679"])
        Some ("Instrument Artist", ["Cap"])
        Some ("Translator & Web Architect", ["cannorin"])
        Some ("Video Animation & Video Music", ["Billain"])
        Some ("Production Assistant", ["minawa"])
        Some ("Photographer", ["MANE"; "tingaara_sora"; "Finn·"]) ] ]

  let pp (x: (string * string list) option) =
    match x with
    | Some (title, persons) ->
      Some (sprintf "%s:\u2002%s" title (persons |> String.concat " / "))
    | None -> None

module Links =
  let [<Literal>] VimeoMovie = "https://vimeo.com/551444345"
  let [<Literal>] Twitter = "https://twitter.com/6h057clu8"
  let [<Literal>] MixCloudWidget = "https://www.mixcloud.com/widget/iframe/?hide_cover=1&feed=%2F0bake%2Fplaylists%2Fghostclub%2F"
  let [<Literal>] Discord = "https://discord.gg/9KpCdUW"
  let [<Literal>] Contact = "https://docs.google.com/forms/d/e/1FAIpQLSdKU2PixQJ1TMyWtZuukNiB39vVnstvA_vKV5PxULDKGMO4wg/viewform"

[<StringEnum; RequireQualifiedAccess>]
type Texts =
  | [<CompiledName("translation:about")>] About
  | [<CompiledName("translation:how-to-join")>] HowToJoin
