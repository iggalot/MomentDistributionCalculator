using MomentDistributionCalculator.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MomentDistributionCalculator.Model
{
    public class MDC_Node : DrawingObject
    {
        private double m_X {get; set;} = 0.0f;
        private double m_Y { get; set; } = 0.0f;
        private double m_Z { get; set; } = 0.0f;

        private Shape m_Shape = null;

        public double X { get { return m_X; } set { m_X = value; } }
        public double Y { get { return m_Y; } set { m_Y = value; } }
        public double Z { get { return m_Z; } set { m_Z = value; } }

        public double Radius { get; set; } = 12;  // size of our node.

        public MDC_Node(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;

            this.MouseEnter += OnMouseEntered;
        }

        //public void Update()
        //{
        //    OnShapeChanged(new ShapeEventArgs());
        //}

        //protected override void OnMouseEnter(MouseEventArgs e)
        //{
        //    base.OnMouseEnter(e);
        //}

        //protected override void OnShapeChanged(ShapeEventArgs e)
        //{
        //    // Do node specific stuff here...

        //    // Call the base class event invocation method
        //    base.OnShapeChanged(e);            {
        //    }
        //}

        public override void Draw(Canvas c)
        {
            // Draw the circle for the node
            m_Shape = DrawingHelpers.DrawCircleHollow(c, this, Radius, Colors.Black);

            // Draw the node text
            DrawingHelpers.DrawText(c, this, this.Index.ToString(), Radius * 0.8, Radius, Colors.Black);
        }
    }
}
