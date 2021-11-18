using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Orion.Api.Tests
{
    public class VariableMonitorTests
    {
        private List<string> _list1 = new List<string>();
        private List<string> _list2 = new List<string>();
        private Dictionary<string, string> _dict1 = new Dictionary<string, string>();


        [Fact]
        public void RunTest()
        {
            VariableMonitor
                .Add(() => _list1.Count)
                .Add(() => _list2.Count, "22")
                .Add(() => _dict1.Count)
                .Add(() => _dict1.Keys.ToList())
                .Add(() => _dict1.Values.ToList())
                ;

            var data = VariableMonitor.Get();

        }


    }
}
