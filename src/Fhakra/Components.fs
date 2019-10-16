module Components

open Fable.Core
open Fable.React

open Shared

[<RequireQualifiedAccess>]
type BadgeProps = VariantColor of string

let badge (props: BadgeProps list) (text: string) =
    let props = props |> mkProps
    ofImport "Badge" "@chakra-ui/core" props [ str text ]

[<StringEnum>]
type ButtonVariant =
    | [<CompiledName "solid">] Solid
    | [<CompiledName "ghost">] Ghost
    | [<CompiledName "outline">] Outline
    | [<CompiledName "link">] Link

[<RequireQualifiedAccess>]
type ButtonProps =
    | Variant of ButtonVariant
    | VariantColor of string
    | LeftIcon of string
    | RightIcon of string
    | IsLoading
    | OnClick of (unit -> unit)

let button (props: ButtonProps list) (text: string) =
    let props = props |> mkProps
    ofImport "Button" "@chakra-ui/core" props [ str text ]

let private mkGroupButton (props: ButtonProps list, text: string) = button props text

let buttonGroup (buttons: List<ButtonProps list * string>) =
    let buttons = buttons |> List.map mkGroupButton
    ofImport "ButtonGroup" "@chakra-ui/core" undefined buttons
