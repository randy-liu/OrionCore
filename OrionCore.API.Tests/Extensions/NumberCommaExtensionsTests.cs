using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrionCore.Api.Extensions.Tests
{
    public class NumberCommaExtensionsTests
    {

        private static object[] newCase(string expected, Expression<Func<string>> expr)
        {
            return new object[] { expected, expr };
        }


        public static IEnumerable<object[]> Comma_TestData()
        {
            yield return newCase("12,345", () => NumberCommaExtensions.Comma((short)12345));
            yield return newCase("12,345", () => NumberCommaExtensions.Comma((ushort)12345));
            yield return newCase("12,345", () => NumberCommaExtensions.Comma((int)12345));
            yield return newCase("12,345", () => NumberCommaExtensions.Comma((uint)12345));
            yield return newCase("12,345", () => NumberCommaExtensions.Comma((long)12345));
            yield return newCase("12,345", () => NumberCommaExtensions.Comma((ulong)12345));
                         
            yield return newCase("2,345.121", () => NumberCommaExtensions.Comma((float)2345.121f));
            yield return newCase("2,345.122", () => NumberCommaExtensions.Comma((double)2345.122));
            yield return newCase("2,345.123", () => NumberCommaExtensions.Comma((decimal)2345.123m));
                         
            yield return newCase("12,345", () => NumberCommaExtensions.Comma((short?)12345));
            yield return newCase("12,345", () => NumberCommaExtensions.Comma((ushort?)12345));
            yield return newCase("12,345", () => NumberCommaExtensions.Comma((int?)12345));
            yield return newCase("12,345", () => NumberCommaExtensions.Comma((uint?)12345));
            yield return newCase("12,345", () => NumberCommaExtensions.Comma((long?)12345));
            yield return newCase("12,345", () => NumberCommaExtensions.Comma((ulong?)12345));
                         
            yield return newCase("2,345.131", () => NumberCommaExtensions.Comma((float?)2345.131f));
            yield return newCase("2,345.132", () => NumberCommaExtensions.Comma((double?)2345.132));
            yield return newCase("2,345.133", () => NumberCommaExtensions.Comma((decimal?)2345.133m));
                         
            yield return newCase(null, () => NumberCommaExtensions.Comma((short?)null));
            yield return newCase(null, () => NumberCommaExtensions.Comma((ushort?)null));
            yield return newCase(null, () => NumberCommaExtensions.Comma((int?)null));
            yield return newCase(null, () => NumberCommaExtensions.Comma((uint?)null));
            yield return newCase(null, () => NumberCommaExtensions.Comma((long?)null));
            yield return newCase(null, () => NumberCommaExtensions.Comma((ulong?)null));
                         
            yield return newCase(null, () => NumberCommaExtensions.Comma((float?)null));
            yield return newCase(null, () => NumberCommaExtensions.Comma((double?)null));
            yield return newCase(null, () => NumberCommaExtensions.Comma((decimal?)null));
                         
                         
            yield return newCase("2,345.11", () => NumberCommaExtensions.Comma((float)2345.113, 2));
            yield return newCase("2,345.12", () => NumberCommaExtensions.Comma((double)2345.123, 2));
            yield return newCase("2,345.13", () => NumberCommaExtensions.Comma((decimal)2345.133, 2));
                         
            yield return newCase("2,345.21", () => NumberCommaExtensions.Comma((float?)2345.213, 2));
            yield return newCase("2,345.22", () => NumberCommaExtensions.Comma((double?)2345.223, 2));
            yield return newCase("2,345.23", () => NumberCommaExtensions.Comma((decimal?)2345.233, 2));
                         
            yield return newCase(null, () => NumberCommaExtensions.Comma((float?)null, 2));
            yield return newCase(null, () => NumberCommaExtensions.Comma((double?)null, 2));
            yield return newCase(null, () => NumberCommaExtensions.Comma((decimal?)null, 2));
        }

        [Theory]
        [MemberData(nameof(Comma_TestData))]
        public void Comma_RunTest(string expected, Expression<Func<string>> expr)
        {
            string actual = expr.Compile().Invoke();
            Assert.Equal(expected, actual);
        }

         



    }
}
