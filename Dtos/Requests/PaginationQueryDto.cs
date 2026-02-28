public class PaginationQueryDto
{
    public int PageNumber { get; set; } = 1; //current page number, default is 1
    public int PageSize { get; set; } = 5; //maximum number of records to return in a single response

    public string? SearchTerm { get; set; } //optional search term for filtering results

    public string? SortByCreatedAt { get; set; } = "CreatedAt";  //optional sorting by created date, can be "asc" or "desc"

    public string? SortOrder {get;set;} = "asc"; //optional sorting order, can be "asc" or "desc"
}