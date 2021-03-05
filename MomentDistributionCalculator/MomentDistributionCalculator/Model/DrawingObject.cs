using MomentDistributionCalculator.Interfaces;
using System.Windows.Controls;

namespace MomentDistributionCalculator.Model
{
    public class DrawingObject : IDrawable
    {
        private MemberDistributedLoad m_BoundingBox = null;

        public virtual void Draw(Canvas c) { }
    }
}
