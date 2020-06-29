//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : It is Controller of Admin
//

using ChatAppBusinessLayer.Interfaces;
using ChatAppCommonLayer.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminBusiness _adminBusiness;

        private static bool success;
        private static string message;

        public AdminController(IAdminBusiness adminBusiness)
        {
            _adminBusiness = adminBusiness;
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
                    return Ok(new { success, message, data });
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
    }
}