module App

open Elmish
open Elmish.React
open Feliz

type Model = string

type Msg =
    | Capitalize
    | LowerCase

let init(): Model = "Original Message"

let update (msg: Msg) (model: Model): Model =
    match msg with
    | Capitalize -> model.ToUpper()
    | LowerCase -> model.ToLower()

let render (state: Model) dispatch =
    Html.div
        [ Html.div [ prop.text ("Current: " + state) ]
          Html.div
              [ Html.button
                  [ prop.onClick (fun _ -> dispatch Capitalize)
                    prop.text "Upper" ]
                Html.button
                    [ prop.onClick (fun _ -> dispatch LowerCase)
                      prop.text "Lower" ] ] ]

Program.mkSimple init update render
|> Program.withReactSynchronous "app-root"
|> Program.withConsoleTrace
|> Program.run
