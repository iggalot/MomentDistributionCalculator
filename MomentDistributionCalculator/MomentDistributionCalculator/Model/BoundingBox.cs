using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomentDistributionCalculator.Model
{
    public class BoundingBox
    {
        public MDC_Node TopLeft { get; set; }  = null;
        public MDC_Node TopRight { get; set; } = null;
        public MDC_Node BottomLeft { get; set; } = null;
        public MDC_Node BottomRight { get; set; } = null;
        public MDC_Node Center { get; set; } = null;

        public BoundingBox(MDC_Node ul, MDC_Node br)
        {
            TopLeft = ul;
            BottomRight = br;

            TopRight.X= Math.Max(ul.X, br.X);
            TopRight.Y = Math.Max(ul.Y, br.Y);

            BottomLeft.X = Math.Max(ul.X, br.X);
            BottomRight.Y = Math.Max(ul.Y, br.Y);
        }
    }
}
