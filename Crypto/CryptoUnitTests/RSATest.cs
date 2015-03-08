using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Crypto;
using System.Security.Cryptography;
using System.Security;

namespace CryptoUnitTests
{
	/// <summary>
	/// Summary description for RSATest
	/// </summary>
	[TestClass]
	public class RSATest
	{
		static byte[] SOURCE = ASCIIEncoding.ASCII.GetBytes("$ùé{{aad8g");

		[TestMethod]
		public void RSACreate()
		{
			RSACrypto crypto = new RSACrypto();
			Assert.IsNotNull(crypto);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RSACreateEmptyXML()
		{
			string s = null;
			RSACrypto crypto = new RSACrypto(s);
		}
		[TestMethod]
		[ExpectedException(typeof(XmlSyntaxException))]
		public void RSACreateInvalidXML()
		{
			RSACrypto crypto = new RSACrypto("azeaze");
		}


		[TestMethod]
		public void RSAEncryptDecrypt()
		{
			byte[] encr;
			byte[] decr;
			//Create new RSACrypto instance.
			RSACrypto crypto = new RSACrypto();
			//Encrypt the data using the RSACrypto instance
			encr = crypto.Encrypt(SOURCE);
			//Decrypt the data using the RSACrypto instance
			decr = crypto.Decrypt(encr);
			//decrypted data should be equal to the source data
			CollectionAssert.AreEqual(SOURCE, decr);
		}

		[TestMethod]
		public void RSAEncryptDecrypPublicPrivateCryptos()
		{
			byte[] encr;
			byte[] decr;


			//Create new RSACrypto instance.
			RSACrypto privateCrypto = new RSACrypto();
			//Create a new RSACrypto instance using the stored public key
			RSACrypto publicCrypto = new RSACrypto(privateCrypto.PublicKeyOnlyXML);
			//Encrypt the data using the public key
			encr = publicCrypto.Encrypt(SOURCE);
			//Decrypt the data using the private key
			decr = privateCrypto.Decrypt(encr);
			//decrypted data should be equal to the source data
			CollectionAssert.AreEqual(SOURCE, decr);
		}

		[TestMethod]
		public void RSAEncryptDecrypPublicPrivateCryptosStoredKey()
		{
			byte[] encr;
			byte[] decr;
			string privateKeyXML;
			string publicKeyXML;

			//Create new RSACrypto instance and get store keys
			RSACrypto crypto1 = new RSACrypto();
			privateKeyXML = crypto1.PublicPrivateKeyXML;
			publicKeyXML = crypto1.PublicKeyOnlyXML;

			//Create a new RSACrypto instance using the stored private key
			RSACrypto privCrypto = new RSACrypto(privateKeyXML);
			//Create a new RSACrypto instance using the stored public key
			RSACrypto pubCrypto = new RSACrypto(publicKeyXML);

			//Encrypt the data using the public key
			encr = pubCrypto.Encrypt(SOURCE);
			//Decrypt the data using the private key
			decr = privCrypto.Decrypt(encr);

			//decrypted data should be equal to the source data
			CollectionAssert.AreEqual(SOURCE, decr);
		}
	}
}
