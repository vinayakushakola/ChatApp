//
// Author   : Vinayak Ushakola
// Date     : 29 June 2020
// Purpose  : It Contains Model used for sending a message
//

namespace ChatAppCommonLayer.RequestModels
{
    public class ChatRequest
    {
        public string Message { get; set; }

        public int ReceiverID { get; set; }
    }
}
