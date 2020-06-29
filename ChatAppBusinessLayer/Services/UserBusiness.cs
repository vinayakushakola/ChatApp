//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : It Interacts between user controller & user repository
//

using ChatAppBusinessLayer.Interfaces;
using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using ChatAppRepositoryLayer.Interfaces;
using System;
using System.Threading.Tasks;

namespace ChatAppBusinessLayer.Services
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository _userRepository;

        public UserBusiness(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RegistrationResponse> UserRegistration(RegistrationRequest userDetails)
        {
            try
            {
                if (userDetails == null)
                    return null;
                else
                    return await _userRepository.UserRegistration(userDetails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<RegistrationResponse> UserLogin(LoginRequest loginDetails)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginDetails.Email) || string.IsNullOrWhiteSpace(loginDetails.Password))
                    return null;
                else
                    return _userRepository.UserLogin(loginDetails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<RegistrationResponse> ForgotPassword(ForgotPasswordRequest forgotPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(forgotPassword.Email))
                    return null;
                else
                    return _userRepository.ForgotPassword(forgotPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ResetPassword(int userID, ResetPasswordRequest resetPassword)
        {
            try
            {
                if (userID <= 0 || resetPassword.Password == null)
                    return false;
                else
                    return await _userRepository.ResetPassword(userID, resetPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
