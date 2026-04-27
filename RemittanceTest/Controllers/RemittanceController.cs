using Microsoft.AspNetCore.Mvc;
using RemittanceTest.Models;
using RemittanceTest.Services;

namespace RemittanceTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RemittanceController : ControllerBase
    {
        // TODO: 1. 請透過建構子注入 (Constructor Injection) 引入 IRemittanceService
        private readonly IRemittanceService _remittanceService;

        public RemittanceController(IRemittanceService remittanceService)
        {
            _remittanceService = remittanceService;
        }

        [HttpPost("{id}/cancel")]
        public IActionResult Cancel(int id)
        {
            // TODO: 2. 呼叫 Service 執行取消邏輯
            // TODO: 3. 根據 Service 回傳的結果，回傳相對應的 HTTP 狀態碼 (Ok / BadRequest / NotFound)

            var result = _remittanceService.CancelRemittance(id);

            if (!result.IsSuccess)
            {
                if (result.Message.Contains("找不到"))
                {
                    return NotFound(new { message = result.Message });
                }
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}