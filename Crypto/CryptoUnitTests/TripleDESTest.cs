using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crypto;
using System.Text;
using System.Security.Cryptography;

namespace CryptoUnitTests
{
	[TestClass]
	public class TripleDESTest
	{
		static byte[] VALIDKEY = ASCIIEncoding.ASCII.GetBytes("asdfewrewqrss323");
		static byte[] VALIDIV = ASCIIEncoding.ASCII.GetBytes("8ragOfqa");
		static byte[] INVALIDKEY = Encoding.ASCII.GetBytes("Testsleutel");
		static byte[] INVALIDIV = Encoding.ASCII.GetBytes("Testvector");

		#region ConstructorTests
		[TestMethod]
		public void EmpytConstructor()
		{
			TripleDESCrypto crypto = new TripleDESCrypto();
			Assert.IsNotNull(crypto);
		}
		[TestMethod]
		public void ConstructorTestKey()
		{
			TripleDESCrypto crypto = new TripleDESCrypto();
			Assert.IsNotNull(crypto.Key);
		}
		[TestMethod]
		public void ConstructorTestKeyIV()
		{
			TripleDESCrypto crypto = new TripleDESCrypto();
			Assert.IsNotNull(crypto.IV);
		}

		[TestMethod]
		[ExpectedException(typeof(CryptographicException))]
		public void ConstructWithInvalidKey()
		{
			TripleDESCrypto crypto = new TripleDESCrypto(INVALIDKEY);
		}
		[TestMethod]
		[ExpectedException(typeof(CryptographicException))]
		public void ConstructWithInvalidKeyAndIV()
		{
			TripleDESCrypto crypto = new TripleDESCrypto(INVALIDKEY, INVALIDIV);
		}
		[TestMethod]
		public void ConstructWithValidKey()
		{
			TripleDESCrypto crypto = new TripleDESCrypto(VALIDKEY);
		}
		[TestMethod]
		public void ConstructWithValidKeyAndIV()
		{
			TripleDESCrypto crypto = new TripleDESCrypto(VALIDKEY, VALIDIV);
		}
		#endregion
		#region EncryptionTests
		[TestMethod]
		public void EncryptDecrypt()
		{
			TripleDESCrypto crypto = new TripleDESCrypto(VALIDKEY, VALIDIV);
			byte[] start = new byte[1];
			start[0] = (byte)'a';
			byte[] crypt = crypto.Encrypt(start);
			byte[] result = crypto.Decrypt(crypt);
			CollectionAssert.AreEqual(start, result);
		}
		[TestMethod]
		public void EncryptDecryptNewCrypto()
		{
			TripleDESCrypto crypto = new TripleDESCrypto(VALIDKEY, VALIDIV);
			byte[] start = new byte[1];
			start[0] = (byte)'a';
			byte[] crypt = crypto.Encrypt(start);

			TripleDESCrypto decrypto = new TripleDESCrypto(crypto.Key, crypto.IV);

			byte[] result = decrypto.Decrypt(crypt);
			CollectionAssert.AreEqual(start, result);
		}
		#endregion
	}
}
