using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mandelbrot
{
    public class Mandelbrot : Form
    {
		// variables for mandelbrotview
		Bitmap MandelbrotView;
		int pixelWidth = 800, pixelHeight = 800, margin = 20, offset = 30;
		int limit = 20;
		float width, height;
		float xmin, ymin, xmax, ymax;

        // variables for colors
        int rmod = 128, rmul = 2, gmod = 32, gmul = 7, bmod = 16, bmul = 14;


        // variables for interface
        TextBox inputIterations = new TextboxWithConstructor(90, 30, 0, 20, 800, "128");
        TextBox inputCenterX = new TextboxWithConstructor(90, 30, 30, 20, 800, "-0,35");
        TextBox inputCenterY = new TextboxWithConstructor(90, 30, 60, 20, 800, "0");
        TextBox inputZoom = new TextboxWithConstructor(90, 30, 90, 20, 800, "1");
        Label labelIterations = new LabelWithConstructor("Iterations", 0, 20, 800, 90);
        Label labelCenterX = new LabelWithConstructor("Center X", 30, 20, 800, 90);
        Label labelCenterY = new LabelWithConstructor("Center Y", 60, 20, 800, 90);
        Label labelZoom = new LabelWithConstructor("Zoomlevel", 90, 20, 800, 90);

        public Mandelbrot()
        {
            System.Diagnostics.Debug.WriteLine(String.Format("Example debug string: {0}", "here!"));

            // setup menu
            this.InitializeMenuItems();

            // set interace variables
            this.Controls.Add(labelIterations);
            this.Controls.Add(inputIterations);
            this.Controls.Add(labelCenterX);
            this.Controls.Add(inputCenterX);
            this.Controls.Add(labelCenterY);
            this.Controls.Add(inputCenterY);
            this.Controls.Add(labelZoom);
            this.Controls.Add(inputZoom);

            // set form variables
            this.Text = "Mandelbrot";
            this.Size = new Size(this.pixelWidth, this.pixelHeight + this.margin + this.offset);

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
            this.xmin = centerX + (-(this.width) / 2);
            this.ymin = centerY + (-(this.height) / 2);
            this.xmax = centerX + ((this.width) / 2);
            this.ymax = centerY + ((this.height) / 2);

            this.Text = "xmin " + xmin.ToString() + " | ymin " + xmin.ToString() +  " | xmax " + xmax.ToString() + " | ymax " + ymax.ToString();

            // create bitmap object for view
			MandelbrotView = new Bitmap(this.pixelWidth, this.pixelHeight);

            // Example of how to set a pixel
            for (int i = 0; i < this.pixelWidth; i++)
            {
                for (int j = 0; j < this.pixelHeight; j++)
                {
                    int MandelBrotNumber = this.CalculateMandelbrot(i, j);
                    MandelbrotView.SetPixel(i,j, Color.FromArgb(MandelBrotNumber % rmod * rmul, MandelBrotNumber % gmod * gmul, MandelBrotNumber % bmod * bmul));
                }
            }

            // after all pixels are set, add the image to the view
            pea.Graphics.DrawImage(MandelbrotView, 0, 0, this.pixelWidth, this.pixelHeight);
			pea.Graphics.DrawRectangle(Pens.Red, this.pixelWidth / 2 - 10, this.pixelHeight / 2 - 10, 20, 20);
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

                if ((a*a + b*b) > this.limit*this.limit)
                {
                    break;
                }
                iteration++;
            }

            return (int) MapRange(iteration, 0, iterations, 0, 255);
        }

        void ChangeMandelbrotOrigin(Object obj, MouseEventArgs mea)
        {
            float tempX = this.MapRange(mea.X, 0, this.pixelWidth, this.xmin, this.xmax);
            float tempY = this.MapRange(mea.Y, 0, this.pixelHeight, this.ymin, this.ymax);

            this.inputCenterX.Text = tempX.ToString();
            this.inputCenterY.Text = tempY.ToString();

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
            menuAfbeelding.Click += this.CreateBitmap;

			MenuItem menuKleuren = new MenuItem("&Verander kleuren");
            MenuItem menuKleurStandaard = new MenuItem("&Standaard");
            menuKleuren.MenuItems.Add(menuKleurStandaard);
			MenuItem menuKleur1 = new MenuItem("&Kleur1");
			menuKleuren.MenuItems.Add(menuKleur1);
			MenuItem menuKleur2 = new MenuItem("&Kleur2");
			menuKleuren.MenuItems.Add(menuKleur2);
			MenuItem menuKleur3 = new MenuItem("&Kleur3");
			menuKleuren.MenuItems.Add(menuKleur3);

            MenuItem menuReset = new MenuItem("&Reset");
            menuReset.Click += this.Reset;

			mainMenu.MenuItems.Add(menuAfbeelding);
            mainMenu.MenuItems.Add(menuKleuren);
            mainMenu.MenuItems.Add(menuReset);

			this.Menu = mainMenu;
        }

        void Reset(Object obj, EventArgs ea)
        {
            this.inputCenterX.Text = (-0.35).ToString();
            this.inputCenterY.Text = (0).ToString();
            this.inputZoom.Text = (1).ToString();
            this.Invalidate();
        }

        void CreateBitmap(Object obj, EventArgs ea)
        {
            this.MandelbrotView.Save("mandelbrot.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
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

    public class TextboxWithConstructor : TextBox
    {
        public TextboxWithConstructor(int width, int height, int offset, int margin, int pixelWidth, string text)
        {
            this.Size = new Size(width, height);
            this.Location = new Point(
                pixelWidth - this.Width - margin,
                margin + 5 - this.Height / 2 + offset
            );
            this.BackColor = Color.White;
            this.Text = text;
        }
    }

    public class LabelWithConstructor : Label
    {
        public LabelWithConstructor(string text, int offset, int margin, int pixelWidth, int textboxWidth)
        {
            this.Text = text;
            this.Size = new Size(50, 30);
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.White;
			this.Location = new Point(
				pixelWidth - textboxWidth - margin - 60,
				margin + 12 - this.Height / 2 + offset
			); ;
        }
    }
}