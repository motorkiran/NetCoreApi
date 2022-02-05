using System.ComponentModel;

[Description("page")]
public class Page : AbstractEntity // Page means Controller
{
    [Description("page_name")]
    public string PageName { get; set; }
}