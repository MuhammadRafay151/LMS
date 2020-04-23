using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
namespace LeaveApplication.Models
{
    public class Excels
    {
        
        public List<Attendance> Read(Stream FileStream)
        {
            DateTime d1;
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
                        var cell3 = dataRow.Cell(4).Value;
                        d1 = DateTime.Parse(cell3.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                        if (Convert.ToBoolean(cell2.ToString())==true)
                        {
                            l1.Add(new Attendance()
                            {
                                EmpNo = Convert.ToInt32(cell.ToString()),
                                EmployeeName = dataRow.Cell(3).Value.ToString(),
                                Abscent = Convert.ToBoolean(cell2.ToString()),
                                Date =d1

                            });
                        }
                       
                       
                    }



                }
            }
            FileStream.Close();
            return l1;

        }
      

    }
}
