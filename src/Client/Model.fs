module Model

open Thoth.Elmish

type Model =
    { FormState : FormBuilder.Types.State }
