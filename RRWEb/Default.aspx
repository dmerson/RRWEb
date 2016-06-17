<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RoundRobinWebApp.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
    </head>
    <body>
        <form id="form1" runat="server">
            <div>
                <table>
                    <tr>
                        <td>
                            Enter Teams:
                            <asp:TextBox ID="txtTeams" runat="server" Width ="400px"></asp:TextBox>
                            <asp:Button ID="btnSend" runat="server" Text="RUN" OnClick="btnSend_Click" />
                            <asp:label ID="lblResult" runat="server"></asp:label>
                        </td>
                        <td><h3>What this does</h3>
                            <div>
                                Given a comma delimitated list of teams, it writes out the teams and a round robin schedule.

                            </div>
                            <h3>How it works</h3>
                            <div> There are basic objects of :RoundRobin-which does the heavy lifting using a looping algorithm.
                                <br/>Each RobinRobin has a list of teams with a bye if necessary, a list of GameWeeks which has a list of Games which has 2 Teams and the Teams have an surrogate id and a name to make for easy modification in the future. 
                                <br/>The basic idea of the algorithm is the following:
                                <br/>Given A ={team1, team2,team3, team4};<br/>
                                Then A[0,x] for each team that is the first item.
                                <br/>
                                For the first row, you can just run up a counter placing an item in each game going forward with item x,y where y =x + numberOfGames<br />After the first row, A[x,y] where row x=1, y=numberOfTeams with counter descending with some fancy math to make it work.<br/>
                                It is all actually easy for visual as the following:
                                <br/>
                                Each GameRow has initial bucket and then n-1 buckets.
                                Each bucket has 2 items.<br/>
                                The first bucket, first item of every row is the initial item (value0);<br/>
                                The first bucket, second item of the first row is the second item(value1)
                                <br />
                                For each rows of buckets afterwards if descends from n for the second item.<br />
                                For the remaining buckets, the first item descends until the end of the row and then circles back making sure to never redo an item in the row. I did some fancy math so I didn&#39;t have to reiterate. Basic for x, y -&gt; y =x+n/2 -1 and then minus 1 more if already present in the row.</div>

                        </td>
                    </tr>

                </table>
              
            </div>
        </form>
    </body>
</html>
