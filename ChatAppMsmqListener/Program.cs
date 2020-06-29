//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : It starts the Msmq Listener
//

namespace ChatAppMsmq
{
    class Program
    {
        static void Main()
        {
            string path = @".\Private$\ChatAppQueue";
            MsmqListener msmqListener = new MsmqListener(path);
            msmqListener.Start();
        }
    }
}
