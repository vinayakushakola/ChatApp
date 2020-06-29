//
// Author   : Vinayak Ushakola
// Date     : 29 June 2020
// Purpose  : It Contains Admin Response Models
//

using System;

namespace ChatAppCommonLayer.ResponseModels
{
    public class RegistrationResponse
    {
        public int AdminID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string UserRole { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
