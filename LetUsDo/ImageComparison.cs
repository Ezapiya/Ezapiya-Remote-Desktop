using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Collections;

namespace LetUsDo
{
    public static class ImageComparison
    {
        //the colormatrix needed to grayscale an image
        //http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
        static readonly ColorMatrix ColorMatrix = new ColorMatrix(new float[][]
        {
            new float[] {.3f, .3f, .3f, 0, 0},
            new float[] {.59f, .59f, .59f, 0, 0},
            new float[] {.11f, .11f, .11f, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {0, 0, 0, 0, 1}
        });

        /// <summary>
        /// Gets the difference between two images as a percentage
        /// </summary>
        /// <param name="img1">The first image</param>
        /// <param name="img2">The image to compare to</param>
        /// <param name="threshold">How big a difference (out of 255) will be ignored - the default is 3.</param>
        /// <returns>The difference between the two images as a percentage</returns>
        public static float PercentageDifference(this Image img1, Image img2, byte threshold = 3)
        {
            byte[,] differences = img1.GetDifferences(img2);

            int diffPixels = 0;

            foreach (byte b in differences)
            {
                if (b > threshold)
                {
                    diffPixels++;
                    break;
                }
            }
            return diffPixels;
            //return diffPixels / 256f;
        }

        /// <summary>
        /// The Bhattacharyya difference (the difference between normalized versions of the histograms of both images)
        /// This tells something about the differences in the brightness of the images as a whole, not so much about where they differ.
        /// </summary>
        /// <param name="img1">The first image to compare</param>
        /// <param name="img2">The second image to compare</param>
        /// <returns>The difference between the images' normalized histograms</returns>
        public static float BhattacharyyaDifference(this Image img1, Image img2)
        {
            byte[,] img1GrayscaleValues = img1.GetGrayScaleValues();
            byte[,] img2GrayscaleValues = img2.GetGrayScaleValues();

            var normalizedHistogram1 = new double[16, 16];
            var normalizedHistogram2 = new double[16, 16];

            double histSum1 = 0.0;
            double histSum2 = 0.0;

            foreach (var value in img1GrayscaleValues) { histSum1 += value; }
            foreach (var value in img2GrayscaleValues) { histSum2 += value; }


            for (int x = 0; x < img1GrayscaleValues.GetLength(0); x++)
            {
                for (int y = 0; y < img1GrayscaleValues.GetLength(1); y++)
                {
                    normalizedHistogram1[x, y] = (double)img1GrayscaleValues[x, y] / histSum1;
                }
            }
            for (int x = 0; x < img2GrayscaleValues.GetLength(0); x++)
            {
                for (int y = 0; y < img2GrayscaleValues.GetLength(1); y++)
                {
                    normalizedHistogram2[x, y] = (double)img2GrayscaleValues[x, y] / histSum2;
                }
            }

            double bCoefficient = 0.0;
            for (int x = 0; x < img2GrayscaleValues.GetLength(0); x++)
            {
                for (int y = 0; y < img2GrayscaleValues.GetLength(1); y++)
                {
                    double histSquared = normalizedHistogram1[x, y] * normalizedHistogram2[x, y];
                    bCoefficient += Math.Sqrt(histSquared);
                }
            }

            double dist1 = 1.0 - bCoefficient;
            dist1 = Math.Round(dist1, 8);
            double distance = Math.Sqrt(dist1);
            distance = Math.Round(distance, 8);
            return (float)distance;

        }






        /// <summary>
        /// Finds the differences between two images and returns them in a doublearray
        /// </summary>
        /// <param name="img1">The first image</param>
        /// <param name="img2">The image to compare with</param>
        /// <returns>the differences between the two images as a doublearray</returns>
        public static byte[,] GetDifferences(this Image img1, Image img2)
        {
            /* Bitmap thisOne = (Bitmap)img1.Resize(16, 16).GetGrayScaleVersion();
             Bitmap theOtherOne = (Bitmap)img2.Resize(16, 16).GetGrayScaleVersion();
             byte[,] differences = new byte[16, 16];
             byte[,] firstGray = thisOne.GetGrayScaleValues();
             byte[,] secondGray = theOtherOne.GetGrayScaleValues();

             for (int y = 0; y < 16; y++)
             {
                 for (int x = 0; x < 16; x++)
                 {
                     differences[x, y] = (byte)Math.Abs(firstGray[x, y] - secondGray[x, y]);
                 }
             }
             thisOne.Dispose();
             theOtherOne.Dispose();
             return differences;*/


            Bitmap thisOne = (Bitmap)img1.Resize(64, 64).GetGrayScaleVersion();
            Bitmap theOtherOne = (Bitmap)img2.Resize(64, 64).GetGrayScaleVersion();
            byte[,] differences = new byte[64, 64];
            byte[,] firstGray = thisOne.GetGrayScaleValues();
            byte[,] secondGray = theOtherOne.GetGrayScaleValues();

            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    differences[x, y] = (byte)Math.Abs(firstGray[x, y] - secondGray[x, y]);
                }
            }
            thisOne.Dispose();
            theOtherOne.Dispose();
            return differences;

        }

        /// <summary>
        /// Gets the lightness of the image in 256 sections (16x16)
        /// </summary>
        /// <param name="img">The image to get the lightness for</param>
        /// <returns>A doublearray (16x16) containing the lightness of the 256 sections</returns>
        public static byte[,] GetGrayScaleValues(this Image img)
        {
            using (Bitmap thisOne = (Bitmap)img.Resize(64, 64).GetGrayScaleVersion())
            {
                byte[,] grayScale = new byte[64, 64];

                for (int y = 0; y < 64; y++)
                {
                    for (int x = 0; x < 64; x++)
                    {
                        grayScale[x, y] = (byte)Math.Abs(thisOne.GetPixel(x, y).R);
                    }
                }
                return grayScale;
            }
        }


        /// <summary>
        /// Converts an image to grayscale
        /// </summary>
        /// <param name="original">The image to grayscale</param>
        /// <returns>A grayscale version of the image</returns>
        public static Image GetGrayScaleVersion(this Image original)
        {
            //http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                //create some image attributes
                ImageAttributes attributes = new ImageAttributes();

                //set the color matrix attribute
                attributes.SetColorMatrix(ColorMatrix);

                //draw the original image on the new image
                //using the grayscale color matrix
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                   0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }
            return newBitmap;

        }

        /// <summary>
        /// Resizes an image
        /// </summary>
        /// <param name="originalImage">The image to resize</param>
        /// <param name="newWidth">The new width in pixels</param>
        /// <param name="newHeight">The new height in pixels</param>
        /// <returns>A resized version of the original image</returns>
        public static Image Resize(this Image originalImage, int newWidth, int newHeight)
        {
            Image smallVersion = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(smallVersion))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return smallVersion;
        }

        /// <summary>
        /// Helpermethod to print a doublearray of 
        /// </summary>
        /// <typeparam name="T">The type of doublearray</typeparam>
        /// <param name="doubleArray">The doublearray to print</param>
        public static void ToConsole<T>(this T[,] doubleArray)
        {
            for (int y = 0; y < doubleArray.GetLength(0); y++)
            {
                Console.Write("[");
                for (int x = 0; x < doubleArray.GetLength(1); x++)
                {
                    Console.Write(string.Format("{0,3},", doubleArray[x, y]));
                }
                Console.WriteLine("]");
            }
        }

    }
}
