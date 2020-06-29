//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : Interface of User Repository
//

using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using System.Threading.Tasks;

namespace ChatAppRepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<RegistrationResponse> UserRegistration(RegistrationRequest userDetails);

        Task<RegistrationResponse> UserLogin(LoginRequest loginDetails);

        Task<RegistrationResponse> ForgotPassword(ForgotPasswordRequest forgotPassword);

        Task<bool> ResetPassword(int userID, ResetPasswordRequest resetPassword);
    }
}
