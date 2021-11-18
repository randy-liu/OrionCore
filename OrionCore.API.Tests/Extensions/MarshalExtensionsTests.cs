using System.Runtime.InteropServices;
using Xunit;

namespace Orion.Api.Extensions.Tests
{
	public class MarshalExtensionsTests
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
		public struct TestStruct
		{
			public int AppId;
			public byte MsgType;
			public short DeviceType;
			public byte Version;
		}


		[Fact]
		public void RunTest()
		{
			var a = new TestStruct
			{
				AppId = 1,
				MsgType = 3,
				DeviceType = 4,
				Version = 5,
			};

			var expected = new byte[] { 1, 0, 0, 0, 3, 4, 0, 5 };

			byte[] array = a.ToByteArray();
			Assert.Equal(expected, array);


			var b = array.ToStruct<TestStruct>();

			Assert.Equal(a.AppId, b.AppId);
			Assert.Equal(a.MsgType, b.MsgType);
			Assert.Equal(a.DeviceType, b.DeviceType);
			Assert.Equal(a.Version, b.Version);
		}

	}
}
