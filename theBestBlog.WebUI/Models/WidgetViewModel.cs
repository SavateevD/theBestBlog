using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using theBestBlog.Domain;
using theBestBlog.Domain.Objects;

namespace theBestBlog.WebUI.Models
{
    public class WidgetViewModel
    {
        public WidgetViewModel(IBlogRepository _blogRepository)
        {
            Categories = _blogRepository.GetAllCategories().ToList();
            Tags = _blogRepository.GetAllTags().ToList();
            LatestPosts = _blogRepository.Posts(0, 10).ToList();
        }

        public IList<Category> Categories { get; private set; }
        public List<Tag> Tags { get; private set; }
        public List<Post> LatestPosts { get; private set; }
    }

}