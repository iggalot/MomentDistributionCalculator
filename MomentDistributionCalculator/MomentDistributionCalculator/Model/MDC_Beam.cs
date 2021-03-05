using System;
using System.Collections.Generic;
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
        private int m_Index = 0;
        private MemberDistributedLoad m_Load = null;

        public MDC_Node Start { get { return m_Start; } set { m_Start = value; } }
        public MDC_Node End { get { return m_End; } set { m_End = value; } }

        public MemberDistributedLoad Load { get { return m_Load; } set { m_Load = value; } }

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

            // Draw the Load
            if (Load != null)
                Load.Draw(c);
        }

        public MDC_Node GetMidPoint
        {
            get
            {
                return new MDC_Node((float)0.5 * (Start.X + End.X), (float)0.5 * (Start.Y + End.Y));
            }
        }

        private double GetLength(MDC_Node start, MDC_Node end)
        {
            return Math.Sqrt(Math.Pow((end.X - start.X), 2) + Math.Pow((end.Y - start.Y), 2));
        }
        public List<MDC_Node> GetNPoints(int N)
        {
            List<MDC_Node> temp = new List<MDC_Node>();

            double dist = GetLength(this.Start, this.End);
            double distX = (float)((this.End.X - this.Start.X)/N);
            double distY = (float)((this.End.Y - this.Start.Y)/N);

            for (int i = 0; i < N + 1; i++)
            {
                MDC_Node tempNode = new MDC_Node((float)(this.Start.X + i * distX), (float)(this.Start.Y + i * distY));
                temp.Add(tempNode);
            }

            return temp;
        }


        public void AddLoad(MemberDistributedLoad load)
        {
            Load = load;
        }
    }

}
