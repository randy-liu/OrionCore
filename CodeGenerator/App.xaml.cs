using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using CodeGenerator.Properties;

namespace CodeGenerator
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
    { 
		public App() : base()
		{
			/* UnhandledException 只能攔截錯誤，不能阻止程式關閉 */
			AppDomain.CurrentDomain.UnhandledException += unhandledExceptionHandler;

			/* ThreadException 用來攔截 UI 錯誤 */
			DispatcherUnhandledException += threadExceptionHandler;
		}


		void threadExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.Message, "操作錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
			e.Handled = true;
		}



		private static void unhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = (Exception)e.ExceptionObject;
			MessageBox.Show(ex.Message, "操作錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
		}



		private void keepLast(StringCollection items)
		{
			var keep = items.Cast<string>().Skip(Math.Max(0, items.Count - 10)).ToArray();
			items.Clear();
			items.AddRange(keep);
		}


		/// <summary>關閉時儲存狀態設定</summary>
		private void onExit(object sender, ExitEventArgs e)
		{
			keepLast(Settings.Default.CntStringItems);
			keepLast(Settings.Default.SolutionItems);

			Settings.Default.Save();
		}
	}

}
