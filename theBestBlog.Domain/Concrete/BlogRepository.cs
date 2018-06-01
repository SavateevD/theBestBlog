using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using theBestBlog.Domain.Objects;

namespace theBestBlog.Domain.Concrete
{
    public class BlogRepository : IBlogRepository
    {
        private EFDBContext context = new EFDBContext();

        public IEnumerable<Post> GetAllPosts()
        {
            var posts = context.Posts
                  .Include(p => p.Category).Include(p => p.Tags)
                  .OrderByDescending(p => p.PostedOn)
                  .ToList();
            return posts;
        }
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
        public int TotalPosts(bool checkIsPublished = true)
        {
            // The method returns the total count of only published posts 
            // but passing false to the method returns the count of all the posts
            int Count = context.Posts
                .Where(p => !checkIsPublished || p.Published == true)
                .Count();
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
        public Post PostById(int Id)
        {
            var post = context.Posts
                                .Where(p => p.Id == Id)
                                .Include(p => p.Category).Include(p => p.Tags)
                                .FirstOrDefault();
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

        #region jqGrid
        public IList<Post> Posts(int pageNo, int pageSize, string sortColumn,
                                    bool sortByAscending)
        {
            IList<Post> posts;

            switch (sortColumn)
            {
                case "Title":
                    if (sortByAscending)
                    {
                        posts = context.Posts.Include(p => p.Category).Include(p => p.Tags)
                                        .OrderBy(p => p.Title)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .ToList();
                    }
                    else
                    {
                        posts = context.Posts.Include(p => p.Category).Include(p => p.Tags)
                                        .OrderByDescending(p => p.Title)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .ToList();
                    }
                    break;
                case "Published":
                    if (sortByAscending)
                    {
                        posts = context.Posts.Include(p => p.Category).Include(p => p.Tags)
                                         .OrderBy(p => p.Published)
                                         .Skip(pageNo * pageSize)
                                         .Take(pageSize)
                                         .ToList();
                    }
                    else
                    {
                        posts = context.Posts.Include(p => p.Category).Include(p => p.Tags)
                                        .OrderByDescending(p => p.Published)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .ToList();
                    }
                    break;
                case "PostedOn":
                    if (sortByAscending)
                    {
                        posts = context.Posts.Include(p => p.Category).Include(p => p.Tags)
                                        .OrderBy(p => p.PostedOn)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .ToList();
                    }
                    else
                    {
                        posts = context.Posts.Include(p => p.Category).Include(p => p.Tags)
                                        .OrderByDescending(p => p.PostedOn)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    }
                    break;
                case "Modified":
                    if (sortByAscending)
                    {
                        posts = context.Posts.Include(p => p.Category).Include(p => p.Tags)
                                        .OrderBy(p => p.Modified)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .ToList();
                    }
                    else
                    {
                        posts = context.Posts.Include(p => p.Category).Include(p => p.Tags)
                                        .OrderByDescending(p => p.Modified)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .ToList();
                    }
                    break;
                case "Category":
                    if (sortByAscending)
                    {
                        posts = context.Posts.Include(p => p.Category).Include(p => p.Tags)
                                        .OrderBy(p => p.Category.Name)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .ToList();
                    }
                    else
                    {
                        posts = context.Posts.Include(p => p.Category).Include(p => p.Tags)
                                        .OrderByDescending(p => p.Category.Name)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .ToList();
                    }
                    break;
                default:
                    posts = context.Posts
                                    .Where(p => p.Published)
                                    .Include(p => p.Category).Include(p => p.Tags)
                                    .OrderByDescending(p => p.PostedOn)
                                    .Skip(pageNo * pageSize)
                                    .Take(5)
                                    .ToList();
                    break;
            }

            return posts;
        }

        #endregion


        public void AddOrUpdatePost(Post post)
        {
            if (post.Id == 0)
            {
                context.Posts.Add(post);
                context.SaveChanges();
            }
            else
            {
                // context.Posts.AddOrUpdate(post); Entity.Migrations

                //var entity = context.Posts.Find(post.Id);

                var entity = context.Posts.Where(p => p.Id == post.Id).Include(p => p.Category).Include(p => p.Tags).First();
                Category c = post.Category;
                List<Tag> t = post.Tags.ToList();

                context.Entry(entity).CurrentValues.SetValues(post);
                entity.Category = c;
                entity.Tags = t;

                context.SaveChanges();
            }


        }

        public void Delete(int id)
        {
            Post post = context.Posts.Where(p => p.Id == id).FirstOrDefault();
            context.Posts.Remove(post);
            context.SaveChanges();
        }

    }


}
