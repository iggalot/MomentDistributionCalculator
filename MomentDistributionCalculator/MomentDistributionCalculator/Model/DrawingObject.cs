using MomentDistributionCalculator.Interfaces;
using System.Windows.Controls;

namespace MomentDistributionCalculator.Model
{
    public class DrawingObject : BaseMDCObject, IDrawable
    {
        private MemberDistributedLoad m_BoundingBox = null;

        public DrawingObject()
        {

        }

        public virtual void Draw(Canvas c) { }
    }
}
