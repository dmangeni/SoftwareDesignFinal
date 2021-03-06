﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace FinalPro
{
    public partial class GameStatistics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] id = Session["New"].ToString().Split(':');
                Label2.Text = getUserId(id[0]).ToString();

                Label3.Visible = false;
                ListBox1.Visible = false;
                ListBox2.Visible = false;
                ListBox3.Visible = false;
            }
        }

        private string getWinner(int gameid)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
            conn.Open();
            string winner = "SELECT Winner FROM GameDataTable WHERE Id = '" + gameid + "'";
            SqlCommand com1 = new SqlCommand(winner, conn);
            string winnerUsername = com1.ExecuteScalar().ToString();
            conn.Close();

            return winnerUsername;
        }

        private int getUserId(string username)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
            conn.Open();
            string id = "SELECT Id FROM UserDataTable WHERE Username = '" + username + "'";
            SqlCommand com1 = new SqlCommand(id, conn);
            int playerId = Convert.ToInt32(com1.ExecuteScalar().ToString());
            conn.Close();

            return playerId;
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label3.Visible = true;
            Label3.Text += getWinner(Int32.Parse(DropDownList1.SelectedItem.Text));

            ListBox1.Visible = true;
            ListBox2.Visible = true;
            ListBox3.Visible = true;
            List<string> ids = new List<string>();
            //query to get all actions with the correct game id
            string query = "SELECT Player FROM ActionDataTable WHERE GameId = '" + DropDownList1.SelectedItem.Text + "'";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand com = new SqlCommand(query, conn);
            SqlDataReader sqlReader = com.ExecuteReader();
            while(sqlReader.Read())
            {
                ids.Add(sqlReader.GetValue(0).ToString());
            }
            conn.Close();
            conn.Open();
            foreach (string s in ids)
            {
                string query2 = "SELECT Username FROM UserDataTable WHERE Id = '" + s + "'";
                SqlCommand com2 = new SqlCommand(query2, conn);
                string username = com2.ExecuteScalar().ToString();
                ListBox1.Items.Add(username);
            }
            conn.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("MainMenuPage.aspx");
        }
    }
}