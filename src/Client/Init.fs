module Init

open Elmish
open Thoth.Elmish.FormBuilder

open Model
open Msg
open EvaluationForm

let init() : Model * Cmd<Msg> =
    let (formState, formCmds) = Form.init formConfig formState

    let initialModel = { FormState = formState }
    initialModel, Cmd.map OnFormMsg formCmds
