using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ConString
    {
        private string ConnectionString = string.Empty;

        public void Conection()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();


        }
    }
}
