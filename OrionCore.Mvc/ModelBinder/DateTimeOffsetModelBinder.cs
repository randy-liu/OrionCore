using Orion.API;
using System;
using System.Globalization;
using System.Web.Mvc;
using Orion.API.Extensions;
using System.Threading;

namespace Orion.Mvc.ModelBinder
{
	/// <summary></summary>
	public class DateTimeOffsetModelBinder : DefaultModelBinder
	{
		/// <summary></summary>
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var value = base.BindModel(controllerContext, bindingContext);
			if(value == null || !(value is DateTimeOffset)) { return value; }

			var date = ThreadTimeZone.PatchZone((DateTimeOffset)value);
			return date;
		}
	}
}
