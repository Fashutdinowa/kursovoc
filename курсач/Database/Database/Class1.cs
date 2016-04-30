using System;
using System.Collections.Generic;
using Clients;
using Orders;
using Couriers;
using Stock;
using Employments;
using System.Data.SQLite;
using System.IO;

namespace Database
{
    public class DB
    {
        public void DB_load()
        {

            if (!File.Exists("mydb.sqlite"))
            {
                SQLiteConnection.CreateFile("mydb.sqlite");
                SQLiteConnection sql = new SQLiteConnection("Data Source=mydb.sqlite;Version=3");
                SQLiteCommand sc = new SQLiteCommand
                (@"create table CLIENT (
                    id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    FIO TEXT NOT NULL,
                    phone_number TEXT NOT NULL, 
                    email TEXT NOT NULL, 
                    address TEXT NOT NULL, 
                    bonus INTEGER NOT NULL,
                    discount INTEGER NOT NULL,
                    parent_personal_card INTEGER,
                    child_personal_card INTEGER )", sql);
                sql.Open();
                sc.ExecuteNonQuery();
                sc = new SQLiteCommand
                (@"create table COURIER (
                    id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    FIO TEXT NOT NULL,
                    phone_number TEXT NOT NULL)", sql);
                sc.ExecuteNonQuery();
                sc = new SQLiteCommand
                (@"create table STOK_HOLYDAY (
                    id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    type TEXT NOT NULL,
                    name TEXT NOT NULL,
                    prise REAL NOT NULL,
                    count integer NOT NULL)", sql);
                sc.ExecuteNonQuery();
                sc = new SQLiteCommand
                (@"create table ORDER_HOLYDAY (id INTEGER PRIMARY KEY AUTOINCREMENT,
                number_person_card INTEGER NOT NULL,
                data_order TEXT NOT NULL,
                devilery_data TEXT NOT NULL,
                adress TEXT NOT NULL,
                courier INTEGER NOT NULL,
                sum REAL NOT NULL,
                order_status TEXT NOT NULL,
discount real not null,
CONSTRAINT client_of_order_FK 
FOREIGN KEY  (number_person_card)
REFERENCES CLIENT (ID)
ON DELETE CASCADE,
CONSTRAINT courier_of_order_FK 
FOREIGN KEY  (courier)
REFERENCES CLIENT (id)
ON DELETE CASCADE)", sql); sc.ExecuteNonQuery();
                sc = new SQLiteCommand
               (@"create table stok_order_key (
id integer primary key autoincrement,
id_order integer not null,
id_stock integer not null,
count_stock integer not null,
CONSTRAINT order_FK 
FOREIGN KEY  (id_order)
REFERENCES ORDER_HOLYDAY  (id)
ON DELETE CASCADE,
CONSTRAINT stock_FK 
FOREIGN KEY  (id_stock)
REFERENCES STOK (id)
ON DELETE CASCADE)", sql);
                sc.ExecuteNonQuery();
                sc = new SQLiteCommand
               (@"create table employment (
id integer primary key autoincrement,
id_order integer not null,
id_courier integer not null,
role text not null,
CONSTRAINT order_employment_FK 
FOREIGN KEY  (id_order)
REFERENCES ORDER_HOLYDAY (ID)
ON DELETE CASCADE,
CONSTRAINT courier_employment_FK 
FOREIGN KEY  (id_courier)
REFERENCES COURIER (ID)
ON DELETE CASCADE)", sql);
                sc.ExecuteNonQuery();
                sql.Close();
            }
            else
            {
                SQLiteConnection sql = new SQLiteConnection("Data Source=mydb.sqlite;Version=3");
                sql.Open();
                SQLiteCommand sc = new SQLiteCommand("update ORDER_HOLYDAY set  order_status = 'выполнено' where devilery_data LIKE '%" + DateTime.Today.AddDays(-1).ToString() + "%'", sql); sc.ExecuteNonQuery();
                sql.Close();
                Client_DB client = new Client_DB();
                client.loading_client();
                Order_DB order = new Order_DB();
                order.loading_order();
                Courier_DB courier = new Courier_DB();
                courier.loading_courier();
                Stok_DB stok = new Stok_DB();
                stok.loading_stock();
                Empoloument_DB em = new Empoloument_DB();
                em.delete_empoloyment(DateTime.Today.AddMonths(-3));
                em.loading_empoloument();

            }
        }
    }
    public class Client_DB
    {
        List<Client> list_client = new List<Client>();
        public List<Client> ls_client
        {
            get {return list_client; }
            set {list_client = value; }
        }
        public void loading_client()
        {
            list_client.Clear();
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("select * from CLIENT", sql);
            sql.Open();
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                list_client.Add(new Client(Convert.ToInt32(record["id"]), record["FIO"].ToString(), record["phone_number"].ToString(), record["email"].ToString(), record["address"].ToString(), Convert.ToInt32(record["bonus"]), Convert.ToInt32(record["discount"]), Convert.ToInt32(record["parent_personal_card"]), Convert.ToInt32(record["child_personal_card"])));
            }
            sql.Close();
        }
        public string Add_client(string FIO, string phone_number, string email, string address, string patern_number_card = "0")
        {
            string number_card = "";
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("insert into CLIENT (FIO, phone_number, email, address, bonus, discount, parent_personal_card, child_personal_card) values ('" + FIO + "', '" + phone_number + "', '" + email + "', '" + address + "', 0, 0, " + patern_number_card + ", 0)", sql);
            sql.Open();
            sc.ExecuteNonQuery();
            sc = new SQLiteCommand("select max(id) from CLIENT", sql);
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                number_card = record["max(id)"].ToString();
            }
            sc = new SQLiteCommand("update CLIENT set child_personal_card = " + number_card + " where id =" + patern_number_card + " and  child_personal_card = 0", sql); sc.ExecuteNonQuery();
            discount_client(number_card, patern_number_card);
            sql.Close();
            this.loading_client();
            return number_card;

        }
        public void discount_client(string number_card, string patern_number_card)
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("update CLIENT set discount = 1 where discount = 0 and child_personal_card =" + number_card, sql);
            sql.Open();
            sc.ExecuteNonQuery();
            string temp = "0";
            sc = new SQLiteCommand("select parent_personal_card from CLIENT where id = " + patern_number_card, sql);
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                sc = new SQLiteCommand("update CLIENT set discount = 2 where discount > 0 and discount < 2 and  child_personal_card =" + patern_number_card, sql); sc.ExecuteNonQuery();
                temp = record["parent_personal_card"].ToString();
            }
            sc = new SQLiteCommand("select id from CLIENT where id = " + temp, sql);
            SQLiteDataReader reader_client = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record_client in reader_client)
            {
                sc = new SQLiteCommand("update CLIENT set discount = 7 where discount > 1 and discount <3 and child_personal_card = " + record_client["id"].ToString(), sql); sc.ExecuteNonQuery();
            }
        }
        public void Editing_client(string number_person_card, string FIO, string phone_number, string email, string address, string patern_number_card)
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("update CLIENT set FIO = '" + FIO + "', phone_number = '" + phone_number + "', email ='" + email + "', parent_personal_card = " + patern_number_card + "  where id = " + number_person_card, sql);
            sql.Open();
            sc.ExecuteNonQuery();
            discount_client(number_person_card, patern_number_card);
            sql.Close();
            this.loading_client();

        }
        public string Delete_client(string number_person_card)
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("select id from ORDER_HOLYDAY where order_status  LIKE '%ожидание%' and  number_person_card =" + number_person_card, sql);
            sql.Open();
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                sql.Close();
                return "Удаление клиента " + number_person_card.ToString() + " невозможно. Пожалуйста, прорерьте правильность данных или убедитесь, что у данного клиента нет текущих заказов. При необходимости обновите список заказов.";
            }

            sc = new SQLiteCommand("delete from CLIENT where id = " + number_person_card, sql); sc.ExecuteNonQuery();
            this.loading_client();
            sql.Close();
            return "Удаление клиента " + number_person_card.ToString() + " выполнено успешно";
        }
        public SQLiteDataReader Select_client()
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            sql.Open();
            SQLiteCommand com = new SQLiteCommand(sql);
            com.CommandText = @"select * from CLIENT";
            SQLiteDataReader sdr = com.ExecuteReader();
            return sdr;
        }
    }
    public class Courier_DB
    {
        List<Courier> list_courier = new List<Courier>();
        public List<Courier> ls_courier
        {
            get { return list_courier; }
            set { list_courier = value; }
        }
        public void loading_courier()
        {
            list_courier.Clear();
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("select * from COURIER", sql);
            sql.Open();
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                list_courier.Add(new Courier(Convert.ToInt32(record["id"]), record["FIO"].ToString(), record["phone_number"].ToString()));
            }
            sql.Close();
        }
        public void Add_courier(string FIO, string phone_number)
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("insert into COURIER (FIO, phone_number) values ('" + FIO + "', '" + phone_number + "')", sql);
            sql.Open();
            sc.ExecuteNonQuery();
            sql.Close();
            this.loading_courier();
        }
        public void Editing_courier(string id, string FIO, string phone_number)
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("update COURIER set FIO = '" + FIO + "', phone_number = '" + phone_number + "' where id = " + id, sql);
            sql.Open();
            sc.ExecuteNonQuery();
            sql.Close();
            this.loading_courier();
        }
        public string Delete_courier(string id)
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("select * from ORDER_HOLYDAY  where order_status  LIKE '%ожидание%' and  courier =" + id, sql);
            sql.Open();
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                sql.Close();
                return "Удаление  курьера " + id.ToString() + " невозможно. Пожалуйста, прорерьте правильность данных. При необходимости обновите список заказов.";
            }
            sc = new SQLiteCommand("delete from COURIER where id = " + id, sql);
            sc.ExecuteNonQuery();
            sql.Close();
            this.loading_courier();
            return "Удаление произведено успешно";
        }
        public SQLiteDataReader Select_courier()
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            sql.Open();
            SQLiteCommand com = new SQLiteCommand(sql);
            com.CommandText = @"select * from COURIER";
            SQLiteDataReader sdr = com.ExecuteReader();
            return sdr;
        }
    }
    public class Stok_DB
    {
        List<Stocks> list_stok = new List<Stocks>();
        public List<Stocks> ls_stok
        {
            get { return list_stok; }
            set { list_stok = value; }
        }
        public void loading_stock()
        {
            list_stok.Clear();
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("select * from STOK_HOLYDAY", sql);
            sql.Open();
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                list_stok.Add(new Stocks(Convert.ToInt32(record["id"]), record["type"].ToString(), record["name"].ToString(), Convert.ToDouble(record["prise"]), Convert.ToInt32(record["count"])));
            }
            sql.Close();
        }
        public void Add_stok(string type, string name, string prise, string count)
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("insert into STOK_HOLYDAY (type, name, prise, count) values ('" + type + "', '" + name + "', " + prise + ", " + count + ")", sql);
            sql.Open();
            sc.ExecuteNonQuery();
            sql.Close();
            this.loading_stock();
        }
        public void Editing_stok(string id, string type, string name, string prise, string count)
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("update STOK_HOLYDAY set type = '" + type + "', name= '" + name + "', prise = " + prise + ", count = " + count + " where id = " + id, sql);
            sql.Open();
            sc.ExecuteNonQuery();
            sql.Close();
            this.loading_stock();
        }
        public string Delete_stok(string id)
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("select * from ORDER_HOLYDAY, stok_order_key  where ORDER_HOLYDAY.order_status  LIKE '%ожидание%' and  stok_order_key.id_stock =" + id, sql);
            sql.Open();
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                sql.Close();
                return "Удаление  товара  невозможно.";
            }
            sc = new SQLiteCommand("delete from STOK_HOLYDAY where id = " + id, sql);
            sc.ExecuteNonQuery();
            sql.Close();
            this.loading_stock();
            return "Удаление произведено успешно";
        }
        public SQLiteDataReader Select_stok()
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            sql.Open();
            SQLiteCommand com = new SQLiteCommand(sql);
            com.CommandText = @"select * from STOK_HOLYDAY";
            SQLiteDataReader sdr = com.ExecuteReader();
            return sdr;
        }
        public List<Stocks> report_popularity()
        {
            List<Stocks> list_stoks = new List<Stocks>();
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            sql.Open();
            SQLiteCommand sc = new SQLiteCommand("select id, type, prise, name from STOK_HOLYDAY", sql);
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                sc = new SQLiteCommand("select count(id_stock) from stok_order_key where id_stock = " + record["id"].ToString(), sql);
                SQLiteDataReader reader_report = sc.ExecuteReader();
                foreach (System.Data.Common.DbDataRecord record_report in reader_report)
                {
                    list_stoks.Add(new Stocks(Convert.ToInt32(record["id"]), record["type"].ToString(), record["name"].ToString(), Convert.ToDouble(record["prise"]), Convert.ToInt32(record_report["count(id_stock)"])));
                }
            }
            sql.Close();
            return list_stoks;
        }
    }
    public class Order_DB
    {
        List<Order> list_order = new List<Order>();
        public List<Order> ls_order
        {
            get { return list_order; }
            set { list_order = value; }
        }
        public void loading_order()
        {
            list_order.Clear();
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("select * from ORDER_HOLYDAY", sql);
            sql.Open();
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                list_order.Add(new Order(Convert.ToInt32(record["id"]), Convert.ToInt32(record["number_person_card"]), Convert.ToDateTime(record["data_order"]), Convert.ToDateTime(record["devilery_data"]), record["adress"].ToString(), Convert.ToInt32(record["courier"]), Convert.ToDouble(record["sum"]), record["order_status"].ToString(), Convert.ToDouble(record["discount"])));
            }
            sql.Close();
        }
        public void add_order(string number_card, string devilery_data, string adress, string courier, List<string> Stock, string sum, string discount = "0", string role = "")
        {
            DateTime now = DateTime.Now;
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("insert into ORDER_HOLYDAY (number_person_card, data_order, devilery_data, adress, courier,  sum, order_status, discount) values (" + number_card + ", '" + now.ToString() + "', '" + devilery_data + "', '" + adress + "', " + courier + ",  " + sum + ", 'ожидание', " + discount + " )", sql);
            sql.Open();
            sc.ExecuteNonQuery();
            string id = "0";
            sc = new SQLiteCommand(@"select max(id) FROM ORDER_HOLYDAY", sql);
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                id = record["max(id)"].ToString();
            }
            Stok_DB stock = new Stok_DB();
            foreach (string s in Stock)
            {
                string[] stocks = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                sc = new SQLiteCommand("select id from STOK_HOLYDAY where type LIKE '" + stocks[0] + "' and name LIKE '" + stocks[1] + "'", sql);
                reader = sc.ExecuteReader();
                foreach (System.Data.Common.DbDataRecord record in reader)
                {
                    sc = new SQLiteCommand("update STOK_HOLYDAY set count = count -" + stocks[2] + " where id = " + record["id"].ToString(), sql); sc.ExecuteNonQuery();
                    sc = new SQLiteCommand("insert into stok_order_key (id_order, id_stock, count_stock)  values ( " + id + ", " + record["id"].ToString() + ", " + stocks[2] + ")", sql); sc.ExecuteNonQuery();
                }
            }
            stock.loading_stock();
            sc = new SQLiteCommand(@"select discount, bonus from CLIENT where id = " + number_card, sql);
            reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                sc = new SQLiteCommand("update CLIENT SET bonus = " + (Convert.ToInt32(record["bonus"]) + (Convert.ToInt32(sum) * Convert.ToInt32(record["discount"]) / 100) - Convert.ToInt32(discount)).ToString() + " where id = " + number_card, sql); sc.ExecuteNonQuery();
            }
            Empoloument_DB empolument = new Empoloument_DB();
            empolument.Add_empoloument(id, courier, role);
            sql.Close();
            this.loading_order();
        }
        public void delete_order(string id)
        {
            Empoloument_DB em = new Empoloument_DB();
            Stok_DB st = new Stok_DB();
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("delete from employment where id_order = " + id, sql);
            sql.Open();
            sc.ExecuteNonQuery();
            sc = new SQLiteCommand("select id_stock, count_stock from stok_order_key where id_order = " + id, sql);
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                sc = new SQLiteCommand("update STOK_HOLYDAY set count = count +" + record["count_stock"].ToString() + " where id = " + record["id_stock"].ToString(), sql); sc.ExecuteNonQuery();
            }
            sc = new SQLiteCommand("delete from stok_order_key where id_order = " + id, sql); sc.ExecuteNonQuery();
            sc = new SQLiteCommand("delete from ORDER_HOLYDAY where id = " + id, sql);
            sc.ExecuteNonQuery();
            sql.Close();
            this.loading_order();
            st.loading_stock();
            em.loading_empoloument();
        }
        public SQLiteDataReader Select_order()
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            sql.Open();
            SQLiteCommand com = new SQLiteCommand(sql);
            com.CommandText = @"select * from ORDER_HOLYDAY";
            SQLiteDataReader sdr = com.ExecuteReader();
            return sdr;
        }
       

    }
    public class Empoloument_DB
    {
        List<Empoloument> list_empoloument = new List<Empoloument>();
        public List<Empoloument> ls_empoloument
        {
            get { return list_empoloument; }
            set { list_empoloument = value; }
        }
        public void loading_empoloument()
        {
            list_empoloument.Clear();
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("select * from employment", sql);
            sql.Open();
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                string fio = "";
                sc = new SQLiteCommand("select  FIO from courier where id = " + record["id_courier"].ToString(), sql);
                SQLiteDataReader reader_courier = sc.ExecuteReader();
                foreach (System.Data.Common.DbDataRecord record_order in reader_courier)
                {
                    fio = reader_courier["FIO"].ToString();
                }
                sc = new SQLiteCommand("select  devilery_data, adress from ORDER_HOLYDAY where id = " + record["id_order"].ToString(), sql);
                SQLiteDataReader reader_order = sc.ExecuteReader();
                foreach (System.Data.Common.DbDataRecord record_order in reader_order)
                {
                    list_empoloument.Add(new Empoloument(Convert.ToInt32(record["id"]), Convert.ToInt32(record["id_order"]), Convert.ToInt32(record["id_courier"]), Convert.ToDateTime(record_order["devilery_data"]), record["role"].ToString(), record_order["adress"].ToString(), fio));
                }
            }
            sql.Close();

        }
        public void Add_empoloument(string id, string courier, string role)
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            SQLiteCommand sc = new SQLiteCommand("insert into employment (id_order, id_courier, role) values (" + id + ", " + courier + ", '" + role + "')", sql);
            sql.Open();
            sc.ExecuteNonQuery();
            sql.Close();
            loading_empoloument();

        }
        public void delete_empoloyment(DateTime data)
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            sql.Open();
            SQLiteCommand sc = new SQLiteCommand("select id from ORDER_HOLYDAY where devilery_data LIKE '%" + data.Date.ToString() + "%' and order_status LIKE '%выполнено%'", sql);
            SQLiteDataReader reader = sc.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                sc = new SQLiteCommand("delete from empoloyment where id_order = " + record["id"], sql); sc.ExecuteNonQuery();
            }
            sql.Close();
        }
        public SQLiteDataReader Select_empoloyment()
        {
            SQLiteConnection sql = new SQLiteConnection(@"Data Source = mydb.sqlite; Version = 3");
            sql.Open();
            SQLiteCommand com = new SQLiteCommand(sql);
            com.CommandText = @"select * from employment";
            SQLiteDataReader sdr = com.ExecuteReader();
            return sdr;
        }
    }

}  
    

