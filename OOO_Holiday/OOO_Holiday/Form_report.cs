using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using Database;
using System.IO;

namespace OOO_Holiday
{
    public partial class Form_report : Form
    {
        Order_DB order_db = new Order_DB();
        Client_DB client = new Client_DB();
        Courier_DB courier = new Courier_DB();
        Empoloument_DB em = new Empoloument_DB();
        Stok_DB stock = new Stok_DB();
        Report.Reports report = new Report.Reports();
        
        public Form_report()
        {
            InitializeComponent();
            this.Text = "Отчёты и статистика";
        }
      

        private void button1_Click_1(object sender, EventArgs e) //день
        {
            try
            {
                report.report_activiti(dateTimePicker1.Value.Date);
            }
            catch
            {
                MessageBox.Show("ERROR!!!");
            }
        } 

        private void button2_Click_1(object sender, EventArgs e) //неделя
        {
            try
            {
                report.report_activiti(dateTimePicker1.Value.Date, dateTimePicker1.Value.Date.AddDays(-7));
            }
            catch
            {
                MessageBox.Show("ERROR!!!");
            }
            
        }

        private void button3_Click_1(object sender, EventArgs e) //месяц
        {
            try
            {
                report.report_activiti(dateTimePicker1.Value.Date, dateTimePicker1.Value.Date.AddMonths(-1));
            }
            catch
            {
                MessageBox.Show("ERROR!!!");
            }
        }

        private void button4_Click_1(object sender, EventArgs e) //квартал
        {
            try
            {
                report.report_activiti(dateTimePicker1.Value.Date, dateTimePicker1.Value.Date.AddMonths(-3));
            }
            catch
            {
                MessageBox.Show("ERROR!!!");
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Diagramma_populity propulity = new Diagramma_populity();
            propulity.Show();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            try
            {
                report.populity();
            }
            catch { MessageBox.Show("ERROR!!!"); }
        }

        private void Form_report_Load(object sender, EventArgs e)
        {
            DB db = new DB();
            db.DB_load();
        }
    }
}
