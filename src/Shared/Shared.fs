namespace Shared

open System

// todo - delete
type Counter =
    { Value : int }

type Undefined = exn

type SessionId = private SessionId of Guid

type JudgeName = JudgeName of String

type BreweryName = BreweryName of String

type BeerName = BeerName of String

type BeerStyle = BeerStyle of String

module SessionId =

    let value (SessionId id) = id

    let create = Guid.NewGuid() |> SessionId

module Score =
    type IScore =
        abstract Score : int

    type AromaScore =
        | AromaScore of int
        interface IScore with
            member this.Score =
                let (AromaScore s) = this in s

    type AppearenceScore =
        | AppearenceScore of int
        interface IScore with
            member this.Score =
                let (AppearenceScore s) = this in s

    type FlavorScore =
        | FlavorScore of int
        interface IScore with
            member this.Score =
                let (FlavorScore s) = this in s

    type MouthfeelScore =
        | MouthfeelScore of int
        interface IScore with
            member this.Score =
                let (MouthfeelScore s) = this in s

    type OverallScore =
        | OverallScore of int
        interface IScore with
            member this.Score =
                let (OverallScore s) = this in s

    type CompositeScore =
        { AromaScore : AromaScore
          AppearenceScore : AppearenceScore
          FlavorScore : FlavorScore
          MouthfeelScore : MouthfeelScore
          OverallScore : OverallScore }

    let private validate max ctor (score : int) =
        if score >= 1 && score < max then Some(ctor score)
        else None

    let aromaScore = validate 15 AromaScore
    let appearenceScore = validate 3 AppearenceScore
    let flavorScore = validate 30 FlavorScore
    let mouthfeelScore = validate 5 MouthfeelScore
    let overallScore = validate 10 OverallScore

    let create aroma appearence flavor mouthfeel overall =
        let s1 = aromaScore aroma
        let s2 = appearenceScore appearence
        let s3 = flavorScore flavor
        let s4 = mouthfeelScore mouthfeel
        let s5 = overallScore overall
        match s1, s2, s3, s4, s5 with
        | (Some v1, Some v2, Some v3, Some v4, Some v5) ->
            Some { AromaScore = v1
                   AppearenceScore = v2
                   FlavorScore = v3
                   MouthfeelScore = v4
                   OverallScore = v5 }
        | _ -> None

    let apply f (s : IScore) = s.Score |> f

    let value s = apply id s

    let totalScore score =
        value score.AromaScore + value score.AppearenceScore
        + value score.FlavorScore + value score.MouthfeelScore
        + value score.OverallScore

module Tasting =
    type Session =
        { Id : SessionId
          Date : DateTime
          Styles : BeerStyle
          Judges : JudgeName list }

    type Evaluation =
        { SessionId : SessionId
          JudgeName : JudgeName
          Comments : String
          Score : Score.CompositeScore
          BreweryName : BreweryName
          BeerName : BeerName }
