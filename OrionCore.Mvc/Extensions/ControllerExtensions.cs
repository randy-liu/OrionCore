using System;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using OrionCore.API.Extensions;
using OrionCore.API.Models;

namespace OrionCore.Mvc.Extensions
{
	/// <summary></summary>
	public static class ControllerExtensions
	{

		/// <summary></summary>
		public static void SetStatusSuccess(this Controller controller, string message)
		{
			controller.TempData["StatusSuccess"] = message;
		}


		/// <summary></summary>
		public static void SetStatusError(this Controller controller, string message)
		{
			controller.TempData["StatusError"] = message;
		}


	}
}