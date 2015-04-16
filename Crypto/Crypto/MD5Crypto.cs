using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Crypto
{
	public class MD5Crypto
	{


		public string GetHash(string source)
		{
			byte[] result = null;
			using (var md5 = MD5.Create())
			{
				byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
				result = md5.ComputeHash(sourceBytes);
			}
			if (result != null)
			{
				return BitConverter.ToString(result).Replace("-", string.Empty);
			}
			else
				throw new ArgumentException();
		}

		public string GetFileChecksum(string fileName)
		{
			byte[] result = null;
			using (var md5 = MD5.Create())
			{
				using (var stream = File.OpenRead(fileName))
				{
					 result = md5.ComputeHash(stream);
				}
			}
			if (result != null)
			{
				return BitConverter.ToString(result).Replace("-", string.Empty);
			}
			else
				throw new ArgumentException();
		}
	}
}
