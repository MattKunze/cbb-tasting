module Msg

open Thoth.Elmish

type Msg =
    | Boop
    | OnFormMsg of FormBuilder.Types.Msg
