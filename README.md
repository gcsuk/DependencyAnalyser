# Dependency Analyser

Dependency analyser provides a WPF application which will scan any directory and catalog NuGet packages. This will allow visibility of a solution's dependencies, and display version conflicts wherever they occur - both in version and target framework.

Alongside this is a Web API application, to which the results can be uploaded. This serves as a central repository, to which several solutions can be uploaded and their dependencies amalgamated.

Having multiple teams upload their solution dependencies into the single database will allow an organisation to see their platform-wide dependencies. This can help with dependency management, and conflict resolution.

Steps to get it working:

* Change the API URL in the WPF application project settings, to where you deploy the web api project - or localhost if debugging.

* Uncomment the database connection string in the Web API config file, and change if necessary.

