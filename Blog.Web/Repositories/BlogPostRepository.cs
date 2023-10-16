using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

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

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var exists = await _dbContext.BlogPosts.FindAsync(id);
            if(exists != null)
            {
                 _dbContext.BlogPosts.Remove(exists);

                await _dbContext.SaveChangesAsync();
                return exists;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbContext.BlogPosts.Include(x =>x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await _dbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await _dbContext.BlogPosts.Include(x=> x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
            
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
           var existingBlog =  await _dbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync( x => x.Id == blogPost.Id);
            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.Content = blogPost.Content;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishDate = blogPost.PublishDate;
                existingBlog.Tags = blogPost.Tags;

                await _dbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }
    }
}
