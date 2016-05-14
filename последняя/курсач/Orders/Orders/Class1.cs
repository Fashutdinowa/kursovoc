using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock;

namespace Orders
{
    public class Order
    {
        public int number;
        public int number_person_card;
        public DateTime data_order;
        public DateTime devilery_data;
        public string address;
        public int courier;
        public double sum;
        public string status;
        public double discount;
        public Order(int id, int person_card, DateTime order_data, DateTime data_develery, string adress, int id_courier, double sum_order, string order_status, double Discount)
        {
            number = id;
            number_person_card = person_card;
            data_order = order_data;
            devilery_data = data_develery;
            address = adress;
            courier = id_courier;
            sum = sum_order;
            status = order_status;
            discount = Discount;
        }
        public override string ToString()
        {
            return number.ToString() + " " + number_person_card.ToString() + " " + data_order.ToString() + " " + devilery_data.ToString() + " " + address + " " + courier.ToString() + " " + sum.ToString() + " " + status + " " + discount.ToString();
        }
    }
}
