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

        public static MainWindow MainWindow { get; set; }



        public static void SetGreen()
        {
            MainWindow.ERight_LeftGates.Fill = Brushes.Black;
            MainWindow.ELeft_LeftGates.Fill = Brushes.LimeGreen;

            MainWindow.ERight_RightGates.Fill = Brushes.LimeGreen;
            MainWindow.ELeft_RightGates.Fill = Brushes.Black;



        }

        public static void SetRed()
        {



            MainWindow.ERight_LeftGates.Fill = Brushes.Red;
            MainWindow.ELeft_LeftGates.Fill = Brushes.Black;

            MainWindow.ERight_RightGates.Fill = Brushes.Black;
            MainWindow.ELeft_RightGates.Fill = Brushes.Red;


        }




    }
}
