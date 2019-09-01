[<RequireQualifiedAccess>]
module SessionForm

open System

open Elmish
open Feliz
open Helpers
open Shared

[<RequireQualifiedAccess>]
type FormKey = Date | Styles

type Model = { Bucket : Map<FormKey, string> }

type Msg =
    | Update of FormKey * string
    | ParseDate
    | Submit

type ExternalMsg =
    | SessionCreated of Tasting.Session

let init() = { Bucket = Map.empty }

let private (|DateTime|_|) str =
    match DateTime.TryParse(str |> Option.defaultValue "") with
    // TODO - this ends up with "Invalid Date" in many cases
    | true, dt -> Some(dt)
    | _ -> None

let private toSession model : Tasting.Session option =
    match model.Bucket.TryFind FormKey.Date, model.Bucket.TryFind FormKey.Styles with
    | DateTime dt, Some styles ->
        Some {
            Id = SessionId.create;
            Date = dt;
            Styles = BeerStyle styles;
            Judges = []
        }
    | _ -> None

let update model msg =
    printf "form update %A" msg
    match msg with
    | Update (key, value) ->
        match Validation.clean value with
        | Some value ->
            { model with Bucket = model.Bucket |> Map.add key value }, Cmd.none
        | None ->
            { model with Bucket = model.Bucket |> Map.remove key }, Cmd.none
    | ParseDate ->
        printf "ParseDate"
        match model.Bucket |> Map.tryFind FormKey.Date with
        | DateTime dt ->
            let formatted = dt.ToString "yyyy/MM/dd"
            { model with Bucket = model.Bucket |> Map.add FormKey.Date formatted }, Cmd.none
        | _ ->
            { model with Bucket = model.Bucket |> Map.remove FormKey.Date }, Cmd.none
    | Submit ->
        match toSession model with
        | Some session -> model, Cmd.ofMsg (SessionCreated session)
        // TODO - show validation errors
        | None -> model, Cmd.none

let private formControls dispatch =
    Html.div [
        prop.classes [ Css.Bulma.Buttons; Css.Bulma.IsRight ]
        prop.children [
            Html.button [
                prop.classes [ Css.Bulma.Button; Css.Bulma.IsPrimary ]
                prop.children [ Html.text "Submit" ]
                prop.onClick (fun _ -> Submit |> dispatch) ] ] ]

let private submitOnEnter dispatch (ev : Browser.Types.KeyboardEvent) =
    // TODO - seems like a crappy way to figure out the type of the target
    // element but not seeing another way to accomplish this at the moment
    let target = ev.target.ToString()
    if ev.key = "Enter" && target = "[object HTMLInputElement]" then
        Submit |> dispatch

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
                        { Type = Controls.Text
                          Label = "Date"
                          Value = model.Bucket |> Map.tryFind FormKey.Date
                          Placeholder = Some "yyyy/mm/dd or whatever"
                          OnChange = (fun update -> Update (FormKey.Date, update) |> dispatch)
                          OnBlur = Some (fun _ -> ParseDate |> dispatch) }

                    Controls.inputField
                        { Type = Controls.Text
                          Label = "Styles"
                          Value = model.Bucket |> Map.tryFind FormKey.Styles
                          Placeholder = None
                          OnChange = (fun update -> Update (FormKey.Styles, update) |> dispatch)
                          OnBlur = None }

                    formControls dispatch ]

                prop.onKeyPress (submitOnEnter dispatch) ] ] ]


let toDomain (model : Model) : Result<Tasting.Session, string> =
    Error "Not working yet"
