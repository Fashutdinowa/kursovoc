using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using Orders;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace Report
{
    public class Reports
    {
        Order_DB order = new Order_DB();
        Stok_DB stok = new Stok_DB();
        public List<Order> report_order(DateTime data_start, DateTime data_end)
        {
            List<Order> list_order = new List<Order>();
            order.loading_order();
            list_order = order.ls_order.FindAll(i => ((i.data_order.Date >= data_start) && (i.data_order.Date <= data_end)));
            return list_order;
        }
        public List<Order> report_order(DateTime data_start)
        {
            List<Order> list_order = new List<Order>();
            order.loading_order();
            list_order = order.ls_order.FindAll(i => i.data_order.Date == data_start);
            return list_order;
        }
        public List<Order> report_order_delivery(DateTime data_start, DateTime data_end)
        {
            List<Order> list_order = new List<Order>();
            order.loading_order();
            list_order = order.ls_order.FindAll(i => ((i.devilery_data.Date >= data_start) && (i.devilery_data.Date <= data_end)));
            return list_order;
        }
        public List<Order> report_order_delivery(DateTime data_start)
        {
            List<Order> list_order = new List<Order>();
            order.loading_order();
            list_order = order.ls_order.FindAll(i => (i.devilery_data >= data_start) && (i.devilery_data <= data_start));
            return list_order;
        }
        public void report_activiti(DateTime dateTime)
        {
            Excel.Application exApp = new Excel.Application();
            exApp.SheetsInNewWorkbook = 2;
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
            workSheet.Cells[1, 1] = "Отчет о деятельности фирмы за " + dateTime.Date.ToString("dd.MM.yyyy") +"(принятых заказов)";
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
            foreach (Orders.Order order in report_order(dateTime.Date))
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
            workSheet.Cells[1, 1] = "Отчет о деятельности фирмы за " + dateTime.Date.ToString("dd.MM.yyyy") + "(обслуженных заказов)";
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
            foreach (Orders.Order order in report_order_delivery(dateTime.Date))
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

        public void report_activiti(DateTime dateTime, DateTime day)
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
            workSheet.Cells[1, 1] = "Отчет о деятельности фирмы за " + dateTime.ToShortDateString() + " - " + day.ToShortDateString() + "( принятых заказов)";
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
            foreach (Orders.Order order in report_order(day, dateTime.Date))
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
            workSheet.Cells[1, 1] = "Отчет о деятельности фирмы за " + dateTime.ToShortDateString() + " - " + day.ToShortDateString() + "(обслуженных заказов)";
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
            foreach (Orders.Order order in report_order_delivery(day, dateTime.Date))
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
        public void populity()
        {
            List<Stock.Stocks> ls = stok.report_popularity();
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
    }
}
