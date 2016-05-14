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
        public int id_courier;
        public string fio;
        int id_order;        
        public DateTime data;
        string role;
        public string address;
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

