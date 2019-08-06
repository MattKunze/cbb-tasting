module View

open Fable.React
open Fulma
open Thoth.Elmish.FormBuilder
open Model
open Msg
open EvaluationForm

let view model dispatch =
    Section.section []
        [ Container.container [ Container.IsFluid ]
              [ Content.content []
                    [ p [] [ str "Howdy" ]
                      Form.render { Config = formConfig
                                    State = model.FormState
                                    Dispatch = dispatch
                                    ActionsArea = div [] []
                                    Loader = Form.DefaultLoader }

                      (*
                      form []
                          [ Field.div []
                                [ Label.label [] [ str "Brewery Name" ]

                                  Control.div []
                                      [ Input.text [ Input.ValueOrDefault
                                                         model.BreweryName
                                                     Input.OnChange(fun ev ->
                                                         !!ev.target?value
                                                         |> ChangeBreweryName
                                                         |> dispatch) ] ] ]

                            Field.div []
                                [ Label.label [] [ str "Beer Name" ]

                                  Control.div []
                                      [ Input.text [ Input.ValueOrDefault
                                                         model.BeerName
                                                     Input.OnChange(fun ev ->
                                                         !!ev.target?value
                                                         |> ChangeBeerName
                                                         |> dispatch) ] ] ]

                            Field.div []
                                [ Label.label [] [ str "Comments" ]

                                  Control.div [ Control.IsLoading true ]
                                      [ Textarea.textarea [] [] ] ] ]
                    *)

                      Button.button [ Button.IsFullWidth
                                      Button.OnClick(fun _ -> dispatch Boop) ]
                          [ str "Click it" ] ] ] ]
