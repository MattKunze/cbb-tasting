[<RequireQualifiedAccess>]
module EvaluationForm

open Elmish
open Feliz
open Helpers
open Shared

[<RequireQualifiedAccess>]
type FormKey = Comments | BreweryName | BeerName

type Model = {
    Session: Tasting.Session
    Bucket : Map<FormKey, string>
}

type Msg =
    | Update of FormKey * string
    | Cancel
    | Submit

type ExternalMsg =
    | EvaluationCanceled
    | EvaluationEntered of Tasting.Evaluation

let init session =
    { Session = session
      Bucket = Map.empty }

let private toEvaluation model : Tasting.Evaluation option =
    match model.Bucket.TryFind FormKey.Comments,
          model.Bucket.TryFind FormKey.BreweryName,
          model.Bucket.TryFind FormKey.BeerName with
    | Some comments, Some breweryName, Some beerName ->
        Some {
            SessionId = model.Session.Id;
            JudgeName = JudgeName "hrmmm";
            Comments = comments;
            BreweryName = BreweryName breweryName;
            BeerName = BeerName beerName
        }
    | _ -> None

let update model msg =
    match msg with
    | Update (key, value) ->
        match Validation.clean value with
        | Some value ->
            { model with Bucket = model.Bucket |> Map.add key value }, Cmd.none
        | None ->
            { model with Bucket = model.Bucket |> Map.remove key }, Cmd.none
    | Cancel -> model, Cmd.ofMsg EvaluationCanceled
    | Submit ->
        match toEvaluation model with
        | Some evaluation -> model, Cmd.ofMsg (EvaluationEntered evaluation)
        | None -> model, Cmd.none

let private formControls dispatch =
    Html.div [
        prop.classes [ Css.Bulma.Buttons; Css.Bulma.IsRight ]
        prop.children [
            Html.button [
                prop.classes [ Css.Bulma.Button ]
                prop.children [ Html.text "Cancel" ]
                prop.onClick (fun _ -> Cancel |> dispatch) ]
            Html.button [
                prop.classes [ Css.Bulma.Button; Css.Bulma.IsPrimary ]
                prop.children [ Html.text "Submit" ]
                prop.onClick (fun _ -> Submit |> dispatch) ] ] ]

let view model (dispatch: Msg -> unit) =
    Html.div [
        prop.className Css.Bulma.Container
        prop.children [
            Html.h2 [
                prop.className Css.Bulma.Subtitle
                prop.children [ Html.text "Enter Evaluation" ] ]

            Html.fieldSet [
                prop.children [
                    Controls.inputField
                        { Type = Controls.LongText
                          Label = "Comments"
                          Value = model.Bucket |> Map.tryFind FormKey.Comments
                          Placeholder = None
                          OnChange = (fun update -> Update (FormKey.Comments, update) |> dispatch)
                          OnBlur = None }

                    Controls.inputField
                        { Type = Controls.Text
                          Label = "Brewery Name"
                          Value = model.Bucket |> Map.tryFind FormKey.BreweryName
                          Placeholder = None
                          OnChange = (fun update -> Update (FormKey.BreweryName, update) |> dispatch)
                          OnBlur = None }

                    Controls.inputField
                        { Type = Controls.Text
                          Label = "Beer Name"
                          Value = model.Bucket |> Map.tryFind FormKey.BeerName
                          Placeholder = None
                          OnChange = (fun update -> Update (FormKey.BeerName, update) |> dispatch)
                          OnBlur = None }

                    formControls dispatch ] ] ] ]
