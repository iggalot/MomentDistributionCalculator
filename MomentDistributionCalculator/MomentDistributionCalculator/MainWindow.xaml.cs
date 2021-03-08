using MomentDistributionCalculator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MomentDistributionCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int NUM_NODES = 10; // number of nodes along the top of the structure
        private const int NUM_BEAMS = NUM_NODES - 1; // number of beams along the top of the structure

        private List<MDC_Beam> m_Model = null;

        public List<MDC_Beam> Members { get { return m_Model; } set { m_Model = value; } }

        public MainWindow()
        {
            InitializeComponent();

            OnUserCreate();
            OnUserUpdate();

        }

        private void OnUserCreate()
        {
            Members = new List<MDC_Beam>();

            // Create some nodes
            double len = (MainCanvas.Width) / (NUM_NODES+1);
            double centerX = (MainCanvas.Width) / 2.0f;
            double centerY = (MainCanvas.Height) / 2.0f;
            double centerZ = 0.0f;  // for 2D space.

            double startX = centerX - NUM_NODES * len / 2.0f;
            double endX = centerX + NUM_NODES * len / 2.0f;
            
            double startY = (MainCanvas.Height / 2.0f) - len / (2.0f);
            double endY = (MainCanvas.Height / 2.0f) + len / (2.0f);

            double startZ = centerZ;
            double endZ = centerZ;

            MDC_Node start = new MDC_Node(startX, startY, startZ);
            MDC_Node end = null; ;
            MDC_Node bottom = null; ;
            for (int i = 1; i < NUM_NODES+1; i++)
            {
                end = new MDC_Node(startX + (i*len), startY, startZ);
                bottom = new MDC_Node(startX + ((i-1) * len), startY + len, startZ);

                // Create some beams
                MDC_Beam beam1 = new MDC_Beam(start, end);
                MDC_Beam beam2 = new MDC_Beam(start, bottom);
                Members.Add(beam1);
                Members.Add(beam2);
                start = end;

                // Add a distributed load
                AddDistributedLoad(beam1, 2.0f);
            }

            // Find the last bottom point and create a member for the rightmost column
            bottom = new MDC_Node(start.X, start.Y + len, start.Z);
            Members.Add(new MDC_Beam(start, bottom));


        }

        public MDC_Beam GetBeam(int id)
        {
            bool found = false;
            MDC_Beam temp = null;

            foreach(MDC_Beam model in Members)
            {
                if(model.Index == id)
                {
                    temp = model;
                    found = true;
                    break;
                }
            }

            if(!found)
            {
                Console.WriteLine("In GetBeam(): Error retrieving beam # " + id + ". No such member found");
                return null;
            } else
            {
                return temp;
            }

           
        }
        protected void AddDistributedLoad(MDC_Beam beam, double intensity)
        {
            MemberDistributedLoad temp = new MemberDistributedLoad(beam, intensity);
            beam.AddLoad(temp);

        }

        private void OnUserUpdate()
        {
            // Draw the Model
            foreach (MDC_Beam item in Members)
            {
                item.Draw(MainCanvas);
            }

        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = Mouse.GetPosition(MainCanvas);
            Coords.Text = p.X.ToString() + " , " + p.Y.ToString();
        }
    }
}
