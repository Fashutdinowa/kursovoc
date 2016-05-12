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
using Couriers;

namespace OOO_Holiday
{
    public partial class Form_courier : Form
    {
        Courier_DB courier = new Courier_DB();
        public Form_courier()
        {
            InitializeComponent();
            this.Text = "Курьер";
        }

        private void button1_Click(object sender, EventArgs e) //добавить
        {
            courier.Add_courier(textBox3.Text, textBox4.Text);
            MessageBox.Show("Курьер добавлен");
        }

        private void Form_courier_Load(object sender, EventArgs e)
        {
            courier.loading_courier();
            foreach (Courier courier in courier.ls_courier)
            {
                comboBox3.Items.Add(courier.id);
                comboBox4.Items.Add(courier.id);
            }
        }

        private void button2_Click(object sender, EventArgs e) //удалить 
        {
            courier.Delete_courier(comboBox3.Text);
            MessageBox.Show("Курьер удалён");
        }

        private void comboBox4_TextChanged(object sender, EventArgs e)
        {
            foreach (Courier c in courier.ls_courier)
            {
                if (c.id == Convert.ToInt32(comboBox4.Text))
                {
                    textBox1.Text = c.FIO;
                    textBox2.Text = c.phone;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e) //редактировать
        {
            courier.Editing_courier(comboBox4.Text, textBox1.Text, textBox2.Text);
            MessageBox.Show("Изменения сохранены");
        }
    }
}
