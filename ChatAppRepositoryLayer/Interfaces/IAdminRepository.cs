//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : Interface of Admin Repository
//

using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using System.Threading.Tasks;

namespace ChatAppRepositoryLayer.Interfaces
{
    public interface IAdminRepository
    {
        Task<AdminRegistrationResponse> AdminRegistration(AdminRegistrationRequest adminDetails);

        Task<AdminRegistrationResponse> AdminLogin(AdminLoginRequest loginDetails);
    }
}
