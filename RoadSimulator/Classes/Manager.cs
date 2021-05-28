using RoadSimulator.Classes.Train;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Threading;
using RoadSimulator.Classes.TrafficTools;

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

        public DispatcherTimer CarTimer;


        private static Mutex TrainMutex = new Mutex();
        private static Mutex CarMutex = new Mutex();


        public Manager(Canvas ImageCanvas,MainWindow mainWindow)
        {


            MapCanvas = ImageCanvas;
            this.mainWindow = mainWindow;

            pathBuilder = new PathBuilder(mainWindow,ImageCanvas);

            CarCollection = new ObservableCollection<Car>();
            CarCollection.CollectionChanged += CarCollection_CollectionChanged;
            TrainCollection = new ObservableCollection<Train>();
            TrainCollection.CollectionChanged += TrainCollection_CollectionChanged;

            CarTimer = new DispatcherTimer();
            CarTimer.Interval = TimeSpan.FromMilliseconds(100);
            CarTimer.Tick += CarTimer_Tick;




        }

     

        private void TrainCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (TrainCollection.Count == 0)
            {
                RailwayGates.OpenGates();
                new Thread(DisplayTrain).Start();

            }
            else if (TrainCollection.Count == 1)
            {
                RailwayGates.CloseGates();

            }
        }

        private void CarCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (CarCollection.Count==0)
            {

                CarTimer.Stop();
               

            }
            else if(CarCollection.Count==1)
            {

                CarTimer.Start();

            }
        }

     

        private void CarTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < CarCollection.Count; i++)
            {

               

                var x =Math.Round(CarCollection[i].CarImage.RenderTransform.Value.OffsetX);
                var y = Math.Round(CarCollection[i].CarImage.RenderTransform.Value.OffsetY);

                var state = CarCollection[i].Animator.GetCurrentProgress(mainWindow);


                if (RailwayGates.IsClosed && state.Value < 0.27 && state.Value>0.23)
                {
                    CarCollection[i].Animator.SetSpeedRatio(mainWindow, 0);
                    CarCollection[i].Stopped = true;
                }
                else 
                {
                    if (CarCollection[i].Stopped==true)
                    {
                        CarCollection[i].Stopped = false;
                        CarCollection[i].Animator.SetSpeedRatio(mainWindow, CarCollection[i].CarSpeed);
                    }


                    if (state.Value > 0.27 && state.Value < 0.31 && CarCollection[i].IsRight)
                    {
                        CarCollection[i].FlipCar();
                    }
                    else if (state.Value > 0.57 && !CarCollection[i].IsRight)
                    {
                        CarCollection[i].FlipCar();
                    }




                    for (int j = 0; j < CarCollection.Count; j++)
                    {

                        if (j == i)
                        {
                            continue;
                        }

                        var x1 = Math.Round(CarCollection[j].CarImage.RenderTransform.Value.OffsetX);
                        var y1 = Math.Round(CarCollection[j].CarImage.RenderTransform.Value.OffsetY);



                        if (((x1 - x) <= 50 && (x1 - x) >= 0) && (((y1 - y) <= 110 || (y - y1) <= 110) && (y1 - y) >= 0))
                        {

                            if (CarCollection[j].CarSpeed < CarCollection[i].CarSpeed|| CarCollection[j].Stopped)
                            {

                                CarCollection[i].CarSpeed = CarCollection[j].CarSpeed - CarCollection[j].CarSpeed / 20;
                                CarCollection[i].Animator.SetSpeedRatio(mainWindow, CarCollection[j].Stopped?0:CarCollection[i].CarSpeed);
                                CarCollection[i].Stopped = true;

                            }


                        }


                    }

                }
            }
        }

        public void LoadCar()
        {
            
            if (CarCollection.Count>=20)
            {
                return;
            }

           
            Car car = new Car();
            AddToCanvas(car);


            car.CarSpeed = rnd.NextDouble();
            if (car.CarSpeed < 0.4)
            {
                car.CarSpeed += 0.4;
            }

            if (CarCollection.Any(x => x.CarSpeed == car.CarSpeed) && CarCollection.Count != 0)
            {
                while (CarCollection.Any(x => x.CarSpeed == car.CarSpeed))
                {

                   
                    car.CarSpeed = rnd.NextDouble();
                    if (car.CarSpeed<0.4)
                    {
                        car.CarSpeed += 0.4;
                    }
                }
            }


            CarCollection.Add(car);
            pathBuilder.MoveCarMatrix(car);
          


        }

        public void LoadTrain()
        {



            if (TrainCollection.Count == 1)
            {
                return;
            }

            Train train = new Train();
            
            AddToCanvas(train);

            train.TrainSpeed = rnd.NextDouble();
            if (train.TrainSpeed < 0.7)
            {
                train.TrainSpeed += 0.7;
            }

            if (CarCollection.Any(x => x.CarSpeed == train.TrainSpeed) && CarCollection.Count != 0)
            {
                while (CarCollection.Any(x => x.CarSpeed == train.TrainSpeed))
                {


                    train.TrainSpeed = rnd.NextDouble();
                    if (train.TrainSpeed < 0.7)
                    {
                        train.TrainSpeed += 0.7;
                    }
                }
            }

            TrainCollection.Add(train);
            pathBuilder.MoveTrainMatrix(train);


        }



        public void AddToCanvas(Car car)
        {
            
            MapCanvas.Children.Add(car.CarImage);
            Canvas.SetTop(car.CarImage,  0);
            Canvas.SetLeft(car.CarImage, 0);
        }

        public void AddToCanvas(Train train)
        {

            MapCanvas.Children.Add(train.TrainImage);
            Canvas.SetTop(train.TrainImage,  0);
            Canvas.SetZIndex(train.TrainImage, 1);
            Canvas.SetLeft(train.TrainImage, 0);
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


        public void DisplayCar()
        {
            CarMutex.WaitOne();

            mainWindow.Dispatcher.Invoke(() => {
                LoadCar();
            });
            Thread.Sleep(1000);
            CarMutex.ReleaseMutex();


        }

        public void DisplayTrain()
        {
            Thread.Sleep(5000);
            mainWindow.Dispatcher.Invoke(() => {
                RailwayGates.CloseGates();
            });
            Thread.Sleep(2000);
            TrainMutex.WaitOne();

            mainWindow.Dispatcher.Invoke(() => {
                LoadTrain();
            });
           
            TrainMutex.ReleaseMutex();


        }







    }
}
