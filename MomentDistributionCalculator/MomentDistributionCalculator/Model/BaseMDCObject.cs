using System.Windows.Controls;

namespace MomentDistributionCalculator.Model
{
    public class BaseMDCObject
    {
        private static double currentIndex = 0;
        private double m_Index = 0;

        /// <summary>
        /// constructor
        /// </summary>
        public BaseMDCObject()
        {

            m_Index = currentIndex;
            currentIndex++;
        }

        /// <summary>
        /// returns the index of this object.
        /// </summary>
        public double Index
        {
            get
            {
                return m_Index;
            }
            private set
            {
                m_Index = value;
            }
        }

        //public virtual void Draw(Canvas c) { }
    }
}