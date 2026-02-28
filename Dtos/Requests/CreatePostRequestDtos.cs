public class CreatePostRequestDtos
{
    public string Title {get;set;}= default!;

    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public string Location { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
}