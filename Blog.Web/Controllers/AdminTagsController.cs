
using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly BlogDbContext _dbContext;
        private readonly ITagRepository _tagRepository;
        public AdminTagsController(BlogDbContext dbContext, ITagRepository tagInterface)
        {
            _dbContext = dbContext;
            _tagRepository = tagInterface;
        }
        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        
        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            //var name = Request.Form["name"];

            //var displayName = Request.Form["displayName"];  html: <input type="text" class="form-control" id="displayName" name="displayName" />
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            await _tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }

        
        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            var list = await _tagRepository.GetAllAsync();
            return View(list);
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            else
            {
                return View(null);
            }


        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            var updateTag = await _tagRepository.UpdateAsync(tag);
            if (updateTag != null)
            {
                //Show Sucess Notification
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }
            else
            {
                //Show error Notification
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await _tagRepository.DeleteAsync(editTagRequest.Id);

            if(deletedTag != null)
            {
                //Show Sucess Notification
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}
