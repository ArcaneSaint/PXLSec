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
		#region properties

		public string PublicPrivateKeyXML { get; set; }
		public string PublicKeyOnlyXML { get; set; }

		#endregion
		#region Constructors
		/// <summary>
		/// Initializes A new RSACrypto instance. Generates a public and private key.
		/// </summary>
		public RSACrypto()
		{
			provider = new RSACryptoServiceProvider(2048); // Generate a new 2048 bit RSA key
			PublicPrivateKeyXML = provider.ToXmlString(true);
			PublicKeyOnlyXML = provider.ToXmlString(false);
		}

		public RSACrypto(string publicKeyXML)
		{
			provider = new RSACryptoServiceProvider(2048);
			provider.FromXmlString(publicKeyXML);
		}

		#endregion

		public byte[] Encrypt(byte[] data)
		{
			return provider.Encrypt(data, true);
		}
		public byte[] Decrypt(byte[] data)
		{
			return provider.Decrypt(data, true);
		}
	}
}
