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
    /// Фрактальное дерево.
    /// </summary>
    class Tree : Fractal<double, double, double, double>
    {
        /// <summary>
        /// Коэффициент отношения длины отрезков между итерациями.
        /// </summary>
        public double koef;

        /// <summary>
        /// Первый угол дерева.
        /// </summary>
        public double firstAngle;

        /// <summary>
        /// Второй угол дерева.
        /// </summary>
        public double secondAngle;

        /// <summary>
        /// Счетчик итераций.
        /// </summary>
        public int counter = 0;

        /// <summary>
        /// Первая линия.
        /// </summary>
        public Line line = new();

        /// <summary>
        /// Метод отрисовки и проверки корректности коэфициента и углов фрактала.
        /// </summary>
        public override void DrawFractal()
        {
            recursionCounter = (int)Form.treeSlider.Value;
            if (double.TryParse(Form.treeKoef.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double k) && k <= 0.7 && k >= 0.3)
            {
                koef = k;
            }
            else
            {
                MessageBox.Show("Введен недопустимый коэффициент", "ой", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (int.TryParse(Form.treeFirstAngle.Text, out int angle1) && angle1 >= 5 && angle1 <= 60)
            {
                firstAngle = (90 - angle1) * Math.PI / 180 + Math.PI * 1.5;
            }
            else
            {
                MessageBox.Show("Введен недопустимый угол", "ой", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (int.TryParse(Form.treeSecondAngle.Text, out int angle2) && angle2 >= 5 && angle2 <= 60)
            {
                secondAngle = (90 + angle2) * Math.PI / 180 + Math.PI * 1.5;
            }
            else
            {
                MessageBox.Show("Введен недопустимый угол", "ой", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            DrawBackground();
            CreatingFirstLine();
            Recursion(line.X2, line.Y2, line.Y1 - line.Y2, recursionCounter, Math.PI * 0.5);
        }

        /// <summary>
        /// Метод отрисовки первой линии.
        /// </summary>
        public void CreatingFirstLine()
        {
            line.Stroke = GetBrush(ref recursionCounter);
            line.X1 = Form.drawingGrid.ActualWidth / 2;
            line.Y1 = Form.drawingGrid.ActualHeight;
            line.X2 = Form.drawingGrid.ActualWidth / 2;
            line.Y2 = Form.drawingGrid.ActualHeight * koef;
            line.StrokeThickness = 1;
            Form.drawingGrid.Children.Add(line);
            recursionCounter--;
        }

        /// <summary>
        /// Метод отрисовки линии по координатам.
        /// </summary>
        /// <param name="x1"> Координата x начала линии. </param>
        /// <param name="y1"> Координата y начала линии. </param>
        /// <param name="x2"> Координата x конца линии. </param>
        /// <param name="y2"> Координата y конца линии. </param>
        /// <param name="counter"> Счетчик для выбора цвета линии по градиенту. </param>
        /// <returns></returns>
        public Line MakeLine(double x1, double y1, double x2, double y2, ref int counter)
        {
            Line myLine = new();
            myLine.Stroke = GetBrush(ref counter);
            myLine.X1 = x1;
            myLine.X2 = x2;
            myLine.Y1 = y1;
            myLine.Y2 = y2;
            myLine.StrokeThickness = 1;
            return myLine;
        }

        /// <summary>
        /// Метод высчитывания смещения координат в итерации в зависимости от угла и длины предыдущей линии.
        /// </summary>
        /// <param name="dx"> Смещение по х. </param>
        /// <param name="dy"> Смещение по у. </param>
        /// <param name="angle"> Угол. </param>
        /// <param name="length"> Длина линии на предыдущей итерации. </param>
        public void GetDxy(out double dx, out double dy, double angle, double length)
        {
            if (angle > Math.PI / 2)
            {
                angle = Math.PI - angle;
                dx = -Math.Cos(angle) * length;
                dy = -Math.Sin(angle) * length;
            }
            else if (angle > Math.PI)
            {
                angle -= Math.PI;
                dx = -Math.Cos(angle) * length;
                dy = Math.Sin(angle) * length;
            }
            else if (angle >= Math.PI * 1.5)
            {
                angle = Math.PI * 2 - angle;
                dx = Math.Cos(angle) * length;
                dy = Math.Sin(angle) * length;
            }
            else
            {
                dx = Math.Cos(angle) * length;
                dy = -Math.Sin(angle) * length;
            }
        }

        /// <summary>
        /// Рекурсивный метод для отрисовки фрактала после первой линии.
        /// </summary>
        /// <param name="x"> Координата х начала новой линии. </param>
        /// <param name="y"> Координата у начала новой линии. </param>
        /// <param name="length"> Длина линии на предыдущей итерации. </param>
        /// <param name="counter"> Счетчик количества итераций. </param>
        /// <param name="angle"> Угол предыдущего отрезка. </param>
        public override void Recursion(double x, double y, double length, int counter, double angle)
        {
            if (counter == 0)
                return;
            length *= koef;
            double aangle = (angle - firstAngle + Math.PI * 2) % (2 * Math.PI);
            GetDxy(out double dx, out double dy, aangle, length);
            Form.drawingGrid.Children.Add(MakeLine(x, y, x + dx, y + dy, ref counter));
            Recursion(x + dx, y + dy, length, counter - 1, aangle);
            aangle = (angle - secondAngle + Math.PI * 2) % (2 * Math.PI);
            GetDxy(out dx, out dy, aangle, length);
            Form.drawingGrid.Children.Add(MakeLine(x, y, x + dx, y + dy, ref counter));
            Recursion(x + dx, y + dy, length, counter - 1, aangle);
        }
    }

}
