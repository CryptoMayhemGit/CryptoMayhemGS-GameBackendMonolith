# Introduction 
The solutions contains a set of necessary projects for the backend of a strategic web game.

# Getting Started
1.	Installation process
	1.1 Enviroment Variable
	To run solution you neet to add enviroment variable 'MayhemConfigurationType' and
	set one from two options: 
	- Development - for development purpose 
	- Production - for production purpose
	1.2 Nuget
	Solutions is depends from private nuget. You neet to add package source:
	- Name: Mayhem Nugets
	- Source: https://pkgs.dev.azure.com/AdriaGames/CryptoMayhem/_packaging/MayhemNugets/nuget/v3/index.json

# Build and Test
Tests use entity framework in memory provider, so there is no need to install or configure 
others dependencies. However, the tests that check the dapper, use the real database hosted
by azure. To run these tests, you must add yourself to trusted hosts in azure mssql.

Tests from categories: "Building" and "Improvement" don't work because it is an old remnant 
that is not known if it will be used

# References
Each solution contain md file with description, momdules and tech stack information.

- BlockchainWorkersDescription.md
- MayhemConsumersDescription.md
- MayhemNftGeneratorDescription.md
- MayhemWebApiDescription.md
- MissionsWorkerDescription.md
- NotificationWorkerDescription.md
- PathWorkerDescription.md