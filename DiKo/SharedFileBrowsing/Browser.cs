using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using SQLDAL;

namespace DiKo.SharedFileBrowsing
{
    class Browser
    {
        private bool asc = false;
        private bool programFirstStart = false;


        public bool SwitchBool()
        {
            return !asc;
        }

        private string SetOrder()
        {
            if (asc == true)
            {
                return "ASC";
            }
            else
            {
                return "DESC";
            }
        }



        public List<FileShareHandler> GetData()
        {
            List<FileShareHandler> dataList = new List<FileShareHandler>();
            SqlDataReader myReader = null;
            SqlConnection con = SQLDAL.SQLDAL.returnSqlConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM"+ SQLDAL.SQLDAL.database, con);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                dataList.Add(new FileShareHandler(myReader["Name"].ToString(), myReader["Path"].ToString(), myReader["Extension"].ToString(), myReader["Size"].ToString()));
                
                
                
                

            }
            con.Close();
            return dataList;
        }

        public List<FileShareHandler> SearchByName(string name)
        {
            List<FileShareHandler> filteredList = new List<FileShareHandler>();
            SqlDataReader myReader = null;
            SqlConnection con = SQLDAL.SQLDAL.returnSqlConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM" + SQLDAL.SQLDAL.database + " WHERE Name LIKE '%" + name+"%'", con);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                filteredList.Add(new FileShareHandler(myReader["Name"].ToString(), myReader["Path"].ToString(), myReader["Extension"].ToString(), myReader["Size"].ToString()));
            }
            con.Close();
            return filteredList;
        }

        public List<FileShareHandler> SortBy(string type)
        {
            List<FileShareHandler> sortList = new List<FileShareHandler>();
            SqlDataReader myReader = null;
            SqlConnection con = SQLDAL.SQLDAL.returnSqlConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM"+ SQLDAL.SQLDAL.database + " ORDER BY "+type +" "+SetOrder(), con);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {

                sortList.Add(new FileShareHandler(myReader["Name"].ToString(), myReader["Path"].ToString(), myReader["Extension"].ToString(), myReader["Size"].ToString()));

            }
            con.Close();
            return sortList;
        }

        public List<FileShareHandler> GetWishListData()
        {
            List<FileShareHandler> dataList = new List<FileShareHandler>();
            SqlDataReader myReader = null;
            SqlConnection con = SQLDAL.SQLDAL.returnSqlConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM" + SQLDAL.SQLDAL.wishlist, con);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                dataList.Add(new FileShareHandler(myReader["Name"].ToString(), myReader["Path"].ToString(), myReader["Extension"].ToString(), myReader["Size"].ToString()));
            }
            con.Close();
            return dataList;
        }
        public static List<FileShareHandler> GetMySharedFiles()
        {
            List<FileShareHandler> dataList = new List<FileShareHandler>();
            SqlDataReader myReader = null;
            SqlConnection con = SQLDAL.SQLDAL.returnSqlConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM" + SQLDAL.SQLDAL.database, con);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                dataList.Add(new FileShareHandler(myReader["FileName"].ToString(), myReader["FileExtension"].ToString(), myReader["FilePath"].ToString(), myReader["FileSize"].ToString()));
            }
            con.Close();
            return dataList;
        }



    }
}

