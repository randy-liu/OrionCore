using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Xunit;


namespace Orion.Api.Extensions.Tests
{
	public class NotifyPropertyExtensionsTests
	{

		public partial class EnvironmentSettingPanel : INotifyPropertyChanged
		{
			#pragma warning disable 0067
			public event PropertyChangedEventHandler PropertyChanged;
			#pragma warning restore 0067

			public string Domain { get; set; }

		}




		[Fact]
		public void RunTest()
		{
			var obj = new EnvironmentSettingPanel();
			obj.PropertyChanged += (s, e) => { };

			obj.TriggerChanged(x => x.Domain);

		}



	}

}
