using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Profile;
using Ghsaa.Models;
using System.Xml;
using System.ServiceModel.Syndication;
//using System.ServiceModel.Syndication;
namespace Ghsaa.Controllers
{

    //[RequireHttps]
    public class HomeController : Controller
    {

        public ActionResult News()
        {
            var model = new HNews();
            string strFeed = "http://www.chron.com/rss/feed/Technology-289.php";// "http://rssfeeds.khou.com/khou/local";
            // "http://www.click2houston.com/feeds/rssFeed/feedServlet";//?obfType=GMG_RSS_DETAIL&siteId=800003&categoryId=80045&nbRows=20&FeedFetchDays=10&includeFeeds=True";
            using (XmlReader reader = XmlReader.Create(strFeed))
            {
                SyndicationFeed rssData = SyndicationFeed.Load(reader);
                model.HoustonFeed = rssData; ;
            }
            return View(model);


        }
        public ActionResult Donate()
        {
            return View();
        }
      
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
          public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

            public ActionResult FileUpload(HttpPostedFileBase file)
    {
        if (file != null)
        {
            string pic = System.IO.Path.GetFileName(file.FileName);
            string path = System.IO.Path.Combine(
                                   Server.MapPath("~/Images/profile"), pic); 
            // file is uploaded
            file.SaveAs(path);

            // save the image path path to the database or you can send image 
            // directly to database
            // in-case if you want to store byte[] ie. for DB
            using (MemoryStream ms = new MemoryStream()) 
            {
                 file.InputStream.CopyTo(ms);
                 byte[] array = ms.GetBuffer();
            }

        }
        // after successfully uploading redirect the user
        return RedirectToAction("actionname", "controller name");
    }
    }
}