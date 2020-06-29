//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : It is Controller of Admin
//

using ChatAppBusinessLayer.Interfaces;
using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminBusiness _adminBusiness;
        private readonly IConfiguration _configuration;

        private static bool success;
        private static string message;
        private static string token;

        public AdminController(IAdminBusiness adminBusiness, IConfiguration configuration)
        {
            _adminBusiness = adminBusiness;
            _configuration = configuration;
        }

        /// <summary>
        /// Admin Registration
        /// </summary>
        /// <param name="adminDetails">Admin Registration Details</param>
        /// <returns>If data found return Ok else Not Found or Bad Request</returns>
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration(AdminRegistrationRequest adminDetails)
        {
            try
            {
                var data = await _adminBusiness.AdminRegistration(adminDetails);
                if (data != null)
                {
                    success = true;
                    message = "Admin Account Created Successfully";
                    return Ok(new { success, message, data });
                }
                else
                {
                    message = "No Data Provided";
                    return NotFound(new { success, message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        /// <summary>
        /// Admin Login
        /// </summary>
        /// <param name="loginDetails">Admin Login Details</param>
        /// <returns>If data found return Ok else Not Found or Bad Request</returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> AdminLogin(AdminLoginRequest loginDetails)
        {
            try
            {
                var data = await _adminBusiness.AdminLogin(loginDetails);
                if (data != null)
                {
                    success = true;
                    message = "Admin Successfully Logged In";
                    token  = GenerateToken(data, "login");
                    return Ok(new { success, message, data, token });
                }
                else
                {
                    message = "No Admin Account Present with this Email-ID and Password";
                    return NotFound(new { success, message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        /// <summary>
        /// Generates Token
        /// </summary>
        /// <param name="adminDetails">Admin Response Details</param>
        /// <param name="tokenType">Token Type</param>
        /// <returns>It return token else exception</returns>
        private string GenerateToken(RegistrationResponse adminDetails, string tokenType)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("AdminID", adminDetails.AdminID.ToString()),
                    new Claim("Email", adminDetails.Email.ToString()),
                    new Claim("TokenType", tokenType),
                    new Claim("UserRole", adminDetails.UserRole.ToString())
                };

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"],
                    claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}