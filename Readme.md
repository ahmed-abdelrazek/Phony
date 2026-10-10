# Phony

An Arabic WPF desktop application for mobile store management, using MVVM and LiteDB.

Author's website: [ShababConquer](https://shababco.blogspot.com/).

## Libraries and components

The application references the following NuGet packages in [Phony.csproj](src/Phony.csproj):

- [LiteDB](https://github.com/mbdavid/LiteDB) for the local database.
- [Prism.Wpf](https://github.com/PrismLibrary/Prism) for MVVM and commands.
- [MahApps.Metro](https://github.com/MahApps/MahApps.Metro), [ControlzEx](https://github.com/ControlzEx/ControlzEx), and [MaterialDesignThemes.MahApps](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) for the interface and themes.
- [Dragablz](https://github.com/ButchersBoy/Dragablz) for tabs.
- [MahApps.Metro.SimpleChildWindow](https://github.com/punker76/MahApps.Metro.SimpleChildWindow) for child windows.
- [BarcodeLib](https://github.com/barnhill/barcodelib) for barcodes.
- [RazorEngineCore](https://github.com/adoconnection/RazorEngineCore) for HTML report templates.
- [Microsoft.Web.WebView2](https://learn.microsoft.com/en-us/microsoft-edge/webview2/) for displaying and printing reports.
- [Microsoft.Data.SqlClient](https://github.com/dotnet/SqlClient) for importing data from the SQL Server version.
- [Riok.Mapperly](https://github.com/riok/mapperly) for generated model/DTO mappings.
- [Microsoft.Xaml.Behaviors.Wpf](https://github.com/microsoft/XamlBehaviorsWpf) for XAML behaviors.

The repository also includes a WPF color picker and preview dialog in `src/Views`.

## How to build

Build on Windows with the [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0). Both projects target `net10.0-windows`.

For Visual Studio, use **Visual Studio 2026 (18.0 or higher)** with the **.NET desktop development** workload and .NET 10 SDK installed. See Microsoft's [Windows installation requirements](https://learn.microsoft.com/en-us/dotnet/core/install/windows). Open `Phony.slnx` and build the solution.

Alternatively, from the repository root:

```powershell
dotnet build Phony.slnx
```

NuGet dependencies are restored as part of the build.

## How to run

For a framework-dependent build, install the **.NET 10 Desktop Runtime** for Windows from the [.NET 10 download page](https://dotnet.microsoft.com/en-us/download/dotnet/10.0). The SDK includes the runtime for development.

The **Microsoft Edge WebView2 Runtime** is required to display and print invoice reports. Install it if it is missing; see Microsoft's [WebView2 Runtime deployment documentation](https://learn.microsoft.com/en-us/microsoft-edge/webview2/concepts/distribution). Installing the NuGet SDK package alone does not provide the runtime.

To run from source on Windows:

```powershell
dotnet run --project src/Phony.csproj
```

## Tests

Run the xUnit regression suite on Windows with the .NET 10 SDK:

```powershell
dotnet test tests/Phony.Tests/Phony.Tests.csproj
```

See [tests/README.md](tests/README.md) for test coverage and isolation details.

## Screenshots

![Alt text](Images/Capture01.PNG "Login Window")

![Alt text](Images/Capture02.PNG "UI Settings")

![Alt text](Images/Capture03.PNG "Database Settings")

![Alt text](Images/Capture04.PNG "Move From SQLServer Version Settings")

![Alt text](Images/Capture05.PNG "Move From SQLServer Version Advance Settings")

![Alt text](Images/Capture06.PNG "Login Window with data")

![Alt text](Images/Capture07.PNG "Normal Main Window")

![Alt text](Images/Capture08.PNG "Scrolled Normal Main Window")

![Alt text](Images/Capture09.PNG "Maximized Main Window")

![Alt text](Images/Capture10.PNG "Items Window with 5000+ item")

![Alt text](Images/Capture11.PNG "Items Window with Add New Item Flyout panel opened 1")

![Alt text](Images/Capture12.PNG "Items Window with Add New Item Flyout panel opened 2")

![Alt text](Images/Capture13.PNG "Sales Bills ")

![Alt text](Images/Capture14.PNG "Add item to a sale bill")

![Alt text](Images/Capture15.PNG "Sales bill with total discounts etc")

![Alt text](Images/Capture16.PNG "A4 Sales Bill with item")

![Alt text](Images/Capture17.PNG "A4 Sales Bill with service")

![Alt text](Images/Capture18.PNG "A8 Sales Bill with item")

![Alt text](Images/Capture19.PNG "A8 Sales Bill with service")

![Alt text](Images/Capture21.PNG "Dark Login Window with different Accent and primary colors")

![Alt text](Images/Capture22.PNG "Main Window with different Accent and primary colors")

![Alt text](Images/Capture23.PNG "Main Window with different Accent and primary colors with edit current user")

![Alt text](Images/Capture24.PNG "Barcode Encoder")