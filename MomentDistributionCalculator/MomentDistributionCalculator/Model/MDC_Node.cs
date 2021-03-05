using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MomentDistributionCalculator.Model
{
    public class MDC_Node
    {
        private static int currentIndex = 0;

        private float m_X {get; set;} = 0.0f;
        private float m_Y { get; set; } = 0.0f;
        private int m_Index = 0;

        private float m_Width =20;
        private float m_Height = 20;

        public float X { get { return m_X; } set { m_X = value; } }
        public float Y { get { return m_Y; } set { m_Y = value; } }

        public int Index
        {
            get
            {
                return m_Index;
            }
            set
            {
                m_Index = value;
            }
        }

        public MDC_Node(float x, float y)
        {
            X = x;
            Y = y;

            Index = currentIndex;
            currentIndex++;
        }

        public void Draw(Canvas c)
        {
            // Draw a circle node
            Ellipse myEllipse = new Ellipse();
            myEllipse.Fill = new SolidColorBrush(Colors.Transparent);
            myEllipse.Stroke = Brushes.Black;
            myEllipse.StrokeThickness = 2.0f;

            myEllipse.Width = m_Width;
            myEllipse.Height = m_Height;
            Canvas.SetLeft(myEllipse, this.X - m_Width/2.0f);
            Canvas.SetTop(myEllipse, this.Y - m_Width/2.0f);

            myEllipse.HorizontalAlignment = HorizontalAlignment.Left;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;

            c.Children.Add(myEllipse);

            // Draw a text for the index lavel
            TextBlock textBlock = new TextBlock();
            textBlock.Text = Index.ToString();
            textBlock.Foreground = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(textBlock, this.X + m_Width/2.0f);
            Canvas.SetTop(textBlock, this.Y);
            c.Children.Add(textBlock);

        }
    }
}
