using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RERPAPI.Model.Entities;
using RERPAPI.Repo.GenericEntity;

namespace RERPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        UnitRepo unitrepo = new UnitRepo();

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            try
            {
                List<UnitCount> units = unitrepo.GetAll();
                return Ok(new
                {
                    status = 1,
                    data = units
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
        [HttpPost("savedata")]
        public async Task<IActionResult> SaveUnit([FromBody] UnitCount unit)
        {
            try
            {
                if (unit.ID <= 0) await unitrepo.CreateAsync(unit);
                else await unitrepo.UpdateAsync(unit);

                return Ok(new
                {
                    status = 1,
                    data = unit
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
    }
}
