using System.ComponentModel;

[Description("user_action")]
public class UserAction
{
    [Description("action_id")]
    public int ActionId { get; set; }

    [Description("user_id")]
    public int UserId { get; set; }

    [Description("page_id")]
    public int PageId { get; set; }
}