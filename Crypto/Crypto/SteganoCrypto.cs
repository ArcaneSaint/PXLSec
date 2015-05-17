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
		/// <summary>
		/// Encodes a message within picture.
		/// </summary>
		/// <param name="message">Message that will be encoded within the picture.</param>
		/// <param name="imagePath">Path to the image.</param>
		/// <returns>An image with the message embedded in it.</returns>
		public Bitmap EncodePicture(string message, string imagePath)
		{
			using (Bitmap image = new Bitmap(imagePath))
			{
				int arraycounter = 0;
				byte[] values = StringToBytes(message);
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
		/// <summary>
		/// Encodes a message within picture and writes it to the disk.
		/// </summary>
		/// <param name="message">Message that will be encoded within the picture.</param>
		/// <param name="imagePath">Path to the image.</param>
		/// <param name="savePath">Path where the result should be written.</param>
		/// <returns>If saving the image succeeded or not.</returns>
		public bool EncodeSavePicture(string message, string imagePath, string savePath)
		{
			using (Bitmap image = new Bitmap(imagePath))
			{
				int arraycounter = 0;
				byte[] values = StringToBytes(message);
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
		/// <summary>
		/// Retrieves a message from an image.
		/// </summary>
		/// <param name="encodedPath">Path to the image with a message in it.</param>
		/// <param name="originalPath">Path to the original source image.</param>
		/// <returns>The message hidden in the image.</returns>
		public string decodePicture(string encodedPath, string originalPath)
		{
			// byte R, G, B;
			string result = "";

			using (Bitmap originalPicture = new Bitmap(originalPath), encodedPicture = new Bitmap(encodedPath))
			{
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
			}

			return result;
		}
	}
}
