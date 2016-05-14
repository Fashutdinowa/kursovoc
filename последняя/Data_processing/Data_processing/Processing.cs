using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp;
using System.IO;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using System.Text.RegularExpressions;
using Stock;
using System.Diagnostics;



namespace Data_processing
{
    public class Processing
    {
        public void Mail(string client, string address, List<string> product_list, int count) //рассылка
        {
            var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(@"mailing" + Convert.ToString(count) + ".pdf", FileMode.Create));
            doc.Open();

            BaseFont baseFont = BaseFont.CreateFont(@"times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            iTextSharp.text.Phrase text = new Phrase("Здравствуйте " + client + ". Мы рады представить вам список наших предложений.", new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.NORMAL, new BaseColor(Color.Black)));
            doc.Add(text);

            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Прайс лист", new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.NORMAL, new BaseColor(Color.Orange))));
            cell.BackgroundColor = new BaseColor(Color.Black);
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Товар", new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.NORMAL, new BaseColor(Color.Black))));
            cell.BackgroundColor = new BaseColor(Color.White);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Цена (p.)", new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.NORMAL, new BaseColor(Color.Black))));
            cell.BackgroundColor = new BaseColor(Color.White);
            table.AddCell(cell);

            for (int i = 0; i < product_list.Count; i++)
            {
                string[] new_string = product_list[i].Split(new char[] { '-' });

                cell = new PdfPCell(new Phrase(new_string[0], new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.NORMAL, new BaseColor(Color.Black))));
                cell.BackgroundColor = new BaseColor(Color.White);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(new_string[1], new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.NORMAL, new BaseColor(Color.Black))));
                cell.BackgroundColor = new BaseColor(Color.White);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

            }
            doc.Add(table);
            doc.Close();

            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("sm.lily7@yandex.ru", "OOO Holiday");
            // кому отправляем
            MailAddress to = new MailAddress(address);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Прайс лист";
            // письмо представляет код html
            m.IsBodyHtml = true;
            m.Attachments.Add(new Attachment("mailing" + Convert.ToString(count) + ".pdf"));
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 25);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("sm.lily7@yandex.ru", "kylexy15");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }

        public void Route_list(string courier, List<string> addres) //маршрутный лист
        {
                WebClient Client = new WebClient();
                int temp = 0;
                List<string> rout = new List<string>();
                rout.Add(addres[temp]);
                while (addres.Count != 1)
                {
                    string val_address = "";
                    double min_path = 10000;
                    int number_address = 0;
                    int count = 0;
                    for (int j = 0; j < addres.Count; j++)
                    {
                        if (j != temp)
                        {
                            using (Stream strm = Client.OpenRead("http://maps.googleapis.com/maps/api/directions/xml?origin=Ульяновск " + addres[temp] + "&destination=Ульяновск " + addres[j] + "&sensor=false"))
                            {
                                StreamReader sr = new StreamReader(strm);
                                FileStream f = new FileStream(count + ".xml", FileMode.CreateNew);
                                StreamWriter sw = new StreamWriter(f);
                                sw.WriteLine(sr.ReadToEnd());
                                sw.Close();
                                f.Close();

                                XmlDocument doc = new XmlDocument();
                                doc.Load(count + ".xml");
                                XmlNodeList elements = doc.GetElementsByTagName("distance");
                                string str = elements[elements.Count - 1].InnerXml;
                                string match = Regex.Match(str, @"(?<=<text>)(.*)(?=</text>)").ToString();
                                match = match.Replace("k", "").Replace("m", "").Replace(" ", "").Replace(".", ",");
                                double match_double = Convert.ToDouble(match);
                                if (match_double < min_path)
                                {
                                    min_path = Convert.ToDouble(match);
                                    number_address = j;
                                    val_address = addres[j];
                                }
                                count++;
                            }
                        }
                    }
                    rout.Add(val_address);
                    addres.RemoveAt(temp);
                    temp = addres.IndexOf(val_address);
                    for (int i = 0; i < count; i++)
                    {
                        File.Delete(i + ".xml");
                    }
                }
                var pdf = new Document();
                PdfWriter.GetInstance(pdf, new FileStream(@"rout.pdf", FileMode.Create));
                pdf.Open();

                BaseFont baseFont = BaseFont.CreateFont(@"times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                foreach (string str in rout)
                {
                    iTextSharp.text.Phrase text = new Phrase(str + Environment.NewLine, new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.NORMAL, new BaseColor(Color.Black)));
                    pdf.Add(text);
                }
                pdf.Close();
                System.Diagnostics.Process.Start("rout.pdf");
        }

        public void Check(List<string> prise_line, double prise, string data) //чек
        {
            var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(@"check.pdf", FileMode.Create));
            doc.Open();

            BaseFont baseFont = BaseFont.CreateFont(@"times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            foreach (string stoc in prise_line)
            {
                iTextSharp.text.Phrase list_product = new Phrase(Convert.ToString(stoc) + Environment.NewLine, new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.NORMAL, new BaseColor(Color.Black)));
                doc.Add(list_product);
            }
            iTextSharp.text.Phrase sum = new Phrase("Общая сумма к оплате:  " + Convert.ToString(prise) + Environment.NewLine, new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.NORMAL, new BaseColor(Color.Black)));
            doc.Add(sum);
            iTextSharp.text.Phrase data_check = new Phrase(data, new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.NORMAL, new BaseColor(Color.Black)));
            doc.Add(data_check);
            doc.Close();
        }

        public void Print(string addres)
        {
            System.Diagnostics.Process command = new System.Diagnostics.Process();
            command.StartInfo.FileName = @"C:\Program Files (x86)\Adobe\Reader 10.0\Reader\AcroRd32.exe";
            command.StartInfo.Arguments = "/h /p " + addres;
            command.StartInfo.UseShellExecute = false;
            command.StartInfo.CreateNoWindow = true;
            command.Start();
            command.WaitForExit(31000);
            if (!command.HasExited)
                command.Kill();
            else command.Close();
        } //печать
    }
}
