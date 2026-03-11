using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Orion.Api.Extensions
{

    /// <summary>Socket 加解密傳輸擴充方法。</summary>
    public static class SocketExtensions
    {

        /// <summary>將資料加密後透過 Socket 傳送。</summary>
        /// <param name="socket">目標 Socket。</param>
        /// <param name="algorithm">對稱式加密演算法。</param>
        /// <param name="data">要傳送的原始資料。</param>
        public static void SendEncrypt(this Socket socket, SymmetricAlgorithm algorithm, byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var encryptStream = new CryptoStream(memoryStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    encryptStream.Write(data, 0, data.Length);
                }
                socket.Send(memoryStream.ToArray());
            }
        }


        /// <summary>從 Socket 接收資料並解密。</summary>
        /// <param name="socket">來源 Socket。</param>
        /// <param name="algorithm">對稱式加密演算法。</param>
        /// <returns>解密後位元組陣列。</returns>
        public static byte[] ReceiveDecrypt(this Socket socket, SymmetricAlgorithm algorithm)
        {
            var buffer = new byte[4096];
            int len = socket.Receive(buffer, buffer.Length, SocketFlags.None);

            using (var inStream = new MemoryStream(buffer, 0, len))
            using (var outStream = new MemoryStream())
            using (var decryptStream = new CryptoStream(inStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read))
            {
                decryptStream.CopyTo(outStream);
                return outStream.ToArray();
            }
        }

    }
}
