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
    /// Ковер Серпинского.
    /// </summary>
    class Carpet : Fractal<double, double, double, double>
    {
        /// <summary>
        /// Коэффикиент толщины обводки.
        /// </summary>
        public const double gkoef = 0.3;

        /// <summary>
        /// Метод отрисовки фрактала.
        /// </summary>
        public override void DrawFractal()
        {
            recursionCounter = (int)Form.carpetSlider.Value;
            DrawZeroSqare();
            if (recursionCounter != 0)
            {
                DrawFirstSqare(out double len);
                Recursion((Form.drawingGrid.ActualWidth - len) / 2 + len / 3, len / 3, len / 3, recursionCounter);
            }
        }

        /// <summary>
        /// Метод отрисовки квадрата.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="length"></param>
        /// <param name="counter"></param>
        public void DrawSqare(double x, double y, double length, int counter)
        {
            Polygon polygon = new();
            polygon.Stroke = Brushes.White;
            polygon.Fill = Brushes.White;
            polygon.StrokeThickness = 1;
            Point Point1 = new(x, y);
            Point Point2 = new(x, y + length);
            Point Point3 = new(x + length, y + length);
            Point Point4 = new(x + length, y);
            PointCollection pointCollection = new();
            pointCollection.Add(Point1);
            pointCollection.Add(Point2);
            pointCollection.Add(Point3);
            pointCollection.Add(Point4);
            polygon.Points = pointCollection;
            Polygon colPolygon = new();
            Point1 = new Point(x - length * gkoef, y - length * gkoef);
            Point2 = new Point(x - length * gkoef, y + length + length * gkoef);
            Point3 = new Point(x + length + length * gkoef, y + length + length * gkoef);
            Point4 = new Point(x + length + length * gkoef, y - length * gkoef);
            PointCollection colPointCollection = new();
            colPolygon.Stroke = GetBrush(ref counter);
            colPolygon.Fill = GetBrush(ref counter);
            colPolygon.StrokeThickness = 1;
            colPointCollection.Add(Point1);
            colPointCollection.Add(Point2);
            colPointCollection.Add(Point3);
            colPointCollection.Add(Point4);
            colPolygon.Points = colPointCollection;
            Form.drawingGrid.Children.Add(colPolygon);
            Form.drawingGrid.Children.Add(polygon);
        }

        /// <summary>
        /// Метод отрисовки нулевого квадрата.
        /// </summary>
        public void DrawZeroSqare()
        {
            double len = Math.Min(Form.drawingGrid.ActualHeight, Form.drawingGrid.ActualWidth);
            Polygon polygon = new();
            polygon.Stroke = Brushes.Black;
            polygon.Fill = Brushes.Black;
            polygon.StrokeThickness = 1;
            Point Point1 = new((Form.drawingGrid.ActualWidth - len) / 2, 0);
            Point Point2 = new((Form.drawingGrid.ActualWidth - len) / 2, len);
            Point Point3 = new((Form.drawingGrid.ActualWidth - len) / 2 + len, len);
            Point Point4 = new((Form.drawingGrid.ActualWidth - len) / 2 + len, 0);
            PointCollection pointCollection = new();
            pointCollection.Add(Point1);
            pointCollection.Add(Point2);
            pointCollection.Add(Point3);
            pointCollection.Add(Point4);
            polygon.Points = pointCollection;
            Form.drawingGrid.Children.Add(polygon);
        }

        /// <summary>
        /// Метод отрисовки первого квадрата.
        /// </summary>
        /// <param name="len"></param>
        public void DrawFirstSqare(out double len)
        {
            len = Math.Min(Form.drawingGrid.ActualHeight, Form.drawingGrid.ActualWidth);
            DrawSqare((Form.drawingGrid.ActualWidth - len) / 2 + len / 3, len / 3, len / 3, recursionCounter);
            recursionCounter--;
        }

        /// <summary>
        /// Рекурсивный метод для отрисовки итераций после первой.
        /// </summary>
        /// <param name="x"> Координата х квадрата на предыдущей итерации. </param>
        /// <param name="y"> Координата у квадрата на предыдущей итерации. </param>
        /// <param name="lenght"> длина стороды квадрата на предыдущей итерации. </param>
        /// <param name="counter"> Счетчик итераций. </param>
        /// <param name="angle"> Неиспользуемая переменная. </param>
        public override void Recursion(double x, double y, double lenght, int counter, double angle = 0)
        {
            if (counter == 0)
                return;
            double dx, dy, len;
            dx = x - lenght * 2 / 3;
            dy = y - lenght * 2 / 3;
            len = lenght / 3;
            DrawSqare(dx, dy, len, counter);
            Recursion(dx, dy, len, counter - 1);

            dx = x + lenght / 3;
            dy = y - lenght * 2 / 3;
            DrawSqare(dx, dy, len, counter);
            Recursion(dx, dy, len, counter - 1);

            dx = x + lenght * 4 / 3;
            dy = y - lenght * 2 / 3;
            DrawSqare(dx, dy, len, counter);
            Recursion(dx, dy, len, counter - 1);

            dx = x + lenght * 4 / 3;
            dy = y + lenght / 3;
            DrawSqare(dx, dy, len, counter);
            Recursion(dx, dy, len, counter - 1);

            dx = x + lenght * 4 / 3;
            dy = y + lenght * 4 / 3;
            DrawSqare(dx, dy, len, counter);
            Recursion(dx, dy, len, counter - 1);

            dx = x + lenght / 3;
            dy = y + lenght * 4 / 3;
            DrawSqare(dx, dy, len, counter);
            Recursion(dx, dy, len, counter - 1);

            dx = x - lenght * 2 / 3;
            dy = y + lenght * 4 / 3;
            DrawSqare(dx, dy, len, counter);
            Recursion(dx, dy, len, counter - 1);

            dx = x - lenght * 2 / 3;
            dy = y + lenght / 3;
            DrawSqare(dx, dy, len, counter);
            Recursion(dx, dy, len, counter - 1);
        }

    }
}
