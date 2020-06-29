//
// Author   : Vinayak Ushakola
// Date     : 29 June 2020
// Purpose  : It Encodes the data
//

using System;
using System.Text;

namespace ChatAppCommonLayer.Models
{
    public class EncodeDecode
    {
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
    }
}
