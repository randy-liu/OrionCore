using System.ComponentModel;

namespace Orion.Api
{
	/// <summary>事件類型</summary>
	public enum NotifiStatus
	{

		/// <summary>無</summary>
		[Description("無")]
		None,

		/// <summary>更新</summary>
		[Description("更新")]
		Change,

		/// <summary>逾時</summary>
		[Description("逾時")]
		Timeout,

		/// <summary>失敗</summary>
		[Description("失敗")]
		Failure,

		/// <summary>完成</summary>
		[Description("完成")]
		Complete,
	}


	/*########################################################*/

	/// <summary>可設定事件類型</summary>
	public interface INotifiStatusable
	{
		/// <summary>設定事件類型</summary>
		void SetNotifiStatus(NotifiStatus status);
	}

}
