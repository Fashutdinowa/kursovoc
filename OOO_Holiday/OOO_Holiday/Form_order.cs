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
using Stock;
using Couriers;
using Orders;
using Data_processing;

namespace OOO_Holiday
{
    public partial class Form_order : Form
    {
        Processing proc = new Processing();
        Stok_DB stok = new Stok_DB();
        Courier_DB courier = new Courier_DB();
        Order_DB order = new Order_DB();
        double prise = 0; //сумма
        List<string> list_s = new List<string>();

        public Form_order(string number_card)
        {
            InitializeComponent();
            this.Text = "Заказ";
            this.number_card = number_card;
        } string number_card;

        private void button3_Click(object sender, EventArgs e) //далее
        {
            if (textBox3.Text != "" && label8.Text == "0")
            {
                MessageBox.Show("Аниматор не добавлен в заказ");
                return;
            }
            if (DateTime.Parse(dateTimePicker1.Text) < DateTime.Now) MessageBox.Show("Выбрана неверная дата доставки");
            else
            {
                if (textBox2.Text != "")
                {
                    if (comboBox3.Text != "")
                    {
                        int courierid = 0;
                        foreach (Courier courier in courier.ls_courier)
                        {
                            if (courier.FIO == comboBox3.Text)
                            {
                                courierid = courier.id;
                            }
                        }
                        
                        proc.Check(list_s, prise, Convert.ToString(DateTime.Now));
                        Form_payment payment = new Form_payment(number_card, prise, dateTimePicker1.Text, textBox2.Text, Convert.ToString(courierid), list_s, textBox3.Text);
                        payment.Show();
                        this.Hide();
                    }
                    else MessageBox.Show("Поле курьера не заполнено");
                }
                else MessageBox.Show("Поле адреса доставки не заполнено");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form_order_Load(object sender, EventArgs e)
        {
            courier.loading_courier();
            stok.loading_stock();
            foreach (Stocks stok in stok.ls_stok)
            {
                comboBox1.Items.Add(stok.type);
            }
            foreach (Courier courier in courier.ls_courier)
            {
                comboBox3.Items.Add(courier.FIO);
            }
            label7.Text = "0";
            label8.Text = "0";
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void button1_Click(object sender, EventArgs e) //добавить товар в заказ
        {
            foreach (Stocks stok in stok.ls_stok)
            {
                if (stok.type == comboBox1.Text && stok.name == comboBox2.Text)
                {
                    listBox1.Items.Add(stok.type + "  " + stok.name + "  " + textBox1.Text + "  " + label7.Text);
                    list_s.Add(stok.type + "  " + stok.name + "  " + textBox1.Text + "  " + label7.Text);
                    
                }
            }
            prise += Convert.ToDouble(label7.Text);
           
            comboBox1.Text = "";
            comboBox2.Text = "";
            textBox1.Text = "";
            label7.Text = "";
            label8.Text = "";
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            foreach (Stocks stock in stok.ls_stok)
            {
                if (stock.type == comboBox1.Text)
                {
                    comboBox2.Items.Add(stock.name);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            foreach (Stocks stock in stok.ls_stok)
            {
                if (stock.name == comboBox2.Text && textBox1.Text != null)
                {
                    try
                    {
                        if (stock.count < Convert.ToInt32(textBox1.Text))
                        {
                            MessageBox.Show("Не хватает товара на складе");
                            textBox1.Text = Convert.ToString(stock.count);
                        }
                        else
                        {
                            label7.Text = Convert.ToString(Convert.ToInt32(textBox1.Text) * stock.price);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Неверно введено количество");
                    }
                }
            }
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != null) label8.Text = "1000";
            prise += 1000;
        }

        private void comboBox2_TextChanged(object sender, EventArgs e) //вывод максимального количества товара
        {
            foreach (Stocks stok in stok.ls_stok)
            {
                if (stok.type == comboBox1.Text && stok.name == comboBox2.Text)
                {
                    textBox1.Text = Convert.ToString(stok.count);
                }
            }
        }
    }
}
