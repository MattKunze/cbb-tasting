[<RequireQualifiedAccess>]
module SessionDetails

open System

open Feliz
open Helpers
open Shared

type Model = {
    Session: Tasting.Session
}

type Msg =
| EndSession

let init session = {
    Session = session
}

let update model msg =
    match msg with
    | EndSession -> model

let view model (dispatch: Msg -> unit) =
    let (BeerStyle style) = model.Session.Styles
    let date = model.Session.Date.ToString("d")

    Html.div [
        prop.className Css.Bulma.Subtitle
        prop.children [
            Html.div [
                prop.className Css.Bulma.Subtitle
                prop.children [ Html.text (sprintf "Session: %s @ %s" style date) ]
            ]
            Html.div [
                prop.classes [ Css.Bulma.Buttons; Css.Bulma.IsRight ]
                prop.children [
                    Html.button [
                        prop.classes [ Css.Bulma.Button; Css.Bulma.IsPrimary ]
                        prop.children [ Html.text "End it"]
                        prop.onClick (fun _ -> EndSession |> dispatch)
                    ]
                ]
            ]
        ]
    ]
