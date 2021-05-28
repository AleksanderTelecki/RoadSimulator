using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RoadSimulator.Classes.TrafficTools
{
    public static class RailwayGates
    {

        public static MainWindow MainWindow { get; set; }
        public static bool IsClosed;

        public static void CloseGates()
        {
            MainWindow.RightGates.Source = new BitmapImage(new Uri("./Resources/GateCloseRight.png", UriKind.Relative));
            MainWindow.LeftGates.Source = new BitmapImage(new Uri("./Resources/GateCloseLeft.png", UriKind.Relative));
            TrafficLights.SetRed();
            IsClosed = true;

        }

        public static void OpenGates()
        {
            MainWindow.RightGates.Source = new BitmapImage(new Uri("./Resources/GateOpenRight.png", UriKind.Relative));
            MainWindow.LeftGates.Source = new BitmapImage(new Uri("./Resources/GateOpenLeft.png", UriKind.Relative));
            TrafficLights.SetGreen();
            IsClosed = false;
        }







    }
}
