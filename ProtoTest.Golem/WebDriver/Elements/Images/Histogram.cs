﻿using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Golem.WebDriver.Elements.Images
{
    /// <summary>
    ///     A class which facilitates working with RGB histograms
    ///     It encapsulates a Bitmap and lets you get information about the Bitmap
    /// </summary>
    public class Histogram
    {
        private static readonly Pen[] p = {Pens.Red, Pens.Green, Pens.Blue};

        public Histogram(Bitmap bitmap)
        {
            Bitmap = bitmap;
            Red = new byte[256];
            Green = new byte[256];
            Blue = new byte[256];
            CalculateHistogram();
        }

        /// <summary>
        ///     Constructs a new Histogram from a file, given its path
        /// </summary>
        /// <param name="filePath">The path to the image to work with</param>
        public Histogram(string filePath) : this((Bitmap) Image.FromFile(filePath))
        {
        }

        /// <summary>
        ///     The red values in an image
        /// </summary>
        public byte[] Red { get; private set; }

        /// <summary>
        ///     The green values in an image
        /// </summary>
        public byte[] Green { get; private set; }

        /// <summary>
        ///     The blue values in an image
        /// </summary>
        public byte[] Blue { get; private set; }

        /// <summary>
        ///     The bitmap to get histogram info for
        /// </summary>
        public Bitmap Bitmap { get; private set; }

        /// <summary>
        ///     Calculates the values in the histogram
        /// </summary>
        private void CalculateHistogram()
        {
            var newBmp = (Bitmap) Bitmap.Resize(16, 16);
            Color c;
            for (var x = 0; x < newBmp.Width; x++)
            {
                for (var y = 0; y < newBmp.Height; y++)
                {
                    c = newBmp.GetPixel(x, y);
                    Red[c.R]++;
                    Green[c.G]++;
                    Blue[c.B]++;
                }
            }
        }

        /// <summary>
        ///     Gets a bitmap with the RGB histograms
        /// </summary>
        /// <returns>Three histograms for R, G and B values in the Histogram</returns>
        public Bitmap Visualize()
        {
            var oneColorHeight = 100;
            var margin = 10;

            float[] maxValues = {Red.Max(), Green.Max(), Blue.Max()};
            byte[][] values = {Red, Green, Blue};


            var histogramBitmap = new Bitmap(276, oneColorHeight*3 + margin*4);
            var g = Graphics.FromImage(histogramBitmap);
            g.FillRectangle(Brushes.White, 0, 0, histogramBitmap.Width, histogramBitmap.Height);
            var yOffset = margin + oneColorHeight;

            for (var i = 0; i < 256; i++)
            {
                for (var color = 0; color < 3; color++)
                {
                    g.DrawLine(p[color], margin + i, yOffset*(color + 1), margin + i,
                        yOffset*(color + 1) - (values[color][i]/maxValues[color])*oneColorHeight);
                }
            }

            for (var i = 0; i < 3; i++)
            {
                g.DrawString(p[i].Color.ToKnownColor() + ", max value: " + maxValues[i], SystemFonts.SmallCaptionFont,
                    Brushes.Silver, margin + 11, yOffset*i + margin + margin + 1);
                g.DrawString(p[i].Color.ToKnownColor() + ", max value: " + maxValues[i], SystemFonts.SmallCaptionFont,
                    Brushes.Black, margin + 10, yOffset*i + margin + margin);
                g.DrawRectangle(p[i], margin, yOffset*i + margin, 256, oneColorHeight);
            }
            g.Dispose();

            return histogramBitmap;
        }

        /// <summary>
        ///     Gives a human-readable representation of the RGB values in the histogram
        /// </summary>
        /// <returns>a human-readable representation of the RGB values in the histogram</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < 256; i++)
            {
                sb.Append(string.Format("RGB {0,3} : ", i) +
                          string.Format("({0,3},{1,3},{2,3})", Red[i], Green[i], Blue[i]));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Gets the variance between two histograms (http://en.wikipedia.org/wiki/Variance) as a percentage of the maximum
        ///     possible variance: 256 (for a white image compared to a black image)
        /// </summary>
        /// <param name="histogram">the histogram to compare this one to</param>
        /// <returns>A percentage which tells how different the two histograms are</returns>
        public float GetVariance(Histogram histogram)
        {
            //
            double diffRed = 0, diffGreen = 0, diffBlue = 0;
            for (var i = 0; i < 256; i++)
            {
                diffRed += Math.Pow(Red[i] - histogram.Red[i], 2);
                diffGreen += Math.Pow(Green[i] - histogram.Green[i], 2);
                diffBlue += Math.Pow(Blue[i] - histogram.Blue[i], 2);
            }

            diffRed /= 256;
            diffGreen /= 256;
            diffBlue /= 256;
            const double maxDiff = 512;
            return (float) (diffRed/maxDiff + diffGreen/maxDiff + diffBlue/maxDiff)/3;
        }
    }
}