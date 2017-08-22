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

        public static SqlConnection myconn = returnSqlConnection();

        public static void ConnecToDB()
        {

             try
            {
                SqlConnection myConnection = new SqlConnection(@"Data Source=BYTEFORCEMAINPC\BYTESQL;Initial Catalog=DiKoDB;Integrated Security=True;");
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
        {   DropMySharedTable();
            foreach (FileShareHandler fs in fileShareHandler)
            {
                Console.WriteLine(fs.FileName, fs.FilePath, fs.FileExtension, fs.FileSize);
                SqlCommand cmd = new SqlCommand("INSERT INTO DiKoDB.dbo.MySharedFiles(FileName,FileExtension,FilePath,FileSize) VALUES('" + fs.FileName + "','" + fs.FileExtension +"','" + fs.FileSize + "','" + fs.FilePath +"');",myconn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }

        public static void DropMySharedTable(){
            SqlCommand cmd = new SqlCommand("DROP TABLE IF EXISTS [DiKoDB].[dbo].[MySharedFiles];IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='MySharedFiles' AND xtype='U')CREATE TABLE [DiKoDB].[dbo].[MySharedFiles] (FileName TEXT, FileExtension TEXT, FileSize TEXT,FilePath TEXT);",myconn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }     

        public static SqlConnection returnSqlConnection(){

            return new SqlConnection(@"Data Source=BYTEFORCEMAINPC\BYTESQL;Initial Catalog=DiKoDB;Integrated Security=True;");
        }

        public static void WriteWishListToDB(List<FileShareHandler> fileShareHandler)
        {
            DropMyWishList();
            foreach (FileShareHandler fs in fileShareHandler)
            {
                Console.WriteLine(fs.FileName, fs.FilePath, fs.FileExtension, fs.FileSize);
                SqlCommand cmd = new SqlCommand("INSERT INTO DiKoDB.dbo.WishList(FileName,FileExtension,FilePath,FileSize) VALUES('" + fs.FileName + "','" + fs.FileExtension + "','" + fs.FileSize + "','" + fs.FilePath + "');", myconn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }
        public static void DropMyWishList()
        {
            SqlCommand cmd = new SqlCommand("DROP TABLE IF EXISTS [DiKoDB].[dbo].[WishList];IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='WishList' AND xtype='U')CREATE TABLE [DiKoDB].[dbo].[WishList] (FileName TEXT, FileExtension TEXT, FileSize TEXT,FilePath TEXT);", myconn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
     

    }
}
