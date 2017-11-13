using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService12
    {

        [OperationContract]
        List<Datos> GetData(string value);


        [OperationContract]
        List<Customers> Consultar(string valor);


    }



    [DataContract]
    public class Datos
    {
       

        [DataMember]
       public string id { get; set; }

        [DataMember]
        public int campo2 { get; set; }
        [DataMember]
        public int campo3 { get; set; }
        [DataMember]
        public int campo4 { get; set; }

    }


    [DataContract]
    public class Customers
    {


        [DataMember]
        public string _CustomerID { get; set; }

        [DataMember]
        public string _CompanyName { get; set; }
        [DataMember]
        public string _ContactName { get; set; }
        [DataMember]
        public string _ContactTitle { get; set; }
        [DataMember]
        public string _Address { get; set; }
        [DataMember]
        public string _City { get; set; }
        [DataMember]
        public string _Region { get; set; }
        [DataMember]
        public string _PostalCode { get; set; }
        [DataMember]
        public string _Country { get; set; }
        [DataMember]
        public string _Phone { get; set; }
        [DataMember]
        public string _Fax { get; set; }

    }
}
