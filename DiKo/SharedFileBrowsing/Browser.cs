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

        public SqlConnection GetConnenction()
        {
            SqlConnection con = new SqlConnection("Data Source = .;Initial Catalog = domain;Integrated Security = True");
            return con;
        }

        public List<string> GetUsers()
        {
            List<string> userList = new List<string>();
            SqlDataReader myReader = null;
            SqlConnection con = GetConnenction();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select users from tablename", con);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                userList.Add(myReader["Column1"].ToString());
            }
            con.Close();
            return userList;
        }
    }
}
