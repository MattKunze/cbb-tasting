module Types

type Page =
    | Loading
    | CreateSession
    | SessionDetails of int
    | EnterEvaluation
    | NotFound

type Model =
    { CurrentPage: Page }

type Msg =
    | PageChanged of Page
    | NavigateToCreateSession
    | NavigateToSessionDetails of int
    | NavigateToEnterEvaluation
