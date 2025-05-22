using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RERPAPI.Model.Common;
using RERPAPI.Model.Context;
using RERPAPI.Model.DTO;
using RERPAPI.Model.Entities;
using RERPAPI.Repo.GenericEntity;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace RERPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeSuppliesController : ControllerBase
    {
      
        private readonly RTCContext _context;
        RTCContext db = new RTCContext();

        OfficeSupplyUnitRepo osurepo = new OfficeSupplyUnitRepo();
        public OfficeSuppliesController(RTCContext context)
        {
            _context = context;
        }

        [HttpGet("getall")]
        public IActionResult GetOfficeSupplies([FromQuery] string keyword = "")
        {
            List<OficeSuppliesDTO> result = SQLHelper<OficeSuppliesDTO>.ProcedureToList(
                "spGetOfficeSupply",
                new string[] { "@KeyWord" },
               new object[] { (object)(keyword ?? "") }  // đảm bảo không null
            );
            var data = result.Where(x => x.IsDeleted == false).ToList();
            return Ok(new
            {
                status = 0,
                data = data
            });
        }

        [HttpGet("GetOfficeSupplyUnit")]
        public IActionResult getunini()
        {
            List<OfficeSupplyUnit> result = SQLHelper<OfficeSupplyUnit>.FindAll();
            var data = result.Where(x => x.IsDeleted == false).ToList();
            return Ok(
                new { status = 0,data=data}
            );
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OfficeSupply>> GetOfficeSupply(int id)
        {
            var officeSupply = await _context.OfficeSupplies.FindAsync(id);

            if (officeSupply == null)
            {
                return NotFound();
            }

            return officeSupply;
        }
     
        [HttpPost("delete-vpp")]
        public async Task<IActionResult> DeleteVpp([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest("Danh sách ID không hợp lệ.");

            foreach (var id in ids)
            {
                var item = await db.OfficeSupplies.FindAsync(id);
                if (item != null)
                {
                    item.IsDeleted = true; // Gán trường IsDeleted thành true
                    db.OfficeSupplies.Update(item); // Cập nhật lại mục
                }
            }

            await db.SaveChangesAsync();
            return Ok(new { message = "Đã xóa thành công." });
        }
        [HttpGet]
        [Route("next-codeRTC")]
        public async Task<IActionResult> GetNextCodeRTC()
        {
            var allCodes = await db.OfficeSupplies
    .Where(x => x.CodeRTC.StartsWith("VPP"))
    .Select(x => x.CodeRTC)
    .ToListAsync();

            int maxNumber = 0;
            foreach (var code in allCodes)
            {
                var numberPart = code.Substring(3);
                if (int.TryParse(numberPart, out int num))
                {
                    if (num > maxNumber)
                        maxNumber = num;
                }
            }

            int nextNumber = maxNumber + 1;
            var nextCodeRTC = "VPP" + nextNumber;
            return Ok(nextCodeRTC);

        }

        [HttpPost]
        [Route("themVPP")]
        public async Task<IActionResult> AddVPP([FromBody] OfficeSuppliesRepo input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tạo CodeRTC tiếp theo (giống logic trong next-codeRTC)
            var maxCode = await db.OfficeSupplies
                .Where(x => x.CodeRTC.StartsWith("VPP"))
                .OrderByDescending(x => x.CodeRTC)
                .Select(x => x.CodeRTC)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(maxCode))
            {
                var numberPart = maxCode.Substring(3);
                if (int.TryParse(numberPart, out int currentNumber))
                {
                    nextNumber = currentNumber + 1;
                }
            }

            var newCodeRTC = "VPP" + nextNumber;

            var newVP = new OfficeSupply
            {
                CodeRTC = newCodeRTC, // Gán CodeRTC đã sinh
                CodeNCC = input.CodeNCC,
                NameRTC = input.NameRTC,
                NameNCC = input.NameNCC,
                Price = input.Price,
                RequestLimit = input.RequestLimit ?? 1,
                Type = input.Type,
                IsDeleted = false,
                SupplyUnitID = input.SupplyUnitID,
                CreatedBy = input.CreatedBy,
                CreatedDate = input.CreatedDate ?? DateTime.Now,
                UpdatedBy = input.UpdatedBy,
                UpdatedDate = input.UpdatedDate ?? DateTime.Now,
            };

            await db.OfficeSupplies.AddAsync(newVP);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(AddVPP), new { id = newVP.ID }, newVP);
        }

        [HttpPost("capnhatVPP")]
        public async Task<IActionResult> UpdateVPPAsync([FromBody] OfficeSuppliesRepo input)
        {
            var offlice = await db.OfficeSupplies.FindAsync(input.ID);
            if (offlice == null)
            {
                return NotFound();
            }

            // Kiểm tra từng thuộc tính và cập nhật chỉ khi không bị bỏ trống hoặc null
            if (!string.IsNullOrEmpty(input.CodeNCC))
            {
                offlice.CodeNCC = input.CodeNCC;
            }

            if (!string.IsNullOrEmpty(input.NameNCC))
            {
                offlice.NameNCC = input.NameNCC;
            }

            if (!string.IsNullOrEmpty(input.NameRTC))
            {
                offlice.NameRTC = input.NameRTC;
            }

            if (input.RequestLimit > 0)
            {
                offlice.RequestLimit = input.RequestLimit;
            }

            if (input.SupplyUnitID > 0)
            {
                offlice.SupplyUnitID = input.SupplyUnitID;
            }

            if (input.Type.HasValue)
            {
                offlice.Type = input.Type;
            }
            // Update the offlice in the database
            db.OfficeSupplies.Update(offlice);
            await db.SaveChangesAsync();

            return Ok();
        }
        //danh sách tính
        [HttpPost("savedatas")]
        public async Task<IActionResult> SaveDST([FromBody] OfficeSupplyUnit dst)
        {
            try
            {
                if (dst.ID <= 0)
                {
                    dst.IsDeleted = false;
                    await osurepo.CreateAsync(dst);
                }
                else await osurepo.UpdateAsync(dst);

                return Ok(new
                {
                    status = 1,
                    data = dst
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
        [HttpGet("getbyid")]
        public IActionResult GetByID(int id)
        {
            try
            {
                OfficeSupplyUnit dst = osurepo.GetByID(id);
                return Ok(new
                {
                    status = 1,
                    data = dst
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

        [HttpPost("deleteOfficeSupplyUnit")]
        public async Task<IActionResult> DeleteOfficeSupplyUnit([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest("Danh sách ID không hợp lệ.");

            foreach (var id in ids)
            {
                var item = await db.OfficeSupplyUnits.FindAsync(id);
                if (item != null)
                {
                    item.IsDeleted = true; // Gán trường IsDeleted thành true
                    db.OfficeSupplyUnits.Update(item); // Cập nhật lại mục
                }
            }

            await db.SaveChangesAsync();
            return Ok(new { message = "Đã xóa thành công." });
        }


    }
}
