using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using KP_Gamenotebook.Models;

namespace KP_Gamenotebook
{
   
    class Word
    {
        public word.Application wordapp;
        public word.Documents worddocuments;
        public word.Document worddocument;
        string outputPath = @"C:\Users\ReaLBERG\Desktop\3 курс\АИС\LB9\test" + Path.GetRandomFileName() + ".doc";
        public Word(string template = @"C:\Users\ReaLBERG\Desktop\3 курс\АИС\LB9\lb9tttt.doc")
        {
            wordapp = new word.Application();
            wordapp.Visible = true;
            Object newTemplate = false;
            Object documentType = word.WdNewDocumentType.wdNewBlankDocument;
            Object visible = true;
            wordapp.Documents.Add(template, newTemplate, ref documentType, ref visible);
            worddocuments = wordapp.Documents;
            worddocument = worddocuments.get_Item(1);
            worddocument.Activate();
        }
        public void Replace(string wordr, string replacement)
        {
            word.Range range = worddocument.StoryRanges[word.WdStoryType.wdMainTextStory];
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: wordr, ReplaceWith: replacement);
            TrySave();

        }
        public void ReplaceBookMark(string bookmark, string replacement)
        {
            worddocument.Bookmarks[bookmark].Range.Text = replacement;
            TrySave();

        }
        public void TrySave()
        {
            try
            {
                worddocument.SaveAs(outputPath, word.WdSaveFormat.wdFormatDocument);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Close()
        {
         
            wordapp.Quit();
        }
        
        public void TableCreate(int row, int col, string[] startt, Model[] gameNotebooks)
        {

            Object start = 70;
            Object end = 70;
            word.Range wordrange = worddocument.Range(ref start, ref end);
       
            Object defaultTableBehavior = word.WdDefaultTableBehavior.wdWord9TableBehavior;
            Object autoFitBehavior = word.WdAutoFitBehavior.wdAutoFitWindow;
            word.Table wordtable = worddocument.Tables.Add(wordrange, row, col, ref defaultTableBehavior, ref autoFitBehavior);
            for (int i = 1; i <= col; i++)
            {
                word.Range wordcellrange = worddocument.Tables[1].Cell(1, i).Range;
                int j = i - 1;
                wordcellrange.Text = startt[j];
            }
            
            int k = 0;
            for (int i = 2; i <= row; i++)
            {
                word.Range wordcellrange = worddocument.Tables[1].Cell(i, 1).Range;
                wordcellrange.Text = Convert.ToString(gameNotebooks[k].ID_model);
                wordcellrange = worddocument.Tables[1].Cell(i, 2).Range;
                wordcellrange.Text = gameNotebooks[k].Name;
                wordcellrange = worddocument.Tables[1].Cell(i, 3).Range;
                wordcellrange.Text = gameNotebooks[k].Price;
                k++;
            }
            wordrange.InlineShapes.AddPicture(@"C:\\Users\\ReaLBERG\\Desktop\\3 курс\\АИС\\Graf.bmp");
        }
        public void TableCreateReview(int row, int col, string[] startt, Reviews[] gameNotebooks)
        {

            Object start = 104;
            Object end = 104;
            word.Range wordrange = worddocument.Range(ref start, ref end);

            Object defaultTableBehavior = word.WdDefaultTableBehavior.wdWord9TableBehavior;
            Object autoFitBehavior = word.WdAutoFitBehavior.wdAutoFitWindow;
            word.Table wordtable = worddocument.Tables.Add(wordrange, row, col, ref defaultTableBehavior, ref autoFitBehavior);
            for (int i = 1; i <= col; i++)
            {
                word.Range wordcellrange = worddocument.Tables[1].Cell(1, i).Range;
                int j = i - 1;
                wordcellrange.Text = startt[j];
            }

            int k = 0;
            for (int i = 2; i <= row; i++)
            {
                word.Range wordcellrange = worddocument.Tables[1].Cell(i, 1).Range;
                wordcellrange.Text = Convert.ToString(gameNotebooks[k].ID_review);
                wordcellrange = worddocument.Tables[1].Cell(i, 2).Range;
                wordcellrange.Text = gameNotebooks[k].Rating;
                wordcellrange = worddocument.Tables[1].Cell(i, 3).Range;
                wordcellrange.Text = gameNotebooks[k].Review_text;

                k++;
            }
            
        }
    }
}
