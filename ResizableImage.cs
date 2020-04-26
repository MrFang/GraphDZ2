using System.Drawing;

namespace GraphDZ2
{
    class ResizableImage
    {
        /**
         * splinesCoeficientsColor[y][coeffIdx][x]
        */
        private double[][][] splinesCoefficientsRed, splinesCoefficientsGreen, splinesCoefficientsBlue;
        private readonly Bitmap img;

        public ResizableImage(string path) {
            img = new Bitmap(path);

            // INIT
            splinesCoefficientsRed
                = splinesCoefficientsGreen
                = splinesCoefficientsBlue
                = new double[img.Height][][];
            
            for (int lineIdx = 0; lineIdx < img.Height; lineIdx++) {
                splinesCoefficientsRed[lineIdx]
                    = splinesCoefficientsGreen[lineIdx]
                    = splinesCoefficientsBlue[lineIdx]
                    = new double[3][];

                for (int coeffIdx = 0; coeffIdx < 3; coeffIdx++) {
                    splinesCoefficientsRed[lineIdx][coeffIdx] 
                        = splinesCoefficientsGreen[lineIdx][coeffIdx]
                        = splinesCoefficientsBlue[lineIdx][coeffIdx]
                        = new double[img.Width];
                }

                // Compute splines
                double[] x = new double[img.Width], y = new double[img.Width];

                // Red
                for (int i = 0; i < img.Width; i++) {
                    x[i] = i;
                    y[i] = img.GetPixel(i, lineIdx).R;
                }

                spline(
                    img.Width,
                    x,
                    y,
                    splinesCoefficientsRed[lineIdx][0],
                    splinesCoefficientsRed[lineIdx][1],
                    splinesCoefficientsRed[lineIdx][2]
                );

                // Green
                for (int i = 0; i < img.Width; i++) {
                    y[i] = img.GetPixel(i, lineIdx).G;
                }

                spline(
                    img.Width,
                    x,
                    y,
                    splinesCoefficientsGreen[lineIdx][0],
                    splinesCoefficientsGreen[lineIdx][1],
                    splinesCoefficientsGreen[lineIdx][2]
                );

                // Blue
                for (int i = 0; i < img.Width; i++) {
                    y[i] = img.GetPixel(i, lineIdx).B;
                }

                spline(
                    img.Width,
                    x,
                    y,
                    splinesCoefficientsBlue[lineIdx][0],
                    splinesCoefficientsBlue[lineIdx][1],
                    splinesCoefficientsBlue[lineIdx][2]
                );
            }

        }

        public Bitmap getImage(byte scaleFactor) {
            Bitmap newImg = new Bitmap(img.Width * scaleFactor, img.Height * scaleFactor);

            // Масштабирование по горизонтали
            for (int j = 0; j < img.Height; j++) {
                double[] x = new double[img.Width];
                double[] yRed = new double[img.Width], yGreen = new double[img.Width], yBlue = new double[img.Width];

                for (int i = 0; i < img.Width; i++) {
                    x[i] = i;
                    yRed[i] = img.GetPixel(i, j).R;
                    yGreen[i] = img.GetPixel(i, j).G;
                    yBlue[i] = img.GetPixel(i, j).B;
                }

                for (int i = 0; i < newImg.Width; i++) {
                    int red = (int)seval(
                        img.Width,
                        (double)i / scaleFactor,
                        x,
                        yRed,
                        splinesCoefficientsRed[j][0],
                        splinesCoefficientsRed[j][1],
                        splinesCoefficientsRed[j][2]
                    );

                    int green = (int)seval(
                        img.Width,
                        (double)i / scaleFactor,
                        x,
                        yGreen,
                        splinesCoefficientsGreen[j][0],
                        splinesCoefficientsGreen[j][1],
                        splinesCoefficientsGreen[j][2]
                    );

                    int blue = (int)seval(
                        img.Width,
                        (double)i / scaleFactor,
                        x,
                        yBlue,
                        splinesCoefficientsBlue[j][0],
                        splinesCoefficientsBlue[j][1],
                        splinesCoefficientsBlue[j][2]
                    );

                    if (red > 255 || red < 0) {
                        if (red < 0) {
                            red = 0;
                        } else {
                            red = 255;
                        }
                    }

                    if (green > 255 || green < 0) {
                        if (green < 0) {
                            green = 0;
                        } else {
                            green = 255;
                        }
                    }

                    if (blue > 255 || blue < 0) {
                        if (blue < 0) {
                            blue = 0;
                        } else {
                            blue = 255;
                        }
                    }

                    newImg.SetPixel(i, j*scaleFactor, Color.FromArgb(red, green, blue));
                }
            }

            // Compute Y splines
            double[][][] ySplinesCoefficientsRed, ySplinesCoefficientsGreen, ySplinesCoefficientsBlue;

            /**
             * ySplinesCoefficientsColor[x][coefIdx][y]
            */
            ySplinesCoefficientsRed
                = ySplinesCoefficientsGreen
                = ySplinesCoefficientsBlue
                = new double[newImg.Width][][];

            for (int colIdx = 0; colIdx < newImg.Width; colIdx++) {

                ySplinesCoefficientsRed[colIdx]
                    = ySplinesCoefficientsGreen[colIdx]
                    = ySplinesCoefficientsBlue[colIdx]
                    = new double[3][];

                for (int coeffIdx = 0; coeffIdx < 3; coeffIdx++) {
                    ySplinesCoefficientsRed[colIdx][coeffIdx]
                        = ySplinesCoefficientsGreen[colIdx][coeffIdx]
                        = ySplinesCoefficientsBlue[colIdx][coeffIdx]
                        = new double[img.Height];
                }

                double[] x = new double[img.Height];
                double[] yRed = new double[img.Height], yGreen = new double[img.Height], yBlue = new double[img.Height];

                for (int i = 0; i < img.Height; i++) {
                    x[i] = i;
                    yRed[i] = newImg.GetPixel(colIdx, i * scaleFactor).R;
                    yGreen[i] = newImg.GetPixel(colIdx, i * scaleFactor).G;
                    yBlue[i] = newImg.GetPixel(colIdx, i * scaleFactor).B;
                }

                // Red
                spline(
                    img.Height,
                    x,
                    yRed,
                    ySplinesCoefficientsRed[colIdx][0],
                    ySplinesCoefficientsRed[colIdx][1],
                    ySplinesCoefficientsRed[colIdx][2]
                );

                // Green
                spline(
                    img.Height,
                    x,
                    yGreen,
                    ySplinesCoefficientsGreen[colIdx][0],
                    ySplinesCoefficientsGreen[colIdx][1],
                    ySplinesCoefficientsGreen[colIdx][2]
                );

                // Blue
                spline(
                    img.Height,
                    x,
                    yBlue,
                    ySplinesCoefficientsBlue[colIdx][0],
                    ySplinesCoefficientsBlue[colIdx][1],
                    ySplinesCoefficientsBlue[colIdx][2]
                );
            }

            // Масштабирование по вериткали
            for (int i = 0; i < newImg.Width; i++) {
                double[] x = new double[img.Height];
                double[] yRed = new double[img.Height], yGreen = new double[img.Height], yBlue = new double[img.Height];

                for (int j = 0; j < img.Height; j++) {
                    x[j] = j;
                    yRed[j] = newImg.GetPixel(i, j * scaleFactor).R;
                    yGreen[j] = newImg.GetPixel(i, j * scaleFactor).G;
                    yBlue[j] = newImg.GetPixel(i, j * scaleFactor).B;
                }

                for (int j = 0; j < newImg.Height; j++) {
                    int red = (int)seval(
                        img.Height,
                        (double)j / scaleFactor,
                        x,
                        yRed,
                        ySplinesCoefficientsRed[i][0],
                        ySplinesCoefficientsRed[i][1],
                        ySplinesCoefficientsRed[i][2]
                    );

                    int green = (int)seval(
                        img.Height,
                        (double)j / scaleFactor,
                        x,
                        yGreen,
                        ySplinesCoefficientsGreen[i][0],
                        ySplinesCoefficientsGreen[i][1],
                        ySplinesCoefficientsGreen[i][2]
                    );

                    int blue = (int)seval(
                        img.Height,
                        (double)j / scaleFactor,
                        x,
                        yBlue,
                        ySplinesCoefficientsBlue[i][0],
                        ySplinesCoefficientsBlue[i][1],
                        ySplinesCoefficientsBlue[i][2]
                    );

                    if (red > 255 || red < 0) {
                        if (red < 0) {
                            red = 0;
                        } else {
                            red = 255;
                        }
                    }

                    if (green > 255 || green < 0) {
                        if (green < 0) {
                            green = 0;
                        } else {
                            green = 255;
                        }
                    }

                    if (blue > 255 || blue < 0) {
                        if (blue < 0) {
                            blue = 0;
                        } else {
                            blue = 255;
                        }
                    }

                    newImg.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            }

            return newImg;
        }

        void spline(int n, double[] x, double[] y, double[] b, double[] c, double[] d ) {
            int nm, i;
            double t;

            nm = n - 1;

            if (n < 3) {
                b[0] = (y[1] - y[0]) / (x[1] - x[0]);
                c[0] = 0;
                d[0] = 0;
                b[1] = b[0];
                c[1] = 0;
                d[1] = 0;
                
                return;
            };

            /*  Построить трехдиагональную схему
                B - Диагональ C - наддиагональ D - правые части
            */

            d[0] = x[1] - x[0];
            c[1] = (y[1] - y[0]) / d[0];

            for (i = 1; i < nm; i++) {
                d[i] = x[i + 1] - x[i];
                b[i] = 2.0 * (d[i - 1] + d[i]);
                c[i + 1] = (y[i + 1] - y[i]) / d[i];
                c[i] = c[i + 1] - c[i];
            }

            /*
                Граничные условия
            */
            b[0] = -d[0];
            b[nm] = -d[nm - 1];
            c[0] = c[nm] = 0.0;

            if (n > 3) {
                c[0] = c[2] / (x[3] - x[1]) - c[1] / (x[2] - x[0]);
                c[nm] = c[n - 2] / (x[nm] - x[n - 3]) - c[n - 3] / (x[n - 2] - x[n - 4]);
                c[0] = c[0] * d[0] * d[0] / (x[3] - x[0]);
                c[nm] = -c[nm] * d[n - 2] * d[n - 2] / (x[nm] - x[n - 4]);
            }

            /*
                Прямой ход
            */

            for (i = 1; i < n; i++) {
                t = d[i - 1] / b[i - 1];
                b[i] = b[i] - t * d[i - 1];
                c[i] = c[i] - t * c[i - 1];
            }

            /*
                Обратная подстановка
            */

            c[nm] = c[nm] / b[nm];

            for (i = nm - 1; i >= 0; i--)
                c[i] = (c[i] - d[i] * c[i + 1]) / b[i];

            /*
                Вычислить коэффициэнты полиномов
            */

            b[nm] = (y[nm] - y[nm - 1]) / d[nm - 1] + d[nm - 1] * (c[nm - 1] + 2.0 * c[nm]);

            for (i = 0; i < nm; i++) {
                b[i] = (y[i + 1] - y[i]) / d[i] - d[i] * (c[i + 1] + 2 * c[i]);
                d[i] = (c[i + 1] - c[i]) / d[i];
                c[i] = 3.0 * c[i];
            }

            c[nm] = 3 * c[nm];
            d[nm] = d[nm - 1];
            
        }

        double seval(int n, double u, double[] x, double[] y, double[] b, double[] c, double[] d ) {
            /*
                входная информация...

                  N       - число заданных точек
                  U       - абсцисса, для которой вычисляется
                           значение сплайна
                  X, Y    - массивы заданных абсцисс и ординат
                  B, C, D - массивы коэффициентов сплайна
            */
            int i = 0, j = 0, k = 0;
            double dx, result;

            if (u <= x[1]) goto end;

            i = 0;
            j = n;

            /*  Двоичный поиск  */

            do {
                k = (j + i) / 2;
                if (u < x[k]) j = k;
                else i = k;
            } while (j > i + 1);

        /*  Вычислить сплайн  */

        end:
            dx = u - x[i];
            result = y[i] + dx * (b[i] + dx * (c[i] + dx * d[i]));
            return (result);
        }
    }
}
