module Fhakra

open Fable.React
open Browser

open Demo

let DemoComponent = FunctionComponent.Of demo

let render() =
    let mountPoint = document.querySelector "#app-root"
    ReactDom.render (DemoComponent { title = "hello fhakra" }, mountPoint)

render()
