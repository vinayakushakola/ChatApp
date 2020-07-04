//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : It is Controller of User
//

using ChatAppBusinessLayer.Interfaces;
using ChatAppCommonLayer.Models;
using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IConfiguration _configuration;

        private static bool success;
        private static string message;
        private static string token;

        public UserController(IUserBusiness userBusiness, IConfiguration configuration)
        {
            _userBusiness = userBusiness;
            _configuration = configuration;
        }

        /// <summary>
        /// Shows all the Users
        /// </summary>
        /// <returns>If Data Found return Ok else Not Found or Bad Request</returns>
        [HttpGet]
        public async Task<IActionResult> GetListOfUsers()
        {
            try
            {
                var data = await _userBusiness.GetListOfUsers();
                if (data != null)
                {
                    success = true;
                    message = "Users Data Fetched  Successfully";
                    return Ok(new { success, message, data });
                }
                else
                {
                    message = "No Data Found";
                    return NotFound(new { success, message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        /// <summary>
        /// User Registration
        /// </summary>
        /// <param name="userDetails">User Details</param>
        /// <returns>If data found return Ok else Not found or Bad request</returns>
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration(RegistrationRequest userDetails)
        {
            try
            {
                if (!ValidateRegisterRequest(userDetails))
                    return BadRequest(new { Message = "Enter Proper Data" });

                var data = await _userBusiness.UserRegistration(userDetails);
                if (data != null)
                {
                    success = true;
                    message = "User Account Created Successfully";
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
        /// User Login
        /// </summary>
        /// <param name="loginDetails">Login Details</param>
        /// <returns>If data found return Ok else Not found or Bad request</returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest loginDetails)
        {
            try
            {
                if (!ValidateLoginRequest(loginDetails))
                    return BadRequest(new { Message = "Enter Proper Data" });

                var data = await _userBusiness.UserLogin(loginDetails);
                if (data != null)
                {
                    success = true;
                    message = "User Successfully Logged In";
                    token = GenerateToken(data, "login");
                    return Ok(new { success, message, data, token });
                }
                else
                {
                    message = "No User Account Present with this Email-ID and Password";
                    return NotFound(new { success, message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="forgotPassword">Forgot Password Data</param>
        /// <returns>If data found return Ok else Not found or Bad request</returns>
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPassword)
        {
            try
            {
                if (!ValidateForgotPasswordRequest(forgotPassword))
                    return BadRequest(new { Message = "Please Enter Proper Data" });

                var data = await _userBusiness.ForgotPassword(forgotPassword);
                if (data != null)
                {
                    token = GenerateToken(data, "ForgotPassword");
                    success = true;
                    message = "Use this token to Reset Password";
                    MsmqSender.SendToMsmq(data.Email, token);
                    return Ok(new { success, message, token });
                }
                else
                {
                    message = "No User Found with this Email-ID: " + forgotPassword.Email;
                    return NotFound(new { success, message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="resetPassword">Reset Password Data</param>
        /// <returns>If reset password is successfull return ok else Not found or Bad request</returns>
        [HttpPost]
        [Authorize]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPassword)
        {
            try
            {
                if (!ValidateResetPasswordRequest(resetPassword))
                    return BadRequest(new { Message = "Please Enter Proper Data" });

                var user = HttpContext.User;
                if (user.HasClaim(u => u.Type == "TokenType"))
                {
                    if ((user.Claims.FirstOrDefault(u => u.Type == "TokenType").Value == "ForgotPassword") ||
                            (user.Claims.FirstOrDefault(u => u.Type == "TokenType").Value == "login"))
                    {
                        int userID = Convert.ToInt32(user.Claims.FirstOrDefault(u => u.Type == "UserID").Value);
                        success = await _userBusiness.ResetPassword(userID, resetPassword);
                        if (success)
                        {
                            success = true;
                            message = "Your Password Changed Successfully";
                            return Ok(new { success, message });
                        }
                        else
                        {
                            message = "Unable to Change the Password";
                            return NotFound(new { success, message });
                        }
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { success, message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        /// <summary>
        /// Generate Token
        /// </summary>
        /// <param name="userDetails">User Details</param>
        /// <param name="tokenType">Token Type</param>
        /// <returns>It returns token</returns>
        private string GenerateToken(RegistrationResponse userDetails, string tokenType)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("UserID", userDetails.ID.ToString()),
                    new Claim("Email", userDetails.Email.ToString()),
                    new Claim("TokenType", tokenType),
                    new Claim("UserRole", userDetails.UserRole.ToString())
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

        /// <summary>
        /// It Validate Registration Request
        /// </summary>
        /// <param name="userDetails">User Details</param>
        /// <returns>If validation successfull return true else false</returns>
        private bool ValidateRegisterRequest(RegistrationRequest userDetails)
        {
            if (userDetails == null || string.IsNullOrWhiteSpace(userDetails.FirstName) ||
                    string.IsNullOrWhiteSpace(userDetails.LastName) || string.IsNullOrWhiteSpace(userDetails.Email) ||
                    string.IsNullOrWhiteSpace(userDetails.Password) ||
                    userDetails.FirstName.Length < 3 || userDetails.LastName.Length < 3 || !userDetails.Email.Contains('@') ||
                    !userDetails.Email.Contains('.') || userDetails.Password.Length < 8)
                return false;
            else
                return true;
        }

        /// <summary>
        /// It Validate The Login Request 
        /// </summary>
        /// <param name="loginDetails">Login Details</param>
        /// <returns>If validation successfull return true else false</returns>
        private bool ValidateLoginRequest(LoginRequest loginDetails)
        {
            if (loginDetails == null || string.IsNullOrWhiteSpace(loginDetails.Email) ||
                string.IsNullOrWhiteSpace(loginDetails.Password) || !loginDetails.Email.Contains('@') ||
                !loginDetails.Email.Contains('.') || loginDetails.Password.Length < 8)
                return false;
            else
                return true;
        }

        /// <summary>
        /// It Validate the Forget Password Request
        /// </summary>
        /// <param name="forgotPassword">Forget Password Data</param>
        /// <returns>If validation is successfull return true else false</returns>
        private bool ValidateForgotPasswordRequest(ForgotPasswordRequest forgotPassword)
        {
            if (forgotPassword == null || string.IsNullOrWhiteSpace(forgotPassword.Email) ||
                !forgotPassword.Email.Contains('@') || !forgotPassword.Email.Contains('.'))
                return false;
            else
                return true;
        }

        /// <summary>
        /// It Validate the Reset Password Request 
        /// </summary>
        /// <param name="resetPassword">Reset Password Data</param>
        /// <returns>If validation is Successfull return true else false</returns>
        private bool ValidateResetPasswordRequest(ResetPasswordRequest resetPassword)
        {
            if (resetPassword == null || string.IsNullOrWhiteSpace(resetPassword.Password) ||
                resetPassword.Password.Length < 8)
                return false;
            else
                return true;
        }
    }
}