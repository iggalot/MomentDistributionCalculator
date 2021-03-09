using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomentDistributionCalculator.Model
{
    /// <summary>
    /// A class for storing text object information on the drawing canvas
    /// </summary>
    public class MDC_Text : DrawingObject
    {
        private double m_X { get; set; } = 0.0f;
        private double m_Y { get; set; } = 0.0f;
        private double m_Z { get; set; } = 0.0f;

        private double m_radius = 20;  // size of our node

        public double X { get { return m_X; } set { m_X = value; } }
        public double Y { get { return m_Y; } set { m_Y = value; } }
        public double Z { get { return m_Z; } set { m_Z = value; } }
    }
}
