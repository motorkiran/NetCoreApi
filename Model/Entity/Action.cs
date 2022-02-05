using System.ComponentModel;

[Description("action")]
public class Action : AbstractEntity
{
    [Description("action_name")]
    public string ActionName { get; set; }

    [Description("page_id")]
    public int PageId { get; set; }
}