module Demo

open Fable.React
open Browser

open Components
open Layout
open Shared
open Theme

type DemoProps =
    { title: string }

let private Content =
    FunctionComponent.Of
    <| fun (props: DemoProps) ->
        let (colorMode, toggleColorMode) = useColorMode()

        container []
            [ div []
                  [ str (sprintf "%s - mode %A - " props.title colorMode)
                    badge [ BadgeProps.VariantColor "pink" ] "Badge?" ]
              div []
                  [ button
                      [ ButtonProps.VariantColor "teal"
                        ButtonProps.Variant Solid
                        ButtonProps.RightIcon
                            (match colorMode with
                             | ColorMode.Light -> "moon"
                             | ColorMode.Dark -> "sun")
                        ButtonProps.OnClick toggleColorMode ]
                        (match colorMode with
                         | ColorMode.Light -> "Light mode"
                         | ColorMode.Dark -> "Dark mode") ]
              div [] [ str nbsp ]
              div []
                  [ buttonGroup
                      [ ([ ButtonProps.VariantColor "warning" ], "Ready")
                        ([ ButtonProps.VariantColor "danger" ], "Fire")
                        ([ ButtonProps.VariantColor "success" ], "Aim") ] ]
              div []
                  [ str "maybe an icon?"
                    icon (Icon.Default "link") ] ]

let demo (props: DemoProps) = semanticThemeProvider [ Content props ]
