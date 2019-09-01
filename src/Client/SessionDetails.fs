[<RequireQualifiedAccess>]
module SessionDetails

open System

open Elmish
open Feliz
open Helpers
open Shared

type Model = {
    Session: Tasting.Session
}

type Msg =
| NewEvaluation
| EndSession

type ExternalMsg =
| EnterEvaluation
| SessionEnded

let init session = {
    Session = session
}

let update model msg =
    match msg with
    | NewEvaluation -> model, Cmd.ofMsg EnterEvaluation
    | EndSession -> model, Cmd.ofMsg SessionEnded

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
                        prop.children [ Html.text "New Eval"]
                        prop.onClick (fun _ -> NewEvaluation |> dispatch) ]
                    Html.button [
                        prop.classes [ Css.Bulma.Button; Css.Bulma.IsWarning ]
                        prop.children [ Html.text "End it"]
                        prop.onClick (fun _ -> EndSession |> dispatch) ] ] ] ] ]
