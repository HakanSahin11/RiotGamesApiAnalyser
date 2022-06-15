# RiotGamesApiAnalyser
- NOTICE: You will need to implement your own personal API key into the RiotConnection.cs to make a connection

Personal spare time project of mine.
Made to keep a track of the decay state for high ranking League of Legends accounts.
This is meant for people who has a lot of high ranked accounts in the game, to keep track of their decay status.
Decay is basically the interval whereas a player is needed to have played X number of games before Y amount of time, if the player fails to meet these requirements, then they would suffer a penalty and might demote.

The Application is made through C# MVC and has a direct connection to the Riot Games Developer API.
The database for the utilization is through an SQL database which stores the Summoner names and Regions of formerly searched users.

API Endpoint documentation:

Account: Returns User account info, which contains unique PUUID & SummonerId, through the listed username & region
https://{region}.api.riotgames.com/lol/summoner/v4/summoners/by-name/{SummonerName}?api_key={_ApiKey}

Rank: Returns User ranking info based on SummernerId & Region
https://{region}.api.riotgames.com/lol/league/v4/entries/by-summoner/{SummonerId}?api_key={_ApiKey}

MatchID: Returns users 11 most recent matches based on PUUID & Region
https://{region}.api.riotgames.com/lol/match/v5/matches/by-puuid/{puuId}/ids?queue=420&start=0&count=11&api_key={_ApiKey}

Match: Returns single Match info based on individual MatchID & Region
https://{region}.api.riotgames.com/lol/match/v5/matches/{MatchID}?api_key={_ApiKey}

![lolapi](https://user-images.githubusercontent.com/59696753/173826954-5ea65fc6-8d2f-4343-aa43-a33ce3d42cd0.png)
