using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ghsaa.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.IO;
using System.Drawing;

namespace Ghsaa.Controllers
{
    public class MyUserProfilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        AccountController controller = new AccountController();
        // GET: MyUserProfiles
        [Authorize]
        public ActionResult Index()
        {
            var id = getCurrentUserId();
            Debug.WriteLine("User name"+id);
         //   var name = ApplicationUser.Name;
            var user = db.Users.SingleOrDefault(u => u.Id == id);
            var profile = db.userPrfileInfos.Single(p => p.Id == user.userProfile.Id);
            return View(profile);//db.userPrfileInfo.ToList());
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
       
        // GET: MyUserProfiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyUserProfile myUserProfile = db.userPrfileInfos.Find(id);
            if (myUserProfile == null)
            {
                return HttpNotFound();
            }
            return View(myUserProfile);
        }

        // GET: MyUserProfiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyUserProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Photo")] MyUserProfile myUserProfile)
        {
            if (ModelState.IsValid)
            {
                db.userPrfileInfos.Add(myUserProfile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(myUserProfile);
        }

        // GET: MyUserProfiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyUserProfile myUserProfile = db.userPrfileInfos.Find(id);
            if (myUserProfile == null)
            {
                return HttpNotFound();
            }
            return View(myUserProfile);
        }
        
        public ActionResult Confirm(string ConfirmedEmail)
        {
            var id = getCurrentUserId();
            Debug.WriteLine("User name" + id);
            //   var name = ApplicationUser.Name;
            var user = db.Users.SingleOrDefault(u => u.Id == id);
            var profile = db.userPrfileInfos.Single(p => p.Id == user.userProfile.Id);
            return RedirectToAction("Edit", "MyUserProfiles", new { id = user.userProfile.Id }); //iew(profile);//db.userPrfileInfo.ToList());
       

        }
        //public ActionResult Edit(MyUserProfile myUserProfile
        //    )
        //{
        //    //if (id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}
        //    //MyUserProfile myUserProfile = db.userPrfileInfos.Find(id);
        //    if (myUserProfile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(myUserProfile);
        //}

        // POST: MyUserProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Photo")] 
            MyUserProfile myUserProfile, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                
              //  string pic = System.IO.Path.GetFileName(file.FileName);
                byte[] bytes=null;
                if(file!=null&&file.ContentLength>0)
                    return StreamAndSavePic(myUserProfile, file, ref bytes);
                else
                {
                    using (Image image = Image.FromFile(Server.MapPath("~/Images/face.png")))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            string base64String = Convert.ToBase64String(imageBytes);
                            string UrlString = string.Format("data:image/png;base64,{0}", base64String);
                            myUserProfile.Photo = UrlString;
                            db.Entry(myUserProfile).State = EntityState.Modified;
                            db.SaveChanges();
                         //   return View(myUserProfile);//
                    return         RedirectToAction("Index", "MyUserProfiles");

                        }
                    }

                }
            }
            return View(myUserProfile);
        }

        private ActionResult StreamAndSavePic(MyUserProfile myUserProfile, HttpPostedFileBase file, ref byte[] bytes)
        {
            using (
                Stream inputStream = file.InputStream)
            {
                MemoryStream memStream = inputStream as MemoryStream;
                if (memStream == null)
                {
                    memStream = new MemoryStream();
                    inputStream.CopyTo(memStream);
                }
                bytes = memStream.ToArray();
                string base64String = Convert.ToBase64String(bytes);
                string UrlString = string.Format("data:image/png;base64,{0}", base64String);
                myUserProfile.Photo = UrlString;
                db.Entry(myUserProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        // GET: MyUserProfiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyUserProfile myUserProfile = db.userPrfileInfos.Find(id);
            if (myUserProfile == null)
            {
                return HttpNotFound();
            }
            return View(myUserProfile);
        }

        // POST: MyUserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyUserProfile myUserProfile = db.userPrfileInfos.Find(id);
            db.userPrfileInfos.Remove(myUserProfile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
