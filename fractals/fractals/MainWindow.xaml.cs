using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace fractals
{
    /// <summary>
    /// Основное окно приложения.
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Loaded += delegate
            {
                // Установка минимального и максимального размера экрана.
                MinWidth = SystemParameters.PrimaryScreenWidth / 2;
                MinHeight = SystemParameters.PrimaryScreenHeight / 2;
            };
        }

        /// <summary>
        /// Регулярное выражение для проверки символов в textBox.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Регулярное выражение для проверки символов в textBox (для double).
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void DoubleNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9, .]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Установка видимости элементов в зависимости от выбранного фрактала.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void FractalList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (fractalList.SelectedIndex)
            {
                case 0:
                    tree.Visibility = Visibility.Visible;
                    curve.Visibility = Visibility.Hidden;
                    carpet.Visibility = Visibility.Hidden;
                    triangle.Visibility = Visibility.Hidden;
                    array.Visibility = Visibility.Hidden;
                    return;
                case 1:
                    tree.Visibility = Visibility.Hidden;
                    curve.Visibility = Visibility.Visible;
                    carpet.Visibility = Visibility.Hidden;
                    triangle.Visibility = Visibility.Hidden;
                    array.Visibility = Visibility.Hidden;
                    return;
                case 2:
                    tree.Visibility = Visibility.Hidden;
                    curve.Visibility = Visibility.Hidden;
                    carpet.Visibility = Visibility.Visible;
                    triangle.Visibility = Visibility.Hidden;
                    array.Visibility = Visibility.Hidden;
                    return;
                case 3:
                    tree.Visibility = Visibility.Hidden;
                    curve.Visibility = Visibility.Hidden;
                    carpet.Visibility = Visibility.Hidden;
                    triangle.Visibility = Visibility.Visible;
                    array.Visibility = Visibility.Hidden;
                    return;
                case 4:
                    tree.Visibility = Visibility.Hidden;
                    curve.Visibility = Visibility.Hidden;
                    carpet.Visibility = Visibility.Hidden;
                    triangle.Visibility = Visibility.Hidden;
                    array.Visibility = Visibility.Visible;
                    return;
                default:
                    return;
            }
        }

        /// <summary>
        /// Вызов соответствующего фрактала при нажатии кнопки.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            Form.drawingGrid.Children.Clear();
            GC.Collect();
            dynamic fractal;
            if (fractalList.SelectedIndex == 0)
            {
                fractal = new Tree();
            }
            else if (fractalList.SelectedIndex == 1)
            {
                fractal = new Curve();
            }
            else if (fractalList.SelectedIndex == 2)
            {
                fractal = new Carpet();
            }
            else if (fractalList.SelectedIndex == 3)
            {
                fractal = new Triangle();
            }
            else if (fractalList.SelectedIndex == 4)
            {
                fractal = new Array();
            }
            else
                return;
            fractal.DrawFractal();
        }

        /// <summary>
        /// Перерисовка фрактала при изменении размера окна.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            if (Form.drawingGrid.Children.Count > 0)
            {
                drawingGrid.Children.Clear();
                Button_Click(sender, e);
            }
        }

        /// <summary>
        /// Анимация при наведении курсора на выбор цвета.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Windows.Media.Animation.DoubleAnimation doubleAnimation = new();
            doubleAnimation.From = 0.3;
            doubleAnimation.To = 1;
            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2);
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            Form.Colors.BeginAnimation(StackPanel.OpacityProperty, doubleAnimation);
        }

        /// <summary>
        /// Анимация при отведении курсора от выбора цвета.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            System.Windows.Media.Animation.DoubleAnimation doubleAnimation = new();
            doubleAnimation.From = 1;
            doubleAnimation.To = 0.3;
            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2);
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            Form.Colors.BeginAnimation(StackPanel.OpacityProperty, doubleAnimation);
        }

        /// <summary>
        /// Автоматическая перерисовка фрактала при изменении глубины рекурсии.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            if (Form.drawingGrid != null && Form.drawingGrid.Children.Count > 0)
            {
                drawingGrid.Children.Clear();
                Button_Click(sender, e);
            }
        }

        /// <summary>
        /// Анимация при наведении курсора на кнопку увеличения.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void ZoomButton_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Windows.Media.Animation.DoubleAnimation doubleAnimation = new();
            doubleAnimation.From = 0.3;
            doubleAnimation.To = 1;
            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2);
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            Form.x2Button.BeginAnimation(StackPanel.OpacityProperty, doubleAnimation);
        }

        /// <summary>
        /// Анимация при отведении курсора от кнопки увеличения.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void ZoomButton_MouseLeave(object sender, MouseEventArgs e)
        {
            System.Windows.Media.Animation.DoubleAnimation doubleAnimation = new();
            doubleAnimation.From = 1;
            doubleAnimation.To = 0.3;
            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2);
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            Form.x2Button.BeginAnimation(StackPanel.OpacityProperty, doubleAnimation);
        }

        /// <summary>
        /// Анимация при наведении курсора на кнопку увеличения.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void ZoomButton_MouseEnter_1(object sender, MouseEventArgs e)
        {
            System.Windows.Media.Animation.DoubleAnimation doubleAnimation = new();
            doubleAnimation.From = 0.3;
            doubleAnimation.To = 1;
            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2);
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            Form.x3Button.BeginAnimation(StackPanel.OpacityProperty, doubleAnimation);
        }

        /// <summary>
        /// Анимация при отведении курсора от кнопки увеличения.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void ZoomButton_MouseLeave_1(object sender, MouseEventArgs e)
        {
            System.Windows.Media.Animation.DoubleAnimation doubleAnimation = new();
            doubleAnimation.From = 1;
            doubleAnimation.To = 0.3;
            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2);
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            Form.x3Button.BeginAnimation(StackPanel.OpacityProperty, doubleAnimation);
        }

        /// <summary>
        /// Анимация при наведении курсора на кнопку увеличения.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void ZoomButton_MouseEnter_2(object sender, MouseEventArgs e)
        {
            System.Windows.Media.Animation.DoubleAnimation doubleAnimation = new();
            doubleAnimation.From = 0.3;
            doubleAnimation.To = 1;
            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2);
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            Form.x5Button.BeginAnimation(StackPanel.OpacityProperty, doubleAnimation);
        }

        /// <summary>
        /// Анимация при отведении курсора от кнопки увеличения.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void ZoomButton_MouseLeave_2(object sender, MouseEventArgs e)
        {
            System.Windows.Media.Animation.DoubleAnimation doubleAnimation = new();
            doubleAnimation.From = 1;
            doubleAnimation.To = 0.3;
            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2);
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            Form.x5Button.BeginAnimation(StackPanel.OpacityProperty, doubleAnimation);
        }

        // Переменная, хранящая значение увеличения.
        int zoom = 1;

        // Текущая точка drawingGrid в центре экрана.
        double curX = -1, curY = -1;

        /// <summary>
        /// Увеличение фрактала в 2 раза.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void Zoomx2Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            if (curX == -1)
            {
                curX = Form.drawingGrid.ActualWidth / 2;
                curY = Form.drawingGrid.ActualHeight / 2;
            }
            if (zoom != 2)
            {
                drawingGrid.Cursor = Cursors.ScrollAll;
                Form.drawingGrid.RenderTransform = new ScaleTransform(2, 2, curX, curY);
                zoom = 2;
            }
            else
            {
                drawingGrid.Cursor = null;
                Form.drawingGrid.RenderTransform = new ScaleTransform(1, 1);
                curX = Form.drawingGrid.ActualWidth / 2;
                curY = Form.drawingGrid.ActualHeight / 2;
                zoom = 1;
            }

        }

        /// <summary>
        /// Увеличение фрактала в 3 раза.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void Zoomx3Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            if (curX == -1)
            {
                curX = Form.drawingGrid.ActualWidth / 2;
                curY = Form.drawingGrid.ActualHeight / 2;
            }
            if (zoom != 3)
            {
                drawingGrid.Cursor = Cursors.ScrollAll;
                Form.drawingGrid.RenderTransform = new ScaleTransform(3, 3, curX, curY);
                zoom = 3;
            }
            else
            {
                drawingGrid.Cursor = null;
                Form.drawingGrid.RenderTransform = new ScaleTransform(1, 1);
                curX = Form.drawingGrid.ActualWidth / 2;
                curY = Form.drawingGrid.ActualHeight / 2;
                zoom = 1;
            }
        }

        /// <summary>
        /// Увеличение фрактала в 5 раз.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void Zoomx5Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Form = Application.Current.Windows[0] as MainWindow;
            if (curX == -1)
            {
                curX = Form.drawingGrid.ActualWidth / 2;
                curY = Form.drawingGrid.ActualHeight / 2;
            }
            if (zoom != 5)
            {
                drawingGrid.Cursor = Cursors.ScrollAll;
                Form.drawingGrid.RenderTransform = new ScaleTransform(5, 5, curX, curY);
                zoom = 5;
            }
            else
            {
                drawingGrid.Cursor = null;
                Form.drawingGrid.RenderTransform = new ScaleTransform(1, 1);
                curX = Form.drawingGrid.ActualWidth / 2;
                curY = Form.drawingGrid.ActualHeight / 2;
                zoom = 1;
            }
        }

        // Начальные координаты drawingGrid
        double startX, startY;

        /// <summary>
        /// Инициализация начальных точек при нажатии мыши.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void DrawingGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            drawingGrid.CaptureMouse();
            startX = e.GetPosition(window).X;
            startY = e.GetPosition(window).Y;
        }

        /// <summary>
        /// Изменение текущих координат при движении мыши.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void DrawingGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (curX == -1)
            {
                curX = drawingGrid.ActualWidth / 2;
                curY = drawingGrid.ActualHeight / 2;
            }
            if (drawingGrid.IsMouseCaptured)
            {
                Point point = e.GetPosition(window);
                double dx = startX - point.X;
                double dy = startY - point.Y;
                drawingGrid.RenderTransform = new ScaleTransform(zoom, zoom, curX + dx, curY + dy);
            }
        }

        /// <summary>
        /// Изменение текущих координат при отпускании мыши.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void DrawingGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            curX += (startX - e.GetPosition(window).X);
            curY += (startY - e.GetPosition(window).Y);
            drawingGrid.ReleaseMouseCapture();
        }

        /// <summary>
        /// Сохранение drawingGrid как изображения.
        /// </summary>
        /// <param name="sender"> Служебный параметр. </param>
        /// <param name="e"> Служебный параметр. </param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (drawingGrid.Children.Count == 0)
            {
                MessageBox.Show("Вы серьезно хотите сохранить фон? Нарисуйте хотя бы какой-то фрактал.", "ой", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "png (*.png)|*.png";
            if (saveFileDialog.ShowDialog() == true)
            {
                if (System.IO.Path.GetExtension(saveFileDialog.FileName) != ".png")
                {
                    MessageBox.Show("Введен некорректный формат файла", "ой", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                RenderTargetBitmap bitmap = new((int)window.ActualWidth, (int)window.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                bitmap.Render(drawingGrid);

                PngBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                try
                {
                    FileStream fs = File.Open(saveFileDialog.FileName, FileMode.Create);
                    encoder.Save(fs);
                    fs.Close();
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Введен некорректный путь", "ой", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Недостаточный уровень доступа", "ой", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                catch (Exception)
                {
                    MessageBox.Show("Возникли какие то проблемы", "ой", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
    }
}
