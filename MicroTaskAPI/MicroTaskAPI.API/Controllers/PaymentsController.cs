using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroTaskAPI.Application.DTOs.Payment;
using MicroTaskAPI.Application.Interfaces.Services;
using System.Security.Claims;

namespace MicroTaskAPI.API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize(Roles = "Buyer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentCreateDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                ?? throw new UnauthorizedAccessException("Email claim not found.");

            var result = await _paymentService.CreateAsync(email, dto);
            return Ok(result);
        }

        [Authorize(Roles = "Buyer")]
        [HttpGet("buyer/{email}")]
        public async Task<IActionResult> GetByBuyer(string email)
        {
            var result = await _paymentService.GetByBuyerEmailAsync(email);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _paymentService.GetPendingAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            await _paymentService.ApproveAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id, [FromBody] PaymentRejectDto dto)
        {
            await _paymentService.RejectAsync(id, dto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("total")]
        public async Task<IActionResult> GetTotal()
        {
            var result = await _paymentService.GetTotalPaymentsAsync();
            return Ok(new { total = result });
        }
    }
}