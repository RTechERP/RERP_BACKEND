using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RERPAPI.Model.Common;
using RERPAPI.Model.Entities;

namespace RERPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeSupplyRequestSummaryController : ControllerBase
    {
        [HttpGet("GetlistDepartment")]
        public IActionResult GetlistDepartment()
        {
            List<Department> departmentList = SQLHelper<Department>.FindAll().OrderBy(x => x.STT).ToList();
            return Ok(new
            {
                status = 0,
                data = departmentList
            });
        }

        [HttpGet("GetOfficeSupplyRequestSummary")]
        public IActionResult GetOfficeSupplyRequestSummary(int year, int month, string? keyword = "", int departmentId = 0)
        {
            try
            {
                DateTime dateStart = new DateTime(year, month, 1, 0, 0, 0);
                DateTime dateEnd = dateStart.AddMonths(1).AddSeconds(-1);

                List<List<dynamic>> result = SQLHelper<dynamic>.ProcedureToDynamicLists(
                    "spGetOfficeSupplyRequestSummary",
                    new string[] { "@DateStart", "@DateEnd", "@Keyword", "@DepartmentID" },
                    new object[] { dateStart, dateEnd, keyword, departmentId }
                );

                return Ok(new
                {
                    status = 1,
                    data = result[0]
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    status = 0,
                    message = ex.Message,
                    error = ex.ToString()
                });
            }
        }
    }
}
