using MomentDistributionCalculator.Model;
using MomentDistributionCalculator.Helpers;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Threading;

namespace MomentDistributionCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const int APP_IDLE_DELAY = 1000;  // delay in milliseconds for a backgrgound thread

        private const int NUM_NODES = 10; // number of nodes along the top of the structure
        private const int NUM_BEAMS = NUM_NODES - 1; // number of beams along the top of the structure

        private bool m_bFirstPointSelected = false;
        private bool m_bSecondPointSelected = false;
        private bool m_bMouseHasMoved = false;  // has the mouse mved since we first clicked?
        private Point m_FirstSelect;
        private Point m_SecondSelect;

        private Point mouseLeftDownPoint;   // captures the cursor position on a left click on the canvas
        private Line rubberBand = null;    // for the beam snap rubber band
        private Shape rubberBandWindow = null; // for the selection box on mouse down

        private List<MDC_Beam> m_Model = null;

        /// <summary>
        ///  A delegate for system idle.
        /// </summary>
        public delegate void OnApplicationIdle(bool status);




        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public List<DrawingObject> MDC_DrawingObjects { get; set; }

        public List<MDC_Beam> Members { get { return m_Model; } set { m_Model = value; } }
        public bool bFirstPointSelected 
        { 
            get { return m_bFirstPointSelected; } 
            set { m_bFirstPointSelected = value; OnPropertyChanged("bFirstPointSelected"); } 
        }

        public bool bSecondPointSelected
        {
            get { return m_bSecondPointSelected; }
            set { m_bSecondPointSelected = value; OnPropertyChanged("bSecondPointSelected"); }
        }

        public double MouseLeftX_First
        {
            get { return m_FirstSelect.X; }
            set { m_FirstSelect.X = value; OnPropertyChanged("MouseLeftX_First"); }
        }

        public double MouseLeftY_First
        {
            get { return m_FirstSelect.Y; }
            set { m_FirstSelect.Y = value; OnPropertyChanged("MouseLeftY_First"); }
        }

        public double MouseLeftX_Second
        {
            get { return m_SecondSelect.X; }
            set { m_SecondSelect.X = value; OnPropertyChanged("MouseLeftX_Second"); }
        }

        public double MouseLeftY_Second
        {
            get { return m_SecondSelect.Y; }
            set { m_SecondSelect.Y = value; OnPropertyChanged("MouseLeftY_Second"); }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Create the mouse envents
            MainCanvas.MouseLeftButtonUp += MainCanvas_MouseLeftButtonUp;
            MainCanvas.MouseLeftButtonDown += MainCanvas_MouseLeftButtonDown;
            MainCanvas.MouseRightButtonUp += MainCanvas_MouseRightButtonUp;
            MainCanvas.MouseMove += MainCanvas_MouseMove;

            // Create an Application Idle event
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.SystemIdle);
            timer.Tick += (s, e) => { OnIdle(true); };
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();
 
            // Initialize whether or not a first point or a second point was selected.
            bFirstPointSelected = false;
            bSecondPointSelected = false;

            DataContext = this;

            OnUserCreate();
            OnUserUpdate();

        }

        static int count = 0;

        private async void OnIdle(bool status)
        {
            Console.WriteLine(count.ToString() + "In Idle Event");
            await Task.Delay(APP_IDLE_DELAY);
            count++;
        }

        private void OnUserCreate()
        {
            Members = new List<MDC_Beam>();
            MDC_DrawingObjects = new List<DrawingObject>();

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


            //// Draws a dummy structure
            //DummyStructure(len, startX, endX, startY, endY, startZ, endZ);
        }

        private void DummyStructure(double len, double startX, double endX, double startY, double endY, double startZ, double endZ)
        {
            MDC_Node start = new MDC_Node(startX, startY, startZ);
            MDC_Node end = null; ;
            MDC_Node bottom = null; ;
            for (int i = 1; i < NUM_NODES + 1; i++)
            {
                end = new MDC_Node(startX + (i * len), startY, startZ);
                bottom = new MDC_Node(startX + ((i - 1) * len), startY + len, startZ);

                // Create some beams
                MDC_Beam beam1 = new MDC_Beam(start, end);
                MDC_Beam beam2 = new MDC_Beam(start, bottom);
                Members.Add(beam1);
                Members.Add(beam2);

                MDC_DrawingObjects.Add(beam1);
                MDC_DrawingObjects.Add(beam2);

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
            // Clear the canvas
            MainCanvas.Children.Clear();

            // Draw the Model
            foreach (MDC_Beam item in Members)
            {
                item.Draw(MainCanvas);
            }

            // Draw helper features
            if (bFirstPointSelected)
            {
                // Draw the first marker node
                DrawingHelpers.DrawCircle(MainCanvas, MouseLeftX_First, MouseLeftY_First, 15, Colors.Black, Colors.Blue);

                // If the first point is selected, draw a rubber band line
                DrawingHelpers.DrawLine(MainCanvas, MouseLeftX_First, MouseLeftY_First, MouseLeftX_Second, MouseLeftY_Second, Colors.Aqua);
            }

            // Draw helper features
            if (bSecondPointSelected)
            {
                // Draw the first marker node
                DrawingHelpers.DrawCircle(MainCanvas, MouseLeftX_Second, MouseLeftY_Second, 15, Colors.Black, Colors.Blue);
            }
        }

        protected void MainCanvas_MouseMove(object sender, MouseEventArgs args)
        {
            if (MainCanvas.IsMouseCaptured)
            {
                Point currentPoint = args.GetPosition(MainCanvas);

                if (rubberBandWindow == null)
                {
                    //MessageBox.Show("Creating new rubber band window");
                    rubberBandWindow = new Rectangle();
                    rubberBandWindow.Stroke = new SolidColorBrush(Colors.Green);
                    MainCanvas.Children.Add(rubberBandWindow);
                }

                
                // Rubber band selection box
                if(rubberBand == null)
                {
                    rubberBand = new Line();
                    rubberBand.Stroke = new SolidColorBrush(Colors.Blue);
                    MainCanvas.Children.Add(rubberBand);
                }

                double width = Math.Abs(mouseLeftDownPoint.X - currentPoint.X);
                double height = Math.Abs(mouseLeftDownPoint.Y - currentPoint.Y);
                double left = Math.Min(mouseLeftDownPoint.X, currentPoint.X);
                double top = Math.Min(mouseLeftDownPoint.Y, currentPoint.Y);

                rubberBandWindow.Width = width;
                rubberBandWindow.Height = height;
                Canvas.SetLeft(rubberBandWindow, left);
                Canvas.SetTop(rubberBandWindow, top);

  

                // Rubber band line
                if(bFirstPointSelected)
                {
                    rubberBand.X1 = MouseLeftX_First;
                    rubberBand.Y1 = MouseLeftY_First;
                    rubberBand.X2 = MouseLeftX_Second;
                    rubberBand.Y2 = MouseLeftY_Second;
                    MouseLeftX_Second = currentPoint.X;
                    MouseLeftY_Second = currentPoint.Y;
                    //Canvas.SetLeft(rubberBand, mouseLeftDownPoint.X);
                    //Canvas.SetTop(rubberBand, mouseLeftDownPoint.Y);
                    m_bMouseHasMoved = true;
                }


                // Update the status window for the mouse coordinates
                Coords.Text = currentPoint.X.ToString() + " , " + currentPoint.Y.ToString();
            }

            args.Handled = true;



        }

        protected void MainCanvas_MouseLeftButtonDown(object sender, MouseEventArgs args)
        {
            if (!MainCanvas.IsMouseCaptured)
            {
                // Get the position of the mouse and store it
                mouseLeftDownPoint = args.GetPosition(MainCanvas);
                MainCanvas.CaptureMouse();
                args.Handled = true;
            }
        }

        private void MainCanvas_MouseLeftButtonUp(object sender, MouseEventArgs args)
        {
            if (MainCanvas.IsMouseCaptured)
            {
                if (rubberBandWindow != null)
                {
                    //MessageBox.Show("Deleting new rubber band window");
                    MainCanvas.Children.Remove(rubberBandWindow);
                    rubberBandWindow = null;
                }

                if (rubberBand != null)
                {
                    //MessageBox.Show("Deleting new rubber band window");
                    MainCanvas.Children.Remove(rubberBand);
                    rubberBand = null;
                }

                Point p = args.GetPosition(MainCanvas);

                if(!bFirstPointSelected)
                {
                    MainCanvas.Background = Brushes.Gray;
                    MouseLeftX_First = p.X;
                    MouseLeftY_First = p.Y;
                    MouseLeftX_Second = p.X;
                    MouseLeftY_Second = p.Y;

                    bFirstPointSelected = true;
                    args.Handled = true;
                }
                else if ((!bSecondPointSelected) ) {
                    MainCanvas.Background = Brushes.LightGray;
                    MouseLeftX_Second = p.X;
                    MouseLeftY_Second = p.Y;
                    bSecondPointSelected = true;
                    args.Handled = true;
                }

                // If we have two clicks, create the nodes and make the beam member
                if(bFirstPointSelected && bSecondPointSelected)
                {
                    // Create the nodes
                    MDC_Node node1 = new MDC_Node(MouseLeftX_First, MouseLeftY_First, 0.0f);
                    MDC_Node node2 = new MDC_Node(MouseLeftX_Second, MouseLeftY_Second, 0.0f);

                    // both buttons have been clicked, so create a beam member between the two
                    Members.Add(new MDC_Beam(node1, node2));
                    
                    bFirstPointSelected = false;
                    bSecondPointSelected = false;
                    MouseLeftX_First = 0.0;
                    MouseLeftY_First = 0.0;
                    MouseLeftX_Second = 0.0;
                    MouseLeftY_Second = 0.0;
                    m_bMouseHasMoved = false;

 
                }

                //Mouse.Capture(null);   // release the main canvas mouse capture

                this.OnUserUpdate();
            }
        }

        private void MainCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainCanvas.Background = Brushes.LightGray;

            // clear the first and second click
            bFirstPointSelected = false;
            bSecondPointSelected = false;


            this.OnUserUpdate();

        }
    }
}
