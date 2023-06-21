using CommonLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interface
{
    public interface IUserManager
    {
        public UserModel AddUsers(UserModel users);

        public UserModel Login(UserLogin login);

        public UserModel ForgetPassword(UserForgetPassword forgetPassword);

        public string GenerateJWToken(string Email, int UserID);

        public string SendGmail(string to , int UserID);
    }
}
