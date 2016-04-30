using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employments
{
    public class Empoloument
    {
        int id;
        int id_courier;
        string fio;
        int id_order;        
        DateTime data;
        string role;
        string address;
        public Empoloument(int ID, int ID_order, int courier, DateTime Data, string Role, string adress, string FIO)
        {
            id = ID;
            id_order = ID_order;
            id_courier = courier;
            data = Data;
            role = Role;
            address = adress;
            fio = FIO;
        }

        public override string ToString()
        {
            return id_order.ToString() + " " + id_courier.ToString() + " "+fio+" " + data.ToString() + " " + role + " " + address;
        }
    }
}

