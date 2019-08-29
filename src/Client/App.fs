module App

open Elmish
open Fable.React
open Feliz
open Helpers

type Page =
    | Loading
    | CreateSession of CreateSessionForm.Model
    | ExistingSession of SessionDetails.Model
    | EnterEvaluation of EvaluationForm.Model

type Model =
    { ActivePage : Page }

type Msg =
    | BeginNewSession
    | CreateSessionMsg of CreateSessionForm.Msg
    | SessionDetailsMsg of SessionDetails.Msg
    | EnterEvaluationMsg of EvaluationForm.Msg

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

let withCreateSessionHandler msg (pageModel: CreateSessionForm.Model) appModel =
    let update, externalMsg = CreateSessionForm.update pageModel msg
    match externalMsg with
    | CreateSessionForm.ExternalMsg.SessionCreated session ->
        { appModel with ActivePage = ExistingSession(SessionDetails.init session) }
    | _ ->
        { appModel with ActivePage = CreateSession update }

let withSessionDetailsHandler msg (pageModel: SessionDetails.Model) appModel =
    let update, externalMsg = SessionDetails.update pageModel msg
    match externalMsg with
    | SessionDetails.ExternalMsg.SessionEnded -> withBlankSession
    | SessionDetails.ExternalMsg.EnterEvaluation ->
        { appModel with
            ActivePage = EnterEvaluation(EvaluationForm.init pageModel.Session)}
    | _ -> { appModel with ActivePage = ExistingSession update }

let withEnterEvaluationHandler msg (pageModel: EvaluationForm.Model) appModel =
    let update, externalMsg = EvaluationForm.update pageModel msg
    match externalMsg with
    | EvaluationForm.ExternalMsg.EvaluationCanceled ->
        { appModel with ActivePage = ExistingSession(SessionDetails.init pageModel.Session) }
    | EvaluationForm.ExternalMsg.EvaluationEntered evaluation ->
        { appModel with ActivePage = ExistingSession(SessionDetails.init pageModel.Session) }
    | _ -> { appModel with ActivePage = EnterEvaluation update }

let update msg model =
    match msg, model.ActivePage with
    | BeginNewSession, _ -> withBlankSession, Cmd.none
    | CreateSessionMsg childMsg, CreateSession childModel ->
        (withCreateSessionHandler childMsg childModel model), Cmd.none
    | SessionDetailsMsg childMsg, ExistingSession childModel ->
        (withSessionDetailsHandler childMsg childModel model), Cmd.none
    | EnterEvaluationMsg childMsg, EnterEvaluation childModel ->
        (withEnterEvaluationHandler childMsg childModel model), Cmd.none
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
        | EnterEvaluation evaluationModel ->
            EvaluationForm.view evaluationModel (EnterEvaluationMsg >> dispatch)

    layout [ title; body ]
