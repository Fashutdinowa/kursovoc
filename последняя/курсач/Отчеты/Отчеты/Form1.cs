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

namespace Отчеты
{
    public partial class Form1 : Form
    {
        Order_DB order_db = new Order_DB();
        Client_DB client = new Client_DB();
        Courier_DB courier = new Courier_DB();
        Empoloument_DB em = new Empoloument_DB();
        Stok_DB stock = new Stok_DB();
        public Form1()
        {
            InitializeComponent();
        }
        public void report_desin()
        {
            try
            {
                Excel.Application exApp = new Excel.Application();
                exApp.Visible = true;
                exApp.Workbooks.Add();
                Worksheet workSheet = (Worksheet)exApp.ActiveSheet;
                Excel.Range excelCells = (Excel.Range)workSheet.get_Range("A1", "G1").Cells;
                excelCells.Merge(Type.Missing);
                excelCells.Font.Size = 12;
                excelCells.Font.Italic = true;
                excelCells.Font.Bold = true;
                excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
                excelCells.VerticalAlignment = Excel.Constants.xlCenter;
                workSheet.Cells[1, 1] = "Отчет о деятельности фирмы за " + dateTimePicker1.Text + "(принятых заказов)";
                workSheet.Cells[2, 1] = "Номер заказа";
                workSheet.Cells[2, 2] = "Дата регистрации заказа";
                workSheet.Cells[2, 3] = "Дата дoставки заказа";
                workSheet.Cells[2, 4] = "Сумма заказа (руб)";
                workSheet.Cells[2, 5] = "Скидки(руб)";
                workSheet.Cells[2, 6] = "К оплате (руб)";
                workSheet.Cells[2, 7] = "Статус";
                excelCells = (Excel.Range)workSheet.get_Range("A2", "G2");
                excelCells.Font.Bold = true;
                int temp = 3;
                foreach (Orders.Order order in order_db.report_order(dateTimePicker1.Value.Date))
                {
                    workSheet.Cells[temp, 1] = order.number.ToString();
                    workSheet.Cells[temp, 2] = order.data_order.ToString("dd.MM.yyyy");
                    workSheet.Cells[temp, 3] = order.devilery_data.ToString("dd.MM.yyyy");
                    workSheet.Cells[temp, 4] = order.sum.ToString();
                    workSheet.Cells[temp, 5] = order.discount.ToString();
                    workSheet.Cells[temp, 6] = (order.sum - order.discount).ToString();
                    workSheet.Cells[temp, 7] = order.status;
                    workSheet.Columns.AutoFit();
                    temp++;
                }
                var cells = workSheet.get_Range("A2", "G" + (temp - 1).ToString());
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                workSheet = exApp.Worksheets[2];
                excelCells = (Excel.Range)workSheet.get_Range("A1", "G1").Cells;
                excelCells.Merge(Type.Missing);
                excelCells.Font.Size = 12;
                excelCells.Font.Italic = true;
                excelCells.Font.Bold = true;
                excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
                excelCells.VerticalAlignment = Excel.Constants.xlCenter;
                workSheet.Cells[1, 1] = "Отчет о деятельности фирмы за " + dateTimePicker1.Text + "(обслуженных заказов)";
                workSheet.Cells[2, 1] = "Номер заказа";
                workSheet.Cells[2, 2] = "Дата регистрации заказа";
                workSheet.Cells[2, 3] = "Дата дoставки заказа";
                workSheet.Cells[2, 4] = "Сумма заказа (руб)";
                workSheet.Cells[2, 5] = "Скидки(руб)";
                workSheet.Cells[2, 6] = "К оплате (руб)";
                workSheet.Cells[2, 7] = "Статус";
                excelCells = (Excel.Range)workSheet.get_Range("A2", "G2");
                excelCells.Font.Bold = true;
                temp = 3;
                foreach (Orders.Order order in order_db.report_order_delivery(dateTimePicker1.Value.Date))
                {
                    workSheet.Cells[temp, 1] = order.number.ToString();
                    workSheet.Cells[temp, 2] = order.data_order.ToString("dd.MM.yyyy");
                    workSheet.Cells[temp, 3] = order.devilery_data.ToString("dd.MM.yyyy");
                    workSheet.Cells[temp, 4] = order.sum.ToString();
                    workSheet.Cells[temp, 5] = order.discount.ToString();
                    workSheet.Cells[temp, 6] = (order.sum - order.discount).ToString();
                    workSheet.Cells[temp, 7] = order.status;
                    workSheet.Columns.AutoFit();
                    temp++;
                }
                cells = workSheet.get_Range("A2", "G" + (temp - 1).ToString());
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            }
            catch
            { MessageBox.Show("ERROR!!! Проверьте правильность вводимых данных или повторите попытку позже"); }
        }
        public void report_desin(DateTime day)
        {
            try
            {
                Excel.Application exApp = new Excel.Application();
                exApp.Visible = true;
                exApp.Workbooks.Add();
                Worksheet workSheet = (Worksheet)exApp.ActiveSheet;
                Excel.Range excelCells = (Excel.Range)workSheet.get_Range("A1", "G1").Cells;
                excelCells.Merge(Type.Missing);
                excelCells.Font.Size = 12;
                excelCells.Font.Italic = true;
                excelCells.Font.Bold = true;
                excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
                excelCells.VerticalAlignment = Excel.Constants.xlCenter;
                workSheet.Cells[1, 1] = "Отчет о деятельности фирмы за " + dateTimePicker1.Value.ToShortDateString()+" - "+day.ToShortDateString()+  "( принятых заказов)";
                workSheet.Cells[2, 1] = "Номер заказа";
                workSheet.Cells[2, 2] = "Дата регистрации заказа";
                workSheet.Cells[2, 3] = "Дата дoставки заказа";
                workSheet.Cells[2, 4] = "Сумма заказа (руб)";
                workSheet.Cells[2, 5] = "Скидки(руб)";
                workSheet.Cells[2, 6] = "К оплате (руб)";
                workSheet.Cells[2, 7] = "Статус";
                excelCells = (Excel.Range)workSheet.get_Range("A2", "G2");
                excelCells.Font.Bold = true;
                int temp = 3;
                foreach (Orders.Order order in order_db.report_order(day, dateTimePicker1.Value.Date))
                {
                    workSheet.Cells[temp, 1] = order.number.ToString();
                    workSheet.Cells[temp, 2] = order.data_order.ToString("dd.MM.yyyy");
                    workSheet.Cells[temp, 3] = order.devilery_data.ToString("dd.MM.yyyy");
                    workSheet.Cells[temp, 4] = order.sum.ToString();
                    workSheet.Cells[temp, 5] = order.discount.ToString();
                    workSheet.Cells[temp, 6] = (order.sum - order.discount).ToString();
                    workSheet.Cells[temp, 7] = order.status;
                    workSheet.Columns.AutoFit();
                    temp++;
                }
                var cells = workSheet.get_Range("A2", "G" + (temp-1).ToString());
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous; 
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;            
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous; 
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                workSheet = exApp.Worksheets[2];
                excelCells = (Excel.Range)workSheet.get_Range("A1", "G1").Cells;
                excelCells.Merge(Type.Missing);
                excelCells.Font.Size = 12;
                excelCells.Font.Italic = true;
                excelCells.Font.Bold = true;
                excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
                excelCells.VerticalAlignment = Excel.Constants.xlCenter;
                workSheet.Cells[1, 1] = "Отчет о деятельности фирмы за " + dateTimePicker1.Text + " - " + day.ToShortDateString() + "(обслуженных заказов)";
                workSheet.Cells[2, 1] = "Номер заказа";
                workSheet.Cells[2, 2] = "Дата регистрации заказа";
                workSheet.Cells[2, 3] = "Дата дoставки заказа";
                workSheet.Cells[2, 4] = "Сумма заказа (руб)";
                workSheet.Cells[2, 5] = "Скидки(руб)";
                workSheet.Cells[2, 6] = "К оплате (руб)";
                workSheet.Cells[2, 7] = "Статус";
                excelCells = (Excel.Range)workSheet.get_Range("A2", "G2");
                excelCells.Font.Bold = true;
                temp = 3;
                foreach (Orders.Order order in order_db.report_order_delivery(day, dateTimePicker1.Value.Date))
                {
                    workSheet.Cells[temp, 1] = order.number.ToString();
                    workSheet.Cells[temp, 2] = order.data_order.ToString("dd.MM.yyyy");
                    workSheet.Cells[temp, 3] = order.devilery_data.ToString("dd.MM.yyyy");
                    workSheet.Cells[temp, 4] = order.sum.ToString();
                    workSheet.Cells[temp, 5] = order.discount.ToString();
                    workSheet.Cells[temp, 6] = (order.sum - order.discount).ToString();
                    workSheet.Cells[temp, 7] = order.status;
                    workSheet.Columns.AutoFit();
                    temp++;
                }
                cells = workSheet.get_Range("A2", "G" + (temp - 1).ToString());
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            }
            catch
            { MessageBox.Show("ERROR!!! Проверьте правильность вводимых данных или повторите попытку позже"); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            report_desin();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Diagramma_populity propulity = new Diagramma_populity();
            propulity.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DB db = new DB();
            db.DB_load();

            //client.Add_client("Merina ii", "232323", "aaaa@aaa.com", "skalkalkfa", "1");
            //stock.Add_stok("tort", "вылорао", "123", "100");
            //stock.Add_stok("tort", "вцлоаi", "123", "100");
            //stock.Add_stok("tort", "mылвщззфЗЩ", "123", "100");
            //courier.Add_courier("ivanov i. i", "9798987606");
            string[] s = new string[3];
            s[0] = "tort napoleon 2 ";
            s[1] = "tort вцлоаi 1";
            s[2] = "tort medovic 2 ";
            List<string> ls = new List<string>();
            ls.AddRange(s);
            order_db.add_order("1", DateTime.Today.AddDays(-1).ToString(), "juihho", "2", ls, "123", "6", "totr");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            report_desin(dateTimePicker1.Value.Date.AddDays(-7));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            report_desin(dateTimePicker1.Value.Date.AddMonths(-1));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            report_desin(dateTimePicker1.Value.Date.AddMonths(-3));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                List<Stock.Stocks> ls = stock.report_popularity();
                Excel.Application exApp = new Excel.Application();
                object misValue = System.Reflection.Missing.Value;
                exApp.Visible = true;
                exApp.Workbooks.Add(misValue);
                Worksheet workSheet = (Worksheet)exApp.ActiveSheet;
                Excel.Range excelCells = (Excel.Range)workSheet.get_Range("A1", "E1").Cells;
                excelCells.Merge(Type.Missing);
                excelCells.Font.Size = 12;
                excelCells.Font.Italic = true;
                excelCells.Font.Bold = true;
                excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
                excelCells.VerticalAlignment = Excel.Constants.xlCenter;
                workSheet.Cells[1, 1] = "Статистика популярности товаров";
                workSheet.Cells[2, 1] = "№п/п";
                workSheet.Cells[2, 2] = "Тип";
                workSheet.Cells[2, 3] = "Название"; ;
                workSheet.Cells[2, 4] = "Коэф. популярности";
                excelCells = (Excel.Range)workSheet.get_Range("A2", "G2");
                excelCells.Font.Bold = true;
                int temp = 3;
                foreach (Stock.Stocks stock in ls)
                {
                    workSheet.Cells[temp, 1] = stock.id.ToString();
                    workSheet.Cells[temp, 2] = stock.type;
                    workSheet.Cells[temp, 3] = stock.name;
                    workSheet.Cells[temp, 4] = stock.count.ToString();
                    workSheet.Columns.AutoFit();
                    temp++;
                }
                var cells = workSheet.get_Range("A2", "D" + (temp - 1).ToString());
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                Excel.ChartObjects chartsobjrcts = (Excel.ChartObjects)workSheet.ChartObjects(Type.Missing);
                Excel.ChartObject chartsobjrct = chartsobjrcts.Add(250, 20, 400, 350);
                chartsobjrct.Chart.ChartWizard(workSheet.get_Range("C2", "D" + (temp - 1).ToString()), Excel.XlChartType.xlColumnClustered, 2, Excel.XlRowCol.xlRows, Type.Missing, 0, true, "Статистика популярности товаров");
            }
            catch { MessageBox.Show("ERROR!!!"); }
        }           
    }
}
