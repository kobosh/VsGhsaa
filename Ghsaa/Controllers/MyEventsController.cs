using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ghsaa.Models;
using System.Security.Claims;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Ghsaa.Controllers
{
    public class MyEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        public ActionResult Show(int? id)
        {
           // using (var context = new BloggingContext())

           // using (db.Database.BeginTransaction())



                Dictionary<int, List<string>> Names = new Dictionary<int, List<string>>();
                List<string> DicList = null;
                var events = db.MyEvents;
                var attendees = db.Attendees;
                foreach (MyEvent ev in events)
                {
                   
                    DicList = new List<string>();
                    foreach (Attendee attnd in attendees)
                    {
                        if (attnd.MyEventId == ev.Id)
                        { DicList.Add(attnd.Name); }
                    }
                    Names.Add(ev.Id, DicList);
                }
                ViewBag.NameDic = Names;
            
            if (id == null)
            {

                return View(db.MyEvents.ToList());
            }
            else
            {
                //current user name will be added to attendees list
                string userid = getCurrentUserId();
                var user = db.Users.SingleOrDefault(u => u.Id == userid);
                var profile = db.userPrfileInfos.Single(p => p.Id == user.userProfile.Id);
                Attendee attendee = new Attendee();
                attendee.Name = profile.FirstName + " " + profile.LastName;
            //    attendee.MyEventRefId = id.Value;
             var tempEvent= db.MyEvents.Find(id.Value);
                  tempEvent.Id = id.Value;
                  attendee.MyEventId = id.Value;// tempEvent;
                   // tempEvent = null;
                db.Attendees.Add(attendee);
                string names = null;

                var atten = db.Attendees.ToList();
     db.SaveChanges();
               
                return View(db.MyEvents.ToList());

            }

        }

        private static List<string> GetAttendees(Dictionary<int, List<string>> Names,
            List<string> DicList, DbSet<Attendee> attendees, MyEvent ev)
        {
            DicList = new List<string>();
            foreach (Attendee attnd in attendees)
            {
                if (attnd.MyEventId == ev.Id)
                { DicList.Add(attnd.Name); }
            }
            Names.Add(ev.Id, DicList);
            return DicList;
        }
       
        Attendee FindAttendee()
        {
            var id = getCurrentUserId();
            var userProfile=db.Users.Find(id).userProfile;
            Attendee attendee = new Attendee();
             attendee.Name =userProfile.FirstName + " " + userProfile.LastName;
            return  attendee;
        
        
        
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
        // GET: MyEvents
      

        // GET: MyEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyEvent myEvent = db.MyEvents.Find(id);
            if (myEvent == null)
            {
                return HttpNotFound();
            }
            return View(myEvent);
        }

        // GET: MyEvents/Create
        [Authorize]
        public ActionResult Create( )
        {
            return View();
        }

        // POST: MyEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create ([Bind(Include = "Id,EventDate,EventTime,Name,Address,Description,Host")] 
            MyEvent myEvent)
        {
            if (ModelState.IsValid)
            {
                db.MyEvents.Add(myEvent);
                db.SaveChanges();
                return RedirectToAction("show");
            }

            return View(myEvent);
        }

        // GET: MyEvents/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyEvent myEvent = db.MyEvents.Find(id);
            if (myEvent == null)
            {
                return HttpNotFound();
            }
            return View(myEvent);
        }

        // POST: MyEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EventDate,Name,Address,Description,Host")] MyEvent myEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(myEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(myEvent);
        }

        // GET: MyEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyEvent myEvent = db.MyEvents.Find(id);
            if (myEvent == null)
            {
                return HttpNotFound();
            }
            return View(myEvent);
        }

        // POST: MyEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyEvent myEvent = db.MyEvents.Find(id);
            db.MyEvents.Remove(myEvent);
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
