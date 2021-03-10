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
        // Draws a transparent filled circle at an MDC_Node location
        public static void DrawCircleHollow(Canvas c, MDC_Node n, double r, Color color)
        {
            DrawCircle(c, n.X, n.Y, r, color, Colors.Transparent);
        }

        // Draws a transparent filled circle at an MDC_Node location
        public static void DrawCircleHollow(Canvas c, double x, double y, double r, Color color)
        {
            DrawCircle(c, x, y, r, color, Colors.Transparent);
        }

        public static void DrawCircle(Canvas c, double x, double y, double r, Color outline, Color fill)
        {
            // Draw a circle node
            Ellipse myEllipse = new Ellipse();
            myEllipse.Fill = new SolidColorBrush(fill);
            myEllipse.Stroke = new SolidColorBrush(outline);
            myEllipse.StrokeThickness = 2.0f;

            myEllipse.Width = r;
            myEllipse.Height = r;
            Canvas.SetLeft(myEllipse, x - myEllipse.Width / 2.0f);
            Canvas.SetTop(myEllipse, y - myEllipse.Width / 2.0f);

            myEllipse.HorizontalAlignment = HorizontalAlignment.Left;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;

            c.Children.Add(myEllipse);
        }
        /// <summary>
        /// Draws a circle at an MDC_Node reference
        /// </summary>
        /// <param name="c"></param>
        /// <param name="n"></param>
        /// <param name="r"></param>
        /// <param name="outline"></param>
        /// <param name="fill"></param>
        public static void DrawCircle(Canvas c, MDC_Node n, double r, Color outline, Color fill)
        {
            DrawCircle(c, n.X, n.Y, r, outline, fill);
        }

        /// <summary>
        /// Draw text object at an MDC_Node location
        /// </summary>
        /// <param name="c"></param>
        /// <param name="n"></param>
        /// <param name="str"></param>
        /// <param name="h"></param>
        /// <param name="r"></param>
        /// <param name="color"></param>
        public static void DrawText(Canvas c, MDC_Node n, string str, double h, double r, Color color)
        {
            DrawText(c, n.X, n.Y, str, h, r, color);
        }

        /// <summary>
        /// Draw's text at coordinates x, y on the canbas
        /// </summary>
        /// <param name="c"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="str"></param>
        /// <param name="h"></param>
        /// <param name="r"></param>
        /// <param name="color"></param>
        public static void DrawText(Canvas c, double x, double y, string str, double h, double r, Color color)
        {
            // Draw a text for the index lavel
            TextBlock textBlock = new TextBlock();
            textBlock.Text = str;
            textBlock.FontSize = h;
            textBlock.Foreground = new SolidColorBrush(color);
            Canvas.SetLeft(textBlock, x + r/2.0f);
            Canvas.SetTop(textBlock, y);
            c.Children.Add(textBlock);
        }


        /// <summary>
        /// Draws a line between two MDC_Node points
        /// </summary>
        /// <param name="c"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="color"></param>
        public static void DrawLine(Canvas c, MDC_Node start, MDC_Node end, Color color)
        {
            DrawLine(c, start.X, start.Y, end.X, end.Y, color);
        }

        public static Shape DrawLine(Canvas c, double x_start, double y_start, double x_end, double y_end, Color color)
        {
            Line myLine = new Line();
            myLine.Stroke = new SolidColorBrush(color);
            myLine.StrokeThickness = 2;
            myLine.X1 = x_start;
            myLine.Y1 = y_start;
            myLine.X2 = x_end;
            myLine.Y2 = y_end;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;

            c.Children.Add(myLine);

            return myLine;
        }

        /// <summary>
        /// Draws an arrow object with no fill
        /// </summary>
        /// <param name="c">Our canvas object</param>
        /// <param name="point">Point where tip of the arrow is placed</param>
        /// <param name="outline">Color of the arrow outline</param>
        /// <param name="len">Length of the arrow shaft</param>
        public static void DrawArrowsNoFill(Canvas c, double x, double y, double z, Color outline, double len = 30.0f)
        {
            DrawArrows(c, x, y, z, outline, Colors.Transparent);
        }

        public static void DrawArrows(Canvas c, double x, double y, double z, Color outline, Color fill, DirectionVectors dv = DirectionVectors.DIR_VERT_POS, double len = 30.0f)
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

            System.Windows.Point Point1 = new System.Windows.Point(x, y);
            System.Windows.Point Point2 = new System.Windows.Point(x-d1, y+d3);
            System.Windows.Point Point3 = new System.Windows.Point(x-d2, y+d3);
            System.Windows.Point Point4 = new System.Windows.Point(x-d2, y+d4);
            System.Windows.Point Point5 = new System.Windows.Point(x+d2, y+d4);
            System.Windows.Point Point6 = new System.Windows.Point(x+d2, y+d3);
            System.Windows.Point Point7 = new System.Windows.Point(x+d1, y+d3);

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
