using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Crypto;

namespace CryptoUnitTests
{
	/// <summary>
	/// Summary description for RSATest
	/// </summary>
	[TestClass]
	public class RSATest
	{
		[TestMethod]
		public void RSACreate()
		{
			RSACrypto crypto = new RSACrypto();
			Assert.IsNotNull(crypto);
		}


		[TestMethod]
		public void RSAEncryptDecrypt()
		{
			byte[] source = new byte[1];
			source[0] = (byte)'a';
			RSACrypto crypto = new RSACrypto();
			byte[] encr = crypto.Encrypt(source);
			CollectionAssert.AreEqual(source, crypto.Decrypt(encr));
		}

		[TestMethod]
		public void RSAEncryptDecrypPublicPrivateCryptos()
		{
			byte[] source = new byte[1];
			byte[] encr;
			byte[] decr;

			source[0] = (byte)'a';

			RSACrypto privateCrypto = new RSACrypto();
			RSACrypto publicCrypto = new RSACrypto(privateCrypto.PublicKeyOnlyXML);
			encr = publicCrypto.Encrypt(source);
			decr = privateCrypto.Decrypt(encr);
			

			CollectionAssert.AreEqual(source, decr);
		}
	}
}
