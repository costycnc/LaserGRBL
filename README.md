# LaserGRBL [![Donation](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/donate?business=4WQX8HUBXRVUU&no_recurring=0&item_name=LaserGRBL&currency_code=EUR)
Official website [http://lasergrbl.com](http://lasergrbl.com)

LaserGRBL is a Windows GUI for [GRBL](https://github.com/gnea/grbl/wiki). Unlike other GUI LaserGRBL it is specifically developed for use with laser cutter and engraver. In order to use all of LaserGRBL feature, your engraver must supports laser power modulation through gcode "S" command. LaserGRBL is compatible with [Grbl v0.9](https://github.com/grbl/grbl/) and [Grbl v1.1](https://github.com/gnea/grbl/)

All downloads available at https://github.com/arkypita/LaserGRBL/releases

### Support and Donation

Do you like LaserGRBL? Support development with your donation!

[![Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/donate?business=4WQX8HUBXRVUU&no_recurring=0&item_name=LaserGRBL&currency_code=EUR)

### Existing Features

- GCode file loading with engraving/cutting job preview (with alpha blending for grayscale engraving)
- Image import (jpg, bmp...) with line by line GCode generation (horizontal, vertical, and diagonal).
- Image import (jpg, bmp...) with Vectorization!
- Image import (jpg, bmp...) with 1bit dithering, best result with low power laser
- Vector file import (svg only) [Experimental]
- Different color scheme optimized for different safety glasses
- User defined buttons, power to you!
- Grbl Configuration Import/Export
- Configuration, Alarm and Error codes decoding for Grbl v1.1 (with description tooltip)
- Homing button, Feed Hold button, Resume button and Grbl Reset button
- Job time preview and realtime projection
- Jogging (for any Grbl version)
- Feed overrides (for Grbl > v1.1) with easy-to-use interface
- Support for [WiFi connection via ESP8266 WebSocket](http://lasergrbl.com/en/usage/wifi-with-esp8266/)

### Usage

* [LaserGRBL User Interface](http://lasergrbl.com/usage/user-interface/)
* [Connect to arduino-grbl](http://lasergrbl.com/usage/arduino-connection/)
* [Load G-Code and send to machine](http://lasergrbl.com/usage/load-and-send/)
* [Speed and power overrides](http://lasergrbl.com/usage/overrides/)
* [Jogging](http://lasergrbl.com/usage/jogging/)
* [Custom buttons](http://lasergrbl.com/usage/custom-buttons/)
* [Raster Image Import](http://lasergrbl.com/usage/raster-image-import/)
* [Grayscale conversion parameters](http://lasergrbl.com/usage/raster-image-import/import-parameters/)
* [Line 2 line grayscale conversion](http://lasergrbl.com/usage/raster-image-import/line-to-line-tool/)
* [1bit dithering conversion](http://lasergrbl.com/usage/raster-image-import/dithering-tool/)
* [Image vectorization](http://lasergrbl.com/usage/raster-image-import/vectorization-tool/)

### Development Roadmap

Development status and roadmap can be found here: [Roadmap](https://github.com/arkypita/LaserGRBL/issues/64)

### Missing Features

- Minimal Z axis control (LaserGRBL is for XY machine)

#### Screenshot and videos

[<img src="https://cloud.githubusercontent.com/assets/8782035/23578353/fba95768-00d4-11e7-9357-99c00a30631d.jpg">](https://www.youtube.com/watch?v=Uk2fGoNL3Yk)

![Galeon](https://cloud.githubusercontent.com/assets/8782035/21349915/dba84a5a-c6b4-11e6-965f-a74fd283267a.jpg)

![Raster2Laser](https://cloud.githubusercontent.com/assets/8782035/21425748/34400d46-c84b-11e6-99e5-6eb529a98f8f.jpg)

![Alpha](https://cloud.githubusercontent.com/assets/8782035/21351296/1df460c2-c6bc-11e6-8eee-4612bb7978fa.jpg)

![FinalWork](https://cloud.githubusercontent.com/assets/8782035/21907662/bbe988be-d910-11e6-9bdb-75b6e3404e0a.jpg)

![UserDefinedButtons](https://cloud.githubusercontent.com/assets/8782035/23375844/238e5f70-fd2a-11e6-8826-5ff7743bbea0.jpg)

### Compiling

LaserGRBL is written in C# for .NET Framework 3.5 (or higher) and can be compiled with [SharpDevelop](http://www.icsharpcode.net/opensource/sd/) and of course with [Microsoft Visual Studio](https://www.visualstudio.com) IDE/Compiler

### Licensing

LaserGRBL is free software, released under the [GPLv3 license](https://www.gnu.org/licenses/gpl-3.0.en.html).

### Credits and Contribution

LaserGRBL contains some code from:
- [ColorSlider](https://www.codeproject.com/articles/17395/owner-drawn-trackbar-slider) - Copyright Michal Brylka
- [CsPotrace](https://drawing3d.de/Downloads.aspx) - Copyright Peter Selinger, port by Wolfgang Nagl
- [Bezier2Biarc](https://github.com/domoszlai/bezier2biarc) - Copyright Laszlo
- [websocket-sharp](https://github.com/sta/websocket-sharp) - Copyright sta.blockhead
- [Expression Evaluator](https://github.com/vubiostat/expression.cs) - Copyright Will Gray, Jeremy Roberts
- [GCodeFromSVG](https://github.com/svenhb/GRBL-Plotter) - Copyright Sven Hasemann
- [MS SVG Library](https://archive.codeplex.com/?p=svg) - Microsoft Public License
- [Clipper](http://www.angusj.com/delphi/clipper.php) - Angus Johnson. Copyright © 2010-2014

Thanks to:
- Myself, for italian and english language
- Fernando Luna, sqall123, for spanish translation
- Olivier Salvador, guillaume-rico [#848](https://github.com/arkypita/LaserGRBL/pull/848) for french translation
- Gerd Vogel, for german translation
- Anders Lassen, for danish translation
- Gerson Koppe, for brasilian translation
- Alexey Golovin, Newcomere, AlexeyBond, for russian translation
- Yang Haiqiang, for chinese translation
- 00alkskodi00, for slovak translation [#670](https://github.com/arkypita/LaserGRBL/issues/670)
- ddogman, for hungarian translation [#735](https://github.com/arkypita/LaserGRBL/issues/735)
- Petr Bitnar, for czech translation
- Ozzybanan, for polish translation
- onmaker, for traditional chinese translation [#1066](https://github.com/arkypita/LaserGRBL/pull/1066)
- Nikolaos Ntekas, for Greek translation [#1234](https://github.com/arkypita/LaserGRBL/pull/1234)
- Mrjavaci, for Turkish translation [#1293](https://github.com/arkypita/LaserGRBL/pull/1293)
- Filippo Rivato for code contribution [#305](https://github.com/arkypita/LaserGRBL/pull/305) and again [#1251](https://github.com/arkypita/LaserGRBL/pull/1251)
- Fabio Ferretti for code contribution [#592](https://github.com/arkypita/LaserGRBL/pull/592)
- guillaume-rico for code contribution on Smoothie support
- Tobias Falkner, for code contribution [#937](https://github.com/arkypita/LaserGRBL/pull/937)
- gmmanonymus111, for code contribution [#1032](https://github.com/arkypita/LaserGRBL/pull/1032)
---

## 🚀 Cloud-Based Automatic Compilation & Customization (Updated 2026 - CostyCNC Edition)

If you are struggling to configure heavy local development environments (like Visual Studio) or constantly fighting missing NuGet packages and obsolete .NET Framework dependencies, you can compile this software **in less than two minutes directly in the cloud**, without installing anything on your computer.

This CI/CD compilation procedure using GitHub Actions was tested, optimized, and documented by **Boboaca Costel (CostyCNC)** at the age of 60, proving that passion for electronics, CNC machines, and software development has no age limit!

### ⚠️ Troubleshooting: What Didn't Work (Our Negative Experience)
When trying to build LaserGRBL using standard GitHub Actions templates, you will likely hit two major roadblocks that cause the compilation to fail with `exit code 1`:
1. **Missing Legacy Frameworks:** Standard GitHub runners do not pre-install older environments like .NET Framework 4.0. Trying to force installations via package managers (`choco install netfx-3.5`) will fail because those packages are deprecated or missing from repository sources.
2. **Secondary Test Projects Block:** Running MSBuild on the global solution file (`LaserGRBL.sln`) fails because the cloud runner gets stuck compiling the secondary unit test projects (`LaserGRBL.Tests`). 

**The Solution:** We bypassed these errors entirely by targeting *only* the main executable project (`LaserGRBL.csproj`) and forcing an automatic *retargeting* parameter (`/p:TargetFrameworkVersion=v4.8`) right inside the compilation command line.

---

### 🛠️ How to Compile via Browser:
1. **Fork** this repository to your personal GitHub account.
2. Go to **Settings** -> **Actions** -> **General** and select **"Allow all actions and reusable workflows"** to enable the cloud runners.
3. In the **Code** tab, create a new file at the exact path: `.github/workflows/compila.yml`
4. Paste the following optimized configuration inside it:

```yaml
name: Compilazione Automatica CostyCNC

on:
  push:
    branches: [ master ]
  workflow_dispatch:

jobs:
  build:
    runs-on: windows-latest

    steps:
    - name: Download source code
      uses: actions/checkout@v4

    - name: Configure MSBuild
      uses: microsoft/setup-msbuild@v2

    - name: Configure NuGet
      uses: NuGet/setup-nuget@v2

    - name: Restore NuGet packages
      run: nuget restore LaserGRBL.sln

    - name: Compile main executable with .NET 4.8 override
      run: msbuild LaserGRBL/LaserGRBL.csproj /p:Configuration=Release /p:Platform="AnyCPU" /p:TargetFrameworkVersion=v4.8

    - name: Upload compiled EXE artifact
      uses: actions/upload-artifact@v4
      with:
        name: LaserGRBL-CostyCNC
        path: LaserGRBL/bin/Release/
```

5. Save by clicking **Commit changes**.
6. Switch to the **Actions** tab at the top, wait for the circle to turn **green**, click on the completed build, and download your ready-to-use `.exe` file from the **Artifacts** section at the bottom of the page!

---

### 🏷️ Your First Custom Step: How to Change the Window Title
Want to verify your custom factory works? Let's add your branding to the main window title bar (e.g., changing it to *LaserGRBL - CostyCNC Edition*).

1. In your personal fork, navigate to: `LaserGRBL/MainForm.cs`
2. Click the **pencil icon** to edit the file directly in the browser.
3. Search (`Ctrl + F`) for the function `private void RefreshFormTitle()`.
4. Modify the code block by commenting out the original string and adding your custom suffix:

```csharp
private void RefreshFormTitle()
{
    // Original line: string FormTitle = string.Format("LaserGRBL v{0}", Program.CurrentVersion.ToString(3));
    string FormTitle = string.Format("LaserGRBL v{0} - CostyCNC Edition", Program.CurrentVersion.ToString(3));

    if (Core.Type != Firmware.Grbl)
        FormTitle = FormTitle + $" (for {Core.Type})";

    if (Text != FormTitle) Text = FormTitle;
}
```

5. Click **Commit changes**. The cloud will automatically trigger a new build, injecting your personalized title bar into the new `.exe` file!

