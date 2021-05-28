using RoadSimulator.Classes.TrafficTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoadSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //https://docs.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/how-to-animate-an-object-along-a-path-matrix-animation?view=netframeworkdesktop-4.8
        bool pauseresume =true;
        Manager _Manager;

        public MainWindow()
        {
            InitializeComponent();
            this.MouseMove += MainWindow_MouseMove;
            _Manager = new Manager(MapCanvas, this);
            MapCanvas.Loaded += MapCanvas_Loaded;
            this.KeyDown += MainWindow_KeyDown;


            TrafficLights.MainWindow = this;
            RailwayGates.MainWindow = this;


        }


        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                new Thread(_Manager.DisplayCar).Start();
            }
        }



        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            this.Title = $"X:{e.GetPosition(this).X}   Y:{e.GetPosition(this).Y}";
        }



        private void MapCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (pauseresume)
            {
                _Manager.PauseAllCars();
                _Manager.PauseAllTrains();
                pauseresume = false;
            }
            else
            {
                _Manager.ResumeAllCars();
                _Manager.ResumeAllTrains();
                pauseresume = true;
            }

          
            


        }

        private void MapCanvas_Loaded(object sender, RoutedEventArgs e)
        {


            for (int i = 0; i < 6; i++)
            {
                Thread car = new Thread(_Manager.DisplayCar);
                car.Start();

            }

            Thread train = new Thread(_Manager.DisplayTrain);
            train.Start();



        }


      





    }
}
