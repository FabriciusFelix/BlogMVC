using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //get Tags from repository
            var tags = await _tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
           

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //map the view model to domain model
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                PublishDate = addBlogPostRequest.PublishDate,
                UrlHandle = addBlogPostRequest.UrlHandle,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
               
            };
            //Map tags from selected tags
            var selectedTags = new List<Tag>();
            foreach ( var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await _tagRepository.GetAsync(selectedTagAsGuid);
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            //Maping adding tags to domain model
            blogPost.Tags = selectedTags;
            //Finally adding domain to Database
            await _blogPostRepository.AddAsync(blogPost);
            

            return RedirectToAction("Add");
        }

    }
}
