using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace IRSClassLib
{
    public class csExcel
    {
        public class ExcelGenerator
        {
            /// <summary>
            /// Create and Excel File using Excel Xml
            /// </summary>
            /// <param name="FileName">Complete File Path</param>
            /// <param name="Ds">Data Set</param>
            /// <param name="SheetName">Work Sheet Name</param>
            /// <param name="RecordsPerSheet">No of records to show per sheet.</param>
            public static void CreateExcelXML(string FileName, DataSet Ds, string SheetName, int RecordsPerSheet)
            {
                StringWriter S = new StringWriter();
                XmlTextWriter X = new XmlTextWriter(S);
                StreamWriter Writer = new StreamWriter(FileName, false);

                try
                {
                    SheetName = (SheetName.Trim().Length == 0) ? "WorkSheet" : SheetName;
                    RecordsPerSheet = (RecordsPerSheet <= 0) ? 100000 : RecordsPerSheet;

                    X.Formatting = Formatting.Indented;
                    X.Indentation = 4;
                    X.WriteProcessingInstruction("xml", "version=\"1.0\"");
                    X.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");
                    X.WriteStartElement("Workbook", string.Empty);
                    X.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:spreadsheet");
                    X.WriteAttributeString("xmlns:o", "urn:schemas-microsoft-com:office:office");
                    X.WriteAttributeString("xmlns:x", "urn:schemas-microsoft-com:office:excel");
                    X.WriteAttributeString("xmlns:ss", "urn:schemas-microsoft-com:office:spreadsheet");
                    X.WriteAttributeString("xmlns:html", "http://www.w3.org/TR/REC-html40");

                    X.WriteStartElement("DocumentProperties", string.Empty);
                    X.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:office");
                    X.WriteElementString("Author", "Ronald James L. Tuazon");
                    X.WriteElementString("LastAuthor", string.Empty);
                    X.WriteElementString("Created", string.Empty);
                    X.WriteElementString("Company", string.Empty);
                    X.WriteElementString("Version", "1.0");
                    X.WriteEndElement();

                    X.WriteStartElement("ExcelWorkBook", string.Empty);
                    X.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:office");
                    X.WriteEndElement();

                    X.WriteStartElement("Styles", string.Empty);
                    X.WriteStartElement("Style", string.Empty);
                    X.WriteAttributeString("ss:ID", "Default");
                    X.WriteAttributeString("ss:Name", "Normal");
                    X.WriteStartElement("Alignment", string.Empty);
                    X.WriteAttributeString("ss:Vertical", "Bottom");
                    X.WriteEndElement();
                    X.WriteElementString("Borders", string.Empty);
                    X.WriteElementString("Font", string.Empty);
                    X.WriteElementString("Interior", string.Empty);
                    X.WriteElementString("NumberFormat", string.Empty);
                    X.WriteElementString("Protection", string.Empty);
                    X.WriteEndElement();
                    X.WriteEndElement();

                    for (int i = 0; i < Ds.Tables.Count; i++)
                    {
                        int k = 0;
                        for (int j = 0; j < Ds.Tables[i].Rows.Count; j++)
                        {
                            string Name = string.Empty;
                            if (j == 0)
                            {
                                Name = SheetName + " " + Convert.ToString(i + 1) + "." + k.ToString();
                                WriteWorkSheetStart(X, Ds, i, Name);
                                k++;
                            }
                            else if ((j % RecordsPerSheet) == 0)
                            {
                                Name = SheetName + " " + Convert.ToString(i + 1) + "." + k.ToString();
                                WriteWorkSheetEnd(X);
                                WriteWorkSheetStart(X, Ds, i, Name);
                                k++;
                            }
                            // Writing Data Rows
                            X.WriteStartElement("Row");
                            for (int x = 0; x < Ds.Tables[i].Columns.Count; x++)
                            {
                                X.WriteStartElement("Cell");
                                X.WriteStartElement("Data");
                                X.WriteAttributeString("ss:Type", "String");
                                X.WriteValue(Ds.Tables[i].Rows[j][x].ToString());
                                X.WriteEndElement();
                                X.WriteEndElement();
                            }
                            X.WriteEndElement();
                        }
                        WriteWorkSheetEnd(X);
                    }

                    X.WriteEndElement(); // End Workbook Element
                    X.Flush();
                    Writer.Write(S.ToString());
                }
                finally
                {
                    Writer.Close();
                    S.Close();
                    X.Close();
                    S.Dispose();
                    Writer.Dispose();
                }
            }

            private static void WriteWorkSheetStart(XmlTextWriter X, DataSet Ds, int i, string SheetName)
            {
                X.WriteStartElement("Worksheet", string.Empty);
                X.WriteAttributeString("ss:Name", SheetName);
                X.WriteStartElement("Table", string.Empty);
                X.WriteAttributeString("ss:ExpandedColumnCount", Ds.Tables[i].Columns.Count.ToString());
                X.WriteAttributeString("ss:ExpandedRowCount", Convert.ToString(Ds.Tables[i].Rows.Count + 1));
                X.WriteAttributeString("x:FullColumns", "1");
                X.WriteAttributeString("x:FullRows", "1");
                X.WriteStartElement("Row");
                for (int j = 0; j < Ds.Tables[i].Columns.Count; j++)
                {
                    X.WriteStartElement("Cell");
                    X.WriteStartElement("Data");
                    X.WriteAttributeString("ss:Type", "String");
                    X.WriteValue(Ds.Tables[i].Columns[j].ColumnName);
                    X.WriteEndElement();
                    X.WriteEndElement();
                }
                X.WriteEndElement();
            }

            private static void WriteWorkSheetEnd(XmlTextWriter X)
            {
                X.WriteEndElement();
                X.WriteStartElement("WorksheetOptions", string.Empty);
                X.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:excel");
                X.WriteStartElement("Panes", string.Empty);
                X.WriteStartElement("Pane", string.Empty);
                X.WriteElementString("ActiveRow", "2");
                X.WriteElementString("ActiveCol", "1");
                X.WriteEndElement();
                X.WriteEndElement();
                X.WriteElementString("ProtectObjects", "False");
                X.WriteElementString("ProtectScenarios", "False");
                X.WriteEndElement();
                X.WriteEndElement();
            }
        }

        public bool ExtractToCSV(DataSet ds, string FileName, bool WithHeader, out string errorMessage)
        {
            errorMessage = string.Empty;
            int i = 0;
            int j = 0;
            string sLine = string.Empty;
            StreamWriter swFile = new StreamWriter(FileName);
            bool bReturn;

            try
            {
                if (WithHeader == true)
                {
                    //*** HEADER ***//
                    for (i = 0; i <= (ds.Tables[0].Columns.Count - 1); i++)
                    {
                        sLine += ds.Tables[0].Columns[i].ToString().Trim() + ", ";
                    }
                    sLine = sLine.Substring(0, sLine.Length - 1);
                    swFile.WriteLine(sLine);
                    sLine = string.Empty;
                }

                //*** DETAILS ***//
                for (i = 0; i <= (ds.Tables[0].Rows.Count - 1); i++)
                {
                    for (j = 0; j <= (ds.Tables[0].Columns.Count - 1); j++)
                    {
                        if (ds.Tables[0].Rows[i][j].ToString().Length > 0)
                        {
                            if (IsNumericValue(ds.Tables[0].Rows[i][j].ToString().Substring(0, 1)) == true)
                            {
                                sLine += "'" + ds.Tables[0].Rows[i][j].ToString().Trim().Replace(",", "") + ", ";
                            }
                            else
                            {
                                sLine += ds.Tables[0].Rows[i][j].ToString().Trim().Replace(",", "") + ", ";
                            }
                        }
                        else
                        {
                            sLine += ds.Tables[0].Rows[i][j].ToString().Trim().Replace(",", "") + ", ";
                        }
                    }
                    sLine = sLine.Substring(0, sLine.Length - 2);
                    swFile.WriteLine(sLine);
                    sLine = string.Empty;
                }
                bReturn = true;
            }
            catch (Exception ex)
            {
                bReturn = false;
                errorMessage = ex.Message;
            }
            finally
            {
                swFile.Flush();
                swFile.Close();
                swFile.Dispose();
                swFile = null;
            }
            return bReturn;
        }

        private bool IsNumericValue(string TextEntry)
        {
            Regex objNotWholePattern = new Regex("[^0-9]");
            return !objNotWholePattern.IsMatch(TextEntry);
        }
    }
}