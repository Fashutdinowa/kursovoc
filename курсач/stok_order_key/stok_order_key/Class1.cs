using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stok_order_key
{
    public class Key
    {
        public int id;
        public int id_stok;
        public int id_order;
        public int count;
        public Key(int ID, int stok, int order, int counts)
        {
            id = ID;
            id_stok = stok;
            id_order = order;
            count = counts;
        }
    }
}
