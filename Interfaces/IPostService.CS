using System.Security.Cryptography.X509Certificates;

public interface IPostService
{
    Task<ReadPostRequestDtos> CreatePostAsync(CreatePostRequestDtos createPostRequest);
    Task<PagedResponseDtos<ReadPostRequestDtos>> GetAllAsync(PaginationQueryDto paginationQueryDto);

    Task<ReadPostRequestDtos?> GetByIdAsync(Guid id);

    Task<ReadPostRequestDtos> UpdatePostAsync(Guid id, UpdatePostRequestDtos updatePostRequest);

    Task<bool> DeletePostAsync(Guid id);
    
}