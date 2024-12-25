using Walks.Api.Data;
using Walks.Api.Model.Domain;

namespace Walks.Api.Repos
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly WalksDbContext dbContext;

        /*
        * we are going to create a local folder and all the images will go into that directory.
        * Iwebhosting environment can help us pulling the environment and we need to inject it inside our class.
        */
        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, WalksDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<Image> UploadAsync(Image image)
        {
            //upload.

            //get the local file path.
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

            //save.
            using var stream = new FileStream(localPath, FileMode.Create); 
            await image.File.CopyToAsync(stream);


            var path = httpContextAccessor.HttpContext.Request;

            var urlFilePath = $"{path.Scheme}://{path.Host}{path.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            //save changes to th database.
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;



        }
    }
}
