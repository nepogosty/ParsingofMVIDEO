using KP_Gamenotebook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace KP_Gamenotebook
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            WriteALL();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string page = Convert.ToString(Convert.ToInt32(numericUpDown1.Value) * 12);
            FactorialAsync(page);
        }
        static async void FactorialAsync(string page)
        {
            var parser = new Parser();

            string mainPage = "https://www.mvideo.ru/product-list-page-cls?limit=64&region_id=1&q=%D0%B8%D0%B3%D1%80%D0%BE%D0%B2%D1%8B%D0%B5%20%D0%BD%D0%BE%D1%83%D1%82%D0%B1%D1%83%D0%BA%D0%B8&offset=" + page;
            List<string> books = await parser.GetLinks(mainPage);
            string rate;
            string href;
            foreach (string bookUrl in books)
            {
                href = bookUrl.Remove(bookUrl.Length - 3, 3);
                rate = bookUrl.Remove(0, bookUrl.Length - 3);
                parser.Parse(href, rate);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WriteALL();
        }
        private void WriteALL()
        {
            listView1.Items.Clear();
            using (KPgamenotebookContext db = new KPgamenotebookContext())
            {
                comboBox5.Items.Clear();
                comboBox8.Items.Clear();
                comboBox9.Items.Clear();
                listView1.Items.Clear();
                comboBox2.Items.Clear();
                comboBox3.Items.Clear();
                comboBox4.Items.Clear();
                textBox13.Clear();
                comboBox2.SelectedItem = null;
                comboBox3.SelectedItem = null;
                comboBox4.SelectedItem = null;
                comboBox5.SelectedItem = null;
                comboBox8.SelectedItem = null;
                comboBox9.SelectedItem = null;
                textBox12.Clear();
                textBox14.Clear();
                var obj = db.Model;
                var obj2 = db.CPU;
                var obj3 = db.Graphiccard;
                var obj4 = db.Firm;

                foreach (Model u in obj)
                {
                    var item1 = new ListViewItem(new[] { Convert.ToString(u.ID_model), u.Name, u.Price, u.Rating, u.SSD, Convert.ToString(u.RAM), db.Firm.FirstOrDefault(q => q.ID_firm == u.ID_firm).Name_firm, db.CPU.FirstOrDefault(q => q.ID_CPU == u.ID_CPU).Name, db.CPU.FirstOrDefault(q => q.ID_CPU == u.ID_CPU).Frequency, db.Graphiccard.FirstOrDefault(q => q.ID_GC == u.ID_GC).Name });
                    listView1.Items.Add(item1);
             


                }
                foreach (CPU u in obj2)
                {
                    comboBox2.Items.Add(u.Name);
                    comboBox1.Items.Add(u.Name);
                    comboBox9.Items.Add(u.Name);
                }
                foreach (Graphiccard u in obj3)
                {
                    comboBox3.Items.Add(u.Name);
                    comboBox6.Items.Add(u.Name);
                    comboBox8.Items.Add(u.Name);
                }
                foreach (Firm u in obj4)
                {
                    comboBox4.Items.Add(u.Name_firm);
                    comboBox7.Items.Add(u.Name_firm);
                    comboBox5.Items.Add(u.Name_firm);
                }




            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView2.Clear();
            try
            {
                listView2.Columns.Add("ID");
                listView2.Columns.Add("Рейтинг");
                var selected = listView1.SelectedItems[0].Text;
                int selected1 = Convert.ToInt32(selected);
                using (KPgamenotebookContext db = new KPgamenotebookContext())
                {
                    var obj = db.Reviews;

                    foreach (Reviews u in obj)
                    {
                        if (u.ID_model == selected1)
                        {
                            var item2 = new ListViewItem(new[] { Convert.ToString(u.ID_review), u.Rating });
                            listView2.Items.Add(item2);

                        }
                    }
                    var obj2 = db.Model;
                    foreach (Model t in obj2)
                    {
                        if (t.ID_model == selected1)
                        {
                            textBox2.Text = Convert.ToString(t.ID_model);
                            textBox3.Text = t.Name;
                            textBox4.Text = t.Price;
                            textBox5.Text = t.Rating;
                            textBox6.Text = t.SSD;
                            textBox7.Text = Convert.ToString(t.RAM);
                            comboBox1.SelectedItem = db.CPU.FirstOrDefault(q => q.ID_CPU == t.ID_CPU).Name;

                            comboBox6.SelectedItem = db.Graphiccard.FirstOrDefault(q => q.ID_GC == t.ID_GC).Name;
                            comboBox7.SelectedItem = db.Firm.FirstOrDefault(q => q.ID_firm == t.ID_firm).Name_firm;
                            textBox13.Text = Convert.ToString(t.ID_model);
                            textBox12.Text = t.Name;

                        }
                    }
                    int kolvo = 0;
                    foreach(Reviews t in db.Reviews)
                    {
                        if (Convert.ToInt32(textBox13.Text) == t.ID_model)
                        {
                            kolvo++;
                        }

                    }
                    textBox14.Text = Convert.ToString(kolvo);
                    if (Convert.ToInt32(textBox14.Text) == 0)
                    {
                        button8.Visible = false;
                        label34.Visible = true;
                    }
                    if (Convert.ToInt32(textBox14.Text) != 0)
                    {
                        button8.Visible = true;
                        label34.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            try
            {

                var selected = listView2.SelectedItems[0].Text;
                int selected1 = Convert.ToInt32(selected);
                using (KPgamenotebookContext db = new KPgamenotebookContext())
                {
                    var obj = db.Reviews;
                    foreach (Reviews u in obj)
                    {
                        if (u.ID_review == selected1)
                        {
                            textBox1.Text = u.Review_text;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Delete();
        }
        private void Delete()
        {
            using (KPgamenotebookContext db = new KPgamenotebookContext())
            {
                var obj = db.Reviews;
                foreach (Reviews u in obj)
                {
                    if (u.ID_model == Convert.ToInt32(textBox2.Text))
                    {
                        db.Reviews.Remove(u);

                    }
                }
                db.SaveChanges();
                var obj2 = db.Model;
                foreach (Model u in obj2)
                {
                    if (u.ID_model == Convert.ToInt32(textBox2.Text))
                    {
                        db.Model.Remove(u);

                    }
                }
                db.SaveChanges();
                ListView.SelectedIndexCollection collection = listView1.SelectedIndices;
                if (collection.Count != 0)
                    listView1.Items.RemoveAt(collection[0]);
                WriteALL();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Redact();

        }
        private void Redact()
        {
            using (KPgamenotebookContext db = new KPgamenotebookContext())
            {
                try
                {
                    string[] vs = new string[10];

                    vs[0] = textBox2.Text;
                    int ID = Convert.ToInt32(vs[0]);
                    vs[1] = textBox3.Text;
                    vs[2] = textBox4.Text;
                    vs[3] = textBox5.Text;
                    vs[4] = textBox6.Text;
                    vs[5] = textBox7.Text;
                    string stsix = Convert.ToString(comboBox1.SelectedItem);
                    int six = db.CPU.FirstOrDefault(r => r.Name == stsix).ID_CPU;
                    string stsev = Convert.ToString(comboBox6.SelectedItem);
                    int seven = db.Graphiccard.FirstOrDefault(r => r.Name == stsev).ID_GC;
                    string steig = Convert.ToString(comboBox7.SelectedItem);
                    int eight = db.Firm.FirstOrDefault(r => r.Name_firm == steig).ID_firm;
                    vs[9] = db.Model.FirstOrDefault(r => r.ID_model == ID).Href;
                    foreach (Reviews u in db.Reviews)
                    {
                        if (u.ID_model == ID)
                        {
                            u.ID_model = null;
                        }
                    }
                    db.SaveChanges();
                    var obj2 = db.Model;
                    foreach (Model u in obj2)
                    {
                        if (u.ID_model == Convert.ToInt32(textBox2.Text))
                        {
                            db.Model.Remove(u);

                        }
                    }
                    db.SaveChanges();
                    Model model = new Model();
                    model.ID_model = ID;
                    model.Href = vs[9];
                    model.Name = vs[1];
                    model.Price = vs[2];
                    model.Rating = vs[3];
                    model.SSD = vs[4];


                    model.RAM = Convert.ToInt32(vs[5]);


                    model.ID_CPU = six;
                    model.ID_GC = seven;
                    model.ID_firm = eight;
                    db.Model.Add(model);
                    db.SaveChanges();
                    foreach (Reviews u in db.Reviews)
                    {
                        if (u.ID_model == null)
                        {
                            u.ID_model = ID;

                        }
                    }
                    db.SaveChanges();
                    WriteALL();
                }
                catch
                {
                    MessageBox.Show("Пожалуйста, перепроверьте данные");


                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    textBox7.Clear();


                }



                //Model model = db.Model.FirstOrDefault(r => r.ID_model ==ou);
                //model.Name = textBox3.Text;
                //model.Price = textBox4.Text;
                //model.Rating = textBox5.Text;
                //model.SSD = textBox6.Text;
                //model.RAM = Convert.ToInt32(textBox7.Text);
                db.SaveChanges();
            }
        }




        private void button7_Click(object sender, EventArgs e)
        {
            using (KPgamenotebookContext db = new KPgamenotebookContext())
            {
                string c = Convert.ToString(comboBox2.SelectedItem);
                string d = Convert.ToString(comboBox3.SelectedItem);
                string p = Convert.ToString(comboBox4.SelectedItem);
                Model model = new Model();
                int id;

                using (StreamReader sr = new StreamReader(@"C:\Users\ReaLBERG\Desktop\3 курс\АИС\createnote.txt"))
                {
                    id = Convert.ToInt32(sr.ReadToEnd());
                }
                id++;
                using (StreamWriter sw = new StreamWriter(@"C:\Users\ReaLBERG\Desktop\3 курс\АИС\createnote.txt", false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(id);
                }
                model.ID_model = id;
                model.Href = "https://www.mvideo.ru/product-list-page-cls?q=%D0%B8%D0%B3%D1%80%D0%BE%D0%B2%D1%8B%D0%B5+%D0%BD%D0%BE%D1%83%D1%82%D0%B1%D1%83%D0%BA%D0%B8&region_id=2&limit=12";
                model.Name = textBox21.Text;
                model.Price = textBox20.Text;
                model.Rating = textBox19.Text;
                model.SSD = textBox18.Text;
                model.RAM = Convert.ToInt32(textBox17.Text);
                model.ID_CPU = db.CPU.FirstOrDefault(r => r.Name == c).ID_CPU;
                model.ID_GC = db.Graphiccard.FirstOrDefault(r => r.Name == d).ID_GC;
                model.ID_firm = db.Firm.FirstOrDefault(r => r.Name_firm == p).ID_firm;
                db.Model.Add(model);
                db.SaveChanges();
                WriteALL();

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            using (KPgamenotebookContext db = new KPgamenotebookContext())
            {
                string a = Convert.ToString(comboBox5.SelectedItem);
                int id = db.Firm.FirstOrDefault(r => r.Name_firm == a).ID_firm;
                foreach (Model u in db.Model)
                {
                    if (u.ID_firm == id)
                    {
                        u.ID_firm = null;
                    }
                }
                db.SaveChanges();
                foreach (Firm u in db.Firm)
                {
                    if (u.ID_firm == id)
                    {
                        db.Firm.Remove(u);

                    }
                }
                db.SaveChanges();
                Firm header = new Firm();
                header.ID_firm = id;
                header.Name_firm = textBox10.Text;
                string name = textBox10.Text;
                db.Firm.Add(header);
                db.SaveChanges();
                int id2 = db.Firm.FirstOrDefault(r => r.Name_firm == name).ID_firm;
                foreach (Model u in db.Model)
                {
                    if (u.ID_firm == null)
                    {
                        u.ID_firm = id2;

                    }
                }
                db.SaveChanges();
                WriteALL();
            }
        }



        private void button12_Click(object sender, EventArgs e)
        {
            using (KPgamenotebookContext db = new KPgamenotebookContext())
            {

                string a = Convert.ToString(comboBox8.SelectedItem);
                int id = db.Graphiccard.FirstOrDefault(r => r.Name == a).ID_GC;
                foreach (Model u in db.Model)
                {
                    if (u.ID_GC == id)
                    {
                        u.ID_GC = null;
                    }
                }
                db.SaveChanges();
                foreach (Graphiccard u in db.Graphiccard)
                {
                    if (u.ID_GC == id)
                    {
                        db.Graphiccard.Remove(u);

                    }
                }
                db.SaveChanges();
                Graphiccard header = new Graphiccard();
                header.ID_GC = id;
                header.Name = textBox8.Text;
                string name = textBox8.Text;
                db.Graphiccard.Add(header);
                db.SaveChanges();
                int id2 = db.Graphiccard.FirstOrDefault(r => r.Name == name).ID_GC;
                foreach (Model u in db.Model)
                {
                    if (u.ID_GC == null)
                    {
                        u.ID_GC = id2;

                    }
                }
                db.SaveChanges();
                WriteALL();
            }
        }




        private void button13_Click(object sender, EventArgs e)
        {
            using (KPgamenotebookContext db = new KPgamenotebookContext())
            {

                string a = Convert.ToString(comboBox9.SelectedItem);
                int id = db.CPU.FirstOrDefault(r => r.Name == a).ID_CPU;
                foreach (Model u in db.Model)
                {
                    if (u.ID_CPU == id)
                    {
                        u.ID_CPU = null;
                    }
                }
                db.SaveChanges();
                foreach (CPU u in db.CPU)
                {
                    if (u.ID_CPU == id)
                    {
                        db.CPU.Remove(u);

                    }
                }
                db.SaveChanges();
                CPU header = new CPU();
                header.ID_CPU = id;
                header.Name = textBox9.Text;
                string name = textBox9.Text;
                header.Frequency = textBox11.Text;
                db.CPU.Add(header);
                db.SaveChanges();
                int id2 = db.CPU.FirstOrDefault(r => r.Name == name).ID_CPU;
                foreach (Model u in db.Model)
                {
                    if (u.ID_CPU == null)
                    {
                        u.ID_CPU = id2;

                    }
                }
                db.SaveChanges();
                WriteALL();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (KPgamenotebookContext db = new KPgamenotebookContext())
            {
                try
                {
                    int id = Convert.ToInt32(textBox2.Text);
                    string url = db.Model.FirstOrDefault(r => r.ID_model == id).Href;
                    const string browserPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                    Process.Start(browserPath, url);
                }
                catch { MessageBox.Show("У данного ноутбука нет ссылки"); }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
                string outputPath1 = @"C:\Users\ReaLBERG\Desktop\3 курс\АИС\Отчеты\Ценабонусы" + Path.GetRandomFileName() + ".xlsx";
                KPgamenotebookContext db2 = new KPgamenotebookContext();
                int num2 = db2.Model.Count();
                var models = db2.Model;
                Model[] gameNotebooks2 = new Model[num2];
                int p = 1;
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workBook;
                Excel.Worksheet workSheet;
                workBook = excelApp.Workbooks.Add();
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);

                int[] vs = new int[num2];
                int[] vs2 = new int[num2];
                int k = 0;
                foreach (var t in models)
                {
                    int value;
                    int value1;

                    int.TryParse(string.Join("", t.Price.Where(c => char.IsDigit(c))), out value);
                    int.TryParse(string.Join("", t.Bonuses.Where(c => char.IsDigit(c))), out value1);
                    vs[k] = value;
                    vs2[k] = value1;
                    k++;
                }
                int temp;
                for (int i = 0; i < vs.Length - 1; i++)
                {
                    for (int j = i + 1; j < vs.Length; j++)
                    {
                        if (vs[i] > vs[j])
                        {
                            temp = vs[i];
                            vs[i] = vs[j];
                            vs[j] = temp;
                            temp = vs2[i];
                            vs2[i] = vs2[j];
                            vs2[j] = temp;
                        }
                    }
                }
                for (p = 1; p < 24; p++)
                {
                    workSheet.Cells[1, p] = (vs[p - 1]);
                    workSheet.Cells[2, p] = (vs2[p - 1]);
                }
                Excel.ChartObjects chartObjs = (Excel.ChartObjects)workSheet.ChartObjects();
                Excel.ChartObject chartObj = chartObjs.Add(10, 50, 500, 500);
                Excel.Chart xlChart = chartObj.Chart;
                xlChart.ChartType = Excel.XlChartType.xlAreaStacked;
                Excel.SeriesCollection seriesCollection = (Excel.SeriesCollection)xlChart.SeriesCollection(Type.Missing);
                Excel.Series series = seriesCollection.NewSeries();
                series.XValues = workSheet.get_Range("A1", "Q1");
                series.Values = workSheet.get_Range("A2", "Q2");
                xlChart.HasTitle = true;
                xlChart.ChartTitle.Text = "Зависимость бонусов от цены";
                xlChart.HasLegend = false;
                excelApp.Visible = true;
                excelApp.UserControl = true;
                workSheet.SaveAs(outputPath1);
                object misValue = System.Reflection.Missing.Value;
                xlChart.Export("C:\\Users\\ReaLBERG\\Desktop\\3 курс\\АИС\\Graf.bmp", "BMP", misValue);
                excelApp.Quit();


                Word cOMFormatter = new Word(@"C:\Users\ReaLBERG\Desktop\3 курс\АИС\Отчеты\lb9tt.doc");
                KPgamenotebookContext db = new KPgamenotebookContext();
                Console.WriteLine("Пожалуйста, введите имя: ");
                string name = Console.ReadLine();
                cOMFormatter.Replace("{Имя}", name);
                int num = db.Model.Count();
                string[] start = new string[3] { "ID", "Название", "Цена" };
                var gamenotebook = db.Model;
                Model[] gameNotebooks = new Model[num];
                int y = 0;
                foreach (var t in gamenotebook)
                {

                    gameNotebooks[y] = t;

                    y++;

                }
                cOMFormatter.TableCreate(num + 1, 3, start, gameNotebooks);

                cOMFormatter.Close();

            }
        
        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox13.Clear();
            textBox12.Clear(); 
            textBox14.Clear();
           
            if (Convert.ToString(comboBox11.SelectedItem) == "Отзывы определенного ноутбука")
            {
                textBox12.Visible = true;
                textBox13.Visible = true;
                textBox14.Visible = true;
                label32.Visible = true;
                label33.Visible = true;
                button6.Visible = false;
             

            }
            if (Convert.ToString(comboBox11.SelectedItem) != "Отзывы определенного ноутбука")
            {
                textBox13.Visible = false;
                textBox12.Visible = false; 
                textBox14.Visible = false;
                label32.Visible = false;
                label33.Visible = false;
                button6.Visible = false;
              
            }
            if (Convert.ToString(comboBox11.SelectedItem) == "ID/ноутбуки")
            {
                button6.Visible = true;
                button8.Visible = false;
                label34.Visible = false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            using (KPgamenotebookContext db = new KPgamenotebookContext())
            {



                Word cOMFormatter = new Word(@"C:\Users\ReaLBERG\Desktop\3 курс\АИС\Отчеты\lb9ttотзывы.doc");

               
                int num = Convert.ToInt32(textBox14.Text);
                string[] start = new string[3] { "ID","Оценка", "Отзыв" };
                var review = db.Reviews;
                Reviews[] reviews = new Reviews[num];
                int ID = Convert.ToInt32(textBox13.Text);
                int y = 0;
                foreach (var t in review)
                {
                    if (t.ID_model == ID)
                    {
                        reviews[y] = t;

                        y++;
                    }
                }
                cOMFormatter.TableCreateReview(num + 1, start.Length, start, reviews);
                cOMFormatter.Replace("{Имя}", textBox12.Text);
            }
        }
    }
}
