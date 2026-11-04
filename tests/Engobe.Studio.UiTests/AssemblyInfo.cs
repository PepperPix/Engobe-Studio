using Avalonia.Headless;
using Avalonia.Headless.TUnit;
using Engobe.Studio.UiTests;

[assembly: AvaloniaTestApplication(typeof(UiTestApp))]
[assembly: AvaloniaTestIsolation(AvaloniaTestIsolationLevel.PerTest)]
[assembly: AvaloniaTest]
