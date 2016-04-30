using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock
{
    public class Stocks
    {
        public int id;
        public string type;
        public string name;
        public double price;
        public int count;
        public Stocks(int ID, string Type, string Name, double Prise, int Count)
        {
            id = ID;
            type = Type;
            name = Name;
            price = Prise;
            count = Count;

        }
        public override string ToString()
        {
            return id.ToString() + " " + type + " " + name + " " + price.ToString()+" "+count.ToString();
        }
    }
}
