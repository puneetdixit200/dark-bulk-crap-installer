# Dark Industrial UI Notes

This fork modernizes the Bulk Crap Uninstaller WinForms interface while leaving uninstall, scan, filtering, automation, and data behavior unchanged.

## Scope

- Dark industrial glass palette for forms, sidebars, toolbars, menus, dialogs, and list surfaces.
- Dark native title bar on supported Windows versions.
- Dark scrollbar theme requests for scrollable WinForms controls.
- White toolbar and menu text, including disabled top-level items.
- White-tinted toolbar icons for dark backgrounds.
- Dark startup splash/loading screens and readable welcome links.
- Opaque selected-row highlight with black selected text.
- Darkened bottom storage treemap colors.

## Build

The self-contained x64 app can be published with:

```powershell
$targets = (Resolve-Path .tools\GeneratedInterop.targets).Path
$publish = (Resolve-Path .).Path + "\bin\publish\win-x64\"
.\.tools\dotnet\dotnet.exe publish source\BulkCrapUninstaller\BulkCrapUninstaller.csproj `
  -c Release `
  -r win-x64 `
  --self-contained true `
  -p:Platform=x64 `
  -p:UseGeneratedInterop=true `
  -p:DirectoryBuildTargetsPath="$targets" `
  -p:filealignment=512 `
  -p:PublishSingleFile=False `
  -p:PublishReadyToRun=false `
  -p:PublishTrimmed=False `
  -p:PublishDir="$publish" `
  -v:minimal
```

The runnable EXE is written to:

```text
bin\publish\win-x64\BCUninstaller.exe
```

## Verification

The focused UI regression suite is:

```powershell
$targets = (Resolve-Path .tools\GeneratedInterop.targets).Path
.\.tools\dotnet\dotnet.exe test source\BulkCrapUninstallerTests\BulkCrapUninstallerTests.csproj `
  -c Release `
  -p:Platform="Any CPU" `
  -p:UseGeneratedInterop=true `
  -p:DirectoryBuildTargetsPath="$targets" `
  --no-restore `
  --filter "FullyQualifiedName~IndustrialThemeTests" `
  -v:minimal
```

The tests cover palette constants, safe native theme calls, selected-row readability, themed links, toolbar text/icon whitening, dark storage-map coloring, and treemap styling.
