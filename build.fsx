#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.DotNet.Paket
nuget Fake.Core.Target
nuget Fake.Core.Process
nuget Fake.Core.String
nuget Fake.Core.ReleaseNotes
nuget Fake.IO.FileSystem
nuget Fake.Tools.Git
nuget Fake.JavaScript.Yarn 
//"
#load ".fake/build.fsx/intellisense.fsx"

#nowarn "52"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.JavaScript

Target.create "Clean" (fun _ ->
    !! "src/bin"
    ++ "src/obj"
    ++ "output"
    |> Seq.iter Shell.cleanDir
)

Target.create "Install" (fun _ ->
    DotNet.restore
        (DotNet.Options.withWorkingDirectory __SOURCE_DIRECTORY__)
        "gc.sln"
)

Target.create "YarnInstall" (fun _ ->
    Yarn.install id
)

Target.create "Build" (fun _ ->
    Yarn.exec
        "webpack --mode production"
        (fun o ->
            { o with WorkingDirectory = __SOURCE_DIRECTORY__ }
        )
)

Target.create "Watch" (fun _ ->
    Yarn.exec
        "webpack-dev-server --mode development"
        (fun o ->
            { o with WorkingDirectory = __SOURCE_DIRECTORY__ }
        )
)

// Build order
"Clean"
    ==> "Install"
    ==> "YarnInstall"
    ==> "Build"

"Watch"
    <== [ "YarnInstall" ]

// start build
Target.runOrDefault "Build"
