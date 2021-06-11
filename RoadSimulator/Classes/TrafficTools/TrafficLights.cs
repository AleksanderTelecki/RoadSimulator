using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RoadSimulator.Classes.TrafficTools
{
    public static class TrafficLights
    {
        /// <summary>
        /// klasa zawiera metody zmieniajace kolor sygnalizatora przy rogatkach 
        /// </summary>
        public static MainWindow MainWindow { get; set; }

        /// <summary>
        /// metoda zmienia kolor obiektu typu Ellipse sluzacego jako sygnalizator przy przejezdzie kolejowym na kolor zielony
        /// </summary>
        public static void SetGreen()
        {
            MainWindow.ERight_LeftGates.Fill = Brushes.Black;
            MainWindow.ELeft_LeftGates.Fill = Brushes.LimeGreen;

            MainWindow.ERight_RightGates.Fill = Brushes.LimeGreen;
            MainWindow.ELeft_RightGates.Fill = Brushes.Black;
        }

        /// <summary>
        /// metoda zmienia kolor obiektu typu Ellipse sluzacego jako sygnalizator przy przejezdzie kolejowym na czerwony
        /// </summary>
        public static void SetRed()
        {
            MainWindow.ERight_LeftGates.Fill = Brushes.Red;
            MainWindow.ELeft_LeftGates.Fill = Brushes.Black;

            MainWindow.ERight_RightGates.Fill = Brushes.Black;
            MainWindow.ELeft_RightGates.Fill = Brushes.Red;
        }
    }
}
