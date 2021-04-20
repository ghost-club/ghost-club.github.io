module AnimatedText

open Fable.React
open Fable.React.Props
open ReactMotion

type SlotProps = {| targetText: string; period: string; visible: bool |}

let private round (length: float) =
  int (System.Math.Round(length, System.MidpointRounding.AwayFromZero))

let [<Literal>] private alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"

let slot : SlotProps -> ReactElement =
  FunctionComponent.Of((fun (props: SlotProps) ->
    let textLen = props.targetText.Length
    if not props.visible then
      span [Style [Opacity 0.0]] [str props.targetText]
    else
      motion [
        MotionProp.DefaultStyle {| targetLength = 0.0; totalLength = 0.0; |}
        MotionProp.Style
          {| targetLength = opaqueConfig (float textLen) 400 100 0.1
             totalLength  = opaqueConfig (float textLen) 450 100 0.1 |}
      ] <| fun style ->
        let totalLen = min (round style.totalLength + 1) textLen
        let targetLen = min (round style.targetLength + 1) textLen
        let target = props.targetText.Substring(0, targetLen)
        let rest =
          if totalLen = targetLen then props.period
          else
            String.init (max 0 (totalLen - targetLen)) (fun _ ->
              string (alphabets.[round (float alphabets.Length * Fable.Core.JS.Math.random())])
            )
        span [] [ str (target + rest) ]
  ), memoizeWith=memoEqualsButFunctions)