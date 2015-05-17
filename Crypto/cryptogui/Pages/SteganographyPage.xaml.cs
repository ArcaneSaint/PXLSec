using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Crypto;
using Microsoft.Win32;

namespace cryptogui.Pages
{
	/// <summary>
	/// Interaction logic for SteganographyPage.xaml
	/// </summary>
	public partial class SteganographyPage : UserControl
	{
		string file = null;
		string resultFile = null;
		public SteganographyPage()
		{
			InitializeComponent();
		}

		private void btn_ImageSelect_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.DefaultExt = ".txt";
			ofd.Filter = "Image files (*.raw, *.bmp, *.png) | *.raw; *.bmp; *.png";

			// Display OpenFileDialog by calling ShowDialog method
			Nullable<bool> result = ofd.ShowDialog();

			// Get the selected file name and display in a TextBox
			if (result == true)
			{
				// Open document
				file = ofd.FileName;
				img_Image.Source = new BitmapImage(new Uri(ofd.FileName));
				string filename = ofd.SafeFileName;
				txtbox_ImageSelect.Text = filename;
			}
		}

		private void btn_Encrypt_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.DefaultExt = ".png";
			sfd.Filter = "Bitmap (*.bmp) | *.bmp | Portable Network Graphics (*.png) | *.png";
			// Display SaveFileDialog by calling ShowDialog method
			Nullable<bool> result = sfd.ShowDialog();
			if (result == true)
			{
				// Save image
				SteganoCrypto sg = new SteganoCrypto();
				string text = txtbox_Message.Text;
				sg.EncodeSavePicture(text, file, sfd.FileName);
				//r.Save(sfd.FileName);

				resultFile = sfd.FileName;
				img_ResultImage.Source = new BitmapImage(new Uri(sfd.FileName));
				string filename = sfd.SafeFileName;
				txtbox_ResultImageSelect.Text = filename;

				txtbox_Message.Text = "Message ecrypted in file";
			}
			//image.Save(@"../../Images/newStegosaurus.bmp")
			//Dialog
			//Bitmap result = sg.EncodePicture();
		}

		private void btn_Decrypt_Click(object sender, RoutedEventArgs e)
		{
			SteganoCrypto sg = new SteganoCrypto();
			string message = sg.decodePicture(resultFile, file);
			txtbox_Message.Text = message;
		}

		private void btn_ResultImageSelect_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.DefaultExt = ".png";
			ofd.Filter = "Image files (*.raw, *.bmp, *.png) | *.raw; *.bmp; *.png";

			// Display OpenFileDialog by calling ShowDialog method
			Nullable<bool> result = ofd.ShowDialog();

			// Get the selected file name and display in a TextBox
			if (result == true)
			{
				// Open document
				resultFile = ofd.FileName;
				img_ResultImage.Source = new BitmapImage(new Uri(ofd.FileName));
				string filename = ofd.SafeFileName;
				txtbox_ResultImageSelect.Text = filename;
			}
		}
	}
}

//Source="{StaticResource bmp_Source}"