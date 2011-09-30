Freetime-Generic Platform © TekWorcs 2011
----------------------------------------------------------------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------------------------------------------------------------
Contents
----------------------------------------------------------------------------------------------------------------------------------------------

1. Folder Structure
2. Building the Source




----------------------------------------------------------------------------------------------------------------------------------------------
1. Folder Structure
----------------------------------------------------------------------------------------------------------------------------------------------

1.0.x.x --------------------------------------------------- Branch 1.0.x.x 
..1Solution.2008 ------------------------------------------ Folder containing solution files for Visual Studio 2008
..1Solution.2010 ------------------------------------------ Folder containing solution files for Visual Studio 2010
..Assemblies ---------------------------------------------- Folder containing assemblies for third party components
..bin ----------------------------------------------------- Projects output folder
....Debug ------------------------------------------------- Projects output folder for Debug Configuration
....Release ----------------------------------------------- Projects output folder for Release Configuration
..Database ------------------------------------------------ Contains demo and model database 
....MS-SQL ------------------------------------------------ Contains the demo / model MS-SQL database
......Model ----------------------------------------------- Folder containing the model database 
......Demo ------------------------------------------------ Folder containing the demo database
..Documents ----------------------------------------------- Folder containing project documents (e.g release notes, code documentation)
....Code Documentation ------------------------------------ Folder containing the code documentation documents
..Freetime.Authentication --------------------------------- Project folder for Freetime.Authentication
..Freetime.Base.Business ---------------------------------- Project folder for Freetime.Base.Business
..Freetime.Base.Component --------------------------------- Project folder for Freetime.Base.Component
..Freetime.Base.Data -------------------------------------- Project folder for Freetime.Base.Data
..Freetime.Base.Framework --------------------------------- Project folder for Freetime.Base.Framework
..Freetime.Configuration ---------------------------------- Project folder for Freetime.Configuration
..Freetime.Data.Services ---------------------------------- Project folder for Freetime.Data.Services
..Freetime.Data.Services.Host ----------------------------- Project folder for Freetime.Data.Services.Host
..Freetime.Deployment ------------------------------------- Project folder for Freetime.Deployment
..Freetime.Deployment.Configuration ----------------------- Project folder for Freetime.Deployment.Configuration
..Freetime.Deployment.Database ---------------------------- Project folder for Freetime.Deployment.Database
..Freetime.GlobalHandling --------------------------------- Project folder for Freetime.GlobalHandling
..Freetime.PluginManager ---------------------------------- Project folder for Freetime.PluginManager
..Freetime.Web -------------------------------------------- Project folder for Freetime.Web
..Freetime.Web.Authorization ------------------------------ Project folder for Freetime.Web.Authorization
..Freetime.Web.Context ------------------------------------ Project folder for Freetime.Web.Context
..Freetime.Web.Controller --------------------------------- Project folder for Freetime.Web.Controller
..Freetime.Web.Routing ------------------------------------ Project folder for Freetime.Web.Routing
..Freetime.Web.View --------------------------------------- Project folder for Freetime.Web.View
..Licensing ----------------------------------------------- Folder for Licensing details
..Plugins ------------------------------------------------- Folder for application plugins
..Resharper ----------------------------------------------- Folder containing Resharper settings
..Unit.Testing -------------------------------------------- Folder for Unit Test Projects





----------------------------------------------------------------------------------------------------------------------------------------------
2. Building the Source
----------------------------------------------------------------------------------------------------------------------------------------------

The solution files under the solution folders are numbered in this format "00#.SolutionFileName.sln".

e.g. : 001.Freetime.Framework

To run the source build it from the lowest number to the highest.