namespace NajdiraVortaroServilo.Controllers

open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Mvc
open NajdiraVortaroServilo.Data

type VortoInformo =
   { Vorto : string
     Ecoj: uint8
     Signifo : string
     Egalvortoj : string array
     Radikoj : string list
     Idoj : string array
   }

[<ApiController>]
[<Route("[controller]")>]
type VortojController (logger:ILogger<VortojController>) =
   inherit ControllerBase()

   let dictionary = GetDictionary |> Async.RunSynchronously

   [<HttpGet("{vorto}")>]
   member this.Informi (vorto:string) : ActionResult =
      dictionary.Vortoj
      |> Array.tryFindIndex (fun v -> v.Vorto = vorto)
      |> Option.map (fun v ->
         let trovitaVorto = dictionary.Vortoj.[v]
         let signifo = dictionary.Signifoj.[trovitaVorto.Signifo]
         let egalVortoj =
            dictionary.Vortoj
            |> Array.filter
               (fun e -> e.Signifo = trovitaVorto.Signifo && e.Vorto <> trovitaVorto.Vorto)
            |> Array.map (fun e -> e.Vorto)
         let idoj =
            dictionary.Vortoj
            |> Array.filter (fun i -> i.Radikoj |> List.exists (fun r -> v = r))
            |> Array.map (fun i -> i.Vorto)
         let radikoj =
            trovitaVorto.Radikoj |> List.map (fun r -> dictionary.Vortoj.[r].Vorto)
         { Vorto = trovitaVorto.Vorto
           Ecoj = signifo.Ecoj
           Signifo = signifo.Signifo 
           Egalvortoj = egalVortoj
           Radikoj = radikoj 
           Idoj = idoj } |> this.Ok :> ActionResult)
      |> Option.defaultValue (this.NotFound() :> ActionResult)