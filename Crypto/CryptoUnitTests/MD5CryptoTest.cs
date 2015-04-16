using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Crypto;

namespace CryptoUnitTests
{
	[TestClass]
	public class MD5CryptoTest
	{
		[TestMethod]
		public void MD5Create()
		{
			MD5Crypto md5 = new MD5Crypto();
			Assert.IsNotNull(md5);
		}

		[TestMethod]
		public void MD5GetStringHash()
		{
			MD5Crypto md5 = new MD5Crypto();
			string result = null;
			result = md5.GetHash("Hello world");

			Assert.IsNotNull(md5);
		}


	}
}
