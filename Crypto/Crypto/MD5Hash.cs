using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Crypto
{
	public static class MD5Hash
	{
		public static byte[] Encrypt(byte[] source)
		{
			using (MD5 md5 = MD5.Create())
			{
				return md5.ComputeHash(source);
			}
		}

		public static byte[] Encrypt(string path)
		{
			using (MD5 md5 = MD5.Create())
			{
				using (FileStream fstream = File.OpenRead(path))
				{
					return md5.ComputeHash(fstream);
				}
			}
		}
	}
}
