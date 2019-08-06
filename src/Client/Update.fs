module Update

open Elmish
open Thoth.Elmish.FormBuilder
open Model
open Msg
open EvaluationForm

let update msg currentModel =
    match msg with
    | OnFormMsg msg ->
        let (formState, formCmd) =
            Form.update formConfig msg currentModel.FormState
        { currentModel with FormState = formState }, Cmd.map OnFormMsg formCmd
    | _ -> currentModel, Cmd.none
