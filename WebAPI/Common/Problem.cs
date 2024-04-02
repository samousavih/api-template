namespace SampleOrg.WebAPI.Common;

public class Problem
{
    public const string ContentType = "application/problem+json";
    public int Status { get; set; }
    public string Title { get; set; }
    public ProblemType Type { get; set; }
    public string Detail { get; set; }
}