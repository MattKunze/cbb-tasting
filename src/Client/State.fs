module State

open Elmish
open Feliz.Router

open Types

let parseUrl =
    function
    | [] -> Loading
    | [ "new" ] -> CreateSession
    | [ Route.Int sessionId ] -> SessionDetails sessionId
    | [ Route.Int sessionId; "new" ] -> EnterEvaluation
    | _ -> NotFound

let init(): Model * Cmd<Msg> = { CurrentPage = Loading }, Cmd.none

let private currentSessionId model =
    match model.CurrentPage with
    | SessionDetails sessionId -> Some sessionId
    | _ -> None

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =
    match msg with
    | PageChanged newPage -> { model with CurrentPage = newPage }, Cmd.none
    | NavigateToCreateSession -> model, Router.navigate "new"
    | NavigateToSessionDetails sessionId -> model, Router.navigate (sessionId.ToString())
    | NavigateToEnterEvaluation ->
        match currentSessionId model with
        | Some sessionId -> model, Router.navigate (sessionId.ToString(), "new")
        | None -> model, Cmd.none
