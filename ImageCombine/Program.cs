using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
namespace ImageCombine
{
    class Program
    {
        private static void Main()
        {
            DirectoryInfo info = new DirectoryInfo("./images");
            List<string> files = new List<string>();
            var fileinfos = info.GetFiles();
            foreach (var f in fileinfos)
            {
                if (f.Extension.Equals(".png") || f.Extension.Equals(".jpg"))
                {
                    files.Add(f.FullName);
                    Console.WriteLine(f.FullName);
                }
            }

            Bitmap b = CombineBitmap(files.ToArray());

            string pathOut = "out";
            Directory.CreateDirectory(pathOut);
            Console.WriteLine();
            Console.WriteLine("Saving");
            Console.WriteLine();
            b.Save(Path.Combine(pathOut, "out.png"));
            Console.WriteLine();
            Console.WriteLine("Saved to");
            Console.WriteLine("out/out.png");
            Console.Read();
        }
        public static Bitmap CombineBitmap(string[] files)
        {
            //read all images into memory
            List<Bitmap> images = new List<Bitmap>();
            Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (string image in files)
                {
                    //create a Bitmap from the file and add it to the list
                    Bitmap bitmap = new Bitmap(image);

                    //update the size of the final bitmap
                    width += bitmap.Width;
                    height = bitmap.Height > height ? bitmap.Height : height;

                    images.Add(bitmap);
                }

                //create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(Color.Black);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (Bitmap image in images)
                    {
                        g.DrawImage(image,
                          new Rectangle(offset, 0, image.Width, image.Height));
                        offset += image.Width;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }
    }
}
