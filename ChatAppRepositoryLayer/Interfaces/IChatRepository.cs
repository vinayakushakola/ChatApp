//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : Interface of Chat Repository
//

using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using System.Threading.Tasks;

namespace ChatAppRepositoryLayer.Interfaces
{
    public interface IChatRepository
    {
        Task<ChatResponse> SendMessage(int userID, ChatRequest chatRequest);
    }
}
