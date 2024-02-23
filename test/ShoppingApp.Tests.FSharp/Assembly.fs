module Assembly 

open Orleans
open ShoppingApp.Grains
open System
open ShoppingApp.Abstractions

[<assembly: Orleans.ApplicationPartAttribute("ShoppingApp.Grains")>]
()