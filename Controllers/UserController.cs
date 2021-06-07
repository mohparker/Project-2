using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Registration_with_email_validation.Controllers;
using Registration_with_email_validation.Models;

namespace Registration_with_email_validation.Controllers
{
    public class UserController : Controller
    {
      
        //Registration Action
     
       [HttpGet]
        public ActionResult Registration()
        {

            return View();

        }

        //Registration Post
        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVarified, ActivationCode")] User user) //This code will prevent the user from making many attempts when verifying the password and automatically assign unique activation code
        {
            bool Status = false;
            string message = "";


            //

            //Model validation
            if (ModelState.IsValid)
            {

                #region Email is already taken
                var IsExist = IsEmailExist(user.EmailID);
                if (IsExist)
                {

                    ModelState.AddModelError("EmailExist", "Email is already taken");
                    return View(user);

                }
                #region Generation activation code

                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //This line of code avoid confirm password that does not match and so on

                #endregion

                #endregion
               
                // Email verification
                user.IsEmailVerified = false;

                // Save the details to our database
                using (myDatabaseEntities dc = new myDatabaseEntities())
                {

                    dc.Users.Add(user);
                    dc.SaveChanges(); //This code will save changes when user makes on his/her data

                    //Send Email to user
                    SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    message = "Registration successfull done. Acount activation link" +
                        "has been sent to your email id:" + user.EmailID;

                    Status = true;


                }



            }
            else
            {
                message = " Invalid request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;

            return View(user);

        }

 
        //Verify Email



        //Verify Email Link


        //Login


        //Login Post


        //Logout
        [NonAction]
        public bool IsEmailExist(string emailID) //This method will check if the emailID exist in the database or not
        {
            using (myDatabaseEntities dc = new myDatabaseEntities())
            {

                var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();

                return v != null;

            }

        }

        [NonAction]

        public void SendVerificationLinkEmail(string emailID, string activationCode) //This method will send the verification link to the user through his/her email address provided
        {

            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new System.Net.Mail.MailAddress("dotnetawesome@gmail.com", "Donet Awesome");
            var toEmail = new System.Net.Mail.MailAddress(emailID);
            var fromEmailPassword = "************"; //Replace with actiual password
            string subjerct = "Your account is successfully created. Welcome";

            string body = "<br/><br/>We are excited to tell you that your Dontnet Awesome account is" +
                "successfully created. Please click on the below link to verify your account" +
                "<a href='" + link + "'> " + link + "</a> ";



            var smtp = new SmtpClient //Verifying the email of the user and configure the email
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {

                Subject = subjerct,
                Body = body,
                IsBodyHtml = true,



            }) 

                smtp.Send(message); //This line of code is there to to send, receive, and relay outgoing emails between senders and receivers. When an email is sent, it's transferred over the internet from one server to another using SMTP.



        }
    }
}











