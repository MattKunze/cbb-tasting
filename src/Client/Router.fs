module Router

open Elmish
open Elmish.Navigation
open Elmish.UrlParser

open Types

let toHash page =
    match page with
    | Page.Loading -> "#loading"
    | _ -> "#bonk"

let pageParser: Parser<Page -> Page, Page> =
    oneOf [
        map Page.Loading (s "loading")
    ]

let modifyUrl route =
    route |> toHash |> Navigation.modifyUrl

let urlUpdate (result: Page option) model =
    match result with
    | None ->
        model, modifyUrl model.ActivePage
    | Some page ->
        { model with ActivePage = page }, Cmd.none
