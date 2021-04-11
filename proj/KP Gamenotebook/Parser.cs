using AngleSharp;
using AngleSharp.Dom;
using KP_Gamenotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace KP_Gamenotebook
{
    class Parser
    {
        
        public async void Parse(string url1, string rateing)
        {
            //Объект класса
            using (KPgamenotebookContext db = new KPgamenotebookContext())
            {
                Model gameNotebook = new Model();
                Firm firm = new Firm();
                Graphiccard graphiccard = new Graphiccard();
                CPU cPU = new CPU();
               
                //Подключение к HTML-страничке
                IConfiguration config = Configuration.Default.WithDefaultLoader();
                IBrowsingContext context = BrowsingContext.New(config);
                IDocument doc1 = await context.OpenAsync(url1);

                int watch = Convert.ToInt32(url1.Substring(url1.Length - 8));
                var obj = db.Model;
                bool twice = false;

                foreach (Model u in obj)
                {
                    if (watch == u.ID_model)
                    {
                        twice = true;
                    }
                }
                if (twice == false)
                {
                    //Характеристики
                    IEnumerable<IElement> shortDescriptionParams = doc1.All.Where(block =>
                     block.LocalName == "span"
                     && block.ClassList.Contains("c-specification__value")
                     && block.ParentElement.ParentElement.ParentElement.ParentElement.LocalName == "table"
                     && block.ParentElement.ParentElement.ParentElement.ParentElement.ClassList.Contains("c-specification__table"));
                    //

                    //Вспомогательная переменная
                    string text;
                    //Имя
                    IElement name = doc1.QuerySelector("h1.fl-h1");
                    text = name.TextContent;
                    //IElement rating = rate.ToList()[0];

                    string textttt = text;
                    // Добавление фирмы
                    string[] words = text.Split(new char[] { ' ' });
                    var obj2 = db.Firm;

                    bool third = false;
                    foreach (Firm t in obj2)
                    {
                        if (words[2] == t.Name_firm)
                        {
                            gameNotebook.ID_firm = t.ID_firm;
                            third = true;
                            break;
                        }

                    }
                    if (third == false)
                    {
                        firm.Name_firm = words[2];
                        db.Firm.Add(firm);
                        db.SaveChanges();
                        gameNotebook.ID_firm = firm.ID_firm;
                    }
                    ////CPU
                    IElement cpu = shortDescriptionParams.ToList()[1];
                    text = cpu.TextContent;
                    text = DeleteNT(text);
                    string[] words1 = text.Split(new char[] { ' ' });
                    string namecpu = words1[0] + " " + words1[1] + " " + words1[2] + " " + words1[3];
                    string freauq = words1[4] + " " + words1[5];
                    var obj4 = db.CPU;

                    bool fifth = false;
                    foreach (CPU t in obj4)
                    {
                        if (namecpu == t.Name)
                        {
                            gameNotebook.ID_CPU = t.ID_CPU;
                            fifth = true;
                        }

                    }
                    if (fifth == false)
                    {
                        //cPU.ID_CPU = count;
                        //count++;
                        cPU.Name = namecpu;
                        cPU.Frequency = freauq;
                        db.CPU.Add(cPU);
                        db.SaveChanges();
                        gameNotebook.ID_CPU = cPU.ID_CPU;
                    }

                    var obj3 = db.Graphiccard;

                    bool fourth = false;
                    IElement graphiccard1 = shortDescriptionParams.ToList()[3];
                    text = graphiccard1.TextContent;
                    text = DeleteNT(text);
                    foreach (Graphiccard t in obj3)
                    {
                        if (text == t.Name)
                        {
                            gameNotebook.ID_GC = t.ID_GC;
                            fourth = true;
                        }

                    }
                    if (fourth == false)
                    {
                        graphiccard.Name = text;

                        db.Graphiccard.Add(graphiccard);
                        db.SaveChanges();
                        gameNotebook.ID_GC = graphiccard.ID_GC;
                    }

                    gameNotebook.Href = url1;
                    gameNotebook.ID_model = watch;
                    string text1 = textttt.Remove(0, 23);
                    gameNotebook.Name = DeleteNT(text1);
                    //Оценка
                    gameNotebook.Rating = rateing;
                    //Цена
                    IElement price = doc1.QuerySelector("div.fl-pdp-price__current");
                    string text2 = price.TextContent;
                    gameNotebook.Price = text2.Replace("₽", "RUB");
                    //Бонусы*
                    IElement bonuses = doc1.QuerySelector("span.u-color-red.wrapper-text__rouble");
                    if (bonuses == null)
                    {
                        gameNotebook.Bonuses = null;
                    }
                    else
                    {
                        string text3 = bonuses.TextContent;
                        gameNotebook.Bonuses = text3;
                    }

                    ////Diagonal
                    //IElement diagonal = shortDescriptionParams.ToList()[0];
                    //text = diagonal.TextContent;
                    //gameNotebook. = DeleteNT(text);

                    //Ядра
                    //string url3 = url1 + "/specification";
                    //IDocument doc3 = await context.OpenAsync(url3);
                    //IEnumerable<IElement> elements = doc3.All.Where(block =>
                    //  block.LocalName == "div"
                    // && block.ClassList.Contains("product-details-specification-column")
                    //);

                    //RAM
                    IElement ram = shortDescriptionParams.ToList()[2];
                    text = DeleteNT(text);
                    text = ram.TextContent;
                    string NoGB = DeleteNT(text);
                    gameNotebook.RAM = StringTOint(NoGB);
                    
                    //SSD
                    IElement ssd = shortDescriptionParams.ToList()[4];
                    text = ssd.TextContent;
                    gameNotebook.SSD = DeleteNT(text);
                    
                    //Отзывы
                    string url2 = url1 + "/reviews";
                    IDocument doc2 = await context.OpenAsync(url2);
                    //IElement rate = doc2.QuerySelector("span.text-cutter-wrapper");
                    //Текст
                    IEnumerable<IElement> opinion = doc2.All.Where(block =>
                      block.LocalName == "p"
                      && block.OuterHtml.Contains("<p itemprop=\"description\">") == true
                      && block.Children.All(u => u.LocalName == "span"));
                    //Оценка
                    IEnumerable<IElement> points = doc2.All.Where(block =>
                     block.LocalName == "span"
                     && block.OuterHtml.Contains("<span itemprop=\"ratingValue\">") == true);

                    for (int i = 0; i < opinion.Count(); i++)
                    {
                        Reviews reviews = new Reviews();
                        bool sixth = false;
                        IElement element = opinion.ToList()[i];
                        IElement element1 = points.ToList()[i];
                        var obj5 = db.Reviews;
                        foreach (Reviews re in obj5)
                        {
                            if (element.TextContent == re.Review_text)
                            {
                                sixth = true;
                            }
                        }
                        if (sixth == false)
                        {
                            reviews.Review_text = element.TextContent;
                            reviews.Rating = element1.TextContent;
                            reviews.ID_model = watch;
                            db.Reviews.Add(reviews);
                        }

                        reviews.Review_text = element.TextContent;
                    }

                    db.Model.Add(gameNotebook);
                    db.SaveChanges();
                }
                else
                {
                    db.SaveChanges();
                }

            }


        }
        public string DeleteNT(string text)
        {
            text = text.Replace("\n", "");
            text = text.Replace("\t", "");
            return text;
        }
        public int StringTOint(string text)
        {
            int ind = text.Length - 1;
            text = text.Remove(text.Length - 3, 3);

            return Convert.ToInt32(text);
        }

        public async Task<List<string>> GetLinks(string url)
        { //Подключение HTML главной странички
            Console.WriteLine("Начанием загрузку главной страницы");
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument doc = await context.OpenAsync(url);

            Console.WriteLine("Начинаем считывание главной страницы");
            //Через LINQ запрашиваем блоки с тегом а
            //Которые находятся в div
            //C классом title
            IEnumerable<IElement> aElements = doc.All.Where(block =>
            block.LocalName == "a"
            && block.ParentElement.LocalName == "h4"

            );
            IEnumerable<IElement> rating = doc.QuerySelectorAll("span").Where(block =>
          block.ClassName!=null && block.ClassName.Contains("fl-product-tile-rating__stars-value")
          );
            string[] text=new string[rating.Count()];
            for (int i = 0; i < rating.Count(); i++) {
                IElement rateing = rating.ToList()[i];
                 text[i] = rateing.TextContent;
            }
            int b = 0;
            List<string> output = new List<string>();
            foreach (IElement a in aElements.ToList())
            {
                output.Add($"https://www.mvideo.ru{a.GetAttribute("href")}" + text[b]);
                b++;
            }
            Console.WriteLine("Считывание главной страницы завершено");
            return output;
        }
    }
}