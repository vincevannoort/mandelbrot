using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mandelbrot
{
    public class Mandelbrot : Form
    {
        // variables for interface
        TextBox inputIterations = new TextBox();

        // variables for mandelbrotview
        Bitmap MandelbrotView;
		int pixelWidth = 1000, pixelHeight = 1000, margin = 50;
        int limit = 20;
        float width, height;
        float xmin, ymin, xmax, ymax;

        public Mandelbrot()
        {
            // set interace variables
            inputIterations.Size = new Size(200, 30);
            inputIterations.Location = new Point(pixelWidth - inputIterations.Width - margin, margin - inputIterations.Height/2);
            inputIterations.BackColor = Color.White;
            inputIterations.Text = "128";
            this.Controls.Add(inputIterations);

            // set form variables
            this.Text = "Mandelbrot";
            this.Size = new Size(this.pixelWidth, this.pixelHeight);

            // initialise values
            this.width = 2;
            this.height = 2;
            this.xmin = -(this.width) / 2;
            this.ymin = -(this.height) / 2;
            this.xmax = xmin + width;
            this.ymax = ymin + height;

            // create event handler to draw the mandelbrot
            this.Paint += this.DrawMandelbrot;
        }

		void DrawMandelbrot(Object obj, PaintEventArgs pea)
		{
            // create bitmap object for view
			MandelbrotView = new Bitmap(this.pixelWidth, this.pixelHeight);

            // Example of how to set a pixel
            for (int i = 0; i < this.pixelWidth; i++)
            {
                for (int j = 0; j < this.pixelHeight; j++)
                {
                    int MandelBrotNumber = this.CalculateMandelbrot(i, j);
                    MandelbrotView.SetPixel(i,j, Color.FromArgb(MandelBrotNumber % 128 * 2, MandelBrotNumber % 32 * 7, MandelBrotNumber % 16 * 14));
                }
            }


            // after all pixels are set, add the image to the view
            pea.Graphics.DrawImage(MandelbrotView, 0, 0, this.pixelWidth, this.pixelHeight);
		}

        int CalculateMandelbrot(int i, int j)
        {
            // convert pixels to scale
            float a = this.MapRange(i, 0, this.pixelWidth, xmin, xmax);
            float b = this.MapRange(j, 0, this.pixelHeight, ymin, ymax);

            // preserve for later
            float x = a;
            float y = b;

            // temporary for debugging
            if (i == 0 && j == 0)
            {
                this.Text = a.ToString() + "|" + b.ToString();
			}

            // f (a,b) = (a*a-b*b+x, 2*a*b+y)
            int iteration = 0;
            int iterations = int.Parse(this.inputIterations.Text);
            while(iteration < iterations)
            {
                float ta = a; // temporary
                float tb = b; // temporary
                a = (ta * ta - tb * tb) + x;
                b = 2 * ta * tb + y;

                if ((a*a + b*b) > this.limit)
                {
                    break;
                }
                iteration++;
            }

            return (int) MapRange(iteration, 0, iterations, 0, 255);
        }

		// credits to: https://stackoverflow.com/questions/929103/convert-a-number-range-to-another-range-maintaining-ratio
		float MapRange(float number, float min, float max, float newMin, float newMax)
        {
            float oldRange = (max - min);
            float newRange = (newMax - newMin);
            return (((number - min) * newRange) / oldRange) + newMin;
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
