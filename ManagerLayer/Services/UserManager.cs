﻿using CommonLayer;
using ManagerLayer.Interface;
using MassTransit;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository userrepository;



        public UserManager(IUserRepository userrepository , IBusControl bus)
        {
            this.userrepository = userrepository;


        }

        public UserModel AddUsers(UserModel users)
        {
            return userrepository.AddUsers(users);
        }

        public UserModel Login(UserLogin login)
        {
            return userrepository.Login(login); 
        }

        public string GenerateJWToken(string Email, int UserID)
        {
            return userrepository.GenerateJWToken(Email,UserID);
        }

        public UserModel ForgetPassword(UserForgetPassword forgetPassword)
        {
            return userrepository.ForgetPassword(forgetPassword);
        }

        public bool ResetPassword(UserResetPassword userResetPassword, string Email)
        {
            return userrepository.ResetPassword(userResetPassword, Email);
        }

        public string SendGmail(string to, int UserID)
        {
            /*string to = "fagebi2588@anwarb.com";*/ //To address    
            try
            {
                string token = GenerateJWToken(to ,UserID);

                string from = "pankaj080519999@gmail.com"; //From address    
                MailMessage message = new MailMessage(from, to);

                string mailbody = "TOKEN GENERATED : " + token;
                message.Subject = "Your Secret Token Generated";
                message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential("pankaj080519999@gmail.com", "zpxpakwjxzcxwuqf");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                client.Send(message);

                return to;

            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

    }


}
