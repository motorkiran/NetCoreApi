using System.ComponentModel;

public class AbstractEntity
{
    [Description("id")]
    public int Id { get; set; }

    [Description("create_user")]
    public int CreateUser { get; set; }

    [Description("update_user")]
    public int UpdateUser { get; set; }

    [Description("is_active")]
    public bool IsActive { get; set; }

    [Description("is_deleted")]
    public bool IsDeleted { get; set; }

    [Description("create_date")]
    public DateTime CreateDate { get; set; }

    [Description("update_date")]
    public DateTime? UpdateDate { get; set; }
}