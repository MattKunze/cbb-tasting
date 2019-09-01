module App

open Elmish
open Fable.React
open Feliz
open Helpers

[<RequireQualifiedAccess>]
type Page =
    | Loading
    | Session of SessionForm.Model
    | Details of SessionDetails.Model
    | Evaluation of EvaluationForm.Model

type Model =
    { ActivePage : Page }

type Msg =
    | Initialize
    | SessionMsg of SessionForm.Msg
    | SessionExternalMsg of SessionForm.ExternalMsg
    | DetailsMsg of SessionDetails.Msg
    | DetailsExternalMsg of SessionDetails.ExternalMsg
    | EvaluationMsg of EvaluationForm.Msg
    | EvaluationExternalMsg of EvaluationForm.ExternalMsg

let private delay timeout =
    async {
        do! Async.Sleep timeout
        return ()
    }

let init() : Model * Cmd<Msg> =
    { ActivePage = Page.Loading },
    Cmd.OfAsync.perform delay 1000 (fun _ -> Initialize)

(*
let withBlankSession =
    let blankSession = SessionForm.init()
    { ActivePage = Page.Session blankSession }

let withCreateSessionHandler msg (pageModel: SessionForm.Model) appModel =
    let update, externalMsg = SessionForm.update pageModel msg
    match externalMsg with
    | SessionForm.ExternalMsg.SessionCreated session ->
        { appModel with ActivePage = Page.Details(SessionDetails.init session) }
    | _ ->
        { appModel with ActivePage = Page.Session update }

let withSessionDetailsHandler msg (pageModel: SessionDetails.Model) appModel =
    let update, externalMsg = SessionDetails.update pageModel msg
    match externalMsg with
    | SessionDetails.ExternalMsg.SessionEnded -> withBlankSession
    | SessionDetails.ExternalMsg.EnterEvaluation ->
        { appModel with
            ActivePage = Page.Evaluation(EvaluationForm.init pageModel.Session)}
    | _ -> { appModel with ActivePage = Page.Details update }

let withEnterEvaluationHandler msg (pageModel: EvaluationForm.Model) appModel =
    let update, externalMsg = EvaluationForm.update pageModel msg
    match externalMsg with
    | EvaluationForm.ExternalMsg.EvaluationCanceled ->
        { appModel with ActivePage = Page.Details(SessionDetails.init pageModel.Session) }
    | EvaluationForm.ExternalMsg.EvaluationEntered evaluation ->
        { appModel with ActivePage = Page.Details(SessionDetails.init pageModel.Session) }
    | _ -> { appModel with ActivePage = Page.Evaluation update }
*)

let update msg model =
    printf "handler %A - %A" msg model
    match model.ActivePage, msg with
    | _, Initialize ->
        let sessionModel = SessionForm.init()
        { model with ActivePage = Page.Session sessionModel }, Cmd.none
    | Page.Session sessionModel, SessionMsg sessionMsg ->
        let sessionModel, sessionCmd = SessionForm.update sessionModel sessionMsg
        { model with ActivePage = Page.Session sessionModel }, Cmd.map SessionExternalMsg sessionCmd
    | Page.Details detailsModel, DetailsMsg detailsMsg ->
        let detailsModel, detailsCmd = SessionDetails.update detailsModel detailsMsg
        { model with ActivePage = Page.Details detailsModel }, Cmd.map DetailsExternalMsg detailsCmd
    | Page.Details detailsModel, DetailsExternalMsg extMsg ->
        match extMsg with
        | SessionDetails.EnterEvaluation ->
            let evalModel = EvaluationForm.init detailsModel.Session
            { model with ActivePage = Page.Evaluation evalModel }, Cmd.none
        | SessionDetails.SessionEnded ->
            let sessionModel = SessionForm.init()
            { model with ActivePage = Page.Session sessionModel }, Cmd.none
    | Page.Evaluation evalModel, EvaluationMsg evalMsg ->
        let evalModel, evalCmd = EvaluationForm.update evalModel evalMsg
        { model with ActivePage = Page.Evaluation evalModel }, Cmd.map EvaluationExternalMsg evalCmd
    | Page.Evaluation evalModel, EvaluationExternalMsg extMsg ->
        match extMsg with
        | EvaluationForm.EvaluationEntered evaluation ->
            let detailsModel = SessionDetails.init evalModel.Session
            { model with ActivePage = Page.Details detailsModel }, Cmd.none
        | EvaluationForm.EvaluationCanceled ->
            let detailsModel = SessionDetails.init evalModel.Session
            { model with ActivePage = Page.Details detailsModel }, Cmd.none
    | _, SessionExternalMsg extMsg ->
        match extMsg with
        | SessionForm.SessionCreated session ->
            let detailsModel = SessionDetails.init session
            { model with ActivePage = Page.Details detailsModel }, Cmd.none
    (*
    match msg, model.ActivePage with
    | BeginNewSession, _ -> withBlankSession, Cmd.non
    | CreateSessionMsg childMsg, Page.Session childModel ->
        (withCreateSessionHandler childMsg childModel model), Cmd.none
    | SessionDetailsMsg childMsg, Page.Details childModel ->
        (withSessionDetailsHandler childMsg childModel model), Cmd.none
    | EnterEvaluationMsg childMsg, Page.Evaluation childModel ->
        (withEnterEvaluationHandler childMsg childModel model), Cmd.none
    *)
    | _ ->
        printf "Ignoring message %A" msg
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
        | Page.Loading -> Html.text "Loading..."
        | Page.Session sessionModel ->
            SessionForm.view sessionModel (SessionMsg >> dispatch)
        | Page.Details detailsModel ->
            SessionDetails.view detailsModel (DetailsMsg >> dispatch)
        | Page.Evaluation evaluationModel ->
            EvaluationForm.view evaluationModel (EvaluationMsg >> dispatch)

    layout [ title; body ]
