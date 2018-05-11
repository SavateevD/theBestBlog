using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using theBestBlog.Domain.Objects;

namespace theBestBlog.Domain.Concrete
{
    public class BlogRepository : IBlogRepository
    {
        private EFDBContext context = new EFDBContext();

        public IEnumerable<Post> Posts(int pageNo, int pageSize)
        {
            var posts = context.Posts
                              .Where(p => p.Published).Include(p => p.Category).Include(p => p.Tags)
                              .OrderByDescending(p => p.PostedOn)
                              .Skip(pageNo * pageSize)
                              .Take(pageSize)
                              .ToList();
            return posts;
        }
        public int TotalPosts()
        {
            int Count = context.Posts.Count();
            return Count;
        }

        #region PostsForPostsForCategory
        public IEnumerable<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize)
        {
            var posts = context.Posts
                                .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                                .Include(p => p.Category).Include(p => p.Tags)
                                .OrderByDescending(p => p.PostedOn)
                                .Skip(pageNo * pageSize)
                                .Take(pageSize)
                                .ToList();
            return posts;
        }

        public int TotalPostsForCategory(string categorySlug)
        {
            return context.Posts
                        .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                        .Count();
        }

        public Category Category(string categorySlug)
        {
            return context.Categories
                        .FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));
        }
#endregion

        #region PostsForTag
        public IEnumerable<Post> PostsForTag(string tagSlug, int pageNo, int pageSize)
        {
            var posts = context.Posts
                              .Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                              .Include(p => p.Category).Include(p => p.Tags)
                              .OrderByDescending(p => p.PostedOn)
                              .Skip(pageNo * pageSize)
                              .Take(pageSize)
                              .ToList();
            return posts;
        }

        public int TotalPostsForTag(string tagSlug)
        {
            return context.Posts
                        .Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                        .Count();
        }

        public Tag Tag(string tagSlug)
        {
            return context.Tags
                        .FirstOrDefault(t => t.UrlSlug.Equals(tagSlug));
        }
        #endregion

        #region Search
        public IEnumerable<Post> PostsForSearch(string search, int pageNo, int pageSize)
        {
            var posts = context.Posts
                                  .Where(p => p.Published && (p.Title.Contains(search) || p.Category.Name.Equals(search) || p.Tags.Any(t => t.Name.Equals(search))))
                                  .Include(p => p.Category).Include(p => p.Tags)
                                  .OrderByDescending(p => p.PostedOn)
                                  .Skip(pageNo * pageSize)
                                  .Take(pageSize)
                                  .ToList();
            return posts;
        }

        public int TotalPostsForSearch(string search)
        {
            return context.Posts
                    .Where(p => p.Published && (p.Title.Contains(search) || p.Category.Name.Equals(search) || p.Tags.Any(t => t.Name.Equals(search))))
                    .Count();
        }
        #endregion

        #region SinglePost
        public Post Post(int year, int month, string titleSlug)
        {
            var post = context.Posts
                                .Where(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug))
                                .Include(p => p.Category).Include(p => p.Tags)
                                .Single();
            return post;
        }

        #endregion

#region SideBars
        public IEnumerable<Category> GetAllCategories()
        {
            return context.Categories.OrderBy(p => p.Name).ToList();
        }
        public IEnumerable<Tag> GetAllTags()
        {
            return context.Tags.OrderBy(p => p.Name).ToList();
        }
        #endregion

    }


}
