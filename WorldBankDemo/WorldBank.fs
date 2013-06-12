module WorldBankDemo

#if INTERACTIVE
    #r @"..\packages\FSharp.Data.1.1.5\lib\net40\FSharp.Data.dll"
    #r @"..\packages\MSDN.FSharpChart.dll.0.60\lib\MSDN.FSharpChart.dll"
#endif

open System
open FSharp.Data
open FSharp.Net
open System.Windows.Forms
open MSDN.FSharp.Charting

let ListCountries() = 
    let wb =FSharp.Data.WorldBankData.GetDataContext()
    
    wb.Countries
    |> Seq.iter (fun country -> printfn "%s" country.Name)

let Show chart =
    let form = new Form(Visible=true, TopMost=true, Width=700, Height=700)
    let ctl = new ChartControl(chart, Dock=DockStyle.Fill)
    form.Controls.Add(ctl)

let UKArmsImports() =
    let wb = WorldBankData.GetDataContext()

    let importsUK = wb.Countries.``United Kingdom``.Indicators.``Arms imports (constant 1990 US$)``
                    |> Array.ofSeq

    let lineUK = FSharpChart.Line(importsUK, Name="Arms imports UK")

    Show lineUK

let UKUSArms() = 
    let wb = WorldBankData.GetDataContext()

    let importsUK = wb.Countries.``United Kingdom``.Indicators.``Arms imports (constant 1990 US$)``
                    |> Array.ofSeq
    let exportsUK = wb.Countries.``United Kingdom``.Indicators.``Arms exports (constant 1990 US$)``
                    |> Array.ofSeq
    let importsUS = wb.Countries.``United States``.Indicators.``Arms imports (constant 1990 US$)``
                    |> Array.ofSeq
    let exportsUS = wb.Countries.``United States``.Indicators.``Arms exports (constant 1990 US$)``
                    |> Array.ofSeq

    let lineImpUK = FSharpChart.Line(importsUK, Name="Arms imports UK")
    let lineExpUK = FSharpChart.Line(exportsUK, Name="Arms exports UK")
    let lineImpUS = FSharpChart.Line(importsUS, Name="Arms imports US")
    let lineExpUS = FSharpChart.Line(exportsUS, Name="Arms exports US")

    let charts = FSharpChart.Combine [lineImpUK; lineExpUK; lineImpUS; lineExpUS]
                 |> FSharpChart.WithLegend()

    Show charts

let FrenchSectors() =

    let ToLine name (data : seq<int * float>) =
        FSharpChart.Line((data |> Array.ofSeq), Name=name)

    let france = WorldBankData.GetDataContext().Countries.France.Indicators

    [
        france.``Agriculture, value added (% of GDP)``
        france.``Industry, value added (% of GDP)``
        france.``Services, etc., value added (% of GDP)``
    ]
    |> Seq.map (fun series -> ToLine series.Name series :> ChartTypes.GenericChart)
    |> FSharpChart.Combine
    |> FSharpChart.WithLegend()
    |> Show

// Problems with corporate proxy server?  Run this in FSI to find out where to put the config file for FSI:
let CheckConfig() =
    printfn "%s" (AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)

