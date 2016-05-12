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
using Clients;
using Orders;
using Data_processing;

namespace OOO_Holiday
{
    public partial class Form_payment : Form
    {
        Client_DB client = new Client_DB();
        Order_DB order = new Order_DB();
        Processing proc = new Processing();
        int bonus = 0;
        public Form_payment(string number_card, double prise, string date, string address, string id, List<string> list, string animator_role)
        {
            InitializeComponent();
            this.Text = "Оплата";
            this.prise = prise;
            this.number_card = number_card;
            this.date = date;
            this.address = address;
            this.id = id;
            this.list = list;
            this.animator_role = animator_role;
        } double prise;
        string number_card;
        string date;
        string address;
        string id;
        List<string> list;
        string animator_role;

        private void Form_payment_Load(object sender, EventArgs e)
        {
            label6.Text = Convert.ToString(prise);
            client.loading_client();
            foreach(Client cl in client.ls_client)
            {
                if (cl.number_person_card == Convert.ToInt32(number_card))
                {
                    label7.Text = Convert.ToString(cl.bonus);
                    bonus = cl.bonus;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) //показать чек
        {
            System.Diagnostics.Process.Start("check.pdf");
        }

        private void button4_Click(object sender, EventArgs e) //оформить заказ
        {
            if (textBox3.Text == "") textBox3.Text = "0";
            prise = Convert.ToDouble( label6.Text);
            order.add_order(number_card, date, address, id, list, Convert.ToString(prise), textBox3.Text, animator_role);
            MessageBox.Show("Заказ оформлен");
        }

        private void button2_Click(object sender, EventArgs e) //напечатать
        {
            try
            {
                proc.Print(@"check.pdf");
            }
            catch
            {
                MessageBox.Show("Нет доступа к принтеру");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox3.Text != "0" && Convert.ToInt32(textBox3.Text) <= bonus)
            {
                label6.Text = (prise - Convert.ToInt32(textBox3.Text)).ToString();
                label7.Text = (bonus - Convert.ToInt32(textBox3.Text)).ToString();
            }
            else
            {
                label6.Text = Convert.ToString(prise);
                label7.Text = Convert.ToString(bonus);
            }
        }
    }
}
