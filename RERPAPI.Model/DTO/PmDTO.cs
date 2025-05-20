using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RERPAPI.Model.DTO
{
    public class PmDTO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public int EmployeeID { get; set; }
    }
}
