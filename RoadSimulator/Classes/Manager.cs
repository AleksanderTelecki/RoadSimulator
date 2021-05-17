using RoadSimulator.Classes.Train;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace RoadSimulator
{
    public class Manager
    {
        private Random rnd = new Random();
        public Canvas MapCanvas { get; set; }
        public MainWindow mainWindow { get; set;}
        private PathBuilder pathBuilder { get; set; }

        public static ObservableCollection<Car> CarCollection { get; set; }

        public static ObservableCollection<Train> TrainCollection { get; set; }

        public Manager(Canvas ImageCanvas,MainWindow mainWindow)
        {
            MapCanvas = ImageCanvas;
            this.mainWindow = mainWindow;
            pathBuilder = new PathBuilder(mainWindow,ImageCanvas);
            CarCollection = new ObservableCollection<Car>();
            TrainCollection = new ObservableCollection<Train>();



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

        public void LoadNewTrain()
        {

            if (TrainCollection.Count >= 3)
            {
                return;
            }


            Train train = new Train(new Point(0, 0));
            AddToCanvas(train);
            train.TrainSpeed = rnd.Next(8, 27);
            if (TrainCollection.Any(x => train.TrainSpeed == train.TrainSpeed) && TrainCollection.Count != 0)
            {
                while (TrainCollection.Any(x => x.TrainSpeed == train.TrainSpeed))
                {
                    train.TrainSpeed = rnd.Next(7, 27);
                }
            }
            TrainCollection.Add(train);
            pathBuilder.MoveTrainMatrix(train);



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

        public void AddToCanvas(Train train)
        {

            MapCanvas.Children.Add(train.TrainImage);
            Canvas.SetTop(train.TrainImage, train.TrainPoint.Y);
            Canvas.SetLeft(train.TrainImage, train.TrainPoint.X);
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

        public void PauseAllTrains()
        {

            foreach (var item in TrainCollection)
            {
                if (item.Animator != null)
                {
                    item.Animator.Pause(mainWindow);
                }
            }




        }


        public void ColorCange()
        {

            foreach (var item in CarCollection)
            {
                item.CarImage.Source = new BitmapImage(new Uri("./Resources/YellowCar.png", UriKind.Relative));
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

        public void ResumeAllTrains()
        {

            foreach (var item in TrainCollection)
            {
                if (item.Animator != null)
                {
                    item.Animator.Resume(mainWindow);
                }
            }




        }







    }
}
