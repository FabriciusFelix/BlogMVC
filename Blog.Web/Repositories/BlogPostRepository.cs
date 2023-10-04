using Blog.Web.Data;
using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogDbContext _dbContext;
        public BlogPostRepository(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _dbContext.BlogPosts.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();    
            return blogPost;
        }

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
