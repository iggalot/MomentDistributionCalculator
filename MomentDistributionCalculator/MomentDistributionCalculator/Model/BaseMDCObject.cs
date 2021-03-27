using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MomentDistributionCalculator.Model
{
    public class ShapeEventArgs
    {
        public ShapeEventArgs()
        {

            MessageBox.Show("In Event");
        }
    }

public class BaseMDCObject : Shape
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

        protected override Geometry DefiningGeometry => throw new System.NotImplementedException();


        public event EventHandler<ShapeEventArgs> ShapeChanged;
        public event EventHandler<MouseEventArgs> MouseEntered;

        protected virtual void OnShapeChanged(ShapeEventArgs e)
        {
            // Safely raise the event for all subscribers
            ShapeChanged?.Invoke(this, e);
        }



        protected virtual void OnMouseEntered(object sender, MouseEventArgs e)
        {
            MouseEntered?.Invoke(this, e);

            MessageBox.Show("Entered an object");
            this.Fill = Brushes.Red; 
        }

        public void OnMouseLeave(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Left an object");
            this.Fill = Brushes.Green;
        }
        //public virtual void Draw(Canvas c) { }
    }
}