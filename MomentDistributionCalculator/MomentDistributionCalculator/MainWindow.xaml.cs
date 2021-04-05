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
using SharpGL;
using SharpGL.WPF;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Core;

namespace MomentDistributionCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const float WINDOW_WIDTH = 1000;
        private const float WINDOW_HEIGHT = 800;
        private const float CANVAS_WIDTH = 800;
        private const float CANVAS_HEIGHT = 400;

        private const int APP_IDLE_DELAY = 10;  // delay in milliseconds for a backgrgound thread

        private const int NUM_NODES = 10; // number of nodes along the top of the structure
        private const int NUM_BEAMS = NUM_NODES - 1; // number of beams along the top of the structure

        private bool m_bFirstPointSelected = false;
        private bool m_bSecondPointSelected = false;
        private bool m_bMouseHasMoved = false;  // has the mouse mved since we first clicked?

        private Point m_FirstLeftMouseDownPoint = new Point();   // captures the cursor position on a left click on the canvas
        private Point m_SecondLeftMouseDownPoint = new Point();   // captures the cursor position on a left click on the canvas
        private Point m_CurrentMouseLocation = new Point(); // the current mouse location
        //private Line rubberBand = null;    // for the beam snap rubber band
        //private Shape rubberBandWindow = null; // for the selection box on mouse down

        private double m_scaleFactor  = 1.0;  // the scale factor to apply to the canvas

        private string m_statusMessage = "";
//        private ScaleTransform m_scaleTransform = new ScaleTransform(1.0,1.0);  // the render transform to apply to the canvas

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
        public MDC_Grid MainGrid { get; set; }

        public List<MDC_Beam> Members { get { return m_Model; } set { m_Model = value; } }

        public double ScaleFactorX
        {
            get { return m_scaleFactor; }
            set { m_scaleFactor = value;
                OnPropertyChanged("ScaleFactorX");
            }
        }

        public string StatusMessage
        {
            get { return m_statusMessage; }
            set
            {
                m_statusMessage = value;
                OnPropertyChanged("StatusMessage");
            }
        }

        public bool IsSelectedFirstPoint 
        { 
            get { return m_bFirstPointSelected; } 
            set { m_bFirstPointSelected = value; OnPropertyChanged("IsSelectedFirstPoint"); } 
        }

        public bool IsSelectedSecondPoint
        {
            get { return m_bSecondPointSelected; }
            set { m_bSecondPointSelected = value; OnPropertyChanged("IsSelectedSecondPoint"); }
        }

        public Point FirstLeftMouseDownPoint
        {
            get { return m_FirstLeftMouseDownPoint; }
            set { m_FirstLeftMouseDownPoint = value; OnPropertyChanged("FirstLeftMouseDownPoint"); }
        }

        public Point SecondLeftMouseDownPoint
        {
            get { return m_SecondLeftMouseDownPoint; }
            set { m_SecondLeftMouseDownPoint = value; OnPropertyChanged("SecondLeftMouseDownPoint"); }
        }

        public Point CurrentMouseLocationPoint
        {
            get { return m_CurrentMouseLocation; }
            set { m_CurrentMouseLocation = value; OnPropertyChanged("CurrentMouseLocationPoint"); }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Width = WINDOW_WIDTH;
            this.Height = WINDOW_HEIGHT;

            DataContext = this;

            OnUserCreate();
            OnUserUpdate();
        }

        static int count = 0;

        /// <summary>
        /// An idle process task that continuously redraws the screen.  
        /// </summary>
        /// <param name="status"></param>
        private async void OnIdle(bool status)
        {
            //Console.WriteLine(count.ToString() + "In Idle Event");
            await Task.Delay(APP_IDLE_DELAY);

            /// TODO:: Add a "screen is dirty" type of marker so that the screen doesn't update unless something has changed.
            this.OnUserUpdate();  // now update
            count++;
        }

        private void OnUserCreate()
        {
            Members = new List<MDC_Beam>();
            MDC_DrawingObjects = new List<DrawingObject>();

            MainCanvas.Width = CANVAS_WIDTH;
            MainCanvas.Height = CANVAS_HEIGHT;

            // Create the mouse events
            MainCanvas.MouseLeftButtonUp += MainCanvas_MouseLeftButtonUp;
            MainCanvas.MouseLeftButtonDown += MainCanvas_MouseLeftButtonDown;
            MainCanvas.MouseRightButtonUp += MainCanvas_MouseRightButtonUp;
            MainCanvas.MouseMove += MainCanvas_MouseMove;
            CanvasViewBox.MouseWheel += MainCanvas_MouseWheel;

            // Create an Application Idle event
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.SystemIdle);
            timer.Tick += (s, e) => { OnIdle(true); };
            timer.Interval = new TimeSpan(0, 0, APP_IDLE_DELAY / 1000);
            timer.Start();

            // Initialize whether or not a first point or a second point was selected.
            IsSelectedFirstPoint = false;
            IsSelectedSecondPoint = false;

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

            // Create our drawing grid object
            MainGrid = new MDC_Grid(20, 10, MainCanvas.Width, MainCanvas.Height, Colors.DarkGray);
            MDC_DrawingObjects.Add(MainGrid);
            MainCanvas.Children.Add(MainGrid);

            //// Draws a dummy structure
            //DummyStructure(len, startX, endX, startY, endY, startZ, endZ);
        }

   

        /// <summary>
        /// Utility function to autogenerate a default model.
        /// </summary>
        /// <param name="len"></param>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        /// <param name="startY"></param>
        /// <param name="endY"></param>
        /// <param name="startZ"></param>
        /// <param name="endZ"></param>
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

            foreach (DrawingObject item in MDC_DrawingObjects)
            {
                item.Draw(MainCanvas);
            }

            // Draw the Model
            foreach (MDC_Beam item in Members)
            {
                item.Draw(MainCanvas);
            }

            // Draw helper features
            if (IsSelectedFirstPoint)
            {
                // Draw the first marker node
                DrawingHelpers.DrawCircle(MainCanvas, FirstLeftMouseDownPoint.X, FirstLeftMouseDownPoint.Y, 15, Colors.Black, Colors.Blue);

                // If the first point is selected, draw a rubber band line
                DrawingHelpers.DrawLine(MainCanvas, FirstLeftMouseDownPoint.X, FirstLeftMouseDownPoint.Y, CurrentMouseLocationPoint.X, CurrentMouseLocationPoint.Y, Colors.Aqua);

                // Draw a node at the current mouse location
                DrawingHelpers.DrawCircle(MainCanvas, CurrentMouseLocationPoint.X, CurrentMouseLocationPoint.Y, 6, Colors.Black, Colors.Blue);
            }
        }

        protected void MainCanvas_MouseMove(object sender, MouseEventArgs args)
        {
            if (MainCanvas.IsMouseCaptured)
            {
                CurrentMouseLocationPoint = args.GetPosition(MainCanvas);

                //if (rubberBandWindow == null)
                //{
                //    //MessageBox.Show("Creating new rubber band window");
                //    rubberBandWindow = new Rectangle();
                //    rubberBandWindow.Stroke = new SolidColorBrush(Colors.Green);
                //    MainCanvas.Children.Add(rubberBandWindow);
                //}

                //double width = Math.Abs(m_FirstLeftMouseDownPoint.X - CurrentMouseLocationPoint.X);
                //double height = Math.Abs(m_FirstLeftMouseDownPoint.Y - CurrentMouseLocationPoint.Y);
                //double left = Math.Min(m_FirstLeftMouseDownPoint.X, CurrentMouseLocationPoint.X);
                //double top = Math.Min(m_FirstLeftMouseDownPoint.Y, CurrentMouseLocationPoint.Y);

                //rubberBandWindow.Width = width;
                //rubberBandWindow.Height = height;
                //Canvas.SetLeft(rubberBandWindow, left);
                //Canvas.SetTop(rubberBandWindow, top);

                // For the rubber band line
                if (IsSelectedFirstPoint)
                {
                    m_bMouseHasMoved = true;
                }


                // Update the status window for the mouse coordinates
                Coords.Text = CurrentMouseLocationPoint.X.ToString() + " , " + CurrentMouseLocationPoint.Y.ToString();
            }

            args.Handled = true;
        }

        protected void MainCanvas_MouseLeftButtonDown(object sender, MouseEventArgs args)
        {
            if (!MainCanvas.IsMouseCaptured)
            {
                // Get the position of the mouse and store it
                CurrentMouseLocationPoint = args.GetPosition(MainCanvas);
                MainCanvas.CaptureMouse();
                args.Handled = true;
            }
        }

        private void MainCanvas_MouseLeftButtonUp(object sender, MouseEventArgs args)
        {
            if (MainCanvas.IsMouseCaptured)
            {
                //if (rubberBandWindow != null)
                //{
                //    //MessageBox.Show("Deleting new rubber band window");
                //    MainCanvas.Children.Remove(rubberBandWindow);
                //    rubberBandWindow = null;
                //}

                //if (rubberBand != null)
                //{
                //    //MessageBox.Show("Deleting new rubber band window");
                //    MainCanvas.Children.Remove(rubberBand);
                //    rubberBand = null;
                //}

                CurrentMouseLocationPoint = args.GetPosition(MainCanvas);

                if(!IsSelectedFirstPoint)
                {
                    MainCanvas.Background = Brushes.Gray;
                    FirstLeftMouseDownPoint = CurrentMouseLocationPoint;
                    SecondLeftMouseDownPoint = CurrentMouseLocationPoint;  // for the first click assign the second point to be the same as the first.
                    IsSelectedFirstPoint = true;
                    args.Handled = true;
                }
                else if ((!IsSelectedSecondPoint) ) {
                    MainCanvas.Background = Brushes.LightGray;
                    SecondLeftMouseDownPoint = CurrentMouseLocationPoint;
                    IsSelectedSecondPoint = true;
                    args.Handled = true;
                }

                // If we have two clicks, create the nodes and make the beam member
                if(IsSelectedFirstPoint && IsSelectedSecondPoint)
                {
                    // TODO: search if a node is already in the model with the same coordinates.  If so, use that node, otherwise make a new one.
                    // Create the nodes
                    MDC_Node node1 = new MDC_Node(FirstLeftMouseDownPoint.X, FirstLeftMouseDownPoint.Y, 0.0f);
                    MDC_Node node2 = new MDC_Node(SecondLeftMouseDownPoint.X, SecondLeftMouseDownPoint.Y, 0.0f);

                    //node1.Update();

                    // both buttons have been clicked, so create a beam member between the two
                    Members.Add(new MDC_Beam(node1, node2));
                    
                    IsSelectedFirstPoint = false;
                    IsSelectedSecondPoint = false;

                    // Clear our points and reset our move status
                    FirstLeftMouseDownPoint = new Point(0.0,0.0);
                    SecondLeftMouseDownPoint = new Point(0.0, 0.0);
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
            IsSelectedFirstPoint = false;
            IsSelectedSecondPoint = false;

            this.OnUserUpdate();

        }

        private void MainCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //TODO:  Mouse wheel zoom in and out...come up with a better way!
            if (e.Delta > 0)
            {
                ScaleFactorX = ScaleFactorX * 2.0;
            }
            else if (e.Delta < 0)
            {
                ScaleFactorX = ScaleFactorX * 0.5;
            }
            else return;

            // cap the amount we can zoom in and out -- done this way for performance reasons
            if (ScaleFactorX > 10 || ScaleFactorX < 0.1)
            {
                StatusMessage = "Zoom limits reached";
                return;
            }
            else
            {
                StatusMessage = String.Empty;
            }

            MainCanvas.Width = CANVAS_WIDTH * ScaleFactorX;
            MainCanvas.Height = CANVAS_HEIGHT * ScaleFactorX;

            // Remove original grid object
//            this.MDC_DrawingObjects.Remove(MainGrid);

            // Recreate our grid object
            MainGrid.ScaleGrid(ScaleFactorX);
//            MainGrid = new MDC_Grid(MainGrid.XSpacing, MainGrid.YSpacing*ScaleFactorX, MainCanvas.Width, MainCanvas.Height, Colors.DarkGray);
//            this.MDC_DrawingObjects.Add(MainGrid);

            this.OnUserUpdate();
        }

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
        {
            //  Get the OpenGL instance.
            var gl = args.OpenGL;

            //  Add a bit to theta (how much we're rotating the scene) and create the modelview
            //  and normal matrices.
            theta += 0.01f;
            scene.CreateModelviewAndNormalMatrix(theta);

            //  Clear the color and depth buffer.
            gl.ClearColor(0f, 0f, 0f, 1f);
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);

            //  Render the scene in either immediate or retained mode.
            switch (comboRenderMode.SelectedIndex)
            {
                case 0:
                    {
                        scene.RenderRetainedMode(gl, checkBoxUseToonShader.IsChecked.Value); break;
                    }
                case 1:
                    {
                        axies.Render(gl, RenderMode.Design);
                        scene.RenderImmediateMode(gl);
                        break;
                    }
            }
        }

        private void OpenGLControl_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
        {
            OpenGL gl = args.OpenGL;

            //  Initialise the scene.
            scene.Initialise(gl);
        }

        private void OpenGLControl_Resized(object sender, OpenGLRoutedEventArgs args)
        {
            //  Get the OpenGL instance.
            var gl = args.OpenGL;

            //  Create the projection matrix for the screen size.
            scene.CreateProjectionMatrix(gl, (float)ActualWidth, (float)ActualHeight);
        }

        /// <summary>
        /// The axies, which may be drawn.
        /// </summary>
        private readonly Axies axies = new Axies();

        private float theta = 0;

        /// <summary>
        /// The scene we're drawing.
        /// </summary>
        private readonly Scene scene = new Scene();
    }
}
