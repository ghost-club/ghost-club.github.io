(*
Fable binding for intersection-observer

Copyright 2021 cannorin

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*)

module rec IntersectionObserver

open System
open Browser.Types
open Fable.Core
open Fable.Core.JS

type ReadonlyArray<'T> = System.Collections.Generic.IReadOnlyList<'T>

let [<Import("*", "intersection-observer")>] intersectionObserver : IExports = jsNative

type [<AllowNullLiteral>] IExports =
    abstract DOMRect: DOMRectStaticBase<DOMRect>
    abstract DOMRectReadOnly: DOMRectStaticBase<DOMRectReadOnly>
    abstract IntersectionObserverEntry: IntersectionObserverEntryStatic
    abstract IntersectionObserver : IntersectionObserverStatic

type [<AllowNullLiteral>] IStatic<'T> =
    abstract prototype: 'T

type [<AllowNullLiteral>] DOMRectInit =
    abstract height: float option with get, set
    abstract width: float option with get, set
    abstract x: float option with get, set
    abstract y: float option with get, set

type [<AllowNullLiteral>] DOMRectReadOnly =
    abstract botton: float
    abstract height: float
    abstract left: float
    abstract right: float
    abstract top: float
    abstract width: float
    abstract x: float with get
    abstract y: float with get
    abstract toJSON: unit -> obj

type [<AllowNullLiteral>] DOMRect =
    inherit DOMRectReadOnly
    abstract x: float with get, set
    abstract y: float with get, set

type [<AllowNullLiteral>] DOMRectStaticBase<'DOMRect> =
    inherit IStatic<'DOMRect>
    [<Emit "new $0($1...)">] abstract Create: ?x:float * ?y:float * ?width:float * ?height:float -> 'DOMRect
    abstract fromRect: ?other: DOMRectInit -> 'DOMRect

type [<AllowNullLiteral>] IntersectionObserverEntryInit =
    abstract boundingClientRect: DOMRectInit with set
    abstract intersectionRatio: float with set
    abstract intersectionRect: DOMRectInit with set
    abstract isIntersecting: bool with set
    abstract rootBounds: DOMRectInit with set
    abstract target: Element with set
    abstract time: float with set

type [<AllowNullLiteral>] IntersectionObserverEntry =
    abstract boundingClientRect: DOMRect with get
    abstract intersectionRatio: float with get
    abstract intersectionRect: DOMRect with get
    abstract isIntersecting: bool with get
    abstract rootBounds: DOMRect option with get
    abstract target: Element with get
    abstract time: float with get

type [<AllowNullLiteral>] IntersectionObserverEntryStatic =
    inherit IStatic<IntersectionObserverEntry>
    [<Emit "new $0($1...)">] abstract Create: IntersectionObserverEntryInit -> IntersectionObserverEntry

type [<AllowNullLiteral>] IntersectionObserverInit<'Root> =
    abstract root: 'Root with set
    abstract rootMargin: string with set
    abstract threshold: U2<float, ResizeArray<float>> with set

type IntersectionObserverInit = IntersectionObserverInit<U2<Element, Document>>

type IntersectionObserverCallback<'Root> =
    IntersectionObserverEntry[] -> IntersectionObserver<'Root> -> unit

type [<AllowNullLiteral>] IntersectionObserver<'Root> =
    abstract root: 'Root option with get
    abstract rootMargin: string with get
    abstract thresholds: ReadonlyArray<float> with get
    abstract disconnect: unit -> unit
    abstract observe: Element -> unit
    abstract takeRecords: unit -> IntersectionObserverEntry[]
    abstract unobserve: Element -> unit

type IntersectionObserver = IntersectionObserver<U2<Element, Document>>

type [<AllowNullLiteral>] IntersectionObserverStatic =
    inherit IStatic<IntersectionObserver<obj>>
    [<Emit "new $0($1...)">] abstract Create: callback:IntersectionObserverCallback<'Root> * ?options: IntersectionObserverInit<'Root> -> IntersectionObserver<'Root>
