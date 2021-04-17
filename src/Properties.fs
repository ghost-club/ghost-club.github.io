module Properties

open Fable.Core

[<Literal>]
let DomainNameInPunyCode = "xn--pckjp4dudxftf.xn--tckwe"

[<StringEnum>]
type Language = Unspecified | En | Ja with
  static member Flip = function Unspecified -> Unspecified | En -> Ja | Ja -> En

[<StringEnum; RequireQualifiedAccess>]
type UITexts =
  | [<CompiledName("ui:change-to-another-language")>] ChangeToAnotherLanguage

[<Literal>]
let LoremIpsum = """Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."""

[<Literal>]
let LoremIpsumJp = """対象は一つBYと投稿応じコードななため、出所できれる事典が公開者公式の引用要件をしれてはしある、theの作品も、著作する有償を前記よれこととして著作自由なたていますた。
しかし、記事の引用権は、趣旨の引用する利用自由ある文章を採録する、その最小限をありのでコンテンツを著作よれことに削除問いれます。
およびを、手続財団を著作されるているライセンスとごく満たさ扱うことは、許諾なます、過去におけるは用意者の紹介として方針上の問題も有しことに、被説明物も、適法の著作にさて政治から許諾しなているませた。
例証さて、それの削除はなくなどするませな。
または、被策定会を、編集有し他の記事、フリーで必要に注意よれので基づきて、ライセンス見解の該当が記事を判断とどめことで掲げるて、抜粋用いますペディアを運用、引用者引用なけれですとの引用をすることは、少なくとも厳しいとさてよいんで。"""