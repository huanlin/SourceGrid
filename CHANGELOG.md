# SourceGrid

## v6.1.1 (2025-06-22)

- Upgrade target framework to .NET 8 and .NET 9.
- Modify custom exception classes to derive from `System.Exception` (instead of ApplicationException, which is not a good idea). See #3.
- Use GitHub Actions for CI/CD. AppVeyor is no longer used.

## v6.0.0 (January 2022)

- Upgraded project file format to SDK-style using [.NET Upgrade Assistant](https://docs.microsoft.com/en-us/dotnet/core/porting/upgrade-assistant-overview).
- Changed target framework from .NET 3.5 to .NET 6.
- Moved DevAge.* source files into SourceGrid folder.
- Fixed obsolete code. For example: ContextMenu is replaced by ContextMenuStrip.
- Removed unused code files.

## Major Changes of v4.40.4580

- Enhancement: Smooth horizontal and vertical scrolling
- Enhanced Freeze panes(FixedRow and FixedColumn) and made it independent of Header row\column count
- Introduced a boundary(defined by user) to stop auto scrolling
- Filter row support in DataGrid
- Support for Drag and drop of cells
- Performance improvement while loading grid- CreateControl
- Selectable readonly cells
- Introduced a disabled cell mode
- Fixed bugs in clipboard, spanning etc.
