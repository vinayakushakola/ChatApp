//
// Author   : Vinayak Ushakola
// Date     : 29 June 2020
// Purpose  : It Contains Chat Response Models
//

namespace ChatAppCommonLayer.ResponseModels
{
    public class ChatResponse
    {
        public int MessageID { get; set; }

        public int UserID { get; set; }

        public string Message { get; set; }

        public int ReceiverID { get; set; }
    }
}
