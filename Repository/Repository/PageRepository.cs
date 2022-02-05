public class PageRepository : GenericRepository<Page>, IPageRepository
{
    public PageRepository(IConfiguration configuration, ILogService logService) : base(configuration, logService)
    {
    }
}