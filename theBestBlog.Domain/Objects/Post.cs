using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace theBestBlog.Domain.Objects
{
    public class Post
    {
        
        public int Id
        { get; set; }
        [Required(ErrorMessage ="Please enter a title")]
        public string Title
        { get; set; }
        [Required(ErrorMessage = "Please enter a ShortDescription")]
        public string ShortDescription
        { get; set; }
        [Required(ErrorMessage = "Please enter a Description")]
        public string Description
        { get; set; }
        [Required(ErrorMessage = "Please enter a Meta")]
        public string Meta
        { get; set; }
        [Required(ErrorMessage = "Please enter a UrlSlug")]
        public string UrlSlug
        { get; set; }
        
        public bool Published
        { get; set; }
        
        public DateTime PostedOn
        { get; set; }
        
        public DateTime? Modified
        { get; set; }
        
        public Category Category
        { get; set; }
        public IList<Tag> Tags
        { get; set; }
    }
}
