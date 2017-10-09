using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mandelbrot
{
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
            this.BackColor = Color.FromArgb(9, 21, 41);
			this.ForeColor = Color.White;
			this.Location = new Point(
				pixelWidth - textboxWidth - margin - 60,
				margin + 12 - this.Height / 2 + offset
			); ;
		}
	}

    public class ButtonWithConstructor : Button
    {
        public ButtonWithConstructor(string text, int x, int y)
        {
            this.Text = text;
            this.Location = new Point(x, y);
            this.BackColor = Color.White;
        }
    }
}
