using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Drawing;
using System.IO;

namespace Crypto
{
	public partial class SteganoCrypto
	{

		public byte[] StringToBytes(String str)
		{
			byte[] byteArray = new byte[str.Length];
			char[] disectedString = str.ToCharArray();
			for (int i = 0; i < str.Length; i++)
			{
				byteArray[i] = Convert.ToByte(disectedString[i]);
			}
			return byteArray;
		}

		public Bitmap EncodePicture(string str, string imagePath)
		{
			using (Bitmap image = new Bitmap(imagePath))
			{
				int arraycounter = 0;
				byte[] values = StringToBytes(str);
				byte R, G, B;
				for (int i = 0; i < image.Width; i++)
				{
					for (int j = 0; j < image.Height; j++)
					{
						System.Drawing.Color original = image.GetPixel(i, j);

						if (arraycounter <= values.Length - 1)
						{
							R = values[arraycounter];
							R = (byte)(R + original.R);
						}
						else
						{
							R = original.R;
						}
						if (arraycounter + 1 <= values.Length - 1)
						{
							G = values[arraycounter + 1];
							G = (byte)(G + original.G);
						}
						else
						{
							G = original.G;
						}
						if (arraycounter + 2 <= values.Length - 1)
						{
							B = values[arraycounter + 2];
							B = (byte)(B + original.B);
						}
						else
						{
							B = original.B;
						}
						System.Drawing.Color newpixel = System.Drawing.Color.FromArgb(original.A, R, G, B);
						image.SetPixel(i, j, newpixel);
						arraycounter += 3;
					}
				}

				//image.Save(@"../../Images/newStegosaurus.bmp");
				return image;
			}

		}
		public bool EncodeSavePicture(string str, string imagePath, string savePath)
		{
			using (Bitmap image = new Bitmap(imagePath))
			{
				int arraycounter = 0;
				byte[] values = StringToBytes(str);
				byte R, G, B;
				for (int i = 0; i < image.Width; i++)
				{
					for (int j = 0; j < image.Height; j++)
					{
						System.Drawing.Color original = image.GetPixel(i, j);

						if (arraycounter <= values.Length - 1)
						{
							R = values[arraycounter];
							R = (byte)(R + original.R);
						}
						else
						{
							R = original.R;
						}
						if (arraycounter + 1 <= values.Length - 1)
						{
							G = values[arraycounter + 1];
							G = (byte)(G + original.G);
						}
						else
						{
							G = original.G;
						}
						if (arraycounter + 2 <= values.Length - 1)
						{
							B = values[arraycounter + 2];
							B = (byte)(B + original.B);
						}
						else
						{
							B = original.B;
						}
						System.Drawing.Color newpixel = System.Drawing.Color.FromArgb(original.A, R, G, B);
						image.SetPixel(i, j, newpixel);
						arraycounter += 3;
					}
				}
				image.Save(savePath);
				return true;
			}
			return false;

		}
		private string decodePicture(Bitmap encodedPicture, Bitmap originalPicture)
		{
			string result = "";

			for (int i = 0; i < originalPicture.Width; i++)
			{
				for (int j = 0; j < originalPicture.Height; j++)
				{
					System.Drawing.Color newpixel = encodedPicture.GetPixel(i, j);
					System.Drawing.Color oldpixel = originalPicture.GetPixel(i, j);
					if (newpixel.R != oldpixel.R)
					{
						result += Convert.ToChar((byte)(newpixel.R - oldpixel.R));
					}
					if (newpixel.G != oldpixel.G)
					{
						result += Convert.ToChar((byte)(newpixel.G - oldpixel.G));
					}
					if (newpixel.B != oldpixel.B)
					{
						result += Convert.ToChar((byte)(newpixel.B - oldpixel.B));
					}

				}
			}

			return result;
		}

		public string decodePicture(string encodedPath, string originalPath)
		{
			// byte R, G, B;
			string result = "";

			using (Bitmap originalPicture = new Bitmap(originalPath), encodedPicture = new Bitmap(encodedPath))
			{
				result = decodePicture(originalPicture, encodedPicture);
			}

			return result;
		}

	}
}
