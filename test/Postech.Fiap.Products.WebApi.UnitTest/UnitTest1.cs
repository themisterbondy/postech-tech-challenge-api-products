namespace Postech.Fiap.Products.WebApi.UnitTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        const int a = 1;
        const int b = 2;

        Assert.Equal(3, a + b);
    }
}