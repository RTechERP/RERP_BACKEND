using RERPAPI.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RERPAPI.Model.DTO
{
    public class ProjectDTO : Project
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string FullNameSale { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string FullNameTech { get; set; }
        public string ProjectStatusText { get; set; }
        public string FullNamePM { get; set; }
        public int PersonalPriotity { get; set; }
        public string ProjectStatusName { get; set; }
        public int PMID { get; set; }
        public string EndUserName { get; set; }
        public string BussinessField { get; set; }
        public string UserMission { get; set; }
        public string ProjectProcessType { get; set; }
        public int CountNotComplete { get; set; }
        public DateTime PlanDateEndSummary { get; set; }
        public DateTime DateLog { get; set; }
        public int ProjectID { get; set; }
        public decimal TotalPriority { get; set; }
        public long RowNum { get; set; }
        public decimal PriotityText { get; set; }
    }
}
