namespace Fable.React

type FunctionComponent'<'Props> = 'Props -> Fable.React.ReactElement

type ComponentType<'Props> = Fable.React.ReactElementType<'Props>

type [<AllowNullLiteral>] HTMLProps<'T> = interface end

type [<AllowNullLiteral>] FunctionComponentElement<'T> =
    inherit Fable.React.ReactElement

type [<AllowNullLiteral>] ReactPortal =
    inherit Fable.React.ReactElement
    abstract children: Fable.React.ReactElement with get

type [<AllowNullLiteral>] ReactNode =
    inherit Fable.React.ReactElement

type ComponentClass<'T> =
    inherit ComponentType<'T>