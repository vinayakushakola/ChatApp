//
// Author    : Vinayak Ushakola
// Date      : 29 June 2020
// Purpose   : It Contains Method which add chat details into the database
//

using ChatAppCommonLayer.RequestModels;
using ChatAppCommonLayer.ResponseModels;
using ChatAppRepositoryLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ChatAppRepositoryLayer.Services
{
    public class ChatRepository : IChatRepository
    {
        private readonly IConfiguration _configuration;
        private SqlConnection conn;

        public ChatRepository(IConfiguration configuration)
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
        /// Add Message data into the database
        /// </summary>
        /// <param name="userID">UserID</param>
        /// <param name="chatRequest">Chat Data</param>
        /// <returns>If Data Added return Response Data else null or Exception</returns>
        public async Task<ChatResponse> SendMessage(int userID, ChatRequest chatRequest)
        {
            try
            {
                ChatResponse responseData = null;
                SQLConnection();
                using (SqlCommand cmd = new SqlCommand("AddMessageData", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Message", chatRequest.Message);
                    cmd.Parameters.AddWithValue("@ReceiverID", chatRequest.ReceiverID);

                    conn.Open();
                    SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        responseData = new ChatResponse
                        {
                            MessageID = Convert.ToInt32(dataReader["MessageID"]),
                            UserID = Convert.ToInt32(dataReader["UserID"]),
                            Message = dataReader["Message"].ToString(),
                            ReceiverID = Convert.ToInt32(dataReader["ReceiverID"])
                        };
                    }
                };
                return responseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
