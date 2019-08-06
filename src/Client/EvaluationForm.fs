module EvaluationForm

open Fable.Core.JsInterop
open Fable.React
open Fulma
open Thoth.Elmish.FormBuilder
open Thoth.Elmish.FormBuilder.BasicFields
open Msg

let private inputView (state : Types.FieldState)
    (dispatch : Types.IFieldMsg -> unit) =
    let state : Input.State = state :?> Input.State

    let inputProps =
        [ Input.ValueOrDefault state.Value
          Input.Placeholder(state.Placeholder |> Option.defaultValue "")
          Input.OnChange(fun ev ->
              !!ev.target?value
              |> Input.ChangeValue
              |> dispatch) ]
    Field.div [] [ Label.label [] [ str state.Label ]
                   Control.div [] [ Input.text inputProps ] ]

let (formState, formConfig) =
    Form<Msg>
        .Create(OnFormMsg)
        .AddField(BasicInput.Create("breweryName").WithLabel("Brewery Name")
                            .WithPlaceholder("Fill it out").IsRequired()
                            .WithCustomView(inputView))
        .AddField(BasicInput.Create("beerName").WithLabel("Beer Name")
                            .IsRequired().WithCustomView(inputView))
        .Build()
