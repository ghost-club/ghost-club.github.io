[<AutoOpen>]
module Extensions
open System.Runtime.InteropServices

module Fulma =
  open Fable.React
  open Fulma

  [<RequireQualifiedAccess>]
  module Block =
    let inline block (options: GenericOption list) children =
      GenericOptions.Parse(options, parseOptions, "block").ToReactElement(div, children)

  type Option =
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
    open Fable.React.Props

    module Props =
      module Key =
        let inline Src (__source_file__: string, __line__: string) : Prop = Key (__source_file__ + ":" + __line__)
