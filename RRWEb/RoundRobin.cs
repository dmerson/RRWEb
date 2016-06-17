using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundRobinWebApp
{
    public class Team
    {
        public Team(int teamId, string teamName = "")
        {
            TeamId = teamId;
            if (teamName == "")
            {
                teamName = teamId.ToString();
            }
            else
            {
                TeamName = teamName;
            }
        }

        public int TeamId { get; set; }
        public string TeamName { get; set; }
    }

    public class Game
    {
        public enum TypeOfGame
        {
            HomeAway,
            Neutral
        }

        public void SetPossibleHomeTeam(Team possibleHomeTeam)
        {
            Teams.Add(possibleHomeTeam);
        }
        public Game(Team possibleAwayTeam)
        {
            Teams=new List<Team>();
            Teams.Add(possibleAwayTeam);
            
        }
        public Game(Team possibleAwayTeam, Team possibleHomeTeam, TypeOfGame typeOfGame = TypeOfGame.Neutral)
        {
            TeamIdsUsed = new List<int>();
            TeamIdsUsed.Add(possibleAwayTeam.TeamId);
            TeamIdsUsed.Add(possibleHomeTeam.TeamId);
            Teams = new List<Team>();
            Teams.Add(possibleAwayTeam);
            Teams.Add(possibleHomeTeam);
            GameType = typeOfGame;
        }

        public List<int> TeamIdsUsed { get; set; }
        public List<Team> Teams { get; set; }

        public TypeOfGame GameType { get; set; }

        public string GetGameOppositionType()
        {
            if (GameType == TypeOfGame.Neutral)
            {
                return " vs. ";
            }
            else
            {
                return " at ";
            }
        }

        public string ToString()
        {
                if (Teams.Count ==2)
                {
                    return Teams[0].TeamName + GetGameOppositionType() + Teams[1].TeamName;
                }
                else
                {
                    return Teams[0].TeamName;
                }
     
        }
    }

    public class GameWeek
    {
        public GameWeek()
        {
            Games = new List<Game>();
        }

        public List<Game> Games { get; set; }

        public List<int> TeamIdsUsed
        {
            get
            {
                var ids = new List<int>();
                foreach (Game game in Games)
                {
                    foreach (int id in game.TeamIdsUsed.ToList())
                    {
                        ids.Add(id);
                    }
                }
                return ids;
            }
        }
    }

    public class RoundRobin
    {
        private string ByeWeek = "Bye Week";

        public RoundRobin(string listOfTeams, char seperationCharacter = ',')
        {
            try
            {
                List<string> teamNames = listOfTeams.Split(seperationCharacter).ToList();
                ListOfTeams = new List<Team>();
                for (int teamId = 0; teamId < teamNames.Count; teamId++)
                {
                    var team = new Team(teamId, teamNames[teamId]);
                    ListOfTeams.Add(team);
                }
                GamesByWeek = new List<GameWeek>();
            }
            catch (Exception ex)
            {
                throw new Exception("You have not enter a valid team list");
            }

            NumberOfActualTeams = ListOfTeams.Count();
            if (NumberOfActualTeams < 3)
            {
                throw new Exception("Not enough teams for round robin. Just play the other team.");
            }
            int checkForUnevenNumberOfTeams = NumberOfActualTeams % 2;
            if (checkForUnevenNumberOfTeams != 0)
            {
                var team = new Team(NumberOfActualTeams, ByeWeek);
                ListOfTeams.Add(team);
            }
            
        }

        public List<GameWeek> GamesByWeek { get; set; }
        public List<Team> ListOfTeams { get; set; }

        public List<KeyValuePair<int, int>> ScheduleOfGames { get; set; }


        public List<KeyValuePair<int, List<int>>> TeamsByWeek { get; set; }
        public int NumberOfActualTeams { get; set; }



        public string PrintGames()
        {
            StringBuilder sb = new StringBuilder();
            int numberOfTeams = ListOfTeams.Count;
            int numberOfWeeks = numberOfTeams - 1;
            int numberOfGamesPerWeek = numberOfTeams / 2;
            bool runSequenceUpwards = true;
            int currentFirstPairing = numberOfTeams ;
           
            for(int week=1;week <=numberOfWeeks;week++)
            {
                sb.Append("Week " + week  + "<Br>");
                var currentWeek = new GameWeek();
                var secondItem = 1;
                
                if (runSequenceUpwards==true)
                {
                    
                    secondItem = 1;
                }
                else
                {
                    secondItem = currentFirstPairing;

                }
                int counterForLoop = secondItem;
                var theGame = SetFirstGameOfSet(secondItem, currentWeek);
                sb.Append(theGame.ToString() + "<br>");
                GamesByWeek.Add(currentWeek); 
                for (int gameNumber = numberOfGamesPerWeek; gameNumber > 1; gameNumber--)
                {
                    var nextGame = GetNextGameAndSetCounter(runSequenceUpwards, secondItem, numberOfTeams, numberOfGamesPerWeek, ref counterForLoop);
                    sb.Append(nextGame.ToString() + "<br>");
                }


                runSequenceUpwards = false;
                currentFirstPairing--;
            }

            
          
            
            return sb.ToString();
        }

        private Game GetNextGameAndSetCounter(bool runSequenceUpwards, int secondItem, int numberOfTeams,
                                              int numberOfGamesPerWeek, ref int counterForLoop)
        {
            if (runSequenceUpwards == true)
            {
                counterForLoop++;
            }
            else
            {
                counterForLoop = LowerCounterCountWithoutItemsAlreadyUsed(counterForLoop, secondItem, numberOfTeams);
            }
            int opponentId;
            if (runSequenceUpwards)
            {
                opponentId = counterForLoop + numberOfGamesPerWeek - 1;
            }
            else
            {
                opponentId = counterForLoop + numberOfGamesPerWeek;
                if (opponentId > (numberOfTeams - 1))
                {
                    opponentId = opponentId - numberOfTeams + 1;
                }
            }

            var nextGame = new Game(new Team(counterForLoop, ListOfTeams[counterForLoop].TeamName),
                                    new Team(opponentId, ListOfTeams[opponentId].TeamName));
            return nextGame;
        }

        private Game SetFirstGameOfSet(int secondItem, GameWeek currentWeek)
        {
            var theGame = new Game(new Team(0, ListOfTeams[0].TeamName), new Team(secondItem, ListOfTeams[secondItem].TeamName));
            currentWeek.Games.Add(theGame);
            return theGame;
        }

        private static int LowerCounterCountWithoutItemsAlreadyUsed(int counterForLoop, int secondItem, int numberOfTeams)
        {
            counterForLoop--;
            if (counterForLoop == secondItem)
            {
                counterForLoop--;
            }
            if (counterForLoop == 0)
            {
                counterForLoop = numberOfTeams - 1;
            }
            return counterForLoop;
        }


        public string EmitTeamsInHTML()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<table>");
            foreach (Team team in ListOfTeams)
            {
                sb.AppendLine("<tr><td>" + team.TeamName + "</td></tr>");
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }

       
    }
}