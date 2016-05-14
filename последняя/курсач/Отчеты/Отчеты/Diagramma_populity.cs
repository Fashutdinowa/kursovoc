using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database;

namespace Отчеты
{
    public partial class Diagramma_populity : Form
    {
        public Diagramma_populity()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           

        }

        private void Diagramma_populity_Load(object sender, EventArgs e)
        {
            Stok_DB stock = new Stok_DB();
            Order_DB order = new Order_DB();
            stock.Add_stok("tort", "napoleon", "123", "100");
            stock.Add_stok("tort", "iogurtovi", "123", "100");
            stock.Add_stok("tort", "medovic", "123", "100");
            stock.loading_stock();
            order.loading_order();
            List<Stock.Stocks> ls = stock.report_popularity();
            string[] s = new string[ls.Count];
            int[] i = new int[ls.Count];
            int n = 0;
            foreach (Stock.Stocks st in ls)
            {
                s[n] = st.name;
                i[n] = st.count;
                n++;
            }
            chart1.ChartAreas[0].AxisX.Maximum = n; //Задаешь максимальные значения координат
            chart1.ChartAreas[0].AxisY.Maximum = (order.ls_order.Count + 10);

            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.Interval = 1;
            this.chart1.Series["Series1"].Points.DataBindXY(s, i);
            chart1.Show();
        }
    }
}
