﻿using MomentDistributionCalculator.Helpers;
using MomentDistributionCalculator.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MomentDistributionCalculator.Model
{
    /// <summary>
    /// Creates a drawing grid Drawable Object
    /// USAGE:              
    /// MDC_DrawingObjects.Add(new MDC_Grid(5, 5, 200, 200, Colors.Green)   -- for solid grid lines starting at 0,0
    /// MDC_DrawingObjects.Add(new MDC_Grid(5, 5, 200, 200, Colors.Green, 200, 200, new DoubleCollection() { 1, 2, 3, 4, 5, 6, 7, 8 }));

    /// </summary>
    public class MDC_Grid : DrawingObject, INotifyPropertyChanged
    {
        private double m_xSpacing = 2;
        private double m_ySpacing = 2;
        private double m_insertPtX = 0.0f;
        private double m_insertPtY = 0.0f;
        private double m_gridWidth = 0.0f;
        private double m_gridHeight = 0.0f;

        private int lineCount = 0;

        private DoubleCollection m_lineType = new DoubleCollection() { 3, 3 };  // the pattern for the grid line as on-off-on-off
        Color m_Color = Colors.Transparent;

        public List<Line> GridLines { get; private set; } = new List<Line>();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public int LineCount
        {
            get => lineCount;
            set
            {
                lineCount = value;
                OnPropertyChanged("LineCount");;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n_x">number of grids in x direction</param>
        /// <param name="n_y">number of grids in y direction</param>
        /// <param name="width">width of the area to draw a grid over</param>
        /// <param name="height">height of area to draw a grid over</param>
        /// <param name="insert_x">upper left x-coord of the grid</param>
        /// <param name="insert_y">upper left y-coord of the grid</param>
        public MDC_Grid(int n_x, int n_y, double width, double height, Color color, double scale_factor = 1.0f, double insert_x = 0, double insert_y = 0, DoubleCollection lineType = null)
        {
            m_xSpacing = (double) (width / (n_x * scale_factor));
            m_ySpacing = (double) (height / (n_y * scale_factor));
            m_insertPtX = insert_x;
            m_insertPtY = insert_y;
            m_gridWidth = width;
            m_gridHeight = height;

            if (color != null)
                m_Color = color;
            if(lineType != null)
                m_lineType = lineType;

            CreateGrid(m_xSpacing,m_ySpacing,width, height);
        }

        protected void CreateGrid(double x_spacing, double y_spacing, double width, double height)
        {
            // Vertical gridlines
            int count = 0;


            while (m_insertPtX + m_xSpacing * count < m_gridWidth)
            {
                Line line = new Line();
                line.X1 = m_insertPtX + count * m_xSpacing;
                line.X2 = m_insertPtX + count * m_xSpacing;
                line.Y1 = 0 + m_insertPtY;
                line.Y2 = m_gridHeight + m_insertPtY;
                Console.WriteLine(line.X1.ToString()+ " " + line.Y1.ToString() + " " + line.X2.ToString() + " " + line.Y2.ToString());
                line.Stroke = new SolidColorBrush(m_Color);
                line.StrokeThickness = 1;
                line.StrokeDashArray = m_lineType;
                Canvas.SetLeft(line, 0);
                Canvas.SetTop(line,0);

                GridLines.Add(line);

                count++;
            }

            LineCount = count;
            //for (int i = 0; i < n_x * scale_factor + 1; i++)
            //{

            //}

            //// Horizontal gridlines
            //for (int i = 0; i < n_y * scale_factor + 1; i++)
            //{
            //    Line line = new Line();
            //    line.X1 = 0 + insert_x;
            //    line.X2 = width + insert_x;
            //    line.Y1 = insert_y + i * m_ySpacing;
            //    line.Y2 = insert_y + i * m_ySpacing;
            //    line.Stroke = new SolidColorBrush(m_Color);
            //    line.StrokeThickness = 1;
            //    line.StrokeDashArray = m_lineType;

            //    GridLines.Add(line);
            //}
        }

        public void ScaleGrid(double scale_factor)
        {
            //m_gridWidth = m_gridWidth * scale_factor;
            //m_gridHeight = m_gridWidth * scale_factor;
            m_xSpacing = m_xSpacing * scale_factor;
            m_ySpacing = m_ySpacing * scale_factor;

            //m_xSpacing = (double)(width / (n_x * scale_factor));
            //m_ySpacing = (double)(height / (n_y * scale_factor));
            //m_insertPtX = insert_x;
            //m_insertPtY = insert_y;
            //m_gridWidth = width;
            //m_gridHeight = height;

            // Clear the grid lines
            GridLines.Clear();

            CreateGrid(m_xSpacing,m_ySpacing,this.m_gridWidth,this.m_gridHeight);

            // Recreate the lines
            //for (int i = 0; i < n_x * scale_factor + 1; i++)
            //{
            //    Line line = new Line();
            //    line.X1 = insert_x + i * m_xSpacing;
            //    line.X2 = insert_x + i * m_xSpacing;
            //    line.Y1 = 0 + insert_y;
            //    line.Y2 = height + insert_y;
            //    line.Stroke = new SolidColorBrush(m_Color);
            //    line.StrokeThickness = 1;
            //    line.StrokeDashArray = m_lineType;

            //    GridLines.Add(line);
            //}

            //// Horizontal gridlines
            //for (int i = 0; i < n_y * scale_factor + 1; i++)
            //{
            //    Line line = new Line();
            //    line.X1 = 0 + insert_x;
            //    line.X2 = width + insert_x;
            //    line.Y1 = insert_y + i * m_ySpacing;
            //    line.Y2 = insert_y + i * m_ySpacing;
            //    line.Stroke = new SolidColorBrush(m_Color);
            //    line.StrokeThickness = 1;
            //    line.StrokeDashArray = m_lineType;

            //    GridLines.Add(line);
            //}

        }

        /// <summary>
        /// Draws the grid object
        /// </summary>
        /// <param name="c"></param>
        public override void Draw(Canvas c)
        {
            // Draw the grid lines
            foreach(Line line in GridLines)
            {
                DrawingHelpers.DrawLine(c, line.X1, line.Y1, line.X2, line.Y2, m_Color, m_lineType);
            }
        }

        // TODO:: Determine what to do if a window is resized or grid value is changed some how.  Need an update function of somesort here.
    }
}
