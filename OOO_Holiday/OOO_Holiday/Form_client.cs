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
using System.Text.RegularExpressions;



namespace OOO_Holiday
{
    public partial class Form_client : Form
    {
        Client_DB client = new Client_DB();
        public Form_client()
        {
            InitializeComponent();
            this.Text = "Клиент";
        }

        private void button2_Click(object sender, EventArgs e) //далее
        {
            if (this.label10.Text == "0") MessageBox.Show("Номер карыт равен нулю");
            else
            {
                Form_order order = new Form_order(this.label10.Text);
                order.Show();
                this.Hide();
            }
            
        }

        private void Form_client_Load(object sender, EventArgs e)
        {
            client.loading_client();
            foreach (Client client in client.ls_client)
            {
                comboBox1.Items.Add(client.FIO);
                comboBox2.Items.Add(client.phone_number);
                comboBox3.Items.Add(client.address);
                comboBox4.Items.Add(client.email);
                comboBox5.Items.Add(client.number_person_card);
            }
            comboBox5.Text = "0";
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void button3_Click(object sender, EventArgs e) //добавить клиента 
        {
            Regex reg = new Regex(@"\d");
            Match str = reg.Match(comboBox2.Text);
            if (!str.Success) MessageBox.Show("Введены неверные данные в поле номер телефона");
            else
            {
                bool exist = false;
                foreach (Client client in client.ls_client)
                {
                    if (client.number_person_card == Convert.ToInt32(label10.Text)) { MessageBox.Show("Данный клиент уже существует"); exist = true; }
                }
                if (!exist)
                {
                    if (comboBox5.Text == "")
                    {
                        label10.Text = client.Add_client(comboBox1.Text, comboBox2.Text, comboBox4.Text, comboBox3.Text, "0");
                    }
                    else { label10.Text = client.Add_client(comboBox1.Text, comboBox2.Text, comboBox4.Text, comboBox3.Text, comboBox5.Text); }
                    MessageBox.Show("Клиент добавлен");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e) //выбрать клиента 
        {
            bool exist = false;
            foreach (Client client in client.ls_client)
            {
                if (client.FIO == comboBox1.Text && client.phone_number == comboBox2.Text && client.address == comboBox3.Text)
                {
                    label10.Text = Convert.ToString(client.number_person_card);
                    exist = true;
                }
            }
            if (!exist)
            {
                MessageBox.Show("Данного клиента не существует");
                label10.Text = "0";
            }
        }



        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            foreach (Client client in client.ls_client)
            {
                if (client.FIO == comboBox1.Text)
                {
                    comboBox2.Text = client.phone_number;
                    comboBox3.Text = client.address;
                    comboBox4.Text = client.email;
                }
            }
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            foreach (Client client in client.ls_client)
            {
                if (client.phone_number == comboBox2.Text)
                {
                    comboBox1.Text = client.FIO;
                    comboBox3.Text = client.address;
                    comboBox4.Text = client.email;
                }
            }
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            foreach (Client client in client.ls_client)
            {
                if (client.address == comboBox3.Text)
                {
                    comboBox1.Text = client.FIO;
                    comboBox2.Text = client.phone_number;
                    comboBox4.Text = client.email;
                }
            }
        }
    }
}
