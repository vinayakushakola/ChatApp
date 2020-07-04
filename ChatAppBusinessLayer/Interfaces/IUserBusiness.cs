//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : Interface of User Business
//

using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatAppBusinessLayer.Interfaces
{
    public interface IUserBusiness
    {
        Task<List<RegistrationResponse>> GetListOfUsers();

        Task<RegistrationResponse> UserRegistration(RegistrationRequest userDetails);

        Task<RegistrationResponse> UserLogin(LoginRequest loginDetails);

        Task<RegistrationResponse> ForgotPassword(ForgotPasswordRequest forgotPassword);

        Task<bool> ResetPassword(int userID, ResetPasswordRequest resetPassword);
    }
}
