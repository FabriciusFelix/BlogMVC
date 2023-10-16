
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return Ok("This is the get images API Call");

        //}
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            //Call a repository 
            var imageURL = await _imageRepository.UploadAsync(file);
            if (imageURL == null)
            {
                return Problem("Something went wrong!",null,(int)HttpStatusCode.InternalServerError,imageURL);
            }
            return new JsonResult(new { link = imageURL });

        }
    }
}
