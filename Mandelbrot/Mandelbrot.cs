using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mandelbrot
{
    public class Mandelbrot : Form
    {
        // variables for interface
        TextBox inputIterations = new TextBox();
        TextBox inputCenterX = new TextBox();
        TextBox inputCenterY = new TextBox();
        TextBox inputZoom = new TextBox();

        // variables for mandelbrotview
        Bitmap MandelbrotView;
		int pixelWidth = 800, pixelHeight = 800, margin = 50, offset = 30;
        int limit = 20;
        float width, height;
        float xmin, ymin, xmax, ymax;

        public Mandelbrot()
        {
            // setup menu
            this.InitializeMenuItems();

            // set interace variables
            inputIterations.Size = new Size(200, 30);
            inputIterations.Location = new Point(pixelWidth - inputIterations.Width - margin, margin - inputIterations.Height/2);
            inputIterations.BackColor = Color.White;
            inputIterations.Text = "128";
            this.Controls.Add(inputIterations);
            inputCenterX.Size = new Size(200, 30);
            inputCenterX.Location = new Point(pixelWidth - inputCenterX.Width - margin, margin - inputCenterX.Height/2 + offset);
            inputCenterX.BackColor = Color.White;
            inputCenterX.Text = "-0,35";
            this.Controls.Add(inputCenterX);
            inputCenterY.Size = new Size(200, 30);
            inputCenterY.Location = new Point(pixelWidth - inputCenterY.Width - margin, margin - inputCenterY.Height/2 + offset*2);
            inputCenterY.BackColor = Color.White;
            inputCenterY.Text = "0";
            this.Controls.Add(inputCenterY);
            inputZoom.Size = new Size(200, 30);
            inputZoom.Location = new Point(pixelWidth - inputZoom.Width - margin, margin - inputZoom.Height/2 + offset*3);
            inputZoom.BackColor = Color.White;
            inputZoom.Text = "1";
            this.Controls.Add(inputZoom);

            // set form variables
            this.Text = "Mandelbrot";
            this.Size = new Size(this.pixelWidth, this.pixelHeight);

            // create event handler to draw the mandelbrot
            this.Paint += this.DrawMandelbrot;
            this.MouseClick += this.ChangeMandelbrotOrigin;
        }

		void DrawMandelbrot(Object obj, PaintEventArgs pea)
		{
            // initialise values
            float centerX = float.Parse(this.inputCenterX.Text);
            float centerY = float.Parse(this.inputCenterY.Text);
            float zoom = float.Parse(this.inputZoom.Text);
            this.width = 3F / zoom;
            this.height = 3F / zoom;
            this.xmin = (-(this.width) / 2) + centerX;
            this.ymin = (-(this.height) / 2) + centerY;
            this.xmax = ((this.width) / 2) + centerX;
            this.ymax = ((this.height) / 2) + centerY;

            this.Text = "xmin " + xmin.ToString() + " | ymin " + xmin.ToString() +  " | xmax " + xmax.ToString() + " | ymax " + ymax.ToString();

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
            float a = this.MapRange(i, 0, this.pixelWidth, this.xmin, this.xmax);
            float b = this.MapRange(j, 0, this.pixelHeight, this.ymin, this.ymax);

            // preserve for later
            float x = a;
            float y = b;

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

        void ChangeMandelbrotOrigin(Object obj, MouseEventArgs mea)
        {
            //inputCenterX.Text = (this.MapRange(mea.X, 0, this.pixelWidth, this.xmin, this.xmax)).ToString();
            //inputCenterY.Text = (this.MapRange(mea.Y, 0, this.pixelHeight, this.xmin, this.xmax)).ToString();

            // left click
            if (mea.Button == System.Windows.Forms.MouseButtons.Left)
            {
                inputZoom.Text = (float.Parse(this.inputZoom.Text) * 1.35).ToString();
            }
            // right click
            else if(mea.Button == System.Windows.Forms.MouseButtons.Right) {
                inputZoom.Text = (float.Parse(this.inputZoom.Text) / 1.35).ToString();
            }
            this.Invalidate();
        }

        void InitializeMenuItems()
        {
			MainMenu mainMenu = new MainMenu();
			MenuItem menuAfbeelding = new MenuItem("&Afbeelding opslaan");
            MenuItem menuReset = new MenuItem("&Reset");

			mainMenu.MenuItems.Add(menuAfbeelding);
            mainMenu.MenuItems.Add(menuReset);

			this.Menu = mainMenu;
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
