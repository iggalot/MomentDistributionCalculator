using MomentDistributionCalculator.Helpers;
using System.Windows.Controls;
using System.Windows.Media;

namespace MomentDistributionCalculator.Model
{
    public class MDC_Node : DrawingObject
    {
        private double m_X {get; set;} = 0.0f;
        private double m_Y { get; set; } = 0.0f;
        private double m_Z { get; set; } = 0.0f;

        private double m_radius = 20;  // size of our node

        public double X { get { return m_X; } set { m_X = value; } }
        public double Y { get { return m_Y; } set { m_Y = value; } }
        public double Z { get { return m_Z; } set { m_Z = value; } }

        public MDC_Node(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override void Draw(Canvas c)
        {
            // Draw the circle for the node
            DrawingHelpers.DrawCircleHollow(c, this, m_radius, Colors.Black);

            // Draw the node text
            DrawingHelpers.DrawText(c, this, this.Index.ToString(), m_radius * 0.8, m_radius, Colors.Black);
        }
    }
}
