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
using Data_processing;
using Employments;

namespace OOO_Holiday
{
    public partial class Form_route : Form
    {
        Courier_DB courier = new Courier_DB();
        Processing proc = new Processing();
        Empoloument_DB emplou = new Empoloument_DB();
        public Form_route()
        {
            InitializeComponent();
            this.Text = "Маршрутный лист";
        }

        private void Form_route_Load(object sender, EventArgs e)
        {
            courier.loading_courier();
            foreach(Courier cour in courier.ls_courier)
            {
                comboBox1.Items.Add(cour.FIO);
            }
        }

        private void button1_Click(object sender, EventArgs e) //показать маршрутынй лист
        {
            List<string> courier_list = new List<string>();
            emplou.ls_empoloument.Clear();
            emplou.loading_empoloument();
            if (comboBox1.Text != "")
            {
                foreach (Empoloument emp in emplou.ls_empoloument)
                {
                    if (emp.fio == comboBox1.Text && emp.data == dateTimePicker1.Value)
                    {
                        courier_list.Add(emp.address);
                    }
                }
                if (courier_list.Count != 0)
                {
                    try
                    {
                        proc.Route_list(comboBox1.Text, courier_list);
                    }
                    catch { MessageBox.Show("Ошибка"); }
                }
                else MessageBox.Show("Список заказов на данное число пуст");
            }
            else MessageBox.Show("Поле с фамилией курьера не заполнено");
        }

        private void button2_Click(object sender, EventArgs e) //напечатать
        {
            List<string> courier_list = new List<string>();
            emplou.ls_empoloument.Clear();
            emplou.loading_empoloument();
            if (comboBox1.Text != "")
            {
                foreach (Empoloument emp in emplou.ls_empoloument)
                {
                    if (emp.fio == comboBox1.Text && emp.data == dateTimePicker1.Value)
                    {
                        courier_list.Add(emp.address);
                    }
                }
                if (courier_list.Count != 0)
                {
                    try
                    {
                        proc.Print(@"rout.pdf");
                    }
                    catch { MessageBox.Show("Проверьте подключение к принтеру"); }
                }
                else MessageBox.Show("Список заказов на данное число пуст");
            }
            else MessageBox.Show("Поле с фамилией курьера не заполнено");
        }
    }
}
