using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;
        }
        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        public async Task<IActionResult> List()
        {
            //Call the repository
            var list = await _blogPostRepository.GetAllAsync();

            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //Retrieve the result from the repository
            var blogInfo = await _blogPostRepository.GetAsync(id);
            var tags = await _tagRepository.GetAllAsync();
            

            if (blogInfo != null)
            {
                //map the domain model into the view model
                var model = new EditBlogPostRequest
                {
                    Id = blogInfo.Id,
                    Heading = blogInfo.Heading,
                    PageTitle = blogInfo.PageTitle,
                    Content = blogInfo.Content,
                    Author = blogInfo.Author,
                    FeaturedImageUrl = blogInfo.FeaturedImageUrl,
                    UrlHandle = blogInfo.UrlHandle,
                    ShortDescription = blogInfo.ShortDescription,
                    PublishDate = blogInfo.PublishDate,
                    Visible = blogInfo.Visible,
                    Tags = tags.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),

                    SelectedTags = blogInfo.Tags.Select(x => x.Id.ToString()).ToArray(),
                };
                return View(model);
            }


            //pass data to view
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //map the view model to domain model
            var blogPost = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                PublishDate = editBlogPostRequest.PublishDate,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible,

            };
            //Map tags from selected tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in editBlogPostRequest.SelectedTags)
            {
                if ( Guid.TryParse(selectedTagId, out var tagId))
                {
                    var foundTag = await _tagRepository.GetAsync(tagId);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
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
            var updatedBlog = await _blogPostRepository.UpdateAsync(blogPost);
            if (updatedBlog != null)
            {
                //Show Success notification 
                return RedirectToAction("Edit");
            }
            // Show Error notification
            return RedirectToAction("Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            //Talk to repository to delete this blog post and tags
            var blogDelete = await _blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if (blogDelete != null)
            {
                //Show success Notification
                return RedirectToAction("List");
            }

            //Show error notification
             return RedirectToAction("Edit",new { id = editBlogPostRequest.Id});
          
        }

    }
}
