using MomentDistributionCalculator.Helpers;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace MomentDistributionCalculator.Model
{
    public class MemberDistributedLoad : DrawingObject
    {
        private int NUM_ARROWS_PER_DIST_LOAD = 5;
        private BaseMDCObject m_Beam = null;
        public BaseMDCObject AttachedTo { get { return m_Beam; } set { m_Beam = value; } }
        private double m_intensity = 0;

        public double Magnitude { get { return m_intensity; } set { m_intensity = value; } }


        /// <summary>
        /// Points where load arrows are applied
        /// </summary>
        public List<MDC_Node> LoadPoints { get; set; }
        public MemberDistributedLoad(MDC_Beam beam, double intensity)
        {
            LoadPoints = new List<MDC_Node>();

            Magnitude = intensity;
            AttachedTo = beam;

            // draw arrows evenly spaced
            LoadPoints = DrawingGeometryHelpers.GetNPointsLinear(beam, NUM_ARROWS_PER_DIST_LOAD);
        }

        public override void Draw(Canvas c)
        {
            //// Draw a circle node
            //DrawingHelpers.DrawCircle(c, this.AttachedTo.GetBeamMidPoint, 20, Colors.Black, Colors.Blue);

            //foreach (MDC_Node node in LoadPoints)
            //{
            //    DrawingHelpers.DrawArrows(c, node, Colors.Black, Colors.Green, DirectionVectors.DIR_VERT_POS);
            //}

            //// Draw a line -- testing for Rotate2D functionality.
            //DrawingHelpers.DrawLine(
            //    c, 
            //    this.AttachedTo.Start, 
            //    DrawingGeometryHelpers.Translate(this.AttachedTo.Start, 0, -60, 0), 
            //    Colors.Black
            //);

            //// A test arrow
            //DrawingHelpers.DrawArrows(c, new MDC_Node(400, 300, 0), Colors.Black, Colors.Black);

            ////TODO:  Draw line for top of distributed load
        }

    }
}
