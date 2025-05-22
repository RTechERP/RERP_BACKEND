    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using RERPAPI.Model.Entities;
    using RERPAPI.Model.Common;
    using RERPAPI.Model.DTO;
    using RERPAPI.Repo.GenericEntity;
    using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
    using System;
    using static System.Runtime.InteropServices.JavaScript.JSType;
    using Microsoft.EntityFrameworkCore;
    using RERPAPI.Model.Context;

    namespace RERPAPI.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AssetsController : ControllerBase
        {
            private readonly RTCContext _context;
        RTCContext db = new RTCContext();
        public AssetsController(RTCContext context)
            {
                _context = context;
            }
            TSLostReportAssetRepo tslostreport = new TSLostReportAssetRepo();
             TSAllocationEvictionAssetRepo tSAllocationEvictionrepo = new TSAllocationEvictionAssetRepo();
           TSReportBrokenAssetRepo reportrepo = new TSReportBrokenAssetRepo();
            TSStatusAssetRepo tSStatusAssetRepo = new TSStatusAssetRepo();
            TTypeAssetsRepo typerepo = new TTypeAssetsRepo();
            TSAssetManagementRepo tasset = new TSAssetManagementRepo();
            TSSourceAssetsRepo tssourcerepo = new TSSourceAssetsRepo();
            TSAssetAllocationRepo tSAssetAllocationRepo = new TSAssetAllocationRepo();
            TSAssetManagementRepo tSAssetManagementRepo = new TSAssetManagementRepo();
            TSAssetAllocationDetailRepo tSAssetAllocationDetailRepo = new TSAssetAllocationDetailRepo();
            [HttpGet("getallassetsmanagement")]
            public IActionResult GetAllAsset()
            {
                try
                {
                    List<TSAssetManagement> tSAssetManagements = tSAssetManagementRepo.GetAll();
                    var maxSTT = tSAssetManagements
                              .Where(x => x.STT.HasValue)
                              .Max(x => x.STT);

                    return Ok(new
                    {
                  
                        status = 1,
                        data =  maxSTT 
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        status = 0,
                        message = ex.Message,
                        error = ex.ToString()
                    });
                }

            }
            [HttpGet("getall")]
            public IActionResult GetAll()
            {
                try {
                    List<TSSourceAsset> tSSources = tssourcerepo.GetAll();
                    return Ok(new
                    {
                        status = 1,
                        data = tSSources
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        status = 0,
                        message = ex.Message,
                        error = ex.ToString()
                    });
                }
            
            }
        [HttpPost("HRApproved")]
        public async Task<IActionResult> HRApproved([FromBody] List<int> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest("Danh sách ID không hợp lệ.");

                foreach (var id in ids)
                {
                    var item = (from a in db.TSAssetAllocations
                                where a.ID == id
                                && a.Status == 0
                                && a.IsApprovedPersonalProperty == true
                                select a).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        item.Status = 1;
                        item.DateApprovedHR = DateTime.Now;
                        db.TSAssetAllocations.Update(item); // Cập nhật lại mục
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
        [HttpPost("HRCancelApproved")]
        public async Task<IActionResult> HRCancelApproved([FromBody] List<int> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest("Danh sách ID không hợp lệ.");

                foreach (var id in ids)
                {
                    var item = (from a in db.TSAssetAllocations
                                where a.ID == id
                                && a.Status == 1
                                && a.IsApprovedPersonalProperty == false
                                select a).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        item.Status = 0;
                        item.DateApprovedHR = DateTime.Now;
                        db.TSAssetAllocations.Update(item); 
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
        [HttpPost("AccountantApproved")]
        public async Task<IActionResult> AccountantApproved([FromBody] List<int> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest("Danh sách ID không hợp lệ.");

                foreach (var id in ids)
                {
                    var item = (from a in db.TSAssetAllocations
                                where a.ID == id
                                && a.Status == 1
                                && a.IsApproveAccountant == false
                                select a).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        item.IsApproveAccountant = true;
                        item.DateApproveAccountant = DateTime.Now;
                        db.TSAssetAllocations.Update(item);
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
        [HttpPost("AccountantCancelApproved")]
        public async Task<IActionResult> AccountantCancelApproved([FromBody] List<int> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest("Danh sách ID không hợp lệ.");

                foreach (var id in ids)
                {
                    var item = (from a in db.TSAssetAllocations
                                where a.ID == id
                                && a.Status == 1
                                && a.IsApproveAccountant == true
                                select a).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        item.IsApproveAccountant = false;
                        item.DateApproveAccountant = DateTime.Now;
                        db.TSAssetAllocations.Update(item);
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
        [HttpGet("getallreportbroken")]
            public IActionResult GetAllRepoerBroken()
            {
                try
                {
                    List<TSReportBrokenAsset> reportbroken = reportrepo.GetAll();
                    return Ok(new
                    {
                        status = 1,
                        data = reportbroken
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        status = 0,
                        message = ex.Message,
                        error = ex.ToString()
                    });
                }

            }
            [HttpGet("getstatus")]
            public IActionResult GetStatus()
            {
                try
                {
                    List<TSStatusAsset> tSStatusAssets = tSStatusAssetRepo.GetAll();
                    return Ok(new
                    {
                        status = 1,
                        data = tSStatusAssets
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        status = 0,
                        message = ex.Message,
                        error = ex.ToString()
                    });
                }

            }
        
            [HttpGet("getassets")]
            public IActionResult GetList(string? FilterText, int PageNumber, int PageSize, DateTime? DateStart, DateTime? DateEnd, string? Status, string Department)
            {
                try
                {
      
                    DateTime minDate = new DateTime(2020, 12, 5);
                    DateTime maxDate = new DateTime(2025, 12, 19);

               
                    DateTime dateTimeS = DateStart ?? minDate;
                    DateTime dateTimeE = DateEnd ?? maxDate;

          
                    dateTimeS = new DateTime(dateTimeS.Year, dateTimeS.Month, dateTimeS.Day, 0, 0, 0);
                    dateTimeE = new DateTime(dateTimeE.Year, dateTimeE.Month, dateTimeE.Day, 23, 59, 59);

                    var assets = SQLHelper<dynamic>.ProcedureToDynamicLists("spLoadTSAssetManagement",
                        new string[] { "@FilterText", "@PageNumber", "@PageSize", "@DateStart", "@DateEnd", "@Status", "@Department" },
                        new object[] { FilterText, PageNumber, PageSize, dateTimeS, dateTimeE, Status, Department });

                    List<TSAssetManagement> tSAssetManagements = tSAssetManagementRepo.GetAll();
               


                    return Ok(new
                    {
                        status = 1,
                        data = new
                        {
                            assets = assets[0],
                        }
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
            [HttpGet("getallocation")]
            public IActionResult GetAllocation(string? ID)
            {
                try
                {
                    var assetsallocation = SQLHelper<dynamic>.ProcedureToDynamicLists(
          "spLoadTSAllocationEvictionAsset",
          new string[] { "@ID" },          // ← đổi đây
          new object[] { ID }
      );

                    return Ok(new
                    {
                        status = 1,
                        data = new
                        {
                            assetsallocation = assetsallocation[0],
                        }
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
            [HttpGet("getassetsallocationdetail")]
            public IActionResult GetAllocationDetail(string? ID)
            {
                try
                {
                    var assetsallocationdetail = SQLHelper<dynamic>.ProcedureToDynamicLists(
               "spGetTSAssetAllocationDetail",
               new string[] { "@TSAssetAllocationID" },      // <-- đây
               new object[] { ID }                           // <-- và đây
           );

                    return Ok(new
                    {
                        status = 1,
                        data = new
                        {
                            assetsallocationdetail = assetsallocationdetail[0],
                        }
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


            [HttpGet("gettype")]
            public IActionResult GetType()
            {
                try
                {
                    List<TSAsset> tSAssets = typerepo.GetAll();
                    return Ok(new
                    {
                        status = 1,
                        data = tSAssets
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        status = 0,
                        message = ex.Message,
                        error = ex.ToString()
                    });
                }

            }
            [HttpGet("getTSAssestAllocation")]
            public IActionResult GetTSAssetAllocation(
          DateTime? dateStart = null,
          DateTime? dateEnd = null,
          int? employeeID = null,
          string? status = null,
          string? filterText = null,
          int pageSize = 10,
          int pageNumber = 1)
            {
                try
                {
                    // Nếu filterText = null, bạn có thể truyền DBNull.Value hoặc string.Empty tuỳ SP xử lý
                    var assetallocation = SQLHelper<dynamic>.ProcedureToDynamicLists(
                        "spGetTSAssetAllocation",
                        new string[] { "@DateStart", "@DateEnd", "@EmployeeID", "@Status", "@FilterText", "@PageSize", "@PageNumber" },
                        new object[] { dateStart,   dateEnd,      employeeID,      status,
                               filterText ?? string.Empty,
                               pageSize,      pageNumber });

                    return Ok(new
                    {
                        status = 1,
                        data = new { assetallocation = assetallocation[0] }
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

        
            [HttpPost("savedata")]
            public async Task<IActionResult> SaveEmployee([FromBody] TSAssetManagement asset)
            {
                try
                {
                    if (asset.ID <= 0) await tasset.CreateAsync(asset);
                    else await tasset.UpdateAsync(asset);

                    return Ok(new
                    {
                        status = 1,
                        data = asset
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
            [HttpPost("savedatareportbroken")]
            public async Task<IActionResult> SaveReportBroken([FromBody] ReportBrokenFullDto dto)
            {
                try
                {
                    // Lấy ChucVuID từ DB nếu không truyền từ client
                    int? chucVuId = 30;


                    // 1. Cập nhật TSAssetManagement
                    var assetmanagementData = tasset.GetByID(dto.AssetID) ?? new TSAssetManagement();
                    assetmanagementData.ID = dto.AssetID;
                    assetmanagementData.Note = dto.Note;
                    assetmanagementData.Status = dto.Status;
                    assetmanagementData.StatusID = dto.StatusID;
                    if (assetmanagementData.ID > 0)
                        await tasset.UpdateAsync(assetmanagementData);
                    else
                        await tasset.CreateAsync(assetmanagementData);

               
                    var reportbrokendata = new TSReportBrokenAsset

                    {
                        AssetManagementID = dto.AssetID,
                        DateReportBroken = dto.DateReportBroken,
                        Reason = dto.Reason,
                        CreatedDate = dto.DateReportBroken,
                        UpdatedDate = dto.DateReportBroken

                    };
                    await reportrepo.CreateAsync(reportbrokendata);

                    // 3. Thêm  
                    var allocationevictionasset = new TSAllocationEvictionAsset
                    {
                        AssetManagementID = dto.AssetID,
                        EmployeeID = dto.EmployeeID,
                        DepartmentID = dto.DepartmentID,
                        ChucVuID = chucVuId ?? 0,
                        DateAllocation = dto.DateReportBroken,
                        Note = dto.Reason,
                        Status = "Hỏng"
                    };
                    await tSAllocationEvictionrepo.CreateAsync(allocationevictionasset);

                    return Ok(new
                    {
                        status = 1,
                        data = new
                        {
                            assetmanagementData,
                            reportbrokendata,
                            allocationevictionasset
                        }
                    });
                }
                catch (Exception ex)
                {
                    return Ok(new { status = 0, message = ex.Message });
                }
            }

            [HttpPost("savedatareportlost")]
            public async Task<IActionResult> SaveReportLost([FromBody] ReportLostFullDto dto)
            {
                try
                {
                    // Lấy ChucVuID từ DB nếu không truyền từ client
                    int? chucVuId = 30;


                    // 1. Cập nhật TSAssetManagement
                    var assetmanagementData = tasset.GetByID(dto.AssetManagementID) ?? new TSAssetManagement();
                    assetmanagementData.ID = dto.AssetManagementID;
                    assetmanagementData.Note = dto.Note;
                    assetmanagementData.Status = dto.AssetStatus;
                    assetmanagementData.StatusID = dto.AssetStatusID;
 
                    assetmanagementData.UpdatedDate = dto.UpdatedDate;
                    assetmanagementData.UpdatedBy=dto.UpdatedBy;
                    if (assetmanagementData.ID > 0)
                        await tasset.UpdateAsync(assetmanagementData);
                    else
                        await tasset.CreateAsync(assetmanagementData);


                    var reportlostdata = new TSLostReportAsset

                    {
                             AssetManagementID  = dto.AssetManagementID,
                             DateLostReport =dto.DateLostReport,
                             Reason = dto.Reason,
                        CreatedDate = dto.CreatedDate,
                        CreatedBy = dto.CreatedBy,
                        UpdatedDate = dto.UpdatedDate,
                        UpdatedBy = dto.UpdatedBy

                    };
                    await tslostreport.CreateAsync(reportlostdata);

                    // 3. Thêm  
                    var allocationevictionasset = new TSAllocationEvictionAsset
                    {
                        AssetManagementID = dto.AssetManagementID,
                        EmployeeID = dto.EmployeeID,
                        DepartmentID = dto.DepartmentID,
                        ChucVuID = chucVuId ?? 0,
                        DateAllocation = dto.DateAllocation,
                        Note = dto.Reason,
                        Status = "Hỏng",
                        CreatedDate= dto.CreatedDate,
                        CreatedBy=dto.CreatedBy,
                        UpdatedDate= dto.UpdatedDate,
                        UpdatedBy=dto.UpdatedBy

                    };
                    await tSAllocationEvictionrepo.CreateAsync(allocationevictionasset);

                    return Ok(new
                    {
                        status = 1,
                        data = new
                        {
                            assetmanagementData,
                            reportlostdata,
                            allocationevictionasset
                        }
                    });
                }
                catch (Exception ex)
                {
                    return Ok(new { status = 0, message = ex.Message });
                }
            }

        [HttpPost("SaveAllocation")]
        public async Task<IActionResult> SaveAllocation([FromBody] TSAssetAllocationFullDTO dto)
        {
            try
            {
                //Thêm cấp phát
                var allocationModel = new TSAssetAllocation
                {
                    ID = dto.ID,
                    Code = dto.Code,
                    DateAllocation = dto.DateAllocation,
                    EmployeeID = dto.EmployeeID,
                    Note = dto.Note,
                    Status = 1
                };

                if (allocationModel.ID > 0)
                {
                    await tSAssetAllocationRepo.UpdateAsync(allocationModel);
                }
                else
                {
                    allocationModel.ID = await tSAssetAllocationRepo.CreateAsync(allocationModel);
                }

              //Thêm chi tiết
                foreach (var detail in dto.AssetDetails)
                {
                    var detailModel = new TSAssetAllocationDetail
                    {
                        ID = detail.ID,
                        STT = detail.STT,
                        AssetManagementID = detail.AssetManagementID,
                        TSAssetAllocationID = allocationModel.ID,
                        Quantity = detail.Quantity,
                        Note = detail.Note,
                        EmployeeID = detail.EmployeeID
                    };

                    if (detailModel.ID > 0)
                    {
                        await tSAssetAllocationDetailRepo.UpdateAsync(detailModel);
                    }
                    else
                    {
                        await tSAssetAllocationDetailRepo.CreateAsync(detailModel);
                    }

                   //Update AssetManagement tương ứng
                    var existingAsset = tasset.GetByID(detail.AssetManagementID);
              
                    if (existingAsset != null)
                    {
                        existingAsset.EmployeeID = detail.EmployeeID;
                        existingAsset.DepartmentID = detail.DepartmentID;
                        existingAsset.UpdatedDate = DateTime.Now;
                        existingAsset.UpdatedBy = detail.UpdatedBy;
                        existingAsset.StatusID = 2;
                        existingAsset.Status = "Đang sử dụng";

                      
                    }
                    await tasset.UpdateAsync(existingAsset);
                }

                return Ok(new
                {
                    status = 1,
                    message = "Lưu thành công",
                    data = allocationModel
                });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }
       
        [HttpGet("generate-allocation-code")]
        public async Task<IActionResult> GenerateAllocationCode([FromQuery] DateTime? allocationDate)
        {
            if (allocationDate == null)
                return BadRequest("allocationDate is required.");

            var date = allocationDate.Value.Date;


            var latestCode = await _context.TSAssetAllocations
                .Where(x => x.DateAllocation.HasValue &&
                            x.DateAllocation.Value.Date == date &&
                            !string.IsNullOrEmpty(x.Code))
                .OrderByDescending(x => x.ID)
                .Select(x => x.Code)
                .FirstOrDefaultAsync();

            string baseCode = $"TSCP{date:ddMMyyyy}";
            string code = string.IsNullOrEmpty(latestCode) ? $"{baseCode}00000" : latestCode;

            string numberPart = code.Substring(code.Length - 5);
            int nextNumber = int.TryParse(numberPart, out int num) ? num + 1 : 1;

            string numberStr = nextNumber.ToString("D5");
            string newCode = $"{baseCode}{numberStr}";

            return Ok(newCode);
        }


        [HttpGet("generate-allocation-code-asset")]
        public async Task<IActionResult> GenerateAllocationCodeAsset([FromQuery] DateTime? assetdate)
        {
            if (assetdate == null)
                return BadRequest("allocationDate is required.");

            var date = assetdate.Value.Date;
            var startDate = date;
            var endDate = date.AddDays(1);

            var latestCode = await _context.TSAssetManagements
                .Where(x => x.CreatedDate.HasValue &&
                            x.CreatedDate >= startDate &&
                            x.CreatedDate < endDate &&
                            !string.IsNullOrEmpty(x.TSAssetCode))
                .OrderByDescending(x => x.ID)
                .Select(x => x.TSAssetCode)
                .FirstOrDefaultAsync();

            string baseCode = $"TS{date:ddMMyyyy}";
            string code = string.IsNullOrEmpty(latestCode) ? $"{baseCode}00000" : latestCode;

            string numberPart = code.Substring(code.Length - 5);
            int nextNumber = int.TryParse(numberPart, out int num) ? num + 1 : 1;

            string numberStr = nextNumber.ToString("D5");
            string newCode = $"{baseCode}{numberStr}";

            return Ok(newCode);
        }

        [HttpPost("deleteAssetAllocation")]
        public async Task<IActionResult> DeleteAssetAllocation([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest("Danh sách ID không hợp lệ.");

            foreach (var ID in ids)
            {
                var item = await db.TSAssetAllocations.FindAsync(ID);
                if (item != null)
                {
                    item.IsDeleted = true; 
                    db.TSAssetAllocations.Update(item);
                }
            }

            await db.SaveChangesAsync();
            return Ok(new { message = "Đã xóa thành công." });
        }
        [HttpPost("deleteAssetManagement")]
        public async Task<IActionResult> DeleteAssetManagement([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest("Danh sách ID không hợp lệ.");

            foreach (var ID in ids)
            {
                var item = await db.TSAssetManagements.FindAsync(ID);
                if (item != null)
                {
                    item.IsDeleted = true;
                    db.TSAssetManagements.Update(item);
                }
            }

            await db.SaveChangesAsync();
            return Ok(new { message = "Đã xóa thành công." });
        }
        [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteAsset(int id)
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        status = 0,
                        message = "Asset ID phải lớn hơn 0."
                    });
                }
                try
                {
                    int affectedRows = await tasset.DeleteAsync(id);
                    if (affectedRows == 0)
                    {

                        return NotFound(new
                        {
                            status = 0,
                            message = $"Không tìm thấy asset với ID = {id}."
                        });
                    }
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new
                    {
                        status = 0,
                        message = "Đã xảy ra lỗi khi xóa asset.",
                        error = ex.Message
                    });
                }
            }

        }
    }
dffgngcdhmm