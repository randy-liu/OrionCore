using System.Reflection;
using Xunit;

namespace OrionCore.API.Tests
{
    public class AssemblyUtilsTests
    {
        
        [Fact]
        public void GetMeta_Test1()
        {
            AssemblyUtils.GetMeta(Assembly.GetExecutingAssembly());
        }


        [Fact]
        public void GetMeta_Test2()
        {
            AssemblyUtils.GetMeta("OrionCore.API");
        }
    }
}
