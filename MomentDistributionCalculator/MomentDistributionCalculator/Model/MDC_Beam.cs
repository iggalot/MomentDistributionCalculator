using MomentDistributionCalculator.Helpers;
using System;
using System.Windows.Controls;
using System.Windows.Media;

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

        /// <summary>
        /// Default constructor for a member
        /// </summary>
        /// <param name="start">The start node</param>
        /// <param name="end">The end node</param>
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
            DrawingHelpers.DrawLine(c, Start, End, Colors.Red);


            DrawingHelpers.DrawText(c, this.GetBeamMidPoint, m_Index.ToString(), 10, 10, Colors.Blue);

            // Draw the Load
            if (Load != null)
                Load.Draw(c);
        }

        /// <summary>
        /// Returns the mid point of this member based on a linear line
        /// </summary>
        public MDC_Node GetBeamMidPoint
        {
            get
            {
                return DrawingGeometryHelpers.GetMidPoint(Start, End);
            }
        }

        /// <summary>
        /// Adds a load to the member
        /// </summary>
        /// <param name="load"></param>
        public void AddLoad(MemberDistributedLoad load)
        {
            Load = load;
        }
    }

}
