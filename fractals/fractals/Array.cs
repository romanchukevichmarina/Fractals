using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace fractals
{
    /// <summary>
    /// Множество Кантора.
    /// </summary>
    class Array : Fractal<double, double, double, bool>
    {
        /// <summary>
        /// Интервал между отрезками на разных итерациях.
        /// </summary>
        public int distance;

        /// <summary>
        /// Первая линия.
        /// </summary>
        public Line firstLine = new();

        /// <summary>
        /// Метод отрисовки фрактала.
        /// </summary>
        public override void DrawFractal()
        {
            recursionCounter = (int)Form.arraySlider.Value;
            if (int.TryParse(Form.arrayInterval.Text, out int dist) && dist * (recursionCounter - 1) + Form.drawingGrid.ActualHeight * 0.05 * recursionCounter + 20 <= Form.drawingGrid.ActualHeight && dist >= 0)
            {
                distance = dist;
            }
            else
            {
                MessageBox.Show("Введён слишком большой отступ(( \nУменьшите отступ или увеличьте размер окна", "ой", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            DrawFirstLine();
            Recursion(firstLine.X1, firstLine.Y1, firstLine.X2 - firstLine.X1, recursionCounter);
        }

        /// <summary>
        /// Метод отрисовки линии фрактала.
        /// </summary>
        /// <param name="x"> Координата х начала линии. </param>
        /// <param name="y"> Координата у начала линии. </param>
        /// <param name="length"> Длина линии. </param>
        /// <param name="counter"> Счетчик для выбора цвета по градиенту. </param>
        public void DrawArray(double x, double y, double length, int counter)
        {
            Line line = new();
            line.Stroke = GetBrush(ref counter);
            line.X1 = x;
            line.Y1 = y;
            line.X2 = x + length;
            line.Y2 = y;
            line.StrokeThickness = Form.ActualHeight * 0.05;
            Form.drawingGrid.Children.Add(line);
        }

        /// <summary>
        /// Метод отрисовки первой линии.
        /// </summary>
        public void DrawFirstLine()
        {
            firstLine.Stroke = GetBrush(ref recursionCounter);
            firstLine.StrokeThickness = Form.ActualHeight * 0.05;
            firstLine.X1 = 10;
            firstLine.Y1 = 30;
            firstLine.X2 = Form.drawingGrid.ActualWidth - 10;
            firstLine.Y2 = 30;
            Form.drawingGrid.Children.Add(firstLine);
            recursionCounter--;
        }

        /// <summary>
        /// Рекурсивный метод отрисовки итераций после первой.
        /// </summary>
        /// <param name="x"> Координата х начала линии на предыдущей итерации. </param>
        /// <param name="y"> Координата у начала линии на предыдущей итерации. </param>
        /// <param name="length"> Длина линии на предыдущей итерации. </param>
        /// <param name="counter"> Счетчик итераций. </param>
        /// <param name="arg"> Неиспользуемая переменная. </param>
        public override void Recursion(double x, double y, double length, int counter, bool arg = true)
        {
            if (counter == 0)
                return;
            DrawArray(x, y + distance + firstLine.StrokeThickness, length / 3, counter);
            DrawArray(x + length * 2 / 3, y + distance + firstLine.StrokeThickness, length / 3, counter);
            Recursion(x, y + distance + firstLine.StrokeThickness, length / 3, --counter);
            Recursion(x + length * 2 / 3, y + distance + firstLine.StrokeThickness, length / 3, counter);

        }
    }
}
