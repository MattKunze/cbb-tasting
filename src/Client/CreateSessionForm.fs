[<RequireQualifiedAccess>]
module CreateSessionForm

open System

open Feliz
open Helpers
open Shared

type Model =
    { Date : string option
      Styles : string option }

type Msg =
    | SetDate of string
    | SetStyles of string
    | ParseDate
    | Submit

type ExternalMsg =
    | Noop // :P
    | SessionCreated of Tasting.Session

let init() =
    { Date = None
      Styles = None }

let private (|DateTime|_|) str =
    match DateTime.TryParse(str |> Option.defaultValue "") with
    // TODO - this ends up with "Invalid Date" in many cases
    | true, dt -> Some(dt)
    | _ -> None

let private toSession model : Tasting.Session option =
    match model.Date, model.Styles with
    | DateTime dt, Some styles ->
        Some {
            Id = SessionId.create;
            Date = dt;
            Styles = BeerStyle styles;
            Judges = []
        }
    | _ -> None

let private handleInput (str : string) =
    match str with
    | "" -> None
    | _ -> Some str

let update model msg =
    match msg with
    | SetDate update -> { model with Date = handleInput update }, Noop
    | SetStyles update -> { model with Styles = handleInput update }, Noop
    | ParseDate ->
        printf "ParseDate"
        { model with
            Date =
                match model.Date with
                | DateTime dt -> Some (dt.ToString "yyyy/MM/dd")
                | _ -> None
        }, Noop
    | Submit ->
        match toSession model with
        | Some session -> model, SessionCreated session
        // TODO - show validation errors
        | None -> model, Noop

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
    if ev.which = 13.0 && target = "[object HTMLInputElement]" then
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
                        { Label = "Date"
                          Value = model.Date
                          Placeholder = Some "yyyy/mm/dd or whatever"
                          OnChange = (SetDate >> dispatch)
                          OnBlur = Some (fun _ -> ParseDate |> dispatch) }

                    Controls.inputField
                        { Label = "Styles"
                          Value = model.Styles
                          Placeholder = None
                          OnChange = (SetStyles >> dispatch)
                          OnBlur = None }

                    formControls dispatch ]

                prop.onKeyPress (submitOnEnter dispatch) ] ] ]


let toDomain (model : Model) : Result<Tasting.Session, string> =
    Error "Not working yet"
