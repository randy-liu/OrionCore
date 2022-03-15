using Orion.Api.Tests;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Orion.Api.Extensions.Tests
{
	public class ObjectExtensionsTests
	{

		public enum Floor
		{
			None,
			F1,
			F2,
			F3,
			F4,
		}


        /// <summary>New Case 新增案例</summary>
        private static object[] n<T>(object value)
        {
            return new object[] { typeof(T), value };
        }


        public static IEnumerable<object[]> ConvertTo_Test_Data()
		{
            return new[] 
            {
			    n<string>( Guid.NewGuid()),
                n<string>( Floor.F1 ),
                n<string>( new TimeSpan(12, 0, 0) ),
			    n<Guid>( "d5507446-c53d-4767-9e62-e7096951db1a" ),
                n<Floor>( "F1" ),
                n<TimeSpan>( "12:00:00" ),
            };
		}

		[Theory]
		[MemberData(nameof(ConvertTo_Test_Data))]
		public void ConvertTo_Test(Type type, object value)
		{
			object result = value.ConvertTo(type);
			Assert.NotNull(result);
		}





        /*===============================================================*/

        /// <summary>New Case 新增案例</summary>
        private static object[] n(string expected, Func<string> expr)
        {
            return new object[] { expected, expr };
        }



        public static IEnumerable<object[]> Comma_TestData()
        {
            yield return n("12,345", () => ((short)12345).Comma());
            yield return n("12,345", () => ((ushort)12345).Comma());
            yield return n("12,345", () => ((int)12345).Comma());
            yield return n("12,345", () => ((uint)12345).Comma());
            yield return n("12,345", () => ((long)12345).Comma());
            yield return n("12,345", () => ((ulong)12345).Comma());

            yield return n("2,345.121", () => ((float)2345.121f).Comma());
            yield return n("2,345.122", () => ((double)2345.122).Comma());
            yield return n("2,345.123", () => ((decimal)2345.123m).Comma());

            yield return n("12,345", () => ((short?)12345).Comma());
            yield return n("12,345", () => ((ushort?)12345).Comma());
            yield return n("12,345", () => ((int?)12345).Comma());
            yield return n("12,345", () => ((uint?)12345).Comma());
            yield return n("12,345", () => ((long?)12345).Comma());
            yield return n("12,345", () => ((ulong?)12345).Comma());

            yield return n("2,345.131", () => ((float?)2345.131f).Comma());
            yield return n("2,345.132", () => ((double?)2345.132).Comma());
            yield return n("2,345.133", () => ((decimal?)2345.133m).Comma());

            yield return n(null, () => ((short?)null).Comma());
            yield return n(null, () => ((ushort?)null).Comma());
            yield return n(null, () => ((int?)null).Comma());
            yield return n(null, () => ((uint?)null).Comma());
            yield return n(null, () => ((long?)null).Comma());
            yield return n(null, () => ((ulong?)null).Comma());

            yield return n(null, () => ((float?)null).Comma());
            yield return n(null, () => ((double?)null).Comma());
            yield return n(null, () => ((decimal?)null).Comma());


            yield return n("2,345.11", () => ((float)2345.113).Comma(2));
            yield return n("2,345.12", () => ((double)2345.123).Comma(2));
            yield return n("2,345.13", () => ((decimal)2345.133).Comma(2));

            yield return n("2,345.21", () => ((float?)2345.213).Comma(2));
            yield return n("2,345.22", () => ((double?)2345.223).Comma(2));
            yield return n("2,345.23", () => ((decimal?)2345.233).Comma(2));

            yield return n(null, () => ((float?)null).Comma(2));
            yield return n(null, () => ((double?)null).Comma(2));
            yield return n(null, () => ((decimal?)null).Comma(2));
        }

        [Theory]
        [MemberData(nameof(Comma_TestData))]
        public void Comma_RunTest(string expected, Func<string> expr)
        {
            string actual = expr();
            Assert.Equal(expected, actual);
        }



    }





}
