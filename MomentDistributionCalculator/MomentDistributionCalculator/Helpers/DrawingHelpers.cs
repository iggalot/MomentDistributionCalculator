using MomentDistributionCalculator.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MomentDistributionCalculator.Helpers
{
    public enum DirectionVectors{
        DIR_VERT_NEG,  // downward on screen
        DIR_VERT_POS,  // upward on screen
        DIR_HORIZ_NEG, // right on screen
        DIR_HORIZ_POS, // left on screen
        DIR_NORMAL_POS, // normal to the member (acts up if start node is left of end node)
        DIR_NORMAL,NEG // normal to the member (acts down if start node is left of end node)
    }
    /// <summary>
    /// Contains routines for drawing basic objects
    /// </summary>
    public static class DrawingHelpers
    {
        // Draws a transparent filled circle

        public static void DrawCircleHollow(Canvas c, MDC_Node n, double r, Color color)
        {
            DrawCircle(c, n, r, color, Colors.Transparent);
        }

        public static void DrawCircle(Canvas c, MDC_Node n, double r, Color outline, Color fill)
        {
            // Draw a circle node
            Ellipse myEllipse = new Ellipse();
            myEllipse.Fill = new SolidColorBrush(fill);
            myEllipse.Stroke = new SolidColorBrush(outline);
            myEllipse.StrokeThickness = 2.0f;

            myEllipse.Width = r;
            myEllipse.Height = r;
            Canvas.SetLeft(myEllipse, n.X - myEllipse.Width / 2.0f);
            Canvas.SetTop(myEllipse, n.Y - myEllipse.Width / 2.0f);

            myEllipse.HorizontalAlignment = HorizontalAlignment.Left;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;

            c.Children.Add(myEllipse);
        }

        public static void DrawText(Canvas c, MDC_Node n, string str, double h, double r, Color color)
        {
            // Draw a text for the index lavel
            TextBlock textBlock = new TextBlock();
            textBlock.Text = str;
            textBlock.FontSize = h;
            textBlock.Foreground = new SolidColorBrush(color);
            Canvas.SetLeft(textBlock, n.X + r/2.0f);
            Canvas.SetTop(textBlock, n.Y);
            c.Children.Add(textBlock);
        }

        public static void DrawLine(Canvas c, MDC_Node start, MDC_Node end, Color color)
        {
            Line myLine = new Line();
            myLine.Stroke = new SolidColorBrush(color);
            myLine.StrokeThickness = 2;
            myLine.X1 = start.X;
            myLine.Y1 = start.Y;
            myLine.X2 = end.X;
            myLine.Y2 = end.Y;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            c.Children.Add(myLine);
        }

        /// <summary>
        /// Draws an arrow object with no fill
        /// </summary>
        /// <param name="c">Our canvas object</param>
        /// <param name="point">Point where tip of the arrow is placed</param>
        /// <param name="outline">Color of the arrow outline</param>
        /// <param name="len">Length of the arrow shaft</param>
        public static void DrawArrowsNoFill(Canvas c, MDC_Node point, Color outline, double len = 30.0f)
        {
            DrawArrows(c, point, outline, Colors.Transparent);
        }

        public static void DrawArrows(Canvas c, MDC_Node point, Color outline, Color fill, DirectionVectors dv = DirectionVectors.DIR_VERT_POS, double len = 30.0f)
        {
            // draw arrow shape
            Polygon triangle = new Polygon();
            triangle.Stroke = new SolidColorBrush(outline);
            triangle.Fill = new SolidColorBrush(fill);
            triangle.StrokeThickness = 1;

            double d1 = len / 5.0f;
            double d2 = len / 15.0f;
            double d3 = len / 3.0f;
            double d4 = 2 * len / 3.0f;
            double angle = 0.0f;   // angle from point upward measured CCW in radians




            switch (dv)
            {
                case DirectionVectors.DIR_VERT_NEG:
                    //angle = Math.PI;
                    break;
                case DirectionVectors.DIR_VERT_POS:
                    angle = 0;

                    point = DrawingGeometryHelpers.Translate(point, 0.0f, -d4, 0.0f);
                    break;
                case DirectionVectors.DIR_HORIZ_NEG:
                    break;
                case DirectionVectors.DIR_HORIZ_POS:

                    break;
                case DirectionVectors.DIR_NORMAL_POS:
                    break;
                case DirectionVectors.DIR_NORMAL:
                    break;
                case DirectionVectors.NEG:
                    break;
                default:
                    break;
            }

            // points to draw an upward arrow
            MDC_Node p1 = point;
            MDC_Node p2 = DrawingGeometryHelpers.Translate(p1, -d1, d3, 0.0f);
            MDC_Node p3 = DrawingGeometryHelpers.Translate(p1, -d2, d3, 0.0f);
            MDC_Node p4 = DrawingGeometryHelpers.Translate(p1, -d2, d4, 0.0f);
            MDC_Node p5 = DrawingGeometryHelpers.Translate(p1, d2, d4, 0.0f);
            MDC_Node p6 = DrawingGeometryHelpers.Translate(p1, d2, d3, 0.0f);
            MDC_Node p7 = DrawingGeometryHelpers.Translate(p1, d1, d3, 0.0f);

            //// Rotate to orientation
            //p2 = DrawingGeometryHelpers.Rotate2D(p1, p2, angle);
            //p3 = DrawingGeometryHelpers.Rotate2D(p1, p3, angle);
            //p4 = DrawingGeometryHelpers.Rotate2D(p1, p4, angle);
            //p5 = DrawingGeometryHelpers.Rotate2D(p1, p5, angle);
            //p6 = DrawingGeometryHelpers.Rotate2D(p1, p6, angle);
            //p7 = DrawingGeometryHelpers.Rotate2D(p1, p7, angle);

            System.Windows.Point Point1 = new System.Windows.Point(p1.X, p1.Y);
            System.Windows.Point Point2 = new System.Windows.Point(p2.X, p2.Y);
            System.Windows.Point Point3 = new System.Windows.Point(p3.X, p3.Y);
            System.Windows.Point Point4 = new System.Windows.Point(p4.X, p4.Y);
            System.Windows.Point Point5 = new System.Windows.Point(p5.X, p5.Y);
            System.Windows.Point Point6 = new System.Windows.Point(p6.X, p6.Y);
            System.Windows.Point Point7 = new System.Windows.Point(p7.X, p7.Y);

            PointCollection polygonPoints = new PointCollection
            {
                Point1,
                Point2,
                Point3,
                Point4,
                Point5,
                Point6,
                Point7
            };
            triangle.Points = polygonPoints;
            c.Children.Add(triangle);
        }
    }
}
