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
        Manager _Manager;

        public MainWindow()
        {
            InitializeComponent(); 
            _Manager = new Manager(MapCanvas, this);
            MapCanvas.Loaded += MapCanvas_Loaded; 
            TrafficLights.MainWindow = this;
            RailwayGates.MainWindow = this;
        }

        /// <summary>
        /// metoda po uruchomieniu programu tworzy w petli 6 watkow bedacych samochodzikami, a nastepnie jeden, ktory bedzie pociagiem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// metoda zmienia wartosc zmiennej shutDownApp na true by Dispatchery pociagu oraz samochodzikow w klasie Manager nie wyrzucily wyjatku 
        /// anulowania ackji przy zamknieciu glownego okna aplikacji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Manager.shutDownApp = true;                        
        }
    }
}
