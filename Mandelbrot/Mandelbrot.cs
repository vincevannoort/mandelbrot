using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mandelbrot
{
    public class Mandelbrot : Form
    {
        
        Bitmap MandelbrotView;

		int width = 500;
		int height = 500;

        public Mandelbrot()
        {
            // set form variables
            this.Text = "Mandelbrot";
            this.Size = new Size(500, 500);

            // create event handler to draw the mandelbrot
            this.Paint += this.DrawMandelbrot;
        }

		void DrawMandelbrot(Object obj, PaintEventArgs pea)
		{
            // create bitmap object for view
			MandelbrotView = new Bitmap(this.width, this.height);

            // Example of how to set a pixel
            MandelbrotView.SetPixel(20, 20, Color.Blue);


            // after all pixels are set, add the image to the view
            pea.Graphics.DrawImage(MandelbrotView, 0,0, 500, 500);
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
