
namespace Orion.Api
{
	/// <summary>IPasswordHandle 介面</summary>
	public interface IPasswordHandle
	{
		/// <summary>驗證輸入密碼是否符合規則或既有加密值。</summary>
		/// <param name="password">待驗證密碼。</param>
		/// <returns>驗證成功時回傳 `true`。</returns>
		bool Validate(string password);
		
		/// <summary>將密碼進行加密（或雜湊）後回傳。</summary>
		/// <param name="password">原始密碼。</param>
		/// <returns>加密後字串。</returns>
		string Encrypt(string password);

	}
}
