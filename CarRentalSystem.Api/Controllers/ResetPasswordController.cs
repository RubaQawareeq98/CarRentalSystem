using CarRentalSystem.Api.Models.Users;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace CarRentalSystem.Api.Controllers
{
    [Route("api/reset-password")]
    [ApiController]
    public class ResetPasswordController(IEmailService emailService, IUserRepository userRepository) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var user = await userRepository.FindUserByEmailAsync(resetPasswordDto.Email);
            if (user is null)
            {
                return NotFound();
            }
            
            await emailService.SendResetPasswordEmail(user);
            return Ok();
        }
    }
}
