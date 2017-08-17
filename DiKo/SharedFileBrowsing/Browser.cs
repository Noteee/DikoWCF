using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace DiKo.SharedFileBrowsing
{
    class Browser
    {
        private string path = @"Data Source=DESKTOP-54OBGPG\DIKO;Initial Catalog=DiKo;Integrated Security=True";

        public SqlConnection GetConnenction()
        {
            SqlConnection con =
                new SqlConnection(path);
            return con;
        }

        public List<string> GetData()
        {
            List<string> userList = new List<string>();
            SqlDataReader myReader = null;
            SqlConnection con = GetConnenction();
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM[DiKo].[dbo].[SharedFiles]", con);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                userList.Add(myReader["Name"].ToString());
                userList.Add(myReader["Path"].ToString());
                userList.Add(myReader["Extension"].ToString());
                userList.Add(myReader["Size"].ToString());

            }
            con.Close();
            return userList;
        }

        public List<string> SearchByName(string name)
        {
            List<string> filteredList = new List<string>();
            SqlDataReader myReader = null;
            SqlConnection con = GetConnenction();
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM[DiKo].[dbo].[SharedFiles] WHERE Name LIKE '" + name+"%'", con);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                filteredList.Add(myReader["Name"].ToString());
            }
            con.Close();
            return filteredList;
        }

    }
}

