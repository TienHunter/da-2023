using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.DTO
{
    public class PagingParam
    {
        public string keySearch;
        public int pageNumber;
        public int pageSize;
        public List<string>? fieldsSearch;
        public Dictionary<string, string>? fieldToSort;
    }
}
