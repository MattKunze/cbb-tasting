module App

open Feliz
open Feliz.Router

open Helpers
open State
open Types

let render (state: Model) dispatch =
    let currentPage =
        match state.CurrentPage with
        | Loading -> Bulma.Dom.toolbar { Title = "Loading..." }
        | CreateSession -> Bulma.Dom.toolbar { Title = "Create Session" }
        | SessionDetails sessionId -> Bulma.Dom.toolbar { Title = (sprintf "Session: %d" sessionId) }
        | EnterEvaluation -> Bulma.Dom.toolbar { Title = "Evaluation" }
        | NotFound -> Bulma.Dom.toolbar { Title = "Nope, sorry" }

    Router.router
        [ Router.onUrlChanged
            (parseUrl
             >> PageChanged
             >> dispatch)
          Router.application currentPage ]
