using Autofac.Builder;
using CommonLayer;
using ManagerLayer.Interface;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;

namespace FunDoNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;

        //1-RabbitMQ
        private readonly IBusControl _bus;


        //2-RabbitMQ
        public UserController(IUserManager userManager, IBusControl bus)
        {
            this.userManager = userManager;
            this._bus = bus;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Registration(UserModel userRegisterationModel)
        {
            try
            {
                UserModel registerationData = this.userManager.AddUsers(userRegisterationModel);

                if (registerationData != null)
                {
                    return this.Ok(new { Success = true, message = "Registration Successful", result = registerationData });
                }

                return this.BadRequest(new { success = true, message = "User Already Exists" });

            }
            catch (Exception)
            {
                return this.NotFound(new { success = false });
            }
        }

        [HttpPost("Login")]
        // [Route("Login")]
        public IActionResult Login(UserLogin userLoginModel)
        {
            try
            {
                UserModel UserLoginData = this.userManager.Login(userLoginModel);

                if (UserLoginData != null)
                {
                    string token = this.userManager.GenerateJWToken(UserLoginData.Email, UserLoginData.UserID);
                    return this.Ok(new { Success = true, message = "Login Successfull", result = token });
                }

                return this.BadRequest(new { success = true, message = "Login Failed" });

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(UserForgetPassword userForgetModel)
        {
            try
            {
                UserModel UserForgetData = this.userManager.ForgetPassword(userForgetModel);


                if (UserForgetData != null)
                {

                    this.userManager.SendGmail("fagebi2588@anwarb.com", UserForgetData.UserID);

                    Uri uri = new Uri("rabbitmq://localhost/Mail-Queue");
                    var endPoint = await _bus.GetSendEndpoint(uri);
                    await endPoint.Send(UserForgetData);


                    return this.Ok(new { Success = true, message = "Password Fetch Successfull", result = UserForgetData });
                }

                return this.BadRequest(new { success = true, message = "Fetching Failed" });

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
