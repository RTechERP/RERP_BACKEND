using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using RERPAPI.Model.Common;
using RERPAPI.Model.Context;
using RERPAPI.Model.DTO;
using RERPAPI.Model.Entities;
using System.Collections.Generic;

namespace RERPAPI.Controllers
{
    public class dkyVPP : Controller
    {

        RTCContext db = new RTCContext();
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("Getdata")]
        public IActionResult Getdata()
        {
            List<Department> departmentList = SQLHelper<Department>.FindAll().OrderBy(x => x.STT).ToList();
            return Ok(new
            {
                status = 0,
                data = departmentList
            });
        }
        [HttpGet("GetOfficeSupplyRequestsDetail")]
        public IActionResult GetOfficeSupplyRequestsDetail(int OfficeSupplyRequestsID)
        {
            try
            {
                List<List<dynamic>> result = SQLHelper<dynamic>.ProcedureToDynamicLists(
                        "spGetOfficeSupplyRequestsDetail",
                        new string[] { "@OfficeSupplyRequestsID" },
                       new object[] { OfficeSupplyRequestsID }

                    );
                List<dynamic> rs = result[0];
                return Ok(new
                {
                    status = 1,
                    data = rs
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

        [HttpPost("AdminApproved")]
        public async Task<IActionResult> AdminApproved([FromBody] List<int> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest("Danh sách ID không hợp lệ.");

                foreach (var id in ids)
                {
                    var item = (from a in db.OfficeSupplyRequests1
                                where a.ID == id
                                && a.IsApproved == false
                                && a.IsAdminApproved == false
                                select a).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        item.IsAdminApproved = true;
                        item.DateAdminApproved = DateTime.Now;
                        db.OfficeSupplyRequests1.Update(item); // Cập nhật lại mục
                    }
                }
                await db.SaveChangesAsync();
                return Ok(new
                {
                    status = 1,
                    
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

        [HttpPost("UnAdminApproved")]
        public async Task<IActionResult> UnAdminApproved([FromBody] List<int> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest("Danh sách ID không hợp lệ.");

                foreach (var id in ids)
                {
                    var item = (from a in db.OfficeSupplyRequests1
                                where a.ID == id
                                && a.IsApproved == false
                                && a.IsAdminApproved == true
                                select a).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        item.IsAdminApproved = false;
                        item.DateAdminApproved = DateTime.Now;
                        db.OfficeSupplyRequests1.Update(item); // Cập nhật lại mục
                    }
                }
                await db.SaveChangesAsync();
                return Ok(new
                {
                    status = 1,
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

        [HttpPost("IsApproved")]
        public async Task<IActionResult> IsApproved([FromBody] List<int> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest("Danh sách ID không hợp lệ.");
                foreach (var id in ids)
                {
                    var item = (from a in db.OfficeSupplyRequests1
                                where a.ID == id
                                && a.IsAdminApproved == true
                                && a.IsApproved == false
                                select a).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        item.IsApproved = true;
                        item.DateApproved = DateTime.Now;
                        db.OfficeSupplyRequests1.Update(item);
                    }
                }
                await db.SaveChangesAsync();
                return Ok(new
                {
                    status = 1,
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
        [HttpPost("UnIsApproved")]
        public async Task<IActionResult> UnIsApproved([FromBody] List<int> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest("Danh sách ID không hợp lệ.");
                foreach (var id in ids)
                {
                    var item = (from a in db.OfficeSupplyRequests1
                                where a.ID == id
                                && a.IsAdminApproved == true
                                && a.IsApproved == true                             
                                select a).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        item.IsApproved = false;
                        item.DateApproved = DateTime.Now;
                        db.OfficeSupplyRequests1.Update(item);
                    }
                }
                await db.SaveChangesAsync();
                return Ok(new
                {
                    status = 1,
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


        [HttpGet("GetOfficeSupplyRequests")]
        public IActionResult GetOfficeSupplyRequests(string? keyword, int? employeeID, int? departmentID, DateTime? monthInput)
        {
            try
            {
                List<List<dynamic>> result = SQLHelper<dynamic>.ProcedureToDynamicLists(
                    "spGetOfficeSupplyRequests",
                    new string[] { "@KeyWord", "@MonthInput", "@EmployeeID", "@DepartmentID" },
                   new object[] { keyword, monthInput, employeeID, departmentID }  // đảm bảo không null
                );
                List<dynamic> rs = result[0];

                return Ok(new
                {
                    status = 1,
                    data = rs
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
