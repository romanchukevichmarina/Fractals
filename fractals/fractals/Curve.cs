using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace fractals
{
    /// <summary>
    /// Кривая Коха.
    /// </summary>
    class Curve : Fractal<double, double, double, double>
    {
        PointCollection points = new();

        /// <summary>
        /// Метод отрисовки фрактала.
        /// </summary>
        public override void DrawFractal()
        {
            DrawBackground();
            recursionCounter = (int)Form.curveSlider.Value;
            CreatingFirstLine();
            if (recursionCounter != 0)
            {
                MakeFirstCurve();
            }
            Recursion(0, 0, 0, recursionCounter, 0);
        }

        /// <summary>
        /// Создание первой линии.
        /// </summary>
        /// <returns> Коллекцию из первой и последней точек линии. </returns>
        public PointCollection CreatingFirstLine()
        {
            Point Point1 = new(10, Form.drawingGrid.ActualHeight * 0.8);
            Point Point2 = new(Form.drawingGrid.ActualWidth - 10, Form.drawingGrid.ActualHeight * 0.8);
            points.Add(Point1);
            points.Add(Point2);
            return points;
        }

        /// <summary>
        /// Добавление трех точек изгиба фрактала в коллекцию точек.
        /// </summary>
        /// <param name="ind"> Индекс текущей точки. </param>
        public void ThreeSplit(int ind)
        {
            Point a = points[ind];
            Point b = points[ind + 1];
            double vx = (b.X - a.X) / 3;
            double vy = (b.Y - a.Y) / 3;
            a.X += vx;
            a.Y += vy;
            points.Insert(ind + 1, a);
            a.X += vx;
            a.Y += vy;
            points.Insert(ind + 2, a);
        }

        /// <summary>
        /// Создание первой кривой.
        /// </summary>
        public void MakeFirstCurve()
        {
            ThreeSplit(0);
            double x2 = points[2].X;
            double x1 = points[1].X;
            double len = x2 - x1;
            double y = points[1].Y - Math.Sqrt(3) / 2 * len;
            double x = (x1 + x2) / 2;
            points.Insert(2, new Point(x, y));
            recursionCounter--;
        }

        /// <summary>
        /// Копирование кусков кривой и добавление их к другим точкам.
        /// </summary>
        /// <param name="ind"></param>
        public void CopyTriangle(int ind)
        {
            Point a = points[ind + 5];
            a.X += points[ind + 1].X - points[ind + 2].X;
            a.Y += points[ind + 1].Y - points[ind + 2].Y;
            points.Insert(ind + 5, a);
            a = points[ind + 1];
            a.X += points[ind + 6].X - points[ind + 4].X;
            a.Y += points[ind + 6].Y - points[ind + 4].Y;
            points.Insert(ind + 2, a);
        }

        /// <summary>
        /// Отрисовка линии из двух точек.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void MakeLine(Point a, Point b)
        {
            Line line = new();
            line.X1 = a.X;
            line.Y1 = a.Y;
            line.X2 = b.X;
            line.Y2 = b.Y;
            line.StrokeThickness = 1;
            line.Stroke = GetBrush(ref recursionCounter);
            Form.drawingGrid.Children.Add(line);
        }

        /// <summary>
        /// Метод для отрисовки итераций после первой.
        /// </summary>
        /// <param name="x"> Неиспользуемая переменная. </param>
        /// <param name="y"> Неиспользуемая переменная. </param>
        /// <param name="lenght"> Неиспользуемая переменная. </param>
        /// <param name="counter"> Неиспользуемая переменная. </param>
        /// <param name="angle"> Неиспользуемая переменная. </param>
        public override void Recursion(double x, double y, double lenght, int counter, double angle)
        {
            while (counter-- > 0)
            {
                for (int i = points.Count - 2; i >= 0; i--)
                {
                    ThreeSplit(i);
                }
                for (int i = points.Count - 7; i >= 0; i -= 6)
                {
                    CopyTriangle(i);
                }
            }
            recursionCounter++;
            for (int i = 0; i < points.Count - 1; i++)
            {
                MakeLine(points[i], points[i + 1]);
            }
            recursionCounter--;
            int mod = Form.drawingGrid.Children.Count;
            for (int j = recursionCounter; j >= 0; j--)
            {
                for (int i = 0; i < Form.drawingGrid.Children.Count - 1; i++)
                {
                    if (i % mod >= mod / 4 && i % mod < mod * 3 / 4)
                    {
                        (Form.drawingGrid.Children[i + 1] as Line).Stroke = GetBrush(ref j);
                    }
                }
                mod /= 4;
            }
        }
    }
}
