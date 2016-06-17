using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RoundRobinWebApp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (String.IsNullOrEmpty(txtTeams.Text))
                {
                    txtTeams.Text = "test1,test2,test3,test4,test5";
                }
                DrawTeamsAndSchedule();
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            DrawTeamsAndSchedule();
        }

        private void DrawTeamsAndSchedule()
        {
            var rr = new RoundRobin(this.txtTeams.Text);
            this.lblResult.Text = "";
            this.lblResult.Text += rr.EmitTeamsInHTML();
            this.lblResult.Text += rr.PrintGames();
        }
    }
}