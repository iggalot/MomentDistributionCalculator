using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MomentDistributionCalculator.Model
{
    public class MDC_Beam
    {
        private static int currentIndex = 0;

        private MDC_Node m_Start = null;
        private MDC_Node m_End = null;
        private static int m_Index = 0;

        public MDC_Node Start { get { return m_Start; } set { m_Start = value; } }
        public MDC_Node End { get { return m_End; } set { m_End = value; } }

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

        public MDC_Beam(MDC_Node start, MDC_Node end)
        {
            Start = start;
            End = end;

            Index = currentIndex;
            currentIndex++;
        }

        public void Draw(Canvas c)
        {
            // Draw the start node
            Start.Draw(c);
            End.Draw(c);

            // Draw the Beam Line
            Line myLine = new Line();
            myLine.Stroke = Brushes.Red;
            myLine.StrokeThickness = 2;
            myLine.X1 = Start.X;
            myLine.Y1 = Start.Y;
            myLine.X2 = End.X;
            myLine.Y2 = End.Y;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            c.Children.Add(myLine);
        }
    }

}
