//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : It Interacts between chat controller & chatrepository
//

using ChatAppBusinessLayer.Interfaces;
using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using ChatAppRepositoryLayer.Interfaces;
using System;
using System.Threading.Tasks;

namespace ChatAppBusinessLayer.Services
{
    public class ChatBusiness : IChatBusiness
    {
        private readonly IChatRepository _chatRepository;

        public ChatBusiness(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<ChatResponse> SendMessage(int userID, ChatRequest chatRequest)
        {
            try
            {
                if (chatRequest == null)
                    return null;
                else
                    return await _chatRepository.SendMessage(userID, chatRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
