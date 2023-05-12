module Main
    open System
    open RapidDRModel

    [<EntryPoint>]
    let main args = 
        Overseer.run ()
        0
