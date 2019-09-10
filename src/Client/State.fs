module State

open Elmish

open Types

let init (initialPage: Page option) : Model * Cmd<Msg> =
    Router.urlUpdate initialPage { ActivePage = Page.Loading }

let update msg model =
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
    | _ ->
        printf "Ignoring message %A" msg
        model, Cmd.none
