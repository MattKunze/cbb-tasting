module Client

open Elmish
open Elmish.React

open State
open App

Program.mkProgram init update render
|> Program.withReactSynchronous "app-root"
|> Program.withConsoleTrace
|> Program.run
