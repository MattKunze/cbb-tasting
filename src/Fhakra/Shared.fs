module Shared

open Fable.Core
open Fable.Core.JsInterop
open Fable.React

[<Emit("undefined")>]
let undefined: obj = jsNative

let mkProps (li: 'T seq) = keyValueList CaseRules.LowerFirst li

[<Emit("String.fromCharCode(160)")>]
let nbsp: string = jsNative

[<RequireQualifiedAccess>]
type Icon =
| Default of string
| Fi of string

let fiIcons: obj = importAll "react-icons/fi"

let icon (icon: Icon) =
    match icon with
    | Icon.Default icon ->
      let props = createObj [ "name" ==> icon ]
      ofImport "Icon" "@chakra-ui/core" props []
    | Icon.Fi icon ->
      let props = createObj [ "as" ==> fiIcons?("Fi" + icon) ]
      ofImport "Box" "@chakra-ui/core" props []
