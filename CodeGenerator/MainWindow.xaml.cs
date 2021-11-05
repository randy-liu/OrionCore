using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CodeGenerator.Properties;
using CodeGenerator.Models;
using System.Collections.Specialized;
using System.Diagnostics;

namespace CodeGenerator
{
	/// <summary>
	/// MainWindow.xaml 的互動邏輯
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void trigger(string name) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

		public double FormWidth
		{
			get { return Settings.Default.FormWidth; }
			set { Settings.Default.FormWidth = value; trigger(nameof(FormWidth)); }
		}

		public double FormHeight
		{
			get { return Settings.Default.FormHeight; }
			set { Settings.Default.FormHeight = value; trigger(nameof(FormHeight)); }
		}

		public string CntString
		{
			get { return Settings.Default.CntString; }
			set { Settings.Default.CntString = value; trigger(nameof(CntString)); }
		}

		public StringCollection CntStringItems
		{
			get { return Settings.Default.CntStringItems; }
			set { Settings.Default.CntStringItems = value; trigger(nameof(CntStringItems)); }
		}

		public string Solution
		{
			get { return Settings.Default.Solution; }
			set { Settings.Default.Solution = value; trigger(nameof(Solution)); }
		}

		public StringCollection SolutionItems
		{
			get { return Settings.Default.SolutionItems; }
			set { Settings.Default.SolutionItems = value; trigger(nameof(SolutionItems)); }
		}

		public string ClassNameSpace
		{
			get { return Settings.Default.ClassNameSpace; }
			set { Settings.Default.ClassNameSpace = value; trigger(nameof(ClassNameSpace)); }
		}

		public string DataContextName
		{
			get { return Settings.Default.DataContextName; }
			set { Settings.Default.DataContextName = value; trigger(nameof(DataContextName)); }
		}



		private List<OptionItem> _templateSelectItems;
		public List<OptionItem> TemplateSelectItems
		{
			get { return _templateSelectItems; }
			set { _templateSelectItems = value; trigger(nameof(TemplateSelectItems)); }
		}


		private List<OptionItem> _tableSelectItems;
		public List<OptionItem> TableSelectItems
		{
			get { return _tableSelectItems; }
			set { _tableSelectItems = value; trigger(nameof(TableSelectItems)); }
		}


		public bool IsOverride { get; set; }



		public class OptionItem
		{
			public bool IsChecked { get; set; }
			public string Value { get; set; }
			public string Display { get; set; }
		}




		/*============================================================*/

		private DbMetaProvider _dbMetaProvider;
		private Generator _generator;

		public MainWindow()
		{
			_dbMetaProvider = new DbMetaProvider();
			_generator = new Generator(_dbMetaProvider);


			/*初始化组件*/
			DataContext = this;
			InitializeComponent();


			/*建立樣版選項*/
			TemplateSelectItems = _generator.TemplateTypes
				.Select(t => new OptionItem { Value = t.FullName, Display = formatTemplateName(t), })
				.ToList();
			
		}



		private string formatTemplateName(Type type)
		{
			string[] names = type.FullName.Replace("Template", "").Split('.');
			return string.Join(".", names.Skip(names.Length - 2));
		}




		/*########################################################*/


		private void generate()
		{
			/* UI驗證 */
			if (string.IsNullOrWhiteSpace(CntString)) { throw new Exception("請選擇 Connection String"); }
			if (string.IsNullOrWhiteSpace(Solution)) { throw new Exception("請選擇 Solution"); }
			if (string.IsNullOrWhiteSpace(ClassNameSpace)) { throw new Exception("請輸入 NameSpace"); }
			if (string.IsNullOrWhiteSpace(DataContextName)) { throw new Exception("請輸入 DataContext 的名稱"); }


			var tpls = TemplateSelectItems.Where(x => x.IsChecked).Select(x => x.Value).ToList();
			if (tpls.Count == 0) { throw new Exception("請選擇 Table"); }

			var tables = TableSelectItems.Where(x => x.IsChecked).Select(x => x.Value).ToList();
			if (tables.Count == 0) { throw new Exception("請選擇樣版"); }


			/* 清空執行訊息 */
			_execStatusMsg.Clear();

			_generator.BasePath = Path.GetDirectoryName(Solution);
			_generator.DataContextName = DataContextName;
			_generator.NameSpace = ClassNameSpace;
			_generator.IsOverride = IsOverride;

			_generator.Execute(tpls, tables, (isGenerate, filePath) =>
			{
				string msg = isGenerate ? "產生　" : "已存在";
				_execStatusMsg.AppendText(msg + "  " + filePath + "\n");
			});

			_execStatusMsg.AppendText("[執行結束]\n");
			_execStatusMsg.Focus();
		}





		/*########################################################*/


		private void window_Loaded(object sender, RoutedEventArgs e)
		{
			//cntStringBox_Changed(null, null);
		}


		private void cntStringBox_Changed(object sender, SelectionChangedEventArgs e)
		{
			TableSelectItems = new List<OptionItem> ();
			if (string.IsNullOrWhiteSpace(CntString)) { return; }

			/*產生 Table 選項*/
			try
			{
				_dbMetaProvider.SetConnectionString(CntString);
				List<TableInfo> list = _dbMetaProvider.GetTableList();

				TableSelectItems = list
					.Select(x => new OptionItem { Value = x.TableName, Display = x.DisplayName,})
					.ToList();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "操作錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}



		private void cntStringBtn_Click(object sender, EventArgs e)
		{
			var dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.Title = "選擇 DAO DLL";
			dialog.Filter = "DAO|*.dll";
			dialog.RestoreDirectory = true;

			bool? result = dialog.ShowDialog();
			if (result != true || dialog.FileName == null) { return; }

			string connString = dialog.FileName;
			if (!CntStringItems.Contains(connString)) { CntStringItems.Add(connString); trigger(nameof(CntStringItems)); }
			CntString = connString;
		}


		/// <summary>選擇 Solution</summary>
		private void solutionBtn_Click(object sender, EventArgs e)
		{
			var dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.Title = "選擇 Solition";
			dialog.Filter = "Visual Studio Solition|*.sln";
			dialog.RestoreDirectory = true;

			bool? result = dialog.ShowDialog();
			if (result != true || dialog.FileName == null) { return; }

			string path = dialog.FileName;
			if (!SolutionItems.Contains(path)){ SolutionItems.Add(path); trigger(nameof(SolutionItems)); }
			Solution = path;
		}




		private void execBtn_Click(object sender, EventArgs e)
		{
			generate();
		}


		private void close_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void openDir_Click(object sender, RoutedEventArgs e)
		{
			string dirPath = Path.GetDirectoryName(Solution);
			Process.Start("explorer.exe", dirPath);
		}

	}
}
