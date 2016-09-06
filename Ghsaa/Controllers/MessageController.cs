using Ghsaa.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace Ghsaa.Controllers
{
    public class MessageController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpPost]
          [Authorize]
        public ActionResult PostMessage(MessageReplyViewModel vm)
        {
            var username = User.Identity.Name;
            string fullName = "";
            int msgid = 0;
            if (!string.IsNullOrEmpty(username))
            {
               // var user = db.Users.SingleOrDefault(u => u.UserName == username);
                var id = getCurrentUserId();
                Debug.WriteLine("User name" + id);
                //   var name = ApplicationUser.Name;
                var user = db.Users.SingleOrDefault(u => u.Id == id);
                var profile = db.userPrfileInfos.Single(p => p.Id == user.userProfile.Id);
            

                fullName = string.Concat(new string[] {profile.FirstName, " ", profile.LastName });
            }
            Message messagetoPost = new Message();
            if (vm.Message.Subject != string.Empty && vm.Message.MessageToPost != string.Empty)
            {
                messagetoPost.DatePosted = DateTime.Now;
                messagetoPost.Subject = vm.Message.Subject;
                messagetoPost.MessageToPost = vm.Message.MessageToPost;
                messagetoPost.From = fullName;

                db.Messages.Add(messagetoPost);
                db.SaveChanges();
                msgid = messagetoPost.Id;
            }

            return RedirectToAction("Index", "Message", new { Id = msgid });
        }

        public ActionResult Create()
        {
            MessageReplyViewModel vm = new MessageReplyViewModel();

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ReplyMessage(MessageReplyViewModel vm, int messageId)
        {
            var username = User.Identity.Name;

            //var id = getCurrentUserId();
          //  Debug.WriteLine("User name" + id);
            //   var name = ApplicationUser.Name;
           // var user = dbContext.Users.SingleOrDefault(u => u.Id == id);
           // var profile = db.userPrfileInfos.Single(p => p.Id == user.userProfile.Id);
            
            string fullName = "";
            if (!string.IsNullOrEmpty(username))
            {
                var id = getCurrentUserId();
                Debug.WriteLine("User name" + id);
                //   var name = ApplicationUser.Name;
                var user = db.Users.SingleOrDefault(u => u.Id == id);
                var profile = db.userPrfileInfos.Single(p => p.Id == user.userProfile.Id);
            
             //   var user = db.Users.SingleOrDefault(u => u.UserName == username);
                fullName =  string.Concat(new string[] { profile.FirstName, " ",profile.LastName });
            }
            if (vm.Reply.ReplyMessage != null)
            {
                Reply _reply = new Reply();
                _reply.ReplyDateTime = DateTime.Now;
                _reply.MessageId = messageId;
                _reply.ReplyFrom = fullName;
                _reply.ReplyMessage = vm.Reply.ReplyMessage;
                db.Replies.Add(_reply);
                db.SaveChanges();
            }
            //reply to the message owner          - using email template

        /*    var messageOwner = dbContext.Messages.Where(x => x.Id == messageId).Select(s => s.From).FirstOrDefault();
            var users = from user in dbContext.Users
                        orderby user.FirstName
                        select new
                        {
                            FullName = user.FirstName + " " + user.LastName,
                            UserEmail = user.Email
                        };

            var uemail = users.Where(x => x.FullName == messageOwner).Select(s => s.UserEmail).FirstOrDefault();
            SendGridMessage replyMessage = new SendGridMessage();
            replyMessage.From = new MailAddress(username);
            replyMessage.Subject = "Reply for your message :" + dbContext.Messages.Where(i => i.Id == messageId).Select(s => s.Subject).FirstOrDefault();
            replyMessage.Text = vm.Reply.ReplyMessage;


            replyMessage.AddTo(uemail);


            var credentials = new NetworkCredential("YOUR SENDGRID USERNAME", "PASSWORD");
            var transportweb = new Microsoft.Web(credentials);
            transportweb.DeliverAsync(replyMessage);*/
            return RedirectToAction("Index", "Message", new { Id = messageId });

        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteMessage(int messageId)
        {
            Message _messageToDelete = db.Messages.Find(messageId);
            db.Messages.Remove(_messageToDelete);
            db.SaveChanges();

            // also delete the replies related to the message
            var _repliesToDelete = db.Replies.Where(i => i.MessageId == messageId).ToList();
            if (_repliesToDelete != null)
            {
                foreach (var rep in _repliesToDelete)
                {
                    db.Replies.Remove(rep);
                    db.SaveChanges();
                }
            }


            return RedirectToAction("Index", "Message");
        }
        // GET: Message
      //  [Authorize]
        public ActionResult Index(int? Id, int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            MessageReplyViewModel vm = new MessageReplyViewModel();
            var count = db.Messages.Count();

            decimal totalPages = count / (decimal)pageSize;
            ViewBag.TotalPages = Math.Ceiling(totalPages);
            vm.Messages = db.Messages
                                       .OrderBy(x => x.DatePosted).ToPagedList(pageNumber, pageSize);
            ViewBag.MessagesInOnePage = vm.Messages;
            ViewBag.PageNumber = pageNumber;

            if (Id != null)
            {

                var replies = db.Replies.Where(x => x.MessageId == Id.Value).OrderByDescending(x => x.ReplyDateTime).ToList();
                if (replies != null)
                {
                    foreach (var rep in replies)
                    {
                        MessageReplyViewModel.MessageReply reply = new MessageReplyViewModel.MessageReply();
                        reply.MessageId = rep.MessageId;
                        reply.Id = rep.Id;
                        reply.ReplyMessage = rep.ReplyMessage;
                        reply.ReplyDateTime = rep.ReplyDateTime;
                        reply.MessageDetails = db.Messages.Where(x => x.Id == rep.MessageId).Select(s => s.MessageToPost).FirstOrDefault();
                        reply.ReplyFrom = rep.ReplyFrom;
                        vm.Replies.Add(reply);
                    }

                }
                else
                {
                    vm.Replies.Add(null);
                }


                ViewBag.MessageId = Id.Value;
            }

            return View(vm);
        }

        string getCurrentUserId()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    var userIdValue = userIdClaim.Value;
                    return userIdValue;
                }
            }
            return null;
        }
    }
}