//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : It is Controller of Chat
//

using ChatAppBusinessLayer.Interfaces;
using ChatAppCommonLayer.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatBusiness _chatBusiness;

        private static bool success;
        private static string message;

        public ChatController(IChatBusiness chatBusiness)
        {
            _chatBusiness = chatBusiness;
        }

        /// <summary>
        /// Send Message
        /// </summary>
        /// <param name="chatRequest">Chat Request Data</param>
        /// <returns>If Data Found return Ok else Not Found or Bad Request</returns>
        [HttpPost]
        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage(ChatRequest chatRequest)
        {
            try
            {
                var user = HttpContext.User;
                if (user.HasClaim(u => u.Type == "TokenType"))
                {
                    if ((user.Claims.FirstOrDefault(u => u.Type == "UserRole").Value == "User") ||
                            (user.Claims.FirstOrDefault(u => u.Type == "TokenType").Value == "login"))
                    {
                        int userID = Convert.ToInt32(user.Claims.FirstOrDefault(u => u.Type == "UserID").Value);
                        var data = await _chatBusiness.SendMessage(userID, chatRequest);
                        if (data != null)
                        {
                            success = true;
                            message = "Your Message Sent Successfully";
                            return Ok(new { success, message, data });
                        }
                        else
                        {
                            message = "Unable to Send the Message";
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
    }
}