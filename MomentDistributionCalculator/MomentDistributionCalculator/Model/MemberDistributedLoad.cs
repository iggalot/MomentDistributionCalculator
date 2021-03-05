using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MomentDistributionCalculator.Model
{
    public class MemberDistributedLoad
    {
        private int NUM_ARROWS_PER_DIST_LOAD = 5;
        public static int currentId = 0;
        private MDC_Beam m_Beam = null;
        private int m_id = 0;
        private float m_intensity = 0;

        public int Id { get { return m_id; } set { m_id = value; } }
        public float Magnitude { get { return m_intensity; } set { m_intensity = value; } }
        public MDC_Beam AttachedTo { get { return m_Beam; } set { m_Beam = value; } }

        /// <summary>
        /// Points where load arrows are applied
        /// </summary>
        public List<MDC_Node> LoadPoints { get; set; }
        public MemberDistributedLoad(MDC_Beam beam, float intensity)
        {
            LoadPoints = new List<MDC_Node>();

            Id = currentId;
            currentId++;

            Magnitude = intensity;
            AttachedTo = beam;

            // draw arrows evenly spaced
            LoadPoints = AttachedTo.GetNPoints(NUM_ARROWS_PER_DIST_LOAD);
        }

        public void Draw(Canvas c)
        {
            // Draw a circle node
            Ellipse myEllipse = new Ellipse();
            myEllipse.Fill = new SolidColorBrush(Colors.Blue);
            myEllipse.Stroke = Brushes.Black;
            myEllipse.StrokeThickness = 2.0f;

            myEllipse.Width = 20;
            myEllipse.Height = 20;
            Canvas.SetLeft(myEllipse, this.AttachedTo.GetMidPoint.X);
            Canvas.SetTop(myEllipse, this.AttachedTo.GetMidPoint.Y);

            myEllipse.HorizontalAlignment = HorizontalAlignment.Left;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;

            c.Children.Add(myEllipse);


            foreach(MDC_Node node in LoadPoints)
            {
                DrawArrows(c, node);
            }

            //TODO:  Draw line for top of distributed load
        }

        private void DrawArrows(Canvas c, MDC_Node point)
        {
            // Draw uniform load
            // first draw the arrow stem
            Line myLine = new Line();
            myLine.Stroke = Brushes.Green;
            myLine.StrokeThickness = 4.0f;
            myLine.X1 = point.X;
            myLine.Y1 = point.Y;
            myLine.X2 = point.X;
            myLine.Y2 = point.Y - 30;
            c.Children.Add(myLine);

            // then draw the triangle
            Polygon triangle = new Polygon();
            triangle.Stroke = Brushes.Green;
            triangle.Fill = Brushes.Green;
            triangle.StrokeThickness = 3;

            System.Windows.Point Point1 = new System.Windows.Point(point.X, point.Y);
            System.Windows.Point Point2 = new System.Windows.Point(point.X - 3, point.Y - 5);
            System.Windows.Point Point3 = new System.Windows.Point(point.X + 3, point.Y - 5);
            PointCollection polygonPoints = new PointCollection();
            polygonPoints.Add(Point1);
            //Console.WriteLine("DrawArrow: " + Point1.X + " , " + Point1.Y);
            polygonPoints.Add(Point2);
            polygonPoints.Add(Point3);
            triangle.Points = polygonPoints;
            c.Children.Add(triangle);
        }
    }
}
