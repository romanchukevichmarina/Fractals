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
    /// Треугольник Серпинского.
    /// </summary>
    class Triangle : Fractal<Point, Point, Point, bool>
    {
        /// <summary>
        /// Точка 1 первого треугольника.
        /// </summary>
        public Point Point1;

        /// <summary>
        /// Точка 2 первого треугольника.
        /// </summary>
        public Point Point2;

        /// <summary>
        /// Точка 3 первого треугольника.
        /// </summary>
        public Point Point3;

        /// <summary>
        /// Метод отрисовки фрактала.
        /// </summary>
        public override void DrawFractal()
        {
            recursionCounter = (int)Form.triangleSlider.Value;
            DrawFirstTriangle();
            Recursion(Point1, Point2, Point3, recursionCounter);
        }

        /// <summary>
        /// Метод отрисовки треугольника по 3 точкам.
        /// </summary>
        /// <param name="a"> Первая точка. </param>
        /// <param name="b"> Вторая точка. </param>
        /// <param name="c"> Третья точка. </param>
        /// <param name="counter"> Счетчик для выбора цвета по градиенту. </param>
        public void DrawTriangle(Point a, Point b, Point c, int counter)
        {
            Polygon polygon = new();
            polygon.Stroke = GetBrush(ref counter);
            polygon.Fill = Brushes.Transparent;
            polygon.StrokeThickness = 1;
            PointCollection pointCollection = new();
            pointCollection.Add(a);
            pointCollection.Add(b);
            pointCollection.Add(c);
            polygon.Points = pointCollection;
            Form.drawingGrid.Children.Add(polygon);
        }

        /// <summary>
        /// Метод отрисовки первого треугольника.
        /// </summary>
        public void DrawFirstTriangle()
        {
            Point1 = new Point(10 + (Form.drawingGrid.ActualWidth - 20 - (Form.drawingGrid.ActualHeight - 20) / Math.Sqrt(3) * 2) / 2, Form.drawingGrid.ActualHeight - 10);
            Point2 = new Point((Form.drawingGrid.ActualWidth - 20 - (Form.drawingGrid.ActualHeight - 20) / Math.Sqrt(3) * 2) / 2 + 10 + (Form.drawingGrid.ActualHeight - 20) / Math.Sqrt(3), 10);
            Point3 = new Point((Form.drawingGrid.ActualWidth - 20 - (Form.drawingGrid.ActualHeight - 20) / Math.Sqrt(3) * 2) / 2 + (Form.drawingGrid.ActualHeight - 20) / Math.Sqrt(3) * 2 + 10, Form.drawingGrid.ActualHeight - 10);
            DrawTriangle(Point1, Point2, Point3, recursionCounter);
            recursionCounter--;
        }

        /// <summary>
        /// Рекурсивный метод для отрисовки после первой итерации.
        /// </summary>
        /// <param name="a"> Точка угла треугольника в предыдущей итерации. </param>
        /// <param name="b"> Точка угла треугольника в предыдущей итерации. </param>
        /// <param name="c"> Точка угла треугольника в предыдущей итерации. </param>
        /// <param name="counter"> Счетчик итераций. </param>
        /// <param name="trash"> Неиспользуемая переменная. </param>
        public override void Recursion(Point a, Point b, Point c, int counter, bool trash = false)
        {
            if (counter == 0)
                return;
            Point ab = new((b.X - a.X) / 2 + a.X, (b.Y - a.Y) / 2 + a.Y);
            Point bc = new((c.X - b.X) / 2 + b.X, (c.Y - b.Y) / 2 + b.Y);
            Point ca = new((a.X - c.X) / 2 + c.X, (a.Y - c.Y) / 2 + c.Y);
            DrawTriangle(ab, bc, ca, counter--);
            Recursion(ab, bc, b, counter);
            Recursion(bc, ca, c, counter);
            Recursion(ca, ab, a, counter);
        }
    }
}
