//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : It Contains Method which add admin details into the database
//

using ChatAppCommonLayer.Models;
using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using ChatAppRepositoryLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ChatAppRepositoryLayer.Services
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IConfiguration _configuration;
        private SqlConnection conn;

        private static readonly string _admin = "Admin";

        public AdminRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Sql Connection
        /// </summary>
        private void SQLConnection()
        {
            string sqlConnectionString = _configuration.GetConnectionString("ChatAppDBConnection");
            conn = new SqlConnection(sqlConnectionString);
        }

        /// <summary>
        /// Add admin details into the database
        /// </summary>
        /// <param name="adminDetails">Admin Registration Details</param>
        /// <returns>If data added successully, return response data else null or exception</returns>
        public async Task<RegistrationResponse> AdminRegistration(AdminRegistrationRequest adminDetails)
        {
            try
            {
                RegistrationResponse responseData = null;
                adminDetails.Password = EncodeDecode.EncodePasswordToBase64(adminDetails.Password);
                SQLConnection();
                using (SqlCommand cmd = new SqlCommand("AddUserDetails", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", adminDetails.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", adminDetails.LastName);
                    cmd.Parameters.AddWithValue("@Email", adminDetails.Email);
                    cmd.Parameters.AddWithValue("@Password", adminDetails.Password);
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@UserRole", _admin);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                    conn.Open();
                    SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
                    responseData = AdminRegistrationResponseModel(dataReader);
                };
                return responseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Verify Admin Login Details
        /// </summary>
        /// <param name="loginDetails">Admin Login Details</param>
        /// <returns>If data verified, return response data else ull or exception</returns>
        public async Task<RegistrationResponse> AdminLogin(AdminLoginRequest loginDetails)
        {
            try
            {
                RegistrationResponse responseData = null;
                loginDetails.Password = EncodeDecode.EncodePasswordToBase64(loginDetails.Password);
                SQLConnection();
                using (SqlCommand cmd = new SqlCommand("ValidateLogin", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", loginDetails.Email);
                    cmd.Parameters.AddWithValue("@Password", loginDetails.Password);
                    cmd.Parameters.AddWithValue("@UserRole", _admin);

                    conn.Open();
                    SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
                    responseData = AdminRegistrationResponseModel(dataReader);
                };
                return responseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Admin Registration Response Method
        /// </summary>
        /// <param name="dataReader">Sql Data Reader</param>
        /// <returns>It return Response Data or Exception</returns>
        private RegistrationResponse AdminRegistrationResponseModel(SqlDataReader dataReader)
        {
            try
            {
                RegistrationResponse responseData = null;
                while (dataReader.Read())
                {
                    responseData = new RegistrationResponse()
                    {
                        ID = Convert.ToInt32(dataReader["ID"]),
                        FirstName = dataReader["FirstName"].ToString(),
                        LastName = dataReader["LastName"].ToString(),
                        Email = dataReader["Email"].ToString(),
                        IsActive = Convert.ToBoolean(dataReader["IsActive"]),
                        UserRole = dataReader["UserRole"].ToString(),
                        CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]),
                        ModifiedDate = Convert.ToDateTime(dataReader["ModifiedDate"])
                    };
                }
                return responseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
