using MomentDistributionCalculator.Interfaces;
using System.ComponentModel;
using System.Windows.Controls;

namespace MomentDistributionCalculator.Model
{
    public class DrawingObject : BaseMDCObject, IDrawable
    {



        public DrawingObject()
        {

        }

        public virtual void Draw(Canvas c) { }
    }
}
