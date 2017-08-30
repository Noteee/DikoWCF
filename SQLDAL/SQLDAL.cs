using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDAL
{
    public class SQLDAL
    {
        public static string path = @"Data Source=DESKTOP-54OBGPG\DIKO;Initial Catalog=DiKo;Integrated Security=True";
        public static string database = @"[DiKo].[dbo].[SharedFiles]";
        public static string wishlist = @"[DiKo].[dbo].[WishList]";
        public static SqlConnection myconn = returnSqlConnection();

        public static void ConnecToDB()
        {

             try
            {
                SqlConnection myConnection = new SqlConnection(path);
                myConnection.Open();
                Console.WriteLine("Yeah");
                myConnection.Close();
        
            }
            catch(Exception e)
            {
                Console.WriteLine("No");
                Console.WriteLine(e);

           
            }
        }

        public static void WriteListToDB(List<FileShareHandler> fileShareHandler)
        {   dropMySharedTable();
            foreach (FileShareHandler fs in fileShareHandler)
            {
                Console.WriteLine(fs.FileName, fs.FilePath, fs.FileExtension, fs.FileSize);
                SqlCommand cmd = new SqlCommand("INSERT INTO "+ database + "(FileName,FileExtension,FilePath,FileSize) VALUES('" + fs.FileName + "','" + fs.FileExtension +"','" + fs.FilePath + "','" + fs.FileSize +"');",myconn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }

        public static void dropMySharedTable(){
            SqlCommand cmd = new SqlCommand("DROP TABLE IF EXISTS " + database +";IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='MySharedFiles' AND xtype='U')CREATE TABLE "+ database + " (FileName TEXT, FileExtension TEXT, FilePath TEXT,FileSize TEXT);", myconn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }     

        public static SqlConnection returnSqlConnection(){

            return new SqlConnection(path);
        }

        public static void WriteWishList(List<FileShareHandler> fileShareHandler)
        {
            DropMyWishList();
            foreach (FileShareHandler fs in fileShareHandler)
            {
                Console.WriteLine(fs.FileName, fs.FilePath, fs.FileExtension, fs.FileSize);
                SqlCommand cmd = new SqlCommand("INSERT INTO " + wishlist + "(FileName,FileExtension,FilePath,FileSize) VALUES('" + fs.FileName + "','" + fs.FileExtension + "','" + fs.FilePath + "','" + fs.FileSize + "');", myconn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }
        public static void DropMyWishList()
        {
            SqlCommand cmd = new SqlCommand("DROP TABLE IF EXISTS " + wishlist + ";IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='MySharedFiles' AND xtype='U')CREATE TABLE " + wishlist + " (FileName TEXT, FileExtension TEXT, FilePath TEXT,FileSize TEXT);", myconn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }


    }
}
