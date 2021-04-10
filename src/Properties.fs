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