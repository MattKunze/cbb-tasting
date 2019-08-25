[<RequireQualifiedAccess>]
module CreateSessionForm

open Feliz
open Helpers
open Shared

type Model =
    { Date : string option
      Styles : string option }

type Msg =
    | SetDate of string
    | SetStyles of string
    | CreateSession

let init() =
    { Date = None
      Styles = None }

let update model msg =
    match msg with
    | SetDate update -> { model with Date = Some update }
    | SetStyles update -> { model with Styles = Some update }
    | CreateSession ->
        // todo - validate
        model

let private formControls dispatch =
    Html.div [
        prop.classes [ Css.Bulma.Buttons; Css.Bulma.IsRight ]
        prop.children [
            Html.button [
                prop.classes [ Css.Bulma.Button; Css.Bulma.IsPrimary ]
                prop.children [ Html.text "Submit" ]
                prop.onClick (fun _ -> CreateSession |> dispatch) ] ] ]

let view model (dispatch : Msg -> unit) =
    Html.div [
        prop.className Css.Bulma.Container
        prop.children [
            Html.h2 [
                prop.className Css.Bulma.Subtitle
                prop.children [ Html.text "Create Session" ] ]

            Html.fieldSet [
                prop.children [
                    Controls.inputField
                        { Label = "Date"
                          Value = model.Date
                          Placeholder = Some "yyyy/mm/dd or whatever"
                          OnChange = (SetDate >> dispatch) }

                    Controls.inputField
                        { Label = "Styles"
                          Value = model.Styles
                          Placeholder = None
                          OnChange = (SetStyles >> dispatch) }

                    formControls dispatch ] ] ] ]


let toDomain (model : Model) : Result<Tasting.Session, string> =
    Error "Not working yet"
