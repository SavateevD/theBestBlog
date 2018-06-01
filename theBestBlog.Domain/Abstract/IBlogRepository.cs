using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using theBestBlog.Domain.Objects;

namespace theBestBlog.Domain
{
    public interface IBlogRepository
    {
        IEnumerable<Post> GetAllPosts();
        IEnumerable<Post> Posts(int pageNo, int pageSize);
        int TotalPosts(bool checkIsPublished = true);
        IEnumerable<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize);
        int TotalPostsForCategory(string categorySlug);
        Category Category(string categorySlug);
        IEnumerable<Post> PostsForTag(string tagSlug, int pageNo, int pageSize);
        int TotalPostsForTag(string tagSlug);
        Tag Tag(string tagSlug);
        IEnumerable<Post> PostsForSearch(string search, int pageNo, int pageSize);
        int TotalPostsForSearch(string search);
        Post Post(int year, int month, string titleSlug);
        Post PostById(int Id);
        IEnumerable<Category> GetAllCategories();
        IEnumerable<Tag> GetAllTags();

        #region jqGrid
        IList<Post> Posts(int pageNo, int pageSize, string sortColumn,
                            bool sortByAscending);

        #endregion

        void AddOrUpdatePost(Post post);
        void Delete(int id);
    }
}
