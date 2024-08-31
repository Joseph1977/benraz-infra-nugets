#benraz-infra-nuget

To use locally built NuGet packages from multiple projects in your solution while still retaining the ability to publish and debug them without interfering with existing projects that use these packages from nuget.org, follow these steps:

Step-by-Step Instructions

1. Set Up Local NuGet Packages Folder:
- Create a local folder on your machine where you will store your locally built NuGet packages. This will act as a local NuGet feed.
- Example path: C:\LocalNugetPackages

2. Build Your NuGet Packages Locally:
- Open your solution in Visual Studio.
- For each project that you want to package as a NuGet package, ensure that the .csproj file is configured to generate a NuGet package.
- Ensure that your .csproj file has the following properties:
    <PropertyGroup>
       <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
       <PackageOutputPath>c:\BenrazNugetPackages</PackageOutputPath>
    </PropertyGroup>
- GeneratePackageOnBuild: Ensures the NuGet package is generated every time the project is built.
- PackageOutputPath: The local path where the NuGet packages will be saved.

3. Build the Solution:
- Build the solution using Visual Studio or from the command line using dotnet build. The NuGet packages will be generated and placed in the C:\BenrazNugetPackages directory.

bash:
    dotnet build

4. Add Local NuGet Feed to Visual Studio:
- Open Visual Studio.
- Go to Tools > NuGet Package Manager > Package Manager Settings.
- In the left pane, select NuGet Package Sources.
- Click the + button to add a new package source.
- Name the source (e.g., "Local Benraz Packages") and set the source path to C:\BenrazNugetPackages.
- Click Update and then OK to save the settings.

5. Use Local NuGet Packages in Your Projects:
- For any project in your solution that needs to use these locally built packages:
- Open the NuGet Package Manager for the project.
- Browse for the package and select the version from your local feed ("Local Benraz Packages" or whatever you named it).

6. Restore Packages from Local and NuGet.org:
- When restoring packages, ensure that both your local feed and nuget.org are configured as sources in Visual Studio.
- Use the command below to restore packages from both sources:
bash:
    dotnet restore

7. Debug Your Locally Built Packages:
- Since your local projects are referencing the locally built NuGet packages, you can set breakpoints and debug as usual.
- If you want to step into the source code of a NuGet package, ensure that the PDB files (debug symbols) are generated alongside the NuGet packages. Add this to your .csproj:
xml:
    <PropertyGroup>
        <IncludeSymbols>true</IncludeSymbols>
        <SymbolPackageFormat>snupkg</SymbolPackageFormat>
    </PropertyGroup>

8. Ensure Projects Can Still Use Packages from NuGet.org:
- If you remove the local feed or if a package is not found in your local feed, Visual Studio or dotnet restore will fall back to nuget.org.
- This setup allows projects to use either the local feed or nuget.org, based on availability and version constraints.

9. Switching Between Local and Remote Packages:
- If you want to switch back to using packages from nuget.org, you can remove or disable the local feed in the NuGet Package Manager Settings.
- Alternatively, specify the package source directly in the NuGet Package Manager console:
bash:
    Install-Package YourPackageName -Version YourVersionNumber -Source https://api.nuget.org/v3/index.json

10. Publishing the Packages to NuGet.org:
- When you're ready to publish your packages to nuget.org, ensure your .csproj is set up with the necessary metadata (like PackageId, Version, Authors, etc.).
- Use the following command to publish:
bash:
    dotnet nuget push YourPackageName.1.x.x.nupkg --api-key YourNuGetAPIKey --source https://api.nuget.org/v3/index.json

11. Automating Builds and Packaging:
- To automate the process of building and packaging, consider using CI/CD tools like GitHub Actions, Azure DevOps, or Jenkins to build your solution, create packages, and manage feeds.

12. Additional Tips
- Package Versioning: Use proper versioning (e.g., 1.0.0-local vs. 1.0.0) to differentiate between local and published packages.
- Clean Local Feed Regularly: Keep your local feed clean to avoid clutter and confusion over which version is being used.
- Use dotnet pack: You can manually create a NuGet package using the dotnet pack command.
bash:
    dotnet pack -c Release -o C:\BenrazNugetPackages

By following these steps, you will be able to debug and test locally built packages without interfering with projects that use packages from nuget.org. This setup provides flexibility to switch between local development and production-ready builds easily.