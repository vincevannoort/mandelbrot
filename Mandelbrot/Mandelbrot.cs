using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mandelbrot
{
    public class Mandelbrot : Form
    {
        
        Bitmap MandelbrotView;

		int pixelWidth = 500, pixelHeight = 500;
        float width, height;
        float xmin, ymin;

        public Mandelbrot()
        {
            // set form variables
            this.Text = "Mandelbrot";
            this.Size = new Size(this.pixelWidth, this.pixelHeight);

            // initialise values
            this.width = 4;
            this.height = 4;
            this.xmin = -(this.width) / 2;
            this.ymin = -(this.height) / 2;

            // create event handler to draw the mandelbrot
            this.Paint += this.DrawMandelbrot;
        }

		void DrawMandelbrot(Object obj, PaintEventArgs pea)
		{
            // create bitmap object for view
			MandelbrotView = new Bitmap(this.pixelWidth, this.pixelHeight);

            // Example of how to set a pixel
            //MandelbrotView.SetPixel(20, 20, Color.Blue);
            for (int i = 0; i < this.pixelWidth; i++)
            {
                for (int j = 0; j < this.pixelHeight; j++)
                {
                    this.CalculateMandelbrot(i, j);
                }
            }


            // after all pixels are set, add the image to the view
            pea.Graphics.DrawImage(MandelbrotView, 0,0, 500, 500);
		}

        float CalculateMandelbrot(int i, int j)
        {
            // convert pixels to scale
            float a = this.MapRange(i, xmin, xmin + width, 0, this.pixelWidth);
            float b = this.MapRange(j, ymin, ymin + height, 0, this.pixelHeight);
            this.Text = a.ToString() + "|" + b.ToString();
            return 0.2F;
        }

		// credits to: https://stackoverflow.com/questions/4229662/convert-numbers-within-a-range-to-numbers-within-another-range
		float MapRange(float number, float start, float end, float newStart, float newEnd)
        {
            float scale = (float)(newEnd - newStart) / (start - end);
            return (newStart + ((number - start) * scale));
        }

    }

    public class Program
    {
        public static void Main()
        {
            Mandelbrot m = new Mandelbrot();
            Application.Run(m);
        }
    }
}
