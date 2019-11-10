using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagination
{
    public class Pagination
    {
        int _PageLastRow;
        double _TotalPages;
        int _PageFirstRow;
        int _OffsetRows;
        public int PagedLastRow
        {
            get
            { return _PageLastRow; }
        }
    
        public int PageFirstRow
        {
            get
            { return _PageFirstRow; }
        }
       
        public double TotalPages
        {
            get
            { return _TotalPages; }
        }
        public int OffsetRows
        {
           get  { return _OffsetRows; }
        }
        
        /// <summary>
        /// It will calculate First,Last Row of respective page and also calculate number of pages...
        /// </summary>
        /// <param name="TotalRows"></param>
        /// <param name="PageNo"></param>
        /// <param name="PerPage"></param>
        public void CalculateRanges(int TotalRows, int? PageNo, int PerPage)
        {

            _PageLastRow = PageNo.Value * PerPage;

            _PageFirstRow = (_PageLastRow - PerPage)+1;

            _OffsetRows = _PageFirstRow-1;
            //eg 12/2=6 this tells that 12rows can take 6 pages if you set 2 rows per page so if we multiply 2 by 6 then its 12 again ceiling function is for odd number of rows so that we get nearest greater integer for pages.
            _TotalPages = Math.Ceiling((double)TotalRows / PerPage);
        }
    }
}
