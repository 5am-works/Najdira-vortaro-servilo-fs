module NajdiraVortaroServilo.Data

open FSharp.Data

type VortaroJSON = JsonProvider<"http://raw.githubusercontent.com/Najdira/Najdira-redaktilo/master/vortaro.json">

type Vorttipo =
   | Substantivo
   | Verbo
   | Helpvorto
   | Nekonita

type Vorto = { Vorto : string; Signifo: int; Radikoj: int list }
type Signifo = { Signifo : string; Ecoj: uint8; Tipo: Vorttipo; Ekstera: bool }
type Vortaro = { Signifoj: Signifo[]; Vortoj: Vorto[] }

let GetDictionary =
   async {
      let! rezultoj = VortaroJSON.AsyncGetSample()
      let signifoj =
         rezultoj.Signifoj
         |> Array.map (fun r ->
            { Signifo.Signifo = r.Signifo
              Ecoj = uint8(r.Ecoj)
              Tipo =
                 match r.Tipo with
                 | "substantivo" -> Substantivo
                 | "verbo" -> Verbo
                 | "helpvorto" -> Helpvorto
                 | _ -> Nekonita
              Ekstera = r.Ekstera })
      let vortoj =
         rezultoj.Vortoj
         |> Array.map (fun r ->
            { Vorto.Vorto = r.Vorto
              Signifo = r.Signifo
              Radikoj = r.Radikoj |> List.ofArray })
      return { Vortaro.Signifoj = signifoj; Vortoj = vortoj }
   }