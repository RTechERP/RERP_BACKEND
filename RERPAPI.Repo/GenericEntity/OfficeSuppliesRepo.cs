using RERPAPI.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RERPAPI.Repo.GenericEntity
{
    public class OfficeSuppliesRepo
    {
        /* public string Unit { get; set; }
         public string TypeName { get; set; }*/
        public int ID { get; set; }
        public bool IsDeleted { get; set; }

        public string? CodeRTC { get; set; }

        public string? CodeNCC { get; set; }

        public string? NameRTC { get; set; }

        public string? NameNCC { get; set; }

        public int? SupplyUnitID { get; set; }

        public decimal? Price { get; set; }

        public int? RequestLimit { get; set; }

        public int? Type { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
