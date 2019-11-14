﻿using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LeaveApplication.Models
{
    public class Excels
    {
        
        public List<Attendance> Read(Stream FileStream)
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
                        var cell3 = dataRow.Cell(4).Value;
                        l1.Add(new Attendance()
                        {
                            EmpNo = Convert.ToInt32(cell.ToString()),
                            EmployeeName = dataRow.Cell(3).Value.ToString(),
                            Abscent = Convert.ToBoolean(cell2.ToString()),
                            Date = Convert.ToDateTime(cell3)

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
