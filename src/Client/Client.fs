module Client

open Elmish
open Elmish.Navigation
open Elmish.React
#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

open App
open State

Program.mkProgram init update view
|> Program.toNavigable (UrlParser.parseHash Router.pageParser) Router.urlUpdate
#if DEBUG
|> Program.withConsoleTrace
#endif

|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif

|> Program.run
