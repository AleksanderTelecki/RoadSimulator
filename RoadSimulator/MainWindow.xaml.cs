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

        bool pauseresume=true;
        CarManager carManager;
        //Car start position (201,5)
        public MainWindow()
        {
            InitializeComponent();
            this.MouseMove += MainWindow_MouseMove;
            carManager = new CarManager(MapCanvas, this);
            MapCanvas.Loaded += MapCanvas_Loaded;


            Storyboard storyboard = (Storyboard)this.Resources["TheStoryboard"];

        }

       

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            this.Title = $"X:{e.GetPosition(this).X}   Y:{e.GetPosition(this).Y}";
        }

        private void MapCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (pauseresume)
            {
                carManager.PauseAllCars();
                pauseresume = false;
            }
            else
            {
                carManager.ResumeAllCars();
                pauseresume = true;
            }
           
            
        }

        private void MapCanvas_Loaded(object sender, RoutedEventArgs e)
        {

            new Thread(DoSomething).Start();


        }

   
        public void DoSomething()
        {
                this.Dispatcher.Invoke(() => {
                    carManager.LoadNewCar();
                    carManager.LoadNewCar();
                    carManager.LoadNewCar();
                    carManager.LoadNewCar();
                    carManager.LoadNewCar();
                    carManager.LoadNewCar();
                });
        }





    }
}
