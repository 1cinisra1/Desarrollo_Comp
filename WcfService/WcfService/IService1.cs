using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        List<Customers> ObtenerCustomers(string value);

        //void InsertarCustomers(string CompanyName, string ContactName, string ContactTitle, string Address, string City, string Region, string PostalCode, string Country, string Phone,string Fax);
        [OperationContract]
        void InsertarCustomers(Customers customer);


        [OperationContract]
        void InsertarEmployees(Employees employees);
    }
}
