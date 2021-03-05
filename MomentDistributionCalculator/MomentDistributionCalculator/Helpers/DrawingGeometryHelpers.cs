using System;
using System.Collections.Generic;

namespace MomentDistributionCalculator.Model
{
    /// <summary>
    /// Contains Cartesian geometry calculation helper functions
    /// </summary>
    public static class DrawingGeometryHelpers
    {
        /// <summary>
        /// Returns the midpoint as an MDC_Node between two MDC_Node
        /// </summary>
        /// <param name="start">start point</param>
        /// <param name="end">end point</param>
        /// <returns></returns>
        public static MDC_Node GetMidPoint(MDC_Node start, MDC_Node end)
        {
            return new MDC_Node(0.5 * (start.X + end.X), 0.5 * (start.Y + end.Y), 0.5 * (start.Z + end.Z));
        }

        /// <summary>
        /// Returns the distance between two bectors
        /// </summary>
        /// <param name="start">start point</param>
        /// <param name="end">end point</param>
        /// <returns></returns>
        public static double GetLength(MDC_Node start, MDC_Node end)
        {
            return Math.Sqrt(Math.Pow((end.X - start.X), 2) + Math.Pow((end.Y - start.Y), 2) + Math.Pow((end.Z - start.Z),2));
        }

        /// <summary>
        /// Returns a list of points needed to create N equal
        /// segments on a straight line between two MDC_Nodes
        /// </summary>
        /// <param name="start">The start node</param>
        /// <param name="end">The end node</param>
        /// <param name="N">Number of equal seqments to divide the member into</param>
        /// <returns></returns>
        public static List<MDC_Node> GetNPointsLinear(MDC_Node start, MDC_Node end, int N)
        {
            List<MDC_Node> temp = new List<MDC_Node>();

            double dist = GetLength(start, end);
            double distX = (end.X - start.X) / N;
            double distY = (end.Y - start.Y) / N;
            double distZ = (end.Z - start.Z) / N;

            for (int i = 0; i < N + 1; i++)
            {
                MDC_Node tempNode = new MDC_Node((start.X + i * distX), (start.Y + i * distY), (start.Z + i * distZ));
                temp.Add(tempNode);
            }

            return temp;
        }

        /// <summary>
        /// Returns a list of points needed to create N equal
        /// segments on a straight line between the end points of an MDC_Beam object
        /// </summary>
        /// <param name="beam">MDC_Beam object</param>
        /// <param name="n">Number of segments to divide the object into</param>
        /// <returns></returns>
        public static List<MDC_Node> GetNPointsLinear(MDC_Beam beam, int n)
        {
            return DrawingGeometryHelpers.GetNPointsLinear(beam.Start, beam.End, n);
        }

        /// <summary>
        /// Functions for offsetting a node by another vector
        /// </summary>
        /// <param name="n">node to offset</param>
        /// <param name="n2">vector of offset distance</param>
        /// <returns></returns>
        public static MDC_Node Translate(MDC_Node n, MDC_Node n2)
        {
            return DrawingGeometryHelpers.Translate(n, (n2.X-n.X), (n2.Y-n.Y), (n2.Z-n.Z));
        }

        /// <summary>
        /// Function for offsetting a node by distances
        /// </summary>
        /// <param name="n">node to offset</param>
        /// <param name="x">x distance</param>
        /// <param name="y">y distance</param>
        /// <param name="z">z distance</param>
        /// <returns></returns>
        public static MDC_Node Translate(MDC_Node n, double x, double y, double z)
        {
            return new MDC_Node(n.X + x, n.Y + y, n.Z + z);
        }

        /// <summary>
        /// 2D Rotate of a point (or vector) about a base point by a specified angle
        /// </summary>
        /// <param name="n_base">base point of rotation</param>
        /// <param name="n">point</param>
        /// <param name="angle">angle to rotate in radians</param>
        /// <returns></returns>
        public static MDC_Node Rotate2D(MDC_Node n_base, MDC_Node n, double angle)
        {

            double dist = DrawingGeometryHelpers.GetLength(n_base, n);
            double alpha = Math.Atan((n.Y - n_base.Y) / (n.X - n_base.X));  // angle between node and base node w.r.t. left horizontal
            double beta = angle;
            double x1 = Math.Cos(alpha) * dist;
            double y1 = Math.Sin(alpha) * dist;
            double p = Math.Cos(beta) * dist;
            double q = Math.Sin(beta) * dist;
            double r = Math.Cos(alpha) * p;
            double s = Math.Sin(alpha) * p;
            double t = Math.Cos(alpha) * q;
            double u = Math.Sin(alpha) * q;

            double x2 = r - u;
            double y2 = t - s;

            //double newAng = angle + a;

            //double X1 = dist * Math.Cos(a+angle);
            //double Y1 = dist * Math.Sin(a+angle);
            //double newX = n_base.X + X1;
            //double newY = n_base.Y + Y1;

            //Console.WriteLine("-----------------------");

            //Console.WriteLine("                     dist:   " + dist);
            //Console.WriteLine(angle + " + " + a + " = " + newAng);
            //Console.WriteLine("                     n;      (" + n.X + "," + n.Y + ")");
            //Console.WriteLine(n.Index.ToString() + "                   n_base: (" + n_base.X + "," + n_base.Y + ")");
            //Console.WriteLine("                     X1,Y1:  (" + X1 + "," + Y1 + ")");
            //Console.WriteLine("                     X1,Y1:  (" + newX + "," + newY + ")");


            return new MDC_Node(x2, y2, 0);


        }
    }
}
