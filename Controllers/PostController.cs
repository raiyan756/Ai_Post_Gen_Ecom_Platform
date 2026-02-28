using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/posts")]
public class PostController: ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequestDtos createPostRequestDtos)
    {
        var results  = await _postService.CreatePostAsync(createPostRequestDtos);
        return Ok(results);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPost(Guid id)
    {
        var post = await _postService.GetByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostRequestDtos updatePostRequestDtos)
    {
        try
        {
            var updatedPost = await _postService.UpdatePostAsync(id, updatePostRequestDtos);
            return Ok(updatedPost);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        var deleted = await _postService.DeletePostAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok("Post deleted successfully.");
    }
    [HttpGet]
    public async Task<IActionResult> GetAllPosts([FromQuery] PaginationQueryDto paginationQueryDto)
    {
        var pagedPosts = await _postService.GetAllAsync(paginationQueryDto);
        return Ok(pagedPosts);  

    }

}