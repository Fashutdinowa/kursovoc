using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Couriers
{
   public class Courier
        {
            int id;
            string FIO;
            string phone;
        public Courier (int ID, string fio, string phone_number)
        {
            id = ID;
            FIO = fio;
            phone = phone_number;
        }
            public override string ToString()
            {
                return id.ToString() + " " + FIO + " " + phone.ToString();
            }
        
    }
}
