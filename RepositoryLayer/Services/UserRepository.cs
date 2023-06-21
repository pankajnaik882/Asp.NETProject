using CommonLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Schema;
using Tweetinvi.Security;

namespace RepositoryLayer.Services
{
    public class UserRepository : IUserRepository
    {
        //string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=FundooNoteDB;Trusted_Connection=True;MultipleActiveResultSets=True;";
        private readonly IConfiguration config;
        public readonly string connectionString;

        public UserRepository (IConfiguration config)
        {
            this.connectionString = config.GetConnectionString("FundooDB");
            this.config = config;
            
            
        }

        //-----------------UserModel For Adding Users to Database-----------//
        public UserModel AddUsers( UserModel users )
        {

            using (SqlConnection con = new SqlConnection(this.connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_Insert_Users", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FirstName", users.FirstName);
                cmd.Parameters.AddWithValue("@LastName", users.LastName);
                cmd.Parameters.AddWithValue("@Email", users.Email);
                cmd.Parameters.AddWithValue("@Password", EncryptPasswordBase64(users.Password));

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return users;
        }

        //-------------Base64-Encryption Decryption Method----------//

        public static string EncryptPasswordBase64(String text)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(plainTextBytes);

        }

        public static string DecryptPasswordBase64(String base64EncodeData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodeData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        //-------------------------------------Login------------------------------------ 

        public UserModel Login(UserLogin login )
        { 
            UserModel userModel = new UserModel();

            using (SqlConnection con = new SqlConnection(this.connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_Login_Users", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@Email", login.Email);
                cmd.Parameters.AddWithValue("@Password", EncryptPasswordBase64(login.Password));

                con.Open();
               
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userModel.UserID = reader.IsDBNull("userID") ? 0 : reader.GetInt32("userID");
                        userModel.FirstName = reader.GetString(1);
                        userModel.LastName = reader.GetString(2);
                        userModel.Email = reader.GetString(3);
                        
                    }
                }
                con.Close();
            }  
            return userModel;
        }

        //----------------------------------Get Jwt Token------------------------------------//
        public string GenerateJWToken(string Email, int UserID)
        {

            try
            {
                var LoginsecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["Jwt:Key"]));
                var Credentials = new SigningCredentials(LoginsecurityKey, SecurityAlgorithms.HmacSha256);

                var Claims = new[]
                {
                       new Claim( "Email", Email),
                       new Claim("UserID", UserID.ToString()),
                };
                    
                var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Audience"],
                Claims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials : Credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        //--------------------------------Forget Password Method---------------------

        public UserModel ForgetPassword(UserForgetPassword forgetPassword)
        {
            UserModel userModel = new UserModel();

            using (SqlConnection con = new SqlConnection(this.connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_ForgetPassword_Users", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", forgetPassword.Email);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userModel.UserID = reader.IsDBNull("userID") ? 0 : reader.GetInt32("userID");
                        userModel.FirstName = reader.GetString(1);
                        userModel.LastName = reader.GetString(2);
                        userModel.Email = reader.GetString(3);
                        userModel.Password= DecryptPasswordBase64(reader.GetString(4));

                    }
                }
                con.Close();
            }
            return userModel;
        }

        //-----------------------------Reset Password Method--------------------------------

        public bool ResetPassword(UserResetPassword userResetPassword, string Email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this.connectionString))
                {

                    SqlCommand cmd = new SqlCommand("dbo.usp_ResetPassword_Users", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue( "@Email", Email);
                    cmd.Parameters.AddWithValue("@Password", EncryptPasswordBase64(userResetPassword.Password));

                    con.Open();

                    int ResetOrNot = cmd.ExecuteNonQuery();

                    if (ResetOrNot >= 1)
                    {
                        return true;
                    }

                    con.Close();

                }
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

    }
}
