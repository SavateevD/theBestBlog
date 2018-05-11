using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using theBestBlog.Domain;
using theBestBlog.Domain.Objects;

namespace theBestBlog.WebUI.Models
{
    public class ListViewModel
    {
        public ListViewModel(IBlogRepository _blogRepository, int p)
        {
            Posts = _blogRepository.Posts(p - 1, 5).ToList();
            TotalPosts = _blogRepository.TotalPosts();
        }
        public ListViewModel(IBlogRepository _blogRepository, string text, string type, int p)
        {

            switch (type)
            {
                case "Category":
                    Posts = _blogRepository.PostsForCategory(text, p - 1, 5).ToList();
                    TotalPosts = _blogRepository.TotalPostsForCategory(text);
                    Category = _blogRepository.Category(text);
                    break;
                case "Tag":
                    Posts = _blogRepository.PostsForTag(text, p - 1, 10).ToList();
                    TotalPosts = _blogRepository.TotalPostsForTag(text);
                    Tag = _blogRepository.Tag(text);
                    break;
                default:
                    Posts = _blogRepository.PostsForSearch(text, p - 1, 10).ToList();
                    TotalPosts = _blogRepository.TotalPostsForSearch(text);
                    Search = text;
                    break;
                    
            }
        }
        public IList<Post> Posts { get; private set; }
        public int TotalPosts { get; private set; }
        public Category Category { get; private set; }
        public Tag Tag { get; private set; }
        public string Search { get; private set; }
    }
}