using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ghsaa.Models
{
    public class MyUserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }//new
        public string LastName { get; set; }//new
        public string Photo { get; set; } //new
        /*
         * public string FirstName{ get; set; }//new
      public string LastName{ get; set; }//new
      public string Photo { get; set; } //new
         * */
    }
}