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
    /// Базовый класс фракталов.
    /// </summary>
    /// <typeparam name="T1"> Параметр универсального типа. </typeparam>
    /// <typeparam name="T2"> Параметр универсального типа. </typeparam>
    /// <typeparam name="T3"> Параметр универсального типа. </typeparam>
    /// <typeparam name="T5"> Параметр универсального типа. </typeparam>
    class Fractal<T1, T2, T3, T5>
    {
        /// <summary>
        /// Форма.
        /// </summary>
        public MainWindow Form = Application.Current.Windows[0] as MainWindow;

        /// <summary>
        /// Счетчик рекурсии.
        /// </summary>
        public int recursionCounter;

        /// <summary>
        /// Рекурсивный метод для отрисовки фракталов.
        /// </summary>
        /// <param name="arg1"> Параметр универсального типа. </param>
        /// <param name="arg2"> Параметр универсального типа. param>
        /// <param name="arg3"> Параметр универсального типа. </param>
        /// <param name="counter"> Счетчик итераций. </param>
        /// <param name="arg5"> Параметр универсального типа. </param>
        public virtual void Recursion(T1 arg1, T2 arg2, T3 arg3, int counter, T5 arg5) { }

        /// <summary>
        /// Метод выбора кисти для градиента.
        /// </summary>
        /// <param name="counter"> Счетчик итераций. </param>
        /// <returns> Кисть нужного цвета. </returns>
        public Brush GetBrush(ref int counter)
        {
            var sColor = Form.startColor.Color;
            var eColor = Form.endColor.Color;
            double r = sColor.RGB_R, g = sColor.RGB_G, b = sColor.RGB_B;
            double r1 = eColor.RGB_R, g1 = eColor.RGB_G, b1 = eColor.RGB_B;
            double rc = (r - r1) / (recursionCounter + 1) * counter + r1;
            double gc = (g - g1) / (recursionCounter + 1) * counter + g1;
            double bc = (b - b1) / (recursionCounter + 1) * counter + b1;
            return new SolidColorBrush(Color.FromArgb(255, (byte)(rc), (byte)(gc), (byte)(bc)));
        }

        /// <summary>
        /// Отрисовка фона для некоторых фракталов.
        /// </summary>
        public void DrawBackground()
        {
            Polygon polygon = new();
            polygon.Stroke = Brushes.Transparent;
            polygon.Fill = Brushes.Transparent;
            polygon.StrokeThickness = 1;
            Point point1 = new(0, 0);
            Point point2 = new(0, Form.drawingGrid.ActualHeight);
            Point point3 = new(Form.drawingGrid.ActualWidth, Form.drawingGrid.ActualHeight);
            Point point4 = new(Form.drawingGrid.ActualWidth, 0);
            PointCollection points = new();
            points.Add(point1);
            points.Add(point2);
            points.Add(point3);
            points.Add(point4);
            polygon.Points = points;
            Form.drawingGrid.Children.Add(polygon);
        }

        /// <summary>
        /// Метод отрисовки фрактала.
        /// </summary>
        public virtual void DrawFractal() { }
    }
}
