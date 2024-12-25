using Walks.Api.Model.Domain;

namespace Walks.Api.Repos
{
    public interface IImageRepository
    {
       public Task<Image> UploadAsync(Image image); 
    }
}
