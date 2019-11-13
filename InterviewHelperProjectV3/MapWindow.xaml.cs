using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InterviewHelperProjectV3
{
    /// <summary>
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        public string url { get; set; }

        public MapWindow(string url)
        {
            this.url = url;
            InitializeComponent();
            FrameWithinGrid.Source = new Uri(url, UriKind.Absolute);
        }
    }
}
