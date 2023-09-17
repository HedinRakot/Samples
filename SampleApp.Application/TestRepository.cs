using SampleApp.Domain;

namespace SampleApp.Application;

public class TestRepository
{
    public TestRepository()
    {
        Tests.Add(new Test
        {
            Id = 1,
            Name = "test",
        });
    }
    public List<Test> Tests { get; set; } = new List<Test>();
}