//using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.IO.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using A = DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using System.Data.SqlClient;
using System.Data;
using System.Web;
namespace Facesheet
{
    public class FacesheetClass
    {
        string input1 = "", input2 = "", output = "";
        string[] columns;
        ClsExportNew objExport = null;
        public string FilePath = "";

        public string ProcessTemplateFile(string[] args)
        {
            string htmlData = "";
            string studentName = args[6];
            string modifiedDate = args[7];
            string UpdatedDate = args[8];
            string dateOfBirth = args[9];
            int schoolId = 0, studentId = 0, userId = 0;
            string xmlfilelocation = args[2];
            string[] plcT, TextT, plcC, chkC;
            int x = 0, count = 0, lastCount = 0;
            schoolId = Convert.ToInt32(args[4]);
            studentId = Convert.ToInt32(args[3]);
            userId = Convert.ToInt32(args[5]);
            string temp = xmlfilelocation + "\\Temp";
            //   temp = temp.Replace("FacesheetNE", "");
            string Path = xmlfilelocation + "\\Facesheet NA.docx";
            string NewPath = CopyTemplate(temp, Path, "0");
            System.Threading.Thread.Sleep(3000);
            byte[] imageData = loadImageData(args);

            // string temp_inBase64 = Convert.ToBase64String(imageData);
            //byte[] temp_backToBytes = Encoding.UTF8.GetBytes(temp_inBase64);



            // byte[] imageData = File.ReadAllBytes(@"D:\IMageTest\Test.jpg");
            //var fs = new BinaryWriter(new FileStream(@"C:\\tmp\\TESTimg.JPG", FileMode.Append, FileAccess.Write));
            // fs.Write(imageData);
            // fs.Close();


            ProcessImages(NewPath, imageData);
            

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(NewPath, true))
            {
                if (args[0] == "NE")
                {
                    for (int i = 1; i < 6; i++)
                    {
                        CreateQuery1("NE", xmlfilelocation + "\\FacesheetXML" + i + ".xml", i, out plcT, out  TextT, out plcC, out chkC, true, out lastCount, args);
                         replaceWithTexts(wordDoc.MainDocumentPart, plcT, TextT, i,modifiedDate);
                    }
                }
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;

                string content = null;
                using (StreamReader reader = new StreamReader(
                mainPart.HeaderParts.First().GetStream()))
                {
                    content = reader.ReadToEnd();
                }
                System.Text.RegularExpressions.Regex expression = new System.Text.RegularExpressions.Regex("plcLname");
                content = expression.Replace(content, ""+studentName);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.First().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }

                System.Text.RegularExpressions.Regex expression1 = new System.Text.RegularExpressions.Regex("plcDateofBirth");
                content = expression1.Replace(content, "" + dateOfBirth);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.First().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }
                using (StreamReader reader = new StreamReader(
                mainPart.FooterParts.First().GetStream()))
                {
                    content = reader.ReadToEnd();
                }

                System.Text.RegularExpressions.Regex expression2 = new System.Text.RegularExpressions.Regex("plcUpdated");
                content = expression2.Replace(content, ""+modifiedDate);
                using (StreamWriter writer = new StreamWriter(
                mainPart.FooterParts.First().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }


                using (StreamReader reader = new StreamReader(
                mainPart.HeaderParts.Last().GetStream()))
                {
                    content = reader.ReadToEnd();
                }
                System.Text.RegularExpressions.Regex expression3 = new System.Text.RegularExpressions.Regex("plcLname");
                content = expression3.Replace(content, "" + studentName);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.Last().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }

                System.Text.RegularExpressions.Regex expression4 = new System.Text.RegularExpressions.Regex("plcDateofBirth");
                content = expression4.Replace(content, "" + dateOfBirth);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.Last().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }
                using (StreamReader reader = new StreamReader(
                mainPart.FooterParts.Last().GetStream()))
                {
                    content = reader.ReadToEnd();
                }

                System.Text.RegularExpressions.Regex expression5 = new System.Text.RegularExpressions.Regex("plcUpdated");
                content = expression5.Replace(content, "" + modifiedDate);
                using (StreamWriter writer = new StreamWriter(
                mainPart.FooterParts.Last().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }
                //SdtElement headDOB = mainPart.HeaderParts.First().Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtTestData").SingleOrDefault();
                //if (headDOB != null)
                //{
                //    headDOB.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(studentName))));
                //    headDOB.Remove();
                //}
                mainPart.Document.Save();

            }
            return NewPath;
        }

        public string Main(string[] args)
        {
            string htmlData = "";
            int schoolId = 0, studentId = 0, userId = 0;
            string xmlfilelocation = args[2];
            string[] plcT, TextT, plcC, chkC;
            int x = 0, count = 0, lastCount = 0;
            schoolId = Convert.ToInt32(args[4]);
            studentId = Convert.ToInt32(args[3]);
            userId = Convert.ToInt32(args[5]);
            string dateOfBirth = args[9];
            string modifiedDate = args[7];
            string temp = xmlfilelocation + "\\Temp";
            //   temp = temp.Replace("FacesheetNE", "");
            string Path = xmlfilelocation + "\\Facesheet NA.docx";
            string NewPath = CopyTemplate(temp, Path, "0");
            System.Threading.Thread.Sleep(3000);
            byte[] imageData = loadImageData(args);

            // string temp_inBase64 = Convert.ToBase64String(imageData);
            //byte[] temp_backToBytes = Encoding.UTF8.GetBytes(temp_inBase64);



            // byte[] imageData = File.ReadAllBytes(@"D:\IMageTest\Test.jpg");
            //var fs = new BinaryWriter(new FileStream(@"C:\\tmp\\TESTimg.JPG", FileMode.Append, FileAccess.Write));
            // fs.Write(imageData);
            // fs.Close();


            ProcessImages(NewPath, imageData);

            FilePath = NewPath;
            Guid g = Guid.NewGuid();

            string ids = g.ToString();

            Thread thread = new Thread(new ThreadStart(WorkThreadFunction));

            string hPath = temp + "\\HTML" + ids + ".mht"; //liju
            //PageConvert(NewPath, hPath, WdSaveFormat.wdFormatWebArchive);
            System.Threading.Thread.Sleep(3000);

            getNewFilePath();

            //liju



            thread.Abort();

            string HtmlData = URLTOHTML(hPath);

            if (args[0] == "NE")
            {
                for (int i = 1; i < 6; i++)
                {
                    CreateQuery1("NE", xmlfilelocation + "\\FacesheetXML" + i + ".xml", i, out plcT, out  TextT, out plcC, out chkC, true, out lastCount, args);
                    //HtmlData = replaceWithTextsOLD(HtmlData, plcT, TextT, i);
                }
            }



            return HtmlData;
        }

        private byte[] loadImageData(string[] args)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            int schoolId = Convert.ToInt32(args[4]);
            int studentId = Convert.ToInt32(args[3]);
            SqlConnection sqlConnection = new SqlConnection(args[1].ToString());
            SqlCommand command = new SqlCommand("StudentMoreDetailsNE_Client", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@SchoolId", SqlDbType.Int).Value = schoolId;
            command.Parameters.Add("@StudentId", SqlDbType.Int).Value = studentId;
            command.Parameters.Add("@Type", SqlDbType.VarChar).Value = "SM";
            sqlConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(Dt);
            byte[] temp = null;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        temp = GetBytes(Dr["ImageUrl"].ToString());
                        
                    }
                }
            }
            sqlConnection.Close();
            return temp;
        }

        static byte[] GetBytes(string str)
        {
            //byte[] bytes = new byte[str.Length * sizeof(char)];
            //System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            //liju
            byte[] temp_backToBytes = Convert.FromBase64String(str);
            return temp_backToBytes;
        }

        public string getNewFilePath()
        {
            return FilePath;
        }

        public void WorkThreadFunction()
        {
            try
            {

                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                // log errors
            }
        }

        private void replaceWithTexts(MainDocumentPart mainPart, string[] plcT, string[] TextT, int pageno,string modifiedDate)
        {
            TimeSpan tempDatetime;
            int count = plcT.Count();
            NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);
            for (int i = 0; i < count; i++)
            {


                string textData = "";

                if (TextT[i] != null)
                {
                    if (pageno == 1)
                    {
                        if (i == 0 || i == 1 || i == 2 || i == 4 || i == 13 || i == 14)
                        {
                            TextT[i] = "<b>" + TextT[i] + "</b>";
                        }
                        if (i == 5)
                        {
                            TextT[i] = TextT[i] + " in" + " (" + modifiedDate + ")";
                        }
                        if (i == 6)
                        {

                            TextT[i] = TextT[i] + " lbs" + " (" + modifiedDate + ")";
                        }

                        textData = TextT[i];
                    }
                    else
                        textData = TextT[i];
                }
                else
                {
                    textData =  "";
                }

                var paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT[i]);

                foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
                {
                    var paragraphs = converter.Parse(textData);
                    if (paragraphs.Count == 0)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph tempPara = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        para.Parent.Append(tempPara);
                        para.Remove();
                    }
                    else
                    {
                        for (int k = 0; k < paragraphs.Count; k++)
                        {
                                para.Parent.Append(paragraphs[k]);
                            para.Remove();
                        }
                    }
                    //para.RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Run>();
                    //para.Parent.RemoveAttribute
                }
                paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT[i]);
                foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
                {
                    //para.RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Run>();
                }

            }

        }

        //private string replaceWithTextsOLD(string HtmlData, string[] plcT, string[] TextT, int pageno)
        //{
        //    int count = plcT.Count();

        //    for (int i = 0; i < count; i++)
        //    {
        //        if (TextT[i] != null)
        //        {
        //            if (pageno == 1)
        //            {
        //                if (i == 15)
        //                {
        //                    if (TextT[i] == "1")
        //                    {
        //                        TextT[i] = "Male";
        //                    }
        //                    if (TextT[i] == "2")
        //                    {
        //                        TextT[i] = "Female";
        //                    }
        //                    //  HtmlData = HtmlData.Replace(plcT[i], TextT[i]);
        //                }
        //                HtmlData = HtmlData.Replace(plcT[i], TextT[i]);
        //            }
        //            else
        //                HtmlData = HtmlData.Replace(plcT[i], TextT[i]);
        //        }
        //        else
        //        {
        //            HtmlData = HtmlData.Replace(plcT[i], "");
        //        }
        //    }
        //    return HtmlData;
        //}


        private void CreateQuery1(string StateName, string Path, int PageNo, out string[] plcT, out string[] TextT, out string[] plcC, out string[] chkC, bool check, out int lastValue, string[] args)
        {

            lastValue = 0;
            chkC = new string[0];
            plcC = new string[0];

            TextT = new string[0];
            plcT = new string[0];
      
            string[] textValues;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Path);

            XmlNodeList xmlList = null;
            xmlList = xmlDoc.GetElementsByTagName("State");
            int checkCount = 0;
            foreach (XmlNode st in xmlList)
            {
                if (st.Attributes["Name"].Value == StateName)
                {
                    XmlNodeList xmlListColumns = null;
                    xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                    int chkCount = 0, textCount = 0;

                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                        {
                            chkCount++;
                        }
                        else
                        {
                            textCount++;
                        }
                    }

                    chkC = new string[chkCount];
                    plcC = new string[chkCount];

                    TextT = new string[textCount];
                    plcT = new string[textCount];

                    columns = getColumns(PageNo, textCount, args);
                    int j = 0, k = 0, l = 0;

                    if (check == true)
                    {
                        foreach (XmlNode stMs in xmlListColumns)
                        {

                            TextT[k] = columns[l];
                            plcT[k] = stMs.Attributes["PlaceHolder"].Value.ToString().Trim();
                            TextT[k] = TextT[k].Replace(",", ", ");
                            k++;

                            l++;
                        }
                    }
                    else
                    {

                    }
                    columns = null;
                }
            }

        }
    

        private void ProcessImages(string filename,byte[] imageDataByte)
        {
            var xpic = "";
            var xr = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

            using (WordprocessingDocument document = WordprocessingDocument.Open(filename, true))
            {
                var imageParts =
                    from paragraph in document.MainDocumentPart.Document.Body
                    from graphic in paragraph.Descendants<Graphic>()
                    let graphicData = graphic.Descendants<GraphicData>().FirstOrDefault()
                    let pic = graphicData.ElementAt(0)
                    let nvPicPrt = pic.ElementAt(0).FirstOrDefault()
                    let blip = pic.Descendants<Blip>().FirstOrDefault()
                    select new
                    {
                        Id = blip.GetAttribute("embed", xr).Value,
                        Filename = nvPicPrt.GetAttribute("name", xpic).Value
                    };

                foreach (var image in imageParts)
                {
                        ImagePart imagePart = (ImagePart)document.MainDocumentPart.GetPartById(image.Id);
                        //  byte[] imageBytes = imageDataByte; //imageDataByte;
                        BinaryWriter writer = new BinaryWriter(imagePart.GetStream());
                        writer.Write(imageDataByte);
                        writer.Close();
                        //File.ReadAllBytes("Jellyfish.jpg");
                    }
                }
            }


        //public static void PageConvert(string input, string output, WdSaveFormat format)
        //{
        //    try
        //    {
        //        // Create an instance of Word.exe
        //        Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();

        //        // Make this instance of word invisible (Can still see it in the taskmgr).
        //        oWord.Visible = false;

        //        // Interop requires objects.
        //        object oMissing = System.Reflection.Missing.Value;
        //        object isVisible = true;
        //        object readOnly = false;
        //        object oInput = input;
        //        object oOutput = output;
        //        object oFormat = format;
        //        object oFileShare = true;

        //        // Load a document into our instance of word.exe
        //        Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Open(ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

        //        // Make this document the active document.
        //        oDoc.Activate();

        //        // Save this document in Word 2003 format.
        //        oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

        //        // Always close Word.exe.
        //        oWord.Quit(ref oMissing, ref oMissing, ref oMissing);

        //        using (var fs = new FileStream(output, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //        {
        //            fs.Close();
        //        }
        //    }
        //    catch (IOException ex)
        //    {
        //        throw ex;
        //    }




        //}


        private string[] getColumns(int PageNo, int Count, string[] args)
        {
            int schoolId = 0, studentId = 0, userId = 0;
            schoolId = Convert.ToInt32(args[4]);
            studentId = Convert.ToInt32(args[3]);
            userId = Convert.ToInt32(args[5]);
            objExport = new ClsExportNew();
            string[] data = new string[Count];
            string[] data2 = new string[2];
            string temp = "";
            int counter = 0;
            objExport.getIEP1(out data, out data2, studentId, schoolId, args, PageNo);
            //  if (PageNo == 2) objExport.getIEP2(out data, out data2, studentId, schoolId, args[1]);
            //if (PageNo == 3) objExport.getIEP3(out data, out data2, sess.StudentId, schoolId);
            //if (PageNo == 4) objExport.getIEP4(out data, out data2, sess.StudentId, schoolId);
            //if (PageNo == 5) objExport.getIEP5(out data, out data2, sess.StudentId, schoolId);

            return data;
        }





        private string CopyTemplate(string temp, string oldPath, string PageNo)
        {
            try
            {
                string Time = DateTime.Now.TimeOfDay.ToString();
                string[] ar = Time.Split('.');
                Time = ar[0];
                Time = Time.Replace(":", "-");
                string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

                string path = temp;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                Guid g;

                g = Guid.NewGuid();


                string newpath = path + "\\";
                string ids = g.ToString();
                ids = ids.Replace("-", "");

                string newFileName = "IEPTemporyTemplate" + ids.ToString();
                FileInfo f1 = new FileInfo(oldPath);
                if (f1.Exists)
                {
                    if (!Directory.Exists(newpath))
                    {
                        Directory.CreateDirectory(newpath);
                    }

                    f1.CopyTo(string.Format("{0}{1}{2}", newpath, newFileName, f1.Extension));
                }
                return newpath + newFileName + f1.Extension;
            }
            catch (Exception Ex)
            {

                return "";
            }
        }

        public static String URLTOHTML(string Url)
        {
            string result = "";
            try
            {

                using (StreamReader reader = new StreamReader(Url))
                {
                     result = reader.ReadToEnd();
                    //liju
                    //while ((line = reader.ReadLine()) != null)
                    //{
                   //     result += line;
                   // }
                }
                
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return result;

        }
    }
}
