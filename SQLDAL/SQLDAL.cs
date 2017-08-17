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
        public static void ConnecToDB()
        {
            string serverAddress = @"bytesql";

             try
            {
                SqlConnection myConnection = new SqlConnection(@"Data Source=BYTEFORCEMAINPC\SQLEXPRESS;Integrated Security=True");
                myConnection.Open();
                Console.WriteLine("Yeah");
            }
            catch(Exception e)
            {
                Console.WriteLine("No");
                Console.WriteLine(e);

           
            }
        }
    }
}
