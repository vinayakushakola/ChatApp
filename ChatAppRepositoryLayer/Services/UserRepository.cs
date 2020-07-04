//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : It Contains Method which add user details into the database 
//

using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using ChatAppRepositoryLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace ChatAppRepositoryLayer.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private SqlConnection conn;

        private static readonly string _user = "User";

        public UserRepository(IConfiguration configuration)
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
        /// Fetch data from the database
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns>If Data Found return Response Data else null or Exception</returns>
        public async Task<List<RegistrationResponse>> GetListOfUsers()
        {
            try
            {
                List<RegistrationResponse> listOfUsers = new List<RegistrationResponse>();
                SQLConnection();
                using (SqlCommand cmd = new SqlCommand("GetAllUsers", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    conn.Open();
                    SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        var responseData = new RegistrationResponse
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
                        listOfUsers.Add(responseData);
                    }
                };
                return listOfUsers;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Add User Details into the database
        /// </summary>
        /// <param name="userDetails">User Registration Request</param>
        /// <returns>If data added successully, return response data else null or exception</returns>
        public async Task<RegistrationResponse> UserRegistration(RegistrationRequest userDetails)
        {
            try
            {
                RegistrationResponse responseData = null;
                SQLConnection();
                using (SqlCommand cmd = new SqlCommand("AddUserDetails", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", userDetails.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", userDetails.LastName);
                    cmd.Parameters.AddWithValue("@Email", userDetails.Email);
                    cmd.Parameters.AddWithValue("@Password", userDetails.Password);
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@UserRole", _user);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                    conn.Open();
                    SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
                    responseData = RegistrationResponseModel(dataReader);
                };
                return responseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Verify User Login Details
        /// </summary>
        /// <param name="loginDetails">Login Request</param>
        /// <returns>If data verified, return response data else null or exception</returns>
        public async Task<RegistrationResponse> UserLogin(LoginRequest loginDetails)
        {
            try
            {
                RegistrationResponse responseData = null;
                SQLConnection();
                using (SqlCommand cmd = new SqlCommand("ValidateLogin", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", loginDetails.Email);
                    cmd.Parameters.AddWithValue("@Password", loginDetails.Password);
                    cmd.Parameters.AddWithValue("@UserRole", _user);

                    conn.Open();
                    SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
                    responseData = RegistrationResponseModel(dataReader);
                };
                return responseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check email exists or not in the database
        /// </summary>
        /// <param name="forgotPassword">Forgot Passsword Request</param>
        /// <returns>If email exists, return response data else null or exception</returns>
        public async Task<RegistrationResponse> ForgotPassword(ForgotPasswordRequest forgotPassword)
        {
            try
            {
                RegistrationResponse responseData = null;
                SQLConnection();
                using (SqlCommand cmd = new SqlCommand("CheckEmailExists", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", forgotPassword.Email);

                    conn.Open();
                    SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
                    responseData = RegistrationResponseModel(dataReader);
                };
                return responseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Reset Password in the database
        /// </summary>
        /// <param name="userID">User-ID</param>
        /// <param name="resetPassword">Reset Password Data</param>
        /// <returns>If password reset successfull return true else false or Exception</returns>
        public async Task<bool> ResetPassword(int userID, ResetPasswordRequest resetPassword)
        {
            try
            {
                SQLConnection();
                using (SqlCommand cmd = new SqlCommand("ResetUserPassword", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", userID);
                    cmd.Parameters.AddWithValue("@Password", resetPassword.Password);

                    conn.Open();
                    int count = await cmd.ExecuteNonQueryAsync();
                    if (count >= 1)
                        return true;
                };
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Registration Response Method
        /// </summary>
        /// <param name="dataReader">Sql Data Reader</param>
        /// <returns>It return Response Data or Exception</returns>
        private RegistrationResponse RegistrationResponseModel(SqlDataReader dataReader)
        {
            try
            {
                RegistrationResponse responseData = null;
                while (dataReader.Read())
                {
                    responseData = new RegistrationResponse
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
