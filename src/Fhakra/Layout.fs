module Layout

open Fable.React
open Browser

open Shared

[<RequireQualifiedAccess>]
type LayoutProps =
    | MaxWidth of string
    | Margin of string
    | [<CompiledName "mx">] MarginX of string
    | [<CompiledName "my">] MarginY of string
    | Padding of string
    | [<CompiledName "px">] PaddingX of string
    | [<CompiledName "py">] PaddingY of string

let box (props: LayoutProps list) (children: ReactElement list) =
    let props = props |> mkProps
    console.warn props
    ofImport "Box" "@chakra-ui/core" props children

let container (props: LayoutProps list) (children: ReactElement list) =
    box
        [ LayoutProps.MaxWidth "800px"
          LayoutProps.MarginY "5"
          LayoutProps.MarginX "auto"
          LayoutProps.PaddingX "2" ] children
