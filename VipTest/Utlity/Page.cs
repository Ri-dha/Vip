namespace VipTest.Utlity;

public class Page<T>
{
    public Page()
    {
    }

    public Page(bool status, string message, List<T> data, int pagesCount, int currentPage, int totalCount)
    {
        Data = data;
        PagesCount = pagesCount;
        CurrentPage = currentPage;
        TotalCount = totalCount;

    }
    

    public List<T> Data { get; set; }
    public int? PagesCount { get; set; }
    public int CurrentPage { get; set; }
    public string Type { get; set; } = typeof(T).Name;
    public int? TotalCount { get; set; }
}