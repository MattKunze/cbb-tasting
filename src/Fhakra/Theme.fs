module Theme

open Fable.Core
open Fable.Core.DynamicExtensions
open Fable.Core.JsInterop
open Fable.React
open Browser

open Shared

let cssReset() = ofImport "CSSReset" "@chakra-ui/core" undefined []

let colorModeProvider (children: ReactElement list) = ofImport "ColorModeProvider" "@chakra-ui/core" undefined children

type ColorMapping = string * string

type IChakraTheme =
    { colors: ColorMapping list }

[<Import("theme", from = "@chakra-ui/core")>]
let defaultTheme: IChakraTheme = jsNative

let themeProvider (theme: IChakraTheme option) (children: ReactElement list) =
    let props = {| theme = theme |}
    ofImport "ThemeProvider" "@chakra-ui/core" props children

[<Emit("Object.assign($0, $1)")>]
let private objectAssign (lhs: obj) (rhs: obj): obj = failwith "JS Only"

let private extendColors (theme: IChakraTheme) (updates: (string * string) list) =
    let updates = updates |> List.map (fun (fromColor, toColor) -> (toColor, theme.colors.[fromColor]))
    objectAssign theme.colors (keyValueList CaseRules.None updates)

let semanticThemeProvider (children: ReactElement list) =
    // todo - figure out how to do this without mutating
    extendColors defaultTheme
        [ ("green", "success")
          ("yellow", "warning")
          ("red", "danger")
          ("teal", "info") ]
    |> ignore
    themeProvider (Some defaultTheme) [ colorModeProvider (cssReset() :: children) ]

[<RequireQualifiedAccess>]
[<StringEnum>]
type ColorMode =
    | [<CompiledName "light">] Light
    | [<CompiledName "dark">] Dark

type private IUseColorMode =
    { colorMode: ColorMode
      toggleColorMode: unit -> unit }

[<Import("useColorMode", from = "@chakra-ui/core")>]
let private rawUseColorMode: unit -> IUseColorMode = jsNative

let useColorMode() =
    let result = rawUseColorMode()
    result.colorMode, result.toggleColorMode
