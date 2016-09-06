using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ghsaa.Models
{
    public class Video
    {
        public Video()
        { Picture = "no"; }
        public int Id { get; set; }
       [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public  string Picture{ get; set; }
        public string TypeOfVideo { get; set; }
        public Byte[] bytes { get; set; }
    }

    public class VidoeModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        //public DateTime UploadDate { get; set; }
    }
    public class YTVideo
    {
        public YTVideo ()
        {
            Duration = 5;
        }
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string VideoId { get; set; }
        [Display(Name="Duration in Minutes")]
        public int  Duration { get; set; }
        public DateTime UploadDate { get; set; }

        //public DateTime UploadDate { get; set; }
    }
}