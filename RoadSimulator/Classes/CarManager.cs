using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace RoadSimulator
{
    public class CarManager
    {
        private Random rnd = new Random();
        public Canvas MapCanvas { get; set; }
        public MainWindow mainWindow { get; set;}
        private PathBuilder pathBuilder { get; set; }

        public static ObservableCollection<Car> CarCollection { get; set; }


        public CarManager(Canvas ImageCanvas,MainWindow mainWindow)
        {
            MapCanvas = ImageCanvas;
            this.mainWindow = mainWindow;
            pathBuilder = new PathBuilder(mainWindow,ImageCanvas);
            CarCollection = new ObservableCollection<Car>();
           

        }


      

        public void LoadNewCar()
        {
            
            if (CarCollection.Count>=20)
            {
                return;
            }

            //5,201
            Car car = new Car(new Point(0, 0));
            AddToCanvas(car);
            car.CarSpeed = rnd.Next(5, 27);
            if (CarCollection.Any(x=>x.CarSpeed==car.CarSpeed)&&CarCollection.Count!=0)
            {
                while (CarCollection.Any(x => x.CarSpeed == car.CarSpeed))
                {
                    car.CarSpeed = rnd.Next(7, 27);
                }
            }
            CarCollection.Add(car);
            pathBuilder.MoveCarMatrix(car);
          


        }

        public Car GetLoadedCar()
        {
            Car car = new Car(new Point(0, 0));
            AddToCanvas(car);
            return car;
        }



        public void AddToCanvas(Car car)
        {
            
            MapCanvas.Children.Add(car.CarImage);
            Canvas.SetTop(car.CarImage, car.CarPoint.Y);
            Canvas.SetLeft(car.CarImage, car.CarPoint.X);
        }

        public void PauseAllCars()
        {

            foreach (var item in CarCollection)
            {
                if (item.Animator!=null)
                {
                    item.Animator.Pause(mainWindow);
                }
            }




        }

        public void ResumeAllCars()
        {

            foreach (var item in CarCollection)
            {
                if (item.Animator != null)
                {
                    item.Animator.Resume(mainWindow);
                }
            }




        }







    }
}
