using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDZ2
{
    class Utils
    {
        public static double[] gaussMethod(double[][] matrix, double[] column) {
            double[][] m = new double[matrix.Length][];
            double[] c = new double[column.Length];
            
            // Copy matrix
            for (int i = 0; i < matrix.Length; i++) {
                m[i] = new double[matrix[0].Length];
                for (int j = 0; j < matrix[0].Length; j++) {
                    m[i][j] = matrix[i][j];
                }
            }

            // Copy column
            for (int i = 0; i < column.Length; i++) {
                c[i] = column[i];
            }

            int findMaxInColumn(int colIdx) {
                double max = Math.Abs(m[0][colIdx]);
                int maxRowIdx = colIdx;

                for (int i = 1; i < m.Length; i++) {
                    if (Math.Abs(m[i][colIdx]) > max) {
                        max = Math.Abs(m[i][colIdx]);
                        maxRowIdx = i;
                    }
                }

                return maxRowIdx;
            }

            void swapLines(int l1, int l2) {
                double[] temp = m[l1];
                m[l1] = m[l2];
                m[l2] = temp;

                double t = c[l1];
                c[l1] = c[l2];
                c[l2] = t;
            }

            double[] multRowByNum(double num, double[] row) {
                double[] r = new double[row.Length];

                for (int i = 0; i < row.Length; i++) {
                    r[i] = num * row[i];
                }

                return r;
            }

            double[] sumTwoRows(double[] r1, double[] r2) {
                double[] r = new double[r1.Length];

                for (int i = 0; i < r1.Length; i++) {
                    r[i] = r1[i] + r2[i];
                }

                return r;
            }

            void forwardPropagation() {
                for (int j = 0; j < m[0].Length; j++) {
                    int maxRowIdx = findMaxInColumn(j);

                    swapLines(j, maxRowIdx);

                    c[j] = c[j] / m[j][j];
                    m[j] = multRowByNum(1 / m[j][j], m[j]);

                    for (int i = j + 1; i < m.Length; i++) {
                        c[i] = c[i] + c[j] * -m[i][j];
                        m[i] = sumTwoRows(m[i], multRowByNum(-m[i][j], m[j]));
                    }
                }
            }

            void backwardPropagation() {
                for (int i = m.Length - 1; i >= 1; i--) {
                    for (int j = i - 1; j >= 0; j--) {
                        c[j] = c[j] + c[i]*-m[j][i];
                        m[j] = sumTwoRows(m[j], multRowByNum(-m[j][i], m[i]));
                    }
                }
            }

            forwardPropagation();
            backwardPropagation();

            return c;
        }
    }
}
