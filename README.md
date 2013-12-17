ServiceStackv4-Demo-TeamsApi
============================
Demo by Joe Kampschmidt

Simple API for demo of new ServiceStack v4 API. Created to experiment with the new API features.

Some of the features demonstrated with this project:
- dynamic route adding to DTOs at runtime
- dynamic OrmLite properties added to DTOs at runtime
- custom HTML format template used
- serving static files outside /api level 
- a few OrmLite query examples
- IReturn marker interfaces no longer required. See the League DTO/Service/Consumer code
- Example bypass of RequiredRole attribute using AuthSecret in querystring. See Player DTO/Service

## Partial Updates via PATCH ##
See the LeagueService.cs and PartialUpdateTests.cs files to see an experiment with using PATCH to perform partial updates using OrmLite. The test is working and wouldn't take much to make the code generic to any DTO.

This project uses the "Starter" option seen at https://servicestack.net/pricing. 
It is subject to the qutoas outlined at https://servicestack.net/download#free-quotas
