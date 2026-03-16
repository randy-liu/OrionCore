using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Orion.Api
{
	/// <summary>以 SHA256 與 Base64 處理密碼字串。</summary>
	public class PasswordSHA256Handle : IPasswordHandle
	{
		/// <summary>驗證字串是否符合密碼規則（至少 6 碼且包含大小寫英文字母與數字）。</summary>
		/// <param name="password">待驗證密碼字串。</param>
		/// <returns>符合規則時回傳 <c>true</c>。</returns>
		public bool Validate(string password)
		{
			if (password.Length < 6) { return false; }
			if (!Regex.IsMatch(password, @"[a-z]")) { return false; }
			if (!Regex.IsMatch(password, @"[A-Z]")) { return false; }
			if (!Regex.IsMatch(password, @"[0-9]")) { return false; }

			return true;
		}

		/// <summary>將密碼字串做 SHA256 計算後轉成 Base64 字串。</summary>
		/// <param name="password">要加密的原始密碼。</param>
		/// <returns>SHA256 計算結果的 Base64 字串。</returns>
		public string Encrypt(string password)
		{
			using SHA256 sha256 = SHA256.Create();
			byte[] source = Encoding.Default.GetBytes(password);
			byte[] crypto = sha256.ComputeHash(source);
			string result = Convert.ToBase64String(crypto);
			return result;
		}
	}
}
