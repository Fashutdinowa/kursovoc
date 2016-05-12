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


namespace OOO_Holiday
{
    public partial class Form_product : Form
    {
        Stok_DB stok = new Stok_DB();
       
        public Form_product()
        {
            InitializeComponent();
            this.Text = "Товар";
        }

        private void button3_Click(object sender, EventArgs e) //выбрать для редактирования
        {
            textBox3.Text = comboBox1.Text;
            textBox4.Text = comboBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e) //добавить
        {
            stok.Add_stok(comboBox1.Text, comboBox2.Text, textBox2.Text, textBox1.Text);
            MessageBox.Show("Товар добавлен");
            comboBox1.Items.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";

            comboBox3.Items.Clear();
            comboBox1.Items.Clear();
            comboBox4.Items.Clear();
            foreach (Stocks stok in stok.ls_stok)
            {
                comboBox1.Items.Add(stok.type);
                comboBox3.Items.Add(stok.id);
                comboBox4.Items.Add(stok.id);
            }
        }

        private void Form_product_Load(object sender, EventArgs e)
        {
            stok.loading_stock();
            foreach (Stocks stok in stok.ls_stok)
            {
                comboBox1.Items.Add(stok.type);
                comboBox3.Items.Add(stok.id);
                comboBox4.Items.Add(stok.id);
            }
        }

        private void button2_Click(object sender, EventArgs e) //удалить
        {
            foreach (Stocks st in stok.ls_stok)
            {
                if (st.id == Convert.ToInt32(comboBox3.Text))
                {
                    string message = stok.Delete_stok(comboBox3.Text);
                    MessageBox.Show(message);
                    break;
                }
            }
            comboBox3.Text = "";
            comboBox3.Items.Clear();
            comboBox1.Items.Clear();
            comboBox4.Items.Clear();
            foreach (Stocks stok in stok.ls_stok)
            {
                comboBox1.Items.Add(stok.type);
                comboBox3.Items.Add(stok.id);
                comboBox4.Items.Add(stok.id);
            }
        }

        private void comboBox4_TextChanged(object sender, EventArgs e)
        {
            foreach (Stocks st in stok.ls_stok)
            {
                try
                {
                    if (st.id == Convert.ToInt32(comboBox4.Text))
                    {
                        textBox3.Text = st.type;
                        textBox4.Text = st.name;
                        textBox6.Text = Convert.ToString(st.price);
                        textBox5.Text = Convert.ToString(st.count);
                    }
                }
                catch {  }
            }
        }

        private void button4_Click(object sender, EventArgs e) //сохранить
        {
            foreach (Stocks st in stok.ls_stok)
            {
                if (st.id == Convert.ToInt32(comboBox4.Text))
                {
                    stok.Editing_stok(comboBox4.Text, textBox3.Text, textBox4.Text, textBox6.Text, textBox5.Text);
                    MessageBox.Show("Изменения сохранены");
                    break;
                }
            }
            textBox3.Text = "";
            textBox4.Text = "";
            textBox6.Text = "";
            textBox5.Text = "";
        }
    }
}
