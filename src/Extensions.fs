(*
Various extensions for Fable, Fable.React and Fulma

Copyright 2021 cannorin

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*)

[<AutoOpen>]
module Extensions

open System.Runtime.InteropServices
open Fable.Core
open Fable.Core.JsInterop

module Fulma =
  open Fable.React
  open Fulma

  module Screen =
    open Browser.Dom
    open Browser.MediaQueryListExtensions
    type Screen = Fulma.Screen

    let inline check (screen: Screen) =
      match screen with
      | Screen.All -> true
      | Screen.Touch -> window.matchMedia("(max-width: 1023.99px)").matches
      | Screen.Mobile -> window.matchMedia("(max-width: 768.99px)").matches
      | Screen.Tablet -> window.matchMedia("(min-width: 769px)").matches
      | Screen.Desktop -> window.matchMedia("(min-width: 1024px)").matches
      | Screen.WideScreen -> window.matchMedia("(min-width: 1216px)").matches
      | Screen.FullHD -> window.matchMedia("(min-width: 1408px)").matches

    let inline checkOnly (screen: Screen) =
      match screen with
      | Screen.All -> true
      | Screen.Touch -> window.matchMedia("(max-width: 1023.99px)").matches
      | Screen.Mobile -> window.matchMedia("(max-width: 768.99px)").matches
      | Screen.Tablet -> window.matchMedia("(min-width: 769px) and (max-width: 1023.99px)").matches
      | Screen.Desktop -> window.matchMedia("(min-width: 1024px) and (max-width: 1215.99px)").matches
      | Screen.WideScreen -> window.matchMedia("(min-width: 1216px) and (max-width: 1407.99px)").matches
      | Screen.FullHD -> window.matchMedia("(min-width: 1408px)").matches


  [<RequireQualifiedAccess>]
  module Block =
    let inline block (options: GenericOption list) children =
      GenericOptions.Parse(options, parseOptions, "block").ToReactElement(div, children)

  type [<Erase>] Option =
    static member inline CustomClass (x, [<Optional>]__: GenericOption) = CustomClass x
    static member inline CustomClass (x, [<Optional>]__: Section.Option) = Section.CustomClass x
    static member inline CustomClass (x, [<Optional>]__: Container.Option) = Container.CustomClass x
    static member inline CustomClass (x, [<Optional>]__: Columns.Option) = Columns.CustomClass x
    static member inline CustomClass (x, [<Optional>]__: Column.Option) = Column.CustomClass x
    static member inline Props (x, [<Optional>]__: GenericOption) = Props x
    static member inline Props (x, [<Optional>]__: Section.Option) = Section.Props x
    static member inline Props (x, [<Optional>]__: Container.Option) = Container.Props x
    static member inline Props (x, [<Optional>]__: Columns.Option) = Columns.Props x
    static member inline Props (x, [<Optional>]__: Column.Option) = Column.Props x
    static member inline Modifiers (x, [<Optional>]__: GenericOption) = Modifiers x
    static member inline Modifiers (x, [<Optional>]__: Section.Option) = Section.Modifiers x
    static member inline Modifiers (x, [<Optional>]__: Container.Option) = Container.Modifiers x
    static member inline Modifiers (x, [<Optional>]__: Columns.Option) = Columns.Modifiers x
    static member inline Modifiers (x, [<Optional>]__: Column.Option) = Column.Modifiers x
    static member inline CustomClass (x, [<Optional>]__: Hero.Option) = Hero.CustomClass x
    static member inline Props (x, [<Optional>]__: Hero.Option) = Hero.Props x
    static member inline Modifiers (x, [<Optional>]__: Hero.Option) = Hero.Modifiers x
    static member inline CustomClass (x, [<Optional>]__: Button.Option) = Button.CustomClass x
    static member inline Props (x, [<Optional>]__: Button.Option) = Button.Props x
    static member inline Modifiers (x, [<Optional>]__: Button.Option) = Button.Modifiers x
    static member inline CustomClass (x, [<Optional>]__: Heading.Option) = Heading.CustomClass x
    static member inline Props (x, [<Optional>]__: Heading.Option) = Heading.Props x
    static member inline Modifiers (x, [<Optional>]__: Heading.Option) = Heading.Modifiers x

  let inline CustomClass (str: string) : ^Option =
    let inline call_2 (x: ^X, _: ^Y, arg) = ((^X or ^Y): (static member CustomClass: string * ^X -> ^X) arg,x)
    let inline call (x: 'X, y: 'Y, arg) = call_2 (x, y, arg)
    call (Unchecked.defaultof< ^Option >, Unchecked.defaultof<Option>, str)

  let inline Props (props: Props.IHTMLProp list) : ^Option =
    let inline call_2 (x: ^X, _: ^Y, arg) = ((^X or ^Y): (static member Props: Props.IHTMLProp list * ^X -> ^X) arg,x)
    let inline call (x: 'X, y: 'Y, arg) = call_2 (x, y, arg)
    call (Unchecked.defaultof< ^Option >, Unchecked.defaultof<Option>, props)

  let inline Modifiers (modifiers: Modifier.IModifier list) : ^Option =
    let inline call_2 (x: ^X, _: ^Y, arg) = ((^X or ^Y): (static member Modifiers: Modifier.IModifier list * ^X -> ^X) arg,x)
    let inline call (x: 'X, y: 'Y, arg) = call_2 (x, y, arg)
    call (Unchecked.defaultof< ^Option >, Unchecked.defaultof<Option>, modifiers)

module Fable =
  module React =
    open Fable.React
    open Fable.React.Props

    module Props =
      module Key =
        let inline Src (__source_file__: string, __line__: string) : Prop = Key (__source_file__ + ":" + __line__)

module Promise =
  [<Emit("Promise.race($0)")>]
  let race (pr: seq<JS.Promise<'T>>) : JS.Promise<'T> = jsNative
