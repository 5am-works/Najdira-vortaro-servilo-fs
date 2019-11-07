namespace NajdiraVortaroServilo.Controllers

open Microsoft.AspNetCore.Mvc
open NajdiraVortaroServilo.Data

type SerĉRezulto =
   { Vorto : string
     Signifo : string
   }

[<ApiController>]
[<Route("trovi")>]
type SerĉiController (vortaro:IVortaro) =
   inherit ControllerBase()

   let dictionary = vortaro.Vortaro

   [<HttpGet("{peto}")>]
   member this.Serĉi(peto:string) : ActionResult =
      dictionary.Vortoj
      |> Array.toSeq
      |> Seq.filter (fun v ->
            let signifo = dictionary.Signifoj.[v.Signifo]
            v.Vorto.Contains(peto) or signifo.Signifo.Contains(peto))
      |> Seq.map (fun v ->
         { Vorto = v.Vorto
           Signifo = dictionary.Signifoj.[v.Signifo].Signifo
         })
      |> Seq.truncate 5
      |> Seq.toArray
      |> this.Ok :> ActionResult