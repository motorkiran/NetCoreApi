[AttributeUsage(AttributeTargets.All)]
public class SecurityActionAttribute : Attribute
{
    int pageId;
    int actionId;

    public SecurityActionAttribute(int pageId, int actionId)
    {
        this.pageId = pageId;
        this.actionId = actionId;
    }

    public int PageId
    {
        get { return pageId; }
    }

    public Int64 ActionId
    {
        get { return actionId; }
    }
}