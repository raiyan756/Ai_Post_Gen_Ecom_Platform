using Microsoft.EntityFrameworkCore;

public class PostServices : IPostService
{
    private readonly AppDbContext _context;

    public PostServices(AppDbContext context)
    {
        _context = context;
    }
// creating new post 
    public async Task<ReadPostRequestDtos> CreatePostAsync(CreatePostRequestDtos createPostRequest)
    {
        var post = new Post
        {
            Id = Guid.NewGuid(),
            Title = createPostRequest.Title,
            Description = createPostRequest.Description,
            Price = createPostRequest.Price,
            Location = createPostRequest.Location,
            ImageUrl = createPostRequest.ImageUrl
            
        };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return new ReadPostRequestDtos
        {
            Title = post.Title,
            Description = post.Description,
            Price = post.Price,
            Location = post.Location,
            ImageUrl = post.ImageUrl
        };

        
    }
   //Get post by id 
    public async Task<ReadPostRequestDtos?> GetByIdAsync(Guid id)
    {
        var post = await _context.Posts.FindAsync(id);
        if(post == null)
        {
            return null;
        }
        return new ReadPostRequestDtos
        {
            Title = post.Title,
            Description = post.Description,
            Price = post.Price,
            Location = post.Location,
            ImageUrl = post.ImageUrl
        };
    }
//update post by id
    public async Task<ReadPostRequestDtos> UpdatePostAsync(Guid id, UpdatePostRequestDtos updatePostRequestDtos)
    {
        var post = await _context.Posts.FindAsync(id);
        if(post == null)
        {
            throw new Exception("Post not found");
        }


        post.Title = updatePostRequestDtos.Title;
        post.Description = updatePostRequestDtos.Description;
        post.Price = updatePostRequestDtos.Price;   
        post.Location = updatePostRequestDtos.Location;
        post.CreatedAt = DateTime.UtcNow;
        post.ImageUrl = updatePostRequestDtos.ImageUrl;


        await _context.SaveChangesAsync();



        return new ReadPostRequestDtos
        {
            Title = post.Title,
            Description = post.Description,
            Price = post.Price,
            Location = post.Location,
            ImageUrl = post.ImageUrl
            
        };
    }
//delete post by id
    public async Task<bool> DeletePostAsync(Guid id)
    {
        var post = await _context.Posts.FindAsync(id);
        if(post == null)
        {
            return false;
        }
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return true;

    }

    //get all posts with pagination,sorting and filtering

    public async Task<PagedResponseDtos<ReadPostRequestDtos>> GetAllAsync(PaginationQueryDto query)
{
    // 1. Initialize the query as IQueryable to allow for dynamic SQL building
    var dbQuery = _context.Posts.AsQueryable();

    // 2. Filtering: Check if SearchTerm has a value
    if (!string.IsNullOrWhiteSpace(query.SearchTerm))
    {
        dbQuery = dbQuery.Where(p =>
            p.Title.Contains(query.SearchTerm) ||
            p.Description.Contains(query.SearchTerm) ||
            p.Location.Contains(query.SearchTerm));
    }

    // 3. Sorting: Use SortOrder and SortByCreatedAt logic
    // We check if the SortOrder is "desc", otherwise default to "asc"
    if (query.SortOrder?.ToLower() == "desc")
    {
        dbQuery = dbQuery.OrderByDescending(p => p.CreatedAt);
    }
    else
    {
        dbQuery = dbQuery.OrderBy(p => p.CreatedAt);
    }

    // 4. Count the total items AFTER filtering but BEFORE pagination
    var totalCount = await dbQuery.CountAsync();

    // 5. Pagination: Calculate Skip and Take
    // Formula: (PageNumber - 1) * PageSize
    var items = await dbQuery
        .Skip((query.PageNumber - 1) * query.PageSize) 
        .Take(query.PageSize)
        .Select(p => new ReadPostRequestDtos // Manual mapping to match your current style
        {
            Title = p.Title,
            Description = p.Description,
            Price = p.Price,
            Location = p.Location,
            ImageUrl = p.ImageUrl
        })
        .ToListAsync();

    // 6. Wrap the results in the Paged Response DTO
    return new PagedResponseDtos<ReadPostRequestDtos>
    {
       PageNumber = query.PageNumber,
       PageSize = query.PageSize,
       TotalRecords = totalCount,
       Data = items   

    };
}

    
}

