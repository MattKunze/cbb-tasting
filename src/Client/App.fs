module App

open Elmish
open Fable.React
open Feliz

open Helpers
open Types

let private layout (children : ReactElement list) =
    Html.div [ prop.className Css.Bulma.Section
               prop.children [ Html.div [ prop.className Css.Bulma.Container
                                          prop.children children ] ] ]

let private title =
    Html.h2 [ prop.className Css.Bulma.Title
              prop.children [ Html.text "Might work..." ] ]

let view model dispatch =
    let body =
        match model.ActivePage with
        | Page.Loading -> Html.text "Loading..."
        | Page.Session sessionModel ->
            SessionForm.view sessionModel (SessionMsg >> dispatch)
        | Page.Details detailsModel ->
            SessionDetails.view detailsModel (DetailsMsg >> dispatch)
        | Page.Evaluation evaluationModel ->
            EvaluationForm.view evaluationModel (EvaluationMsg >> dispatch)

    layout [ title; body ]
