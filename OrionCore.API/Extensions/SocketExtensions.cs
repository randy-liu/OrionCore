using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Orion.Api.Extensions
{

    /// <summary></summary>
    public static class SocketExtensions
    {

        /// <summary></summary>
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


        /// <summary></summary>
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
