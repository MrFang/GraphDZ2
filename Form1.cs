using System.Drawing;
using System.Windows.Forms;

namespace GraphDZ2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Form1(Bitmap img) {
            InitializeComponent();
            this.Size = img.Size;
            pictureBox1.Size = img.Size;
            pictureBox1.Image = img;
        }
    }
}
