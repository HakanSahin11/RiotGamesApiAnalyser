# RiotGamesApiAnalyser
- Updated App to match the Riot API's updated endpoints, still experimental state
- NOTICE: You will need to implement your own personal API key into the RiotConnection.cs to make a connection

Personal spare time project of mine.
Made to keep a track of the decay state of League of Legends accounts.
This is meant for people who has a lot of high ranked accounts in the game, to keep track of their decay status.
Decay is basically the interval whereas a player is needed to have played X number of games before Y amount of time, if the player fails to meet these requirements, then they would suffer a penalty and might demote.

The Application is made through C# MVC and has a direct connection to the Riot Games Developer API.
The database for the utilization is through an SQL database which stores the Summoner names and Regions of formerly searched users.

