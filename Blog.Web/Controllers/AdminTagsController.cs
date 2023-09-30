
using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BlogDbContext _dbContext;
        public AdminTagsController(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            //var name = Request.Form["name"];

            //var displayName = Request.Form["displayName"];  html: <input type="text" class="form-control" id="displayName" name="displayName" />
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            _dbContext.Tags.Add(tag);
            _dbContext.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {
            var list = _dbContext.Tags.ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var tag = _dbContext.Tags.SingleOrDefault(t => t.Id == id);

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
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            var tagExists = _dbContext.Tags.SingleOrDefault(t => t.Id == tag.Id);
            if (tagExists != null)
            {
                tagExists.Name = tag.Name;  tagExists.DisplayName = tag.DisplayName; _dbContext.SaveChanges();
                // _dbContext.Tags.Update(tag);
                _dbContext.SaveChanges();
                
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }
        [HttpPost]
        public IActionResult Delete(EditTagRequest editTagRequest)
        {
            var tag = _dbContext.Tags.Find(editTagRequest.Id);
            if (tag != null)
            {
                _dbContext.Tags.Remove(tag);
                _dbContext.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}
