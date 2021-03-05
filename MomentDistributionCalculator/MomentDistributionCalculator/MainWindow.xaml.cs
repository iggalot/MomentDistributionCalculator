using MomentDistributionCalculator.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace MomentDistributionCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<MDC_Beam> m_Model = null;

        public ObservableCollection<MDC_Beam> Model { get { return m_Model; } set { m_Model = value; } }

        public MainWindow()
        {
            InitializeComponent();

            Model = new ObservableCollection<MDC_Beam>();

            // Create some nodes
            double len = MainCanvas.Width / 10.0f;
            MDC_Node start = new MDC_Node(20, 30);
            MDC_Node end = null; ;
            MDC_Node bottom = null; ;
            for(int i=1; i<10; i++)
            {
                end = new MDC_Node(start.X+(float)len, 30);
                bottom = new MDC_Node(start.X, 30 + (float)len);
                
                // Create some beams
                Model.Add(new MDC_Beam(start, end));
                Model.Add(new MDC_Beam(start, bottom));
                start = end;
            }
            bottom = new MDC_Node(start.X, 30 + (float)len);
            Model.Add(new MDC_Beam(start, bottom));
            

            // Change color of background
           // MainCanvas.Background = new SolidColorBrush(Colors.Cyan);

            foreach(MDC_Beam item in Model)
            {
                item.Draw(MainCanvas);
            }
        }
    }
}
