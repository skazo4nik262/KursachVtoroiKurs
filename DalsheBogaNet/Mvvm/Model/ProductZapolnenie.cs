using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySqlConnector;

namespace DalsheBogaNet.Mvvm.Model
{
    public class ProductZapolnenie
    {
        private ProductZapolnenie()
        {

        }

        static ProductZapolnenie instance;
        public static ProductZapolnenie Instance
        {
            get
            {
                if (instance == null)
                    instance = new ProductZapolnenie();
                return instance;
            }
        }

        internal IEnumerable<Code> GetAllCode(string sql)
        {
            var result = new List<Code>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                while (reader.Read())
                {
                    Code code = new Code();
                    result.Add(code);
                    code.Codee = reader.GetString("Code");
                    code.Time = reader.GetDateTime("Time");
                }
            }
            return result;
        }
        
        internal IEnumerable<Zakaz> GetAllZakaz(string sql)
        {
            var result = new List<Zakaz>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                Zakaz zakaz = new Zakaz();
                int id;
                while (reader.Read())
                {
                    id = reader.GetInt32("Tovar_ID");
                    if (zakaz.ID != id)
                    {
                        zakaz = new Zakaz();
                        result.Add(zakaz);
                        zakaz.ID = id;
                        zakaz.Name = reader.GetString("Name");
                        zakaz.Postavhik = reader.GetInt32("Postavhik");
                        zakaz.Amount = reader.GetInt32("Amount");
                    }
                }
            }
            return result;
        }

        internal IEnumerable<Product> GetAllProducts(string sql)
        {
            var result = new List<Product>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                Product product = new Product();
                int id;
                while (reader.Read())
                {
                    id = reader.GetInt32("Tovar_ID");
                    if (product.ID != id)
                    {
                        product = new Product();
                        result.Add(product);
                        product.ID = id;
                        product.Name = reader.GetString("Name");
                        product.Description = reader.GetString("Description");
                        product.Price = reader.GetDecimal("Price");
                        product.Size = reader.GetDouble("Size");
                        product.Breakable = reader.GetBoolean("Breakable");
                        product.Postavhik = reader.GetInt32("Postavhik");
                        product.Amount = reader.GetInt32("Amount");
                        product.Code = reader.GetString("Code");
                        product.Time = reader.GetDateTime("Time");
                    }

                }
            }
            return result;
        }
        internal void AddCode(Code code)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;


            string sql = "INSERT INTO Codes VALUES (@Name, @Time)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("Name", code.Codee));
                mc.Parameters.Add(new MySqlParameter("Time", code.Time));
                mc.ExecuteNonQuery();
                Thread.Sleep(2000);
            }
        }
        internal void AddProduct(Product product)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;

                int id = MySqlDB.Instance.GetAutoID("izdelia");

                string sql = "INSERT INTO izdelia VALUES (0, @Name, @Description, @Price, @Size, @Breakable, @Postavhik, @Amount, @Code)";
                using (var mc = new MySqlCommand(sql, connect))
                {
                    mc.Parameters.Add(new MySqlParameter("Name", product.Name));
                    mc.Parameters.Add(new MySqlParameter("Description", product.Description));
                    mc.Parameters.Add(new MySqlParameter("Price", product.Price));
                    mc.Parameters.Add(new MySqlParameter("Size", product.Size));
                    mc.Parameters.Add(new MySqlParameter("Breakable", product.Breakable));
                    mc.Parameters.Add(new MySqlParameter("Postavhik", product.Postavhik));
                    mc.Parameters.Add(new MySqlParameter("Amount", product.Amount));
                    mc.Parameters.Add(new MySqlParameter("Code", product.Code));
                    //mc.Parameters.Add(new MySqlParameter("Time", product.Time));
                    mc.ExecuteNonQuery();
                }
                string sql2 = "INSERT INTO izdelia_info VALUES (0, @Name, @Price, @Postavhik, @Amount, @Code)";
                using (var mc2 =  new MySqlCommand(sql2, connect))
                {
                    mc2.Parameters.Add(new MySqlParameter("Name", product.Name));
                    mc2.Parameters.Add(new MySqlParameter("Price", product.Price));
                    mc2.Parameters.Add(new MySqlParameter("Postavhik", product.Postavhik));
                    mc2.Parameters.Add(new MySqlParameter("Amount", product.Amount));
                    mc2.Parameters.Add(new MySqlParameter("Code", product.Code));
                    //mc2.Parameters.Add(new MySqlParameter("Time", product.Time));
                    mc2.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Column 'Description' cannot be null")
                    MessageBox.Show("Column 'Description' не заполнено");
                else if (ex.Message == "Column 'Name' cannot be null")
                    MessageBox.Show("Поле 'Name' не заполнено");
                else if (ex.Message == "Cannot add or update a child row: a foreign key constraint fails (`autoIzdelia`.`izdelia`, CONSTRAINT `FK_izdelia_postavhik_Postavhik_ID` FOREIGN KEY (`Postavhik`) REFERENCES `postavhik` (`Postavhik_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION)")
                    MessageBox.Show("Введеный поставщик не существует");
                else
                    MessageBox.Show("Что-то пошло не так" +$"\n Код ошибки: \n {ex.Message}");
            }
        }
        internal void Remove(Product product)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;
            string sql = "DELETE FROM izdelia WHERE Tovar_ID = '" + product.ID + "';";
            //sql += "DELETE FROM izdelia WHERE id = '" + product.ID + "';";

            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();

        }
        internal void RemoveCodes()
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;
            string sql = "DELETE FROM codes";
            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
        }
        internal void UpdateProduct(Product product)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;



                var date = DateTime.Now;
                string sql2 = "UPDATE izdelia_info SET Name = @Name, Price = @Price, Postavhik = @Postavhik, Amount = @Amount, Code = @Code WHERE Tovar_ID = "+ product.ID;
                using (var mc2 = new MySqlCommand(sql2, connect))
                {
                    mc2.Parameters.Add(new MySqlParameter("Name", product.Name));
                    mc2.Parameters.Add(new MySqlParameter("Price", product.Price));
                    mc2.Parameters.Add(new MySqlParameter("Postavhik", product.Postavhik));
                    mc2.Parameters.Add(new MySqlParameter("Amount", product.Amount));
                    mc2.Parameters.Add(new MySqlParameter("Code", product.Code));
                    //mc2.Parameters.Add(new MySqlParameter("Time", date));
                }
                string sql = "UPDATE izdelia SET Name = @Name, Description = @Description, Price = @Price, Size = @Size, Breakable = @Breakable, Postavhik = @Postavhik, Amount = @Amount, Code = @Code WHERE Tovar_ID = " + product.ID;
                using (var mc = new MySqlCommand(sql, connect))
                {
                    mc.Parameters.Add(new MySqlParameter("Name", product.Name));
                    mc.Parameters.Add(new MySqlParameter("Description", product.Description));
                    mc.Parameters.Add(new MySqlParameter("Price", product.Price));
                    mc.Parameters.Add(new MySqlParameter("Size", product.Size));
                    mc.Parameters.Add(new MySqlParameter("Breakable", product.Breakable));
                    mc.Parameters.Add(new MySqlParameter("Postavhik", product.Postavhik));
                    mc.Parameters.Add(new MySqlParameter("Amount", product.Amount));
                    mc.Parameters.Add(new MySqlParameter("Code", product.Code));
                    //mc.Parameters.Add(new MySqlParameter("Time", date));
                    mc.ExecuteNonQuery();
                }
                string sql3 = "UPDATE Codes SET Time = @Time";
                using (var mc3 = new MySqlCommand( sql3, connect))
                {
                    mc3.Parameters.Add(new MySqlParameter("Time", date));
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Column 'Description' cannot be null")
                    MessageBox.Show("Column 'Description' не заполнено");
                else if (ex.Message == "Column 'Name' cannot be null")
                    MessageBox.Show("Поле 'Name' не заполнено");
                else if (ex.Message == "Cannot add or update a child row: a foreign key constraint fails (`autoIzdelia`.`izdelia`, CONSTRAINT `FK_izdelia_postavhik_Postavhik_ID` FOREIGN KEY (`Postavhik`) REFERENCES `postavhik` (`Postavhik_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION)")
                    MessageBox.Show("Введеный поставщик не существует");
                else
                    MessageBox.Show("Что-то пошло не так" + $"\n Код ошибки: \n {ex.Message}");
            }
        }
        internal IEnumerable<Product> Search(string searchText)
        {
            string sql = "SELECT Tovar_ID, Name, Description, Price, Amount, Size, Breakable, Postavhik FROM izdelia WHERE";
            sql += " Name LIKE '%" + searchText + "%'";
            sql += " OR Description LIKE '%" + searchText + "%'";

            return GetAllProducts(sql);
        }

    }
}
