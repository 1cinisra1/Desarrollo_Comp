using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class Data
    {
        

        public List<Datos> Consultar (string valor)
        {
            List<Datos> lista = new List<Datos>();
            try
            {

                string _cn = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;

                using (SqlConnection con = new SqlConnection(_cn))
                {
                    string sqlQuery = String.Format("select CtlFlag, NextInvoice, NextCredit, NextDebit from [SysproCompany1].[dbo].[ArControl] where CtlFlag = '" + valor +"'");
                    using (SqlCommand _cmd = new SqlCommand(sqlQuery, con))
                    {
                      
                        con.Open();
                        SqlDataReader dataReader = _cmd.ExecuteReader();
                        Datos Data = null;


                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Data = new Datos();

                                Data.id = dataReader["CtlFlag"].ToString();
                                Data.campo2 = Convert.ToInt32(dataReader["NextInvoice"]);
                                Data.campo3 = Convert.ToInt32(dataReader["NextCredit"]);
                                Data.campo4 = Convert.ToInt32(dataReader["NextDebit"]);


                                
                                 lista.Add(Data);
                                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                                new System.Web.Script.Serialization.JavaScriptSerializer();
                                string sJSON = oSerializer.Serialize(lista);
                            }
                        }

                    }
            }
                return lista;
            }
            catch (Exception e)
            {
                return lista;
            }
        }


        public List<Customers> ConsultarCustomers(string valor)
        {
            List<Customers> lista_Customers = new List<Customers>();
            int estado = 0;
            try
            {

                string _cn = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;

                using (SqlConnection con = new SqlConnection(_cn))
                {
                    using (SqlCommand _cmd = new SqlCommand("[dbo].[sp_Consultar]"))
                    {
                        _cmd.Parameters.AddWithValue("@condicion", valor);
                        _cmd.Parameters.AddWithValue("@estado", estado);
                        
                      

                        using (SqlDataAdapter _da = new SqlDataAdapter())
                        {
                            _cmd.CommandType = CommandType.StoredProcedure;
                            _cmd.Connection = con;
                            con.Open();
                            _da.SelectCommand = _cmd;

                            using (DataTable _dt = new DataTable())
                            {
                                _da.Fill(_dt);


                                for (int i = 0; i < _dt.Rows.Count; i++)
                                {
                                    Customers _customers = new Customers();
                                    _customers._CustomerID = _dt.Rows[i]["CustomerID"].ToString();
                                    _customers._CompanyName = _dt.Rows[i]["CompanyName"].ToString();
                                    _customers._ContactName = _dt.Rows[i]["ContactName"].ToString();
                                    _customers._ContactTitle = _dt.Rows[i]["ContactTitle"].ToString();
                                    _customers._Address = _dt.Rows[i]["Address"].ToString();
                                    _customers._City = _dt.Rows[i]["City"].ToString();
                                    _customers._Region = _dt.Rows[i]["Region"].ToString();
                                    _customers._PostalCode = _dt.Rows[i]["PostalCode"].ToString();
                                    _customers._Country = _dt.Rows[i]["Country"].ToString();
                                    _customers._Phone = _dt.Rows[i]["Phone"].ToString();
                                    _customers._Fax = _dt.Rows[i]["Fax"].ToString();

                                    lista_Customers.Add(_customers);
                                }
                            }

                        }

                    }
                }
                return lista_Customers;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return lista_Customers;
            }
         
        }

        }
       
    
}