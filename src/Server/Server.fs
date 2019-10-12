module App

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe

let now = DateTime.Now.ToString()

let tryGetEnv =
    System.Environment.GetEnvironmentVariable
    >> function
    | null
    | "" -> None
    | x -> Some x

let webApp =
    choose
        [ route "/ping" >=> text "pong"
          route "/when" >=> text now ]

let configureApp (app: IApplicationBuilder) =
    // Add Giraffe to the ASP.NET Core pipeline
    app.UseGiraffe webApp

let configureServices (services: IServiceCollection) =
    // Add Giraffe dependencies
    services.AddGiraffe() |> ignore

let port =
    "SERVER_PORT"
    |> tryGetEnv
    |> Option.map uint16
    |> Option.defaultValue 8085us

[<EntryPoint>]
let main _ =
    WebHostBuilder().UseKestrel().UseUrls(sprintf "http://0.0.0.0:%d/" port)
        .Configure(Action<IApplicationBuilder> configureApp).ConfigureServices(configureServices).Build().Run()
    0
