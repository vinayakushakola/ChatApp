//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : Interface of Chat Business
//

using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatAppBusinessLayer.Interfaces
{
    public interface IChatBusiness
    {
        Task<List<ChatResponse>> GetAllMessages();

        Task<ChatResponse> SendMessage(int userID, ChatRequest chatRequest);
    }
}
