#instructions to use worldfootball scores.

# Introduction #
The Worldfootball score type make it easier to add definition for a league. It uses the results from www.worldfootball.net.

# Global configuration #
WorldFootball site provides the results in several localization. To use the site from your country set the parameter named _worldfootball_ to the url for your country, ex for France: http://www.mondedufoot.fr/.
To set the parameter:
  * open the configuration screen,
  * open the options dialog (gear icon),
  * select the _Score Parameters_ tab,
  * change the URL of the worldfootball site (do not forget the http and the trailing /).

The scores also use the following styles:
  * **Header**: style for headers
  * **Highlight**: style for user highlights
  * **Level1, Level2** ...: styles to show Level 1, Level 2 teams (in rankings)
  * **Level-1, Level-2** ...: styles to show Level -1, Level -2 teams (i.e. last)

# Definition of a Worldfootball score #
  * **Name**: the name of the score
  * **Country**: the code for the country (can be empty for international competitions)
  * **League**: the name of the league
  * **Season**: the season
  * **Type**: the type of the competition (League, Cup, Tournament)
  * **Details**: details for the competition
  * **Nb Teams**: number of teams (to determine the number of matches)
  * **Levels**: the levels to highlights (using styles Level`[n]` and Level`[-n]`)
  * **Highlights**: the elements to highlights (separated with a ','). It will use the _Highlight_ style and highlight the lines where a cell contains one of the elements. For ex: Manc,Chel will highlights Manchester United, Manchester City and Chelsea in Standings but also in Results or Top Scorer or History.
  * **Icon**: the button will download the the icon from the site and saved it in the thumbs directory. **Note** that you need to click the save button to set the icon to the score.

## Customize columns width ##
It is possible to customize the sizes of the columns for the World Football scores.
The following parameters are used to define the sizes of the columns.
Note that they are not required (a default value will be used), they are given here with teir default value.

  * League
    * WF.LeagueResults = "11,5,20,+1,-20,8"
    * WF.LeagueNext = "11,5,20,+1,-20,8"
    * WF.LeagueStandings = "5,0,-20,3,3,3,3,5,3,3"
    * WF.HeaderStandings = ",,Team,Pl,W,D,L,Goals,+/-,Pts"
    * WF.TopScorers = "6,0,-22,-20,-12"
    * WF.History = "-10,0,-24"
    * WF.TopScorerHistory = "-10,0,-20,0,-20,4"
  * Cup
    * WF.CupResults = "11,5,20,+1,-20,8"
    * WF.CupLevel = "-5,17,+1,-17,-15,0"
    * WF.History = "-10,0,-24"
  * Qualification
    * WF.GroupResults = "-12,15,+1,-15,-8,0"
    * WF.HeaderStandings = ",,Team,Pl,W,D,L,Goals,+/-,Pts"
    * WF.GroupStandings = "-6,0,-15,3,3,3,3,6,3,3"
    * WF.CupLevel = "-5,17,+1,-17,-15,0"
    * WF.TopScorers = "6,0,-22,-20,-12"
    * WF.History = "-10,0,-24"
  * Tournament (same as Qualification +)
    * WF.TopScorerHistory = "-10,0,-20,0,-20,4"
  * Team
    * WF.TeamLast = "-10,-8,-10,-2,0,-18,-3,0"
    * WF.TeamResults = "-10,-10,-5,-2,0,-15,-3"
    * WF.TeamHistory = "-50"
    * WF.TeamPlayers = "+4,-40"
    * WF.TeamTransfers = "+6,0,-20,+3,0,-20"