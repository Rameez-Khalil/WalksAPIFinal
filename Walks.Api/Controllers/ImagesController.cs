using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Walks.Api.Model.DTOs;

namespace Walks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        //POST : /api/images/upload.
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(ImageUploadRequestDto requestedImage)
        {
            ValidateFile(requestedImage);

            if (!ModelState.IsValid)
            {
                //return ok
            }

            return BadRequest(ModelState); 
        }

        //method to validate file upload.

        private void ValidateFile(ImageUploadRequestDto file) {

            //check for extensions.
            var allowedExtensions = new List<string> { ".jpeg", ".png", ".jpg" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.File.FileName))) {
                //add the model state error.
                ModelState.AddModelError("File", "Unsupported format"); 
            }

            //check for size
            if(file.File.Length> 10485760)
            {
                //add the file size error.
                ModelState.AddModelError("Size", "Size cannot be more than 10 MBs"); 
            }


        }

    }
}
