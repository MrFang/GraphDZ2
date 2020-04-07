using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphDZ2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            double[][] img = {
                new double[]{ 1, 0, 0, 0 },
                new double[]{ 0, 1, 0, 0 },
                new double[]{ 0, 0, 1, 0 },
                new double[]{ 0, 0, 0, 1 }
            };
            ResizableImage image = new ResizableImage(img);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
