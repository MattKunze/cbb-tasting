module App

open Elmish
open Fable.React
open Feliz
open Helpers
open Shared

type Page =
    | Loading
    | CreateSession of CreateSessionForm.Model
    | ExistingSession of SessionDetails.Model

type Model =
    { ActivePage : Page }

type Msg =
    | BeginNewSession
    | CreateSessionMsg of CreateSessionForm.Msg
    | SessionDetailsMsg of SessionDetails.Msg

let private delay timeout =
    async {
        do! Async.Sleep timeout
        return ()
    }

let init() : Model * Cmd<Msg> =
    { ActivePage = Loading },
    Cmd.OfAsync.perform delay 1000 (fun _ -> BeginNewSession)

let withBlankSession =
    let blankSession = CreateSessionForm.init()
    { ActivePage = CreateSession blankSession }

let withCreateSessionHandler msg childModel pageModel =
    let update = CreateSessionForm.update childModel msg
    match msg, CreateSessionForm.toSession update with
    | CreateSessionForm.Msg.CreateSession, Some session ->
        { pageModel with
            ActivePage = ExistingSession(SessionDetails.init session)
        }
    | _ -> { pageModel with ActivePage = CreateSession update }

let withSessionDetailsHandler msg childModel pageModel =
    let update = SessionDetails.update childModel msg
    match msg with
    | SessionDetails.Msg.EndSession -> withBlankSession
    | _ -> { pageModel with ActivePage = ExistingSession update }

let update msg model =
    match msg, model.ActivePage with
    | BeginNewSession, _ -> withBlankSession, Cmd.none
    | CreateSessionMsg childMsg, CreateSession sessionModel ->
        (withCreateSessionHandler childMsg sessionModel model), Cmd.none
    | SessionDetailsMsg childMsg, ExistingSession detailsModel ->
        (withSessionDetailsHandler childMsg detailsModel model), Cmd.none
    | _ ->
        printf "got invalid dispatch somehow..."
        model, Cmd.none

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
        | ExistingSession detailsModel ->
            SessionDetails.view detailsModel (SessionDetailsMsg >> dispatch)
    layout [ title; body ]
