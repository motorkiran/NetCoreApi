public class PageService : IPageService
{
    private readonly IPageRepository _pageRepository;

    public PageService(IPageRepository pageRepository)
    {
        _pageRepository = pageRepository;
    }

    public Page Get(int id)
    {
        var page = _pageRepository.Get(id);

        return page;
    }

    public List<Page> Get()
    {
        var page = _pageRepository.Get().ToList();

        return page;
    }
}