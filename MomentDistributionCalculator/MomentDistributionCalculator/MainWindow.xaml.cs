using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MomentDistributionCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Change color of background
            MainCanvas.Background = new SolidColorBrush(Colors.Cyan);
            Line myLine = new Line();
            myLine.Stroke = Brushes.Black;
            myLine.StrokeThickness = 2;
            myLine.X1 = 10;
            myLine.Y1 = 10;
            myLine.X2 = 400;
            myLine.Y2 = 400;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            MainCanvas.Children.Add(myLine);
        }
    }
}
