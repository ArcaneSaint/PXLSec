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

namespace imagemanipulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

        public void encodePicture(String str)
        {
            Bitmap image;
            image = new Bitmap(@"../../Images/Stegosaurus.bmp");
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
            image.Save(@"../../Images/newStegosaurus.bmp");
            image.Dispose();

        }


        public String decodePicture()
        {
            byte R, G, B;
            String result = "";

            Bitmap imageOld, imageNew;
            imageOld = new Bitmap(@"../../Images/Stegosaurus.bmp");
            imageNew = new Bitmap(@"../../Images/newStegosaurus.bmp");
            for (int i = 0; i < imageOld.Width; i++)
            {
                for (int j = 0; j < imageOld.Height; j++)
                {
                    System.Drawing.Color newpixel = imageNew.GetPixel(i, j);
                    System.Drawing.Color oldpixel = imageOld.GetPixel(i, j);
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

            imageOld.Dispose();
            imageNew.Dispose();
            return result;
        }

    }
}
