using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crypto
{
	public class RSACrypto
	{
		RSACryptoServiceProvider provider;
		#region Properties

		private string publicPrivateKeyXML;
		public string PublicPrivateKeyXML { get { return publicPrivateKeyXML;} }
		private string publicKeyOnlyXML;
		public string PublicKeyOnlyXML { get { return publicKeyOnlyXML;} }

		#endregion
		#region Constructors
		/// <summary>
		/// Initializes A new RSACrypto instance. Generates a public and private key.
		/// </summary>
		public RSACrypto()
		{
			provider = new RSACryptoServiceProvider(2048); // Generate a new 2048 bit RSA key
			publicPrivateKeyXML = provider.ToXmlString(true);
			publicKeyOnlyXML = provider.ToXmlString(false);
		}
		/// <summary>
		/// Initializes A new RSACrypto instance with given key.
		/// </summary>
		/// <param name="keyXML">The XML containing the key.</param>
		public RSACrypto(string keyXML)
		{
			provider = new RSACryptoServiceProvider(2048);
			provider.FromXmlString(keyXML);
		}
		#endregion
		#region Encryption
		/// <summary>
		/// Encrypts data using this RSACrypto instance's public key
		/// </summary>
		/// <param name="data">data to be encrypted</param>
		/// <returns>encrypted data</returns>
		public byte[] Encrypt(byte[] data)
		{
			return provider.Encrypt(data, true);
		}
		#endregion
		#region Decryption
		/// <summary>
		/// Decrypts data using this RSACrypto instance's private key
		/// </summary>
		/// <param name="data">data to be decrypted</param>
		/// <returns>decrypted data</returns>
		public byte[] Decrypt(byte[] data)
		{
			return provider.Decrypt(data, true);
		}
		#endregion
	}
}
