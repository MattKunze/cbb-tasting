module Types

[<RequireQualifiedAccess>]
type Page =
    | Loading
    | Session of SessionForm.Model
    | Details of SessionDetails.Model
    | Evaluation of EvaluationForm.Model

type Model =
    { ActivePage : Page }

type Msg =
    | Initialize
    | SessionMsg of SessionForm.Msg
    | SessionExternalMsg of SessionForm.ExternalMsg
    | DetailsMsg of SessionDetails.Msg
    | DetailsExternalMsg of SessionDetails.ExternalMsg
    | EvaluationMsg of EvaluationForm.Msg
    | EvaluationExternalMsg of EvaluationForm.ExternalMsg
