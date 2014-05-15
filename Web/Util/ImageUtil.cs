// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace DigitalBeacon.Web.Util
{
	public enum RotateDirection
	{
		Clockwise,
		Counterclockwise
	}

	public static class ImageUtil
	{
		public static byte[] GetPhotoData(HttpRequestBase request, out int width, out int height, int maxWidth = 0, int maxHeight = 0)
		{
			byte[] retVal = null;
			width = 0;
			height = 0;

			if (request.Files.Count == 1 && request.Files[0].ContentLength > 0)
			{
				var image = Image.FromStream(request.Files[0].InputStream);
				if (maxWidth > 0 && maxHeight > 0 && (image.Width > maxWidth || image.Height > maxHeight))
				{
					image = Resize(image, new Size(maxWidth, maxHeight));
				}
				var encoderParameters = new EncoderParameters(1);
				encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
				var outputStream = new MemoryStream();
				image.Save(outputStream, GetEncoderInfo("image/jpeg"), encoderParameters);
				width = image.Width;
				height = image.Height;
				retVal = outputStream.ToArray();
			}
			return retVal;
		}

		public static Image Resize(Image image, Size size)
		{
			float nPercent = Math.Min((float)size.Width / (float)image.Width, (float)size.Height / (float)image.Height);
			int destWidth = (int)(image.Width * nPercent);
			int destHeight = (int)(image.Height * nPercent);
			var bitmap = new Bitmap(destWidth, destHeight);
			using (var graphics = Graphics.FromImage((Image)bitmap))
			{
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(image, 0, 0, destWidth, destHeight);
			}
			return (Image)bitmap;
		}

		public static byte[] Rotate(byte[] imageBytes, RotateDirection direction)
		{
			var image = Image.FromStream(new MemoryStream(imageBytes));
			image.RotateFlip(direction == RotateDirection.Clockwise ? RotateFlipType.Rotate90FlipNone : RotateFlipType.Rotate270FlipNone);
			var encoderParameters = new EncoderParameters(1);
			encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
			var outputStream = new MemoryStream();
			image.Save(outputStream, GetEncoderInfo("image/jpeg"), encoderParameters);
			return outputStream.ToArray();
		}

		private static ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			foreach (var encoder in ImageCodecInfo.GetImageEncoders())
			{
				if (encoder.MimeType == mimeType)
				{
					return encoder;
				}
			}
			return null;
		}
	}
}