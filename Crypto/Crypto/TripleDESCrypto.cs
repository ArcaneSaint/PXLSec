using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crypto
{
	public class TripleDESCrypto
	{
		TripleDESCryptoServiceProvider provider;
		#region Properties
		/// <summary>
		/// The Key used for encrypting/decrypting
		/// </summary>
		public byte[] Key { get { return provider.Key; } }
		/// <summary>
		/// The Initialization Vector used for encrypting/decrypting
		/// </summary>
		public byte[] IV { get { return provider.IV; } }
		#endregion
		#region Constructors
		/// <summary>
		/// Initializes A new TripleDESCrypto instance. Generates a Key and IV.
		/// </summary>
		public TripleDESCrypto()
		{
			provider = new TripleDESCryptoServiceProvider();
			provider.GenerateKey();
			provider.GenerateIV();
		}
		/// <summary>
		/// Initializes A new TripleDESCrypto instance. Generates an IV
		/// </summary>
		/// <param name="key">The Key to be used for encrypting/decrypting</param>
		/// <exception cref="CryptographicException">Thrown when provided Key is invalid.</exception>
		public TripleDESCrypto(byte[] key)
		{
			provider = new TripleDESCryptoServiceProvider();
			provider.Key = key;
			provider.GenerateIV();
		}
		/// <summary>
		/// Initializes A new TripleDESCrypto instance.
		/// </summary>
		/// <param name="key">The Key to be used for encrypting/decrypting</param>
		/// <param name="iv">The Initialization Vector to be used for encrypting/decrypting</param>
		/// <exception cref="CryptographicException">Thrown when provided Key or IV is invalid.</exception>
		public TripleDESCrypto(byte[] key, byte[] iv)
		{
			provider = new TripleDESCryptoServiceProvider();
			provider.Key = key;
			provider.IV = iv;
		}
		#endregion
		#region Encryption
		/// <summary>
		/// Encrypts a string with TripleDES encryption, using this TripleDESCrypto instance's Key and IV. Might result in unexpected behavior if the string is of an unusual encoding.
		/// </summary>
		/// <param name="source">The string to be encrypted</param>
		/// <returns>An array of encrypted bytes</returns>
		public byte[] Encrypt(string source)
		{
			byte[] messageBytes = new byte[source.Length * sizeof(char)];
			System.Buffer.BlockCopy(source.ToCharArray(), 0, messageBytes, 0, messageBytes.Length);
			return Encrypt(messageBytes);
		}
		/// <summary>
		/// Encrypts an array of bytes with TripleDES encryption, using this TripleDESCrypto instance's Key and IV
		/// </summary>
		/// <param name="source">The array of bytes to be encrypted</param>
		/// <returns>An array of encrypted bytes</returns>
		public byte[] Encrypt(byte[] source)
		{
			ICryptoTransform transform = provider.CreateEncryptor(provider.Key, provider.IV);
			MemoryStream memStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
			cryptoStream.Write(source, 0, source.Length);
			cryptoStream.FlushFinalBlock();

			byte[] result = new byte[memStream.Length];
			memStream.Position = 0;
			memStream.Read(result, 0, result.Length);

			return result;
		}
		#endregion
		#region Decryption
		/// <summary>
		/// Decrypts an array of encrypted byte with TripleDES encryption, using this TripleDESCrypto instance's Key and IV
		/// </summary>
		/// <param name="source">The array holding the encrypted bytes</param>
		/// <returns>A Decrypted array of bytes</returns>
		public byte[] Decrypt(byte[] source)
		{
			ICryptoTransform transform = provider.CreateDecryptor(provider.Key, provider.IV);
			MemoryStream memStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);

			cryptoStream.Write(source, 0, source.Length);
			cryptoStream.FlushFinalBlock();

			byte[] result = new byte[memStream.Length];
			memStream.Position = 0;
			memStream.Read(result, 0, result.Length);

			return result;
		}
		#endregion
	}
}
