namespace RapidDRModel
    open System
    open NodaTime

    type RoomContents = 
        | BattleRoom of BattleContext
        | TravelRoom
        | Shop
        | Guild

    type Room(neighbors : Map<string, Room>, description : string, title : string, contents : RoomContents ) = 
        member val Neighbors = neighbors with get,set

        member this.Description = description

        member this.Title = title

        member this.Contents = contents

        member val Travelers = [] with get,set

        member this.EnterBattle (ident : string) = 
            let found = List.exists (fun (u : Unit) -> u.Ident = ident) this.Travelers

            if found then 
                let u = List.find (fun (u : Unit) -> u.Ident = ident) this.Travelers

                match this.Contents with
                | BattleRoom (bc) -> 
                    bc.Join u
                    this.Travelers <- List.filter (fun (un : Unit) -> un.Ident <> ident) this.Travelers
                | _ -> ()
            else ()

        member this.LeaveBattle (ident : string) = 
            match this.Contents with
            | BattleRoom (bc) ->
                let found = List.exists (fun (u : Unit) -> u.Ident = ident) bc.Units

                if found then
                    let u = List.find (fun (u : Unit) -> u.Ident = ident) bc.Units

                    bc.Leave u
                    this.Travelers <- u :: this.Travelers
                else
                    ()
            | _ -> ()

        member this.Move (ident : string) (moveWord : string) = 
            let found = List.exists (fun (u : Unit) -> u.Ident = ident) this.Travelers
            
            if found then 
                let u = List.find (fun (u : Unit) -> u.Ident = ident) this.Travelers

                let mlo = Map.tryFind moveWord this.Neighbors

                match mlo with
                | None -> ()
                | Some rm -> 
                    this.Travelers <- List.filter (fun (un : Unit) -> un.Ident <> ident) this.Travelers
                    rm.Travelers <- u :: rm.Travelers

            else
                ()

        

