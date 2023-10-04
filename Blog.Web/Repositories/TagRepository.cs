using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext _dbContext;

        public TagRepository(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {

            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync();
            return tag;
        }
        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
           return await _dbContext.Tags.ToListAsync();
        }


        public async Task<Tag?> GetAsync(Guid id)
        {
            return  await _dbContext.Tags.SingleOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var tagExists = await _dbContext.Tags.FindAsync(tag.Id);
            if (tagExists != null)
            {
                tagExists.Name = tag.Name; 
                tagExists.DisplayName = tag.DisplayName; 
                await _dbContext.SaveChangesAsync();
                // _dbContext.Tags.Update(tag);
                
                return tagExists;
            }
            return null;
            
        }
        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tag = await _dbContext.Tags.FindAsync(id);
            if (tag != null)
            {
                _dbContext.Tags.Remove(tag);
                await _dbContext.SaveChangesAsync();
                return tag;
            }
            return null;
        }
    }
}
