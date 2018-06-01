using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace theBestBlog.WebUI.Models
{
    public class JqGridViewModel
    {
        // no. of records to fetch
        public int rows
        { get; set; }

        // the page index
        public int page
        { get; set; }

        // sort column name
        public string sidx
        { get; set; }

        // sort order "asc" or "desc"
        public string sord
        { get; set; }
    }
}