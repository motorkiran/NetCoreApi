public class ResultObjectDto
{
    public ResultType ResultType { get; set; }
    public string Message { get; set; } = string.Empty;
    public dynamic Result { get; set; } 
    public bool IsSuccess { get; set; }
}