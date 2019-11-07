namespace NajdiraVortaroServilo.Controllers

open Microsoft.AspNetCore.Mvc
open NajdiraVortaroServilo.Data

[<ApiController>]
[<Route("[controller]")>]
type SignifojController (vortaro:IVortaro) =
   inherit ControllerBase()

   let dictionary = vortaro.Vortaro

   [<HttpGet("{signifo}")>]
   member this.Informi (signifo:string) : ActionResult =
      dictionary.Signifoj
      |> Array.tryFindIndex (fun s -> s.Signifo = signifo)
      |> Option.map (fun s ->
         dictionary.Vortoj
         |> Array.filter (fun v -> v.Signifo = s)
         |> Array.map (fun v -> v.Vorto)
         |> this.Ok :> ActionResult)
      |> Option.defaultValue (this.NotFound() :> ActionResult)