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
using System.Xml.Linq;
using System.Diagnostics;
//using System.ServiceModel.Syndication;
namespace Ghsaa.Controllers
{


    //[RequireHttps]
    public class HomeController : Controller
    {

        bool IsVideo(string mimeString)
        {
              
            string[] mimeArray ={           "video/x-flv",
        "video/mp4",
	"application/x-mpegURL",
	"video/MP2T",
	"video/3gpp",
        "video/quicktime",
	"video/x-msvideo",
	"video/x-ms-wmv"};
            for (int i = 0; i < mimeArray.Length;i++ )
            {
                if (mimeArray[i] == mimeString)
                    return true; 
            }
            return false;
        }
         public ActionResult UploadFiles()
        { return View(); }

        [HttpPost]
        public ContentResult UploadFiles(string t,string d)
        {
            var r = new List<UploadFileResult>();
          
                   // string savedFileName = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(hpf.FileName));
            string  savedFileName=null;
            string path = null;
            HttpPostedFileBase hpf = null;
                foreach (string file in Request.Files)
                {
                     hpf = Request.Files[file] as HttpPostedFileBase;
                    if (hpf.ContentLength == 0)
                        continue;
                   // if(IsVideo(hpf.ContentType))
                     savedFileName = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(hpf.FileName));
                    hpf.SaveAs(savedFileName); // Save the file

                    r.Add(new UploadFileResult()
                    {
                        Name = hpf.FileName,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType
                    });
                    Video v = new Video();
                    v.Title = t;
                    v.Description = d;
                    v.UploadDate = DateTime.Now;
                    v.Picture = hpf.FileName;// Path.Combine(Server.MapPath("~/Uploads/"), file.FileName);
                    v.TypeOfVideo = hpf.ContentType;
                    System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(Video));
                    if (IsVideo(hpf.ContentType))
                        path = Path.Combine(Server.MapPath("~/VideoXml/"), "Videos.xml");
                    else {
                        path = Path.Combine(Server.MapPath("~/VideoXml/"), "Photos.xml");
                    }

                    XDocument doc = XDocument.Load(path);
                    XElement[] param ={ new XElement("Title", v.Title), new XElement("Description",v.Description),
                                        new XElement("UploadDate",  DateTime.Now.ToLongDateString()),
                                     new XElement("Picture",v.Picture),new XElement("ContentType",v.TypeOfVideo)};
                    doc.Root.Add(new XElement("Video", param));
                    doc.Save(path);

                }
            //xml
    
            // Returns json
            return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");
        }

        public ActionResult HubTest()
        {
            return View();
        }
        public ActionResult Video()
        {

            return View();

        }
        [HttpPost]
        public ActionResult Video(HttpPostedFileBase file)
        {
            //string pic = System.IO.Path.GetFileName(file.FileName);
            //file.SaveAs(Path.Combine("~/Uploads/",pic));
                
            Debug.WriteLine(Path.Combine("~/Uploads/","pic"));
            foreach (string upload in Request.Files)
            {

                string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
                string filename = Path.GetFileName(Request.Files[upload].FileName);
                Request.Files[upload].SaveAs(Path.Combine(path, filename));
                Debug.WriteLine("name of file" + filename);
            }
            return View();
        }
        public ActionResult test(string adrs,string t)
        {
            List<string> video = new List<string>();
            video.Add(adrs); video.Add(t);
            video.Add(Request.Browser.Browser);
            
           
            return View(video);
        }
        public ActionResult UploadVideo()
        { return View(); }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult UploadVideo([Bind(Include="Title,Description")]Video v)//, HttpPostedFileBase file)
        {
       // List<List<string>> ls=    getVideos();
            string UrlString = null;

          //  if (  file.ContentLength>0)//  Request.Files.Count > 0)
            {
             try{ 
                // string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
                //string filename = Path.GetFileName(file.FileName);
                //file.SaveAs(Path.Combine(path, filename));
                               

            //      v.UploadDate = DateTime.Now;
            //      v.Picture = file.FileName;// Path.Combine(Server.MapPath("~/Uploads/"), file.FileName);
            //      v.TypeOfVideo = file.ContentType;
            //        System.Xml.Serialization.XmlSerializer writer =
            //new System.Xml.Serialization.XmlSerializer(typeof(Video));

            //        path = Path.Combine(Server.MapPath("~/VideoXml/") , "Videos.xml");
                 
            //        XDocument doc = XDocument.Load(path);
            //        XElement[] param={ new XElement("Title", v.Title), new XElement("Description",v.Description),
            //                            new XElement("UploadDate",  DateTime.Now.ToLongDateString()),
            //                         new XElement("Picture",v.Picture),new XElement("ContentType",v.TypeOfVideo)};
            //        doc.Root.Add(new XElement("Video", param));
            //        doc.Save(path);
            //        return Json("File Uploaded Successfully!");

                 List<string> Td=new List<string> ();
                 Td.Add (v.Title); Td.Add(v.Description);
                 TempData["t"]=v.Title ;
                 TempData["d"] = v.Description;
                 return RedirectToAction("UploadFiles", "Home", new { t=v.Title,d=v.Description});
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            //else
            //{

    
            //    return Json("No files selected.");
            //}  


        }


        public ActionResult DiplayVideos()
        {

          List<List<string>> videos=  getVideos(true);
            return View(videos);

        }
        public ActionResult DiplayPhotos()
        {

            List<List<string>> videos = getVideos(false);
            return View(videos);

        }

        private List<List<string>> getVideos(bool v)
        {
            string path =null;
                
              if(v)  
               path = Path.Combine(Server.MapPath("~/VideoXml/"), "Videos.xml");
              else { path = Path.Combine(Server.MapPath("~/VideoXml/"), "Photos.xml"); }
            XDocument doc =   XDocument.Load(path);
            IEnumerable<XElement> videos =
                from el in doc.Elements()
                select el;

            List<string> videoList = new List<string>();
            List<List<string>> videoListList = new List<List<string>>();
            foreach (var video in doc.Root.Elements())
            {
                videoList = new List<string>();
                foreach (var c in video.Elements())
                  
                    videoList.Add(c.Value);
            
                videoListList.Add(videoList);

            }
            return videoListList;
        }
        public ActionResult News()
        {
            var model = new HNews();
            string strFeed ="http://rssfeeds.khou.com/khou/local";// "http://www.chron.com/rss/feed/Technology-289.php";// "http://rssfeeds.khou.com/khou/local";
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