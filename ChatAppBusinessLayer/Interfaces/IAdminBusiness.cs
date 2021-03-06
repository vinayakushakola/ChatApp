﻿//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : Interface of Admin Business
//

using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using System.Threading.Tasks;

namespace ChatAppBusinessLayer.Interfaces
{
    public interface IAdminBusiness
    {
        Task<RegistrationResponse> AdminRegistration(AdminRegistrationRequest adminDetails);

        Task<RegistrationResponse> AdminLogin(AdminLoginRequest loginDetails);
    }
}
