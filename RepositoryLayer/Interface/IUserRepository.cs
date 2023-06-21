using CommonLayer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IUserRepository
    {
        public UserModel AddUsers(UserModel users);
        public UserModel Login(UserLogin login);
        public UserModel ForgetPassword(UserForgetPassword forgetPassword);
        public string GenerateJWToken(string Email, int UserID);
    }
    
}
