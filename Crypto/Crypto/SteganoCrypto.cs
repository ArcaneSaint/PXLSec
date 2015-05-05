/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;

namespace Crypto
{
	class SteganoCrypto
	{
		internal struct Pixel
		{
			internal byte Red;
			internal byte Green;
			internal byte Blue;
			internal byte Alpha;
		}

		#region Properties
		public BitmapImage image { get; set; }
		#endregion
		#region Constructors
		public SteganoCrypto() 
		{
			
		}
		#endregion

		/*private byte[] GetPixel(int x, int y)
		{
			int stride = image.PixelWidth * 4;
			int size = image.PixelHeight * stride;
			byte[] pixels = new byte[size];
			image.CopyPixels(pixels, stride, 0);
		}*/
/*
		#region Encryption
		public void Encrypt(string message)
		{
			
			//image.CopyPixels()
		}
		#endregion
		#region Decryption
		
		#endregion
	}
}
*/