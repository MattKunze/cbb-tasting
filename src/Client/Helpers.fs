module Helpers

open Feliz
open Zanaptak.TypedCssClasses

module Css =
    type Bulma = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.5/css/bulma.min.css", Naming.PascalCase>

module Controls =
    type InputDefinition =
        { Label : string
          Placeholder : string option
          Value : string option
          OnChange : string -> unit
          OnBlur : (unit -> unit) option }

    let private fieldLabel (label : string) =
        Html.label [
            prop.className Css.Bulma.Label
            prop.children [ Html.text label ] ]

    let private inputControl (definition : InputDefinition) =
        let placeholder = definition.Placeholder |> Option.defaultValue ""
        let value = definition.Value |> Option.defaultValue ""
        Html.div [
            prop.className Css.Bulma.Control

            prop.children [
                Html.input [
                    prop.className Css.Bulma.Input
                    prop.inputType "text"
                    prop.placeholder placeholder
                    prop.valueOrDefault value
                    prop.onTextChange definition.OnChange
                    prop.onBlur (fun _ ->
                        match definition.OnBlur with
                        | Some cb -> cb()
                        | None -> ()
                    ) ] ] ]

    let inputField (definition : InputDefinition) =
        Html.div [
            prop.className Css.Bulma.Field
            prop.children [
                fieldLabel definition.Label
                inputControl definition ] ]
