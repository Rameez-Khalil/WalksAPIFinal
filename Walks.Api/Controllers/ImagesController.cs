using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Walks.Api.Model.Domain;
using Walks.Api.Model.DTOs;
using Walks.Api.Repos;

namespace Walks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")] //api/images/Upload
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto)
        {


            ValidateExtension(imageUploadRequestDto);

            //if valid model.
            if (ModelState.IsValid)
            {
                //convert the DTO into the domain model.
                var imageDomainodel = new Image
                {
                    File = imageUploadRequestDto.File,
                    FileName = imageUploadRequestDto.FileName,
                    FileDescription = imageUploadRequestDto.FileDescription,
                    FileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName),
                    FileSizeInBytes = imageUploadRequestDto.File.Length,
                };

                await imageRepository.UploadAsync(imageDomainodel);
                return Ok(imageDomainodel); 
            }





            return BadRequest(ModelState);


        }


        //validating the file extension.
        private void ValidateExtension(ImageUploadRequestDto imageUploadRequestDto)
        {
            //allowed extensions.
            var extensions = new List<String> { ".jpg", ".jpeg", ".png" };

            //get the extension of the file.
            if (!extensions.Contains(Path.GetExtension(imageUploadRequestDto.File.FileName)))
            {
                //invalidate the model.
                ModelState.AddModelError("file", "Unsupported format");
            }

            //check for the size.
            if (imageUploadRequestDto.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size too large");
            }





        }
    }
        

}
        


    

