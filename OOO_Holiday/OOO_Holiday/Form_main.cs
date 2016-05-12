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
using System.IO;
using Data_processing;
using Clients;
using Stock;
using Couriers;
using Orders;
using Employments;
using System.Data.SQLite;


namespace OOO_Holiday
{
    public partial class Form_main : Form
    {
        Processing proc = new Processing();
        Courier_DB courier = new Courier_DB();

        public Form_main()
        {
            InitializeComponent();
            this.Text = "Главное меню";

            ToolStripMenuItem menu = new ToolStripMenuItem("Меню");
            ToolStripMenuItem order = new ToolStripMenuItem("Оформление заказа");
            ToolStripMenuItem report = new ToolStripMenuItem("Отчёты и статистика");
            ToolStripMenuItem route = new ToolStripMenuItem("Маршрутный лист");
            ToolStripMenuItem mail = new ToolStripMenuItem("Рассылка");
            ToolStripMenuItem datebase = new ToolStripMenuItem("Работа с базой данных");
            ToolStripMenuItem product = new ToolStripMenuItem("Товар");
            ToolStripMenuItem courier = new ToolStripMenuItem("Курьер");
            ToolStripMenuItem client = new ToolStripMenuItem("Клиент");
            menuStrip1.Items.Add(menu);

            order.Click += click_order;
            report.Click += click_report;
            route.Click += click_route;
            mail.Click += click_mail;
            product.Click += click_product;
            courier.Click += click_courier;
            client.Click += click_client;

            menu.DropDownItems.Add(order);
            menu.DropDownItems.Add(report);
            menu.DropDownItems.Add(route);
            menu.DropDownItems.Add(mail);
            menu.DropDownItems.Add(datebase);
            datebase.DropDownItems.Add(product);
            datebase.DropDownItems.Add(courier);
            datebase.DropDownItems.Add(client);
        }

        void click_order(object sender, EventArgs e) //оформить заказ
        {
            Form_client client = new Form_client();
            client.Show();
        }
        void click_report(object sender, EventArgs e) //отчёты и статистика
        {
            Form_report report = new Form_report();
            report.Show();
        }
        void click_route(object sender, EventArgs e) //маршрутный лист
        {
            Form_route route = new Form_route();
            route.Show();
        }
        void click_mail(object sender, EventArgs e) //рассылка
        {
            string date_line = "";
            if (File.Exists("Data_send.txt"))
            {
                using (StreamReader sr = new StreamReader("Data_send.txt"))
                {
                    date_line = sr.ReadToEnd();
                }
            }
            DialogResult result = MessageBox.Show("Выполнить рассылку? (последняя: " + date_line + ")",
            "",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.DefaultDesktopOnly);
            if (result == DialogResult.Yes)
            {
                Processing p = new Processing();
                Client_DB client = new Client_DB();
                Stok_DB stok = new Stok_DB();
                List<string> list = new List<string>();
                client.loading_client();
                stok.loading_stock();
                int count = 0;
                foreach (Stocks st in stok.ls_stok)
                {
                    list.Add(st.type + " " + st.name + " - " + st.price);
                }
                try
                {
                    foreach (Client cl in client.ls_client)
                    {
                        if (cl.email != "")
                            p.Mail(cl.FIO, cl.email, list, count);
                        count++;
                    }
                }
                catch { MessageBox.Show("При отправке произошли ошибки"); return; }
                using (StreamWriter outputFile = new StreamWriter(@"Data_send.txt")) //сохранение даты отправки в файл
                {
                    outputFile.WriteLine(DateTime.Now);
                }
                MessageBox.Show("Отправка сообщений прошла успешно");
            }
        }

        void click_product(object sender, EventArgs e) //товар
        {
            Form_product product = new Form_product();
            product.Show();
        }
        void click_courier(object sender, EventArgs e) //курьер
        {
            Form_courier courier = new Form_courier();
            courier.Show();
        }
        void click_client(object sender, EventArgs e) //клиент
        {
            Form_edit_client edit_client = new Form_edit_client();
            edit_client.Show();
        }

        private void Form_main_Load(object sender, EventArgs e)
        {
            DB db = new DB();
            Client_DB client = new Client_DB();
            Courier_DB courier = new Courier_DB();
            Stok_DB stok = new Stok_DB();
            Order_DB order = new Order_DB();
            Empoloument_DB emploument = new Empoloument_DB();

            db.DB_load();
            DataTable client_bd = new DataTable();
            client_bd.Load(client.Select_client());
            dataGridView1.DataSource = client_bd;
            DataTable courier_bd = new DataTable();
            courier_bd.Load(courier.Select_courier());
            dataGridView2.DataSource = courier_bd;
            DataTable stok_bd = new DataTable();
            stok_bd.Load(stok.Select_stok());
            dataGridView3.DataSource = stok_bd;
            DataTable order_bd = new DataTable();
            order_bd.Load(order.Select_order());
            dataGridView4.DataSource = order_bd;
            DataTable emploument_bd = new DataTable();
            emploument_bd.Load(emploument.Select_empoloyment());
            dataGridView5.DataSource = emploument_bd;
        }
        
        private void Form_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            Client_DB client = new Client_DB();
           
            Stok_DB stok = new Stok_DB();
            Order_DB order = new Order_DB();
            Empoloument_DB emploument = new Empoloument_DB();

            db.DB_load();
            DataTable client_bd = new DataTable();
            client_bd.Load(client.Select_client());
            dataGridView1.DataSource = client_bd;
            DataTable courier_bd = new DataTable();
            courier_bd.Load(courier.Select_courier());
            dataGridView2.DataSource = courier_bd;
            DataTable stok_bd = new DataTable();
            stok_bd.Load(stok.Select_stok());
            dataGridView3.DataSource = stok_bd;
            DataTable order_bd = new DataTable();
            order_bd.Load(order.Select_order());
            dataGridView4.DataSource = order_bd;
            DataTable emploument_bd = new DataTable();
            emploument_bd.Load(emploument.Select_empoloyment());
            dataGridView5.DataSource = emploument_bd;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            courier.loading_courier();
            label6.Text = courier.ls_courier.Count.ToString();
        }
    }
}
