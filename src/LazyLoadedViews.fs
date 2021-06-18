module LazyLoadedViews

open Properties
open Model
open View
open Api

open Fable.React

type Props = {|
  dispatch: (Msg -> unit)
  lang: Language
  flags: Set<Flag>
  api: IResult<All>
|}

let view =
  FunctionComponent.Of((fun (props: Props) ->
    ofList [
      Transition.view {| dispatch = props.dispatch |}
      Menu.view {| lang = props.lang; flags = props.flags; dispatch = props.dispatch |}
      Content.view {| lang = props.lang; api = props.api; canUseWebP = props.flags |> Set.contains CanUseWebP; dispatch = props.dispatch |}
      Footer.view
    ]
  ), memoizeWith=memoEqualsButFunctions)