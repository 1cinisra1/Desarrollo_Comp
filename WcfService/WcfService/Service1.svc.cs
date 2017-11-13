using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public void InsertarCustomers(Customers customer)
        {
            var entity = new northwindEntities();

            entity.Customers.Add(customer);
            entity.SaveChanges();
        }

        public void InsertarEmployees(Employees employees)
        {
            var entity = new northwindEntities();

            entity.Employees.Add(employees);
            entity.SaveChanges();
        }

        //public void InsertarCustomers(string CompanyName, string ContactName, string ContactTitle, string Address, string City, string Region, string PostalCode, string Country, string Phone, string Fax)
        //{
        //    var entity = new northwindEntities();



        //}

        public List<Customers> ObtenerCustomers(string value)
        {
            northwindEntities entity = new northwindEntities();

            if (String.IsNullOrEmpty(value))
            {
                var queryCustomers = from x in entity.Customers
                                    
                                     select x;
                return queryCustomers.ToList<Customers>();
            }
            else
            {
                var queryCustomers = from x in entity.Customers
                                     where x.CustomerID == value
                                     select x;
                return queryCustomers.ToList<Customers>();
            }
            


           

        }

        
    }
}
