module App

open Elmish
open Fable.React
open Feliz
open Helpers
open Shared

type Page =
    | Loading
    | CreateSession of CreateSessionForm.Model

type Model =
    { ActivePage : Page }

type Msg =
    | BeginNewSession
    | CreateSessionMsg of CreateSessionForm.Msg

let private delay timeout =
    async {
        do! Async.Sleep timeout
        return ()
    }

let init() : Model * Cmd<Msg> =
    { ActivePage = Loading },
    Cmd.OfAsync.perform delay 1000 (fun _ -> BeginNewSession)

let withBlankSession model =
    let blankSession = CreateSessionForm.init()
    { model with ActivePage = CreateSession blankSession }

let update msg model =
    match msg, model.ActivePage with
    | BeginNewSession, _ -> (withBlankSession model), Cmd.none
    | CreateSessionMsg childMsg, CreateSession sessionModel ->
        let update = CreateSessionForm.update sessionModel childMsg
        if (childMsg = CreateSessionForm.Msg.CreateSession) then
            printf "also create somehow %A" sessionModel

        { model with ActivePage = CreateSession update }, Cmd.none
    | _ -> model, Cmd.none

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
        | Loading -> Html.text "Loading..."
        | CreateSession sessionModel ->
            CreateSessionForm.view sessionModel (CreateSessionMsg >> dispatch)
    layout [ title; body ]
