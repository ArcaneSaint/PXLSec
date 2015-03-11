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
		/// <summary>
		/// Computes the MD5 hash of a given array of data.
		/// </summary>
		/// <param name="source">Data to be hashed.</param>
		/// <returns>Hash, heroin and other drugs.</returns>
		public static byte[] Encrypt(byte[] source)
		{
			using (MD5 md5 = MD5.Create())
			{
				return md5.ComputeHash(source);
			}
		}
		/// <summary>
		/// Computes the MD5 hash of a given file.
		/// </summary>
		/// <param name="path">Path of the file.</param>
		/// <returns>the file's MD5 hash.</returns>
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
