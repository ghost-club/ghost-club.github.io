[<AutoOpen>]
module Extensions

module Fulma =
  open Fable.React
  open Fulma

  [<RequireQualifiedAccess>]
  module Block =
    let block (options: GenericOption list) children =
      GenericOptions.Parse(options, parseOptions, "block").ToReactElement(div, children)
