using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp4
{
    class Excels
    {

        public List<Attendance> Read(FileStream FileStream)
        {
           
            List<Attendance> l1 = new List<Attendance>();

            using (var excelWorkbook = new XLWorkbook(FileStream))
            {
                var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed();

                foreach (IXLRow dataRow in nonEmptyDataRows)
                {
                   
                    if (dataRow.RowNumber() != 1)
                    {
                        var cell = dataRow.Cell(1).Value;
                        var cell2 = dataRow.Cell(12).Value;
                        l1.Add(new Attendance()
                        {
                            EmpId = Convert.ToInt32(cell.ToString()),
                            Abscent = Convert.ToBoolean(cell2.ToString())

                        });
                        Console.WriteLine(cell.ToString() + "\t" + cell2);
                    }



                }
            }
            FileStream.Close();
            return l1;

        }


    }
}
