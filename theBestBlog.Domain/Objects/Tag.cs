using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace theBestBlog.Domain.Objects
{
    public class Tag
    {
        public int Id
        { get; set; }

        public string Name
        { get; set; }

        public string UrlSlug
        { get; set; }

        public string Description
        { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public IList<Post> Posts
        { get; set; }
    }
}
