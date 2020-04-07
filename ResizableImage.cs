using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDZ2
{
    class ResizableImage
    {
        private readonly double[][] colorArray;
        private double[][][] splinesX;
        private double[][][] splinesY;

        public ResizableImage(double[][] image) {
            colorArray = image;

            // Init splines X
            splinesX = new double[image.Length - 3][][];
            for (int i = 0; i < image.Length - 3; i++) {
                splinesX[i] = new double[image[0].Length][];
            }

            // Init splines Y
            splinesY = new double[image.Length][][];
            for (int i = 0; i < image.Length; i++) {
                splinesY[i] = new double[image[0].Length - 3][];
            }

            computeSplines();
        }

        private void computeSplines() {
            // Init X splines
            for (int y = 0; y < colorArray[0].Length; ++y) {
                for (int x = 0; x < colorArray.Length - 3; ++x) {
                    double[][] matrix = {
                        new double[]{ Math.Pow(x, 3), Math.Pow(x, 2), x, 1},
                        new double[]{ Math.Pow(x+1, 3), Math.Pow(x+1, 2), x+1, 1},
                        new double[]{ Math.Pow(x+2, 3), Math.Pow(x+2, 2), x+2, 1},
                        new double[]{ Math.Pow(x+3, 3), Math.Pow(x+3, 2), x+3, 1},
                    };

                    double[] column = { 
                        colorArray[x][y],
                        colorArray[x+1][y],
                        colorArray[x+2][y],
                        colorArray[x+3][y],
                    };

                    splinesX[x][y] = Utils.gaussMethod(matrix, column);
                }
            }

            // Init Y splines
            for (int x = 0; x < colorArray.Length; ++x) {
                for (int y = 0; y < colorArray[0].Length - 3; ++y) {
                    double[][] matrix = {
                        new double[]{ Math.Pow(y, 3), Math.Pow(y, 2), y, 1 },
                        new double[]{ Math.Pow(y+1, 3), Math.Pow(y+1, 2), y+1, 1 },
                        new double[]{ Math.Pow(y+2, 3), Math.Pow(y+2, 2), y+2, 1 },
                        new double[]{ Math.Pow(y+3, 3), Math.Pow(y+3, 2), y+3, 1 },
                    };

                    double[] column = {
                        colorArray[x][y],
                        colorArray[x][y+1],
                        colorArray[x][y+2],
                        colorArray[x][y+3],
                    };

                    splinesY[x][y] = Utils.gaussMethod(matrix, column);
                }
            }
        }

        public double[][] getImage(int scaleFactor) {
            return null;
        }
    }
}
