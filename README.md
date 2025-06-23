[![Build status](https://github.com/huanlin/SourceGrid/actions/workflows/build-release.yml/badge.svg)](https://github.com/huanlin/SourceGrid/actions/workflows/build-release.yml)
[![NuGet](https://img.shields.io/nuget/v/SourceGrid-huanlin.svg)](https://www.nuget.org/packages/SourceGrid-huanlin/) 

## SourceGrid

SourceGrid is a free open source grid control. Supports virtual grid, custom cells and editors, advanced formatting options and many others features
SourceGrid is a Windows Forms control written entirely in C#, goal is to create a simple but flexible grid to use in all of the cases in which it is necessary to visualize or to change a series of data in a table format. There are a lot of controls of this type available, but often are expensive, difficult to be customize or not compatible with .NET. SourceGrid allows users to have customizable datasource which is not in DataSet format.

![Overview Image](/Doc/Images/SourceGrid_Overview.png)

For more detailed information, Refer article at [CodeProject](https://www.codeproject.com/Articles/3531/SourceGrid-Open-Source-C-Grid-Control)

---

## Note of this Fork

This repository was a fork based on [siemens/sourcegrid](https://github.com/siemens/sourcegrid). Then, it is detached from the original repository on June 2025.

### Preconditions

- Visual Studio 2022
- .NET 8/9

### Build and Release

- Use [MinVer](https://github.com/adamralph/minver) for versioning with tag name. The tag name should be in the format `X.Y.Z` (e.g., `6.1.0`). Note: Do not use `v` prefix.
- Automatically deploy the package to NuGet.org. See .github/workflows/build-release.yml for details.

### Recent Changes

- Upgrade target framework to .NET 8 and .NET 9.
- Modify custom exception classes to derive from `System.Exception` (instead of ApplicationException, which is not a good idea). See #3.
- Use GitHub Actions for CI/CD. AppVeyor is no longer used. 

See [CHANGELOG.md](CHANGELOG.md) for more information.

See https://github.com/siemens/sourcegrid for original README.

---

# License
This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/siemens/sourcegrid/blob/master/LICENSE) file for details 

