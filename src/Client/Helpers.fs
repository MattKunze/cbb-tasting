module Helpers

open Feliz
open Zanaptak.TypedCssClasses

module Bulma =
    type Css = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.5/css/bulma.min.css", Naming.PascalCase>

    module Dom =
        type SuccessColor =
            | Info
            | Success
            | Warning
            | Danger
            | None

        type NavbarAction =
            { Icon: string }

        type ToolbarProps =
            { Title: string }

        let successColorToClassName color =
            match color with
            | Info -> Css.HasTextInfo
            | Success -> Css.HasTextSuccess
            | Warning -> Css.HasTextWarning
            | Danger -> Css.HasTextDanger
            | None -> ""

        let colorIcon (label: string, color: SuccessColor) =
            Html.span
                [ prop.classes
                    [ Css.Icon
                      successColorToClassName color ]
                  prop.children [ Html.i [ prop.className (sprintf "fas fa-%s" label) ] ] ]

        let plainIcon (label: string) = colorIcon (label, None)

        let toolbarLabel (text: string) =
            Html.div
                [ prop.className Css.NavbarItem
                  prop.text text ]

        let toolbar (props: ToolbarProps) =
            Html.nav
                [ prop.classes [ Css.Navbar; Css.IsSpaced ]
                  prop.children
                      [ Html.div
                          [ prop.className Css.NavbarBrand
                            prop.children
                                [ Html.div
                                    [ prop.className Css.NavbarItem
                                      prop.children [ plainIcon "bath" ] ]
                                  Html.div
                                      [ prop.className Css.NavbarItem
                                        prop.children [ colorIcon ("bomb", Danger) ] ]
                                  toolbarLabel props.Title ] ] ] ]
