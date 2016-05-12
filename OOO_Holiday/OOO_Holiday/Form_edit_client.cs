using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Clients;
using Database;

namespace OOO_Holiday
{
    public partial class Form_edit_client : Form
    {
        Client_DB client = new Client_DB();
        public Form_edit_client()
        {
            InitializeComponent();
            this.Text = "Редактирование клиента";
        }

        private void Form_edit_client_Load(object sender, EventArgs e)
        {
            client.loading_client();
            foreach (Client cl in client.ls_client)
            {
                comboBox4.Items.Add(Convert.ToString(cl.number_person_card));
                comboBox1.Items.Add(Convert.ToString(cl.number_person_card));
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            foreach (Client cl in client.ls_client)
            {
                if (cl.number_person_card == Convert.ToInt32(comboBox1.Text))
                {
                    textBox1.Text = cl.FIO;
                    textBox2.Text = cl.phone_number;
                    textBox3.Text = cl.address;
                    textBox4.Text = cl.email;
                    textBox5.Text = Convert.ToString(cl.parent_personal_card);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) //удалить клиента
        {
            foreach (Client cl in client.ls_client)
            {
                if (cl.number_person_card == Convert.ToInt32(comboBox4.Text))
                {
                    string message =  client.Delete_client(comboBox4.Text);
                    MessageBox.Show(message);
                    break;
                }
            }
            comboBox4.Items.Clear();
            comboBox1.Items.Clear();
            foreach (Client cl in client.ls_client)
            {
                comboBox4.Items.Add(Convert.ToString(cl.number_person_card));
                comboBox1.Items.Add(Convert.ToString(cl.number_person_card));
            }
            comboBox4.Text = "";
        }

        private void button1_Click(object sender, EventArgs e) //редактировать
        {
            foreach (Client cl in client.ls_client)
            {
                if (cl.number_person_card == Convert.ToInt32(comboBox1.Text))
                {
                    client.Editing_client(comboBox1.Text, textBox1.Text, textBox2.Text, textBox4.Text, textBox3.Text, textBox5.Text);
                    MessageBox.Show("Изменения сохранены");
                    break;
                }
            }
        }
    }
}
