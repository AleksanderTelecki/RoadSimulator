﻿using RoadSimulator.Classes.Train;
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

        public DispatcherTimer timer;

        public Manager(Canvas ImageCanvas,MainWindow mainWindow)
        {
            MapCanvas = ImageCanvas;
            this.mainWindow = mainWindow;
            pathBuilder = new PathBuilder(mainWindow,ImageCanvas);
            CarCollection = new ObservableCollection<Car>();
            CarCollection.CollectionChanged += CarCollection_CollectionChanged;

            TrainCollection = new ObservableCollection<Train>();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;

        }

        private void CarCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (CarCollection.Count==0)
            {

                timer.Stop();

            }
            else if(CarCollection.Count==1)
            {

                timer.Start();

            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < CarCollection.Count; i++)
            {

               
                var x =Math.Round(CarCollection[i].CarImage.RenderTransform.Value.OffsetX);
                var y = Math.Round(CarCollection[i].CarImage.RenderTransform.Value.OffsetY);


                for (int j = 0; j < CarCollection.Count; j++)
                {

                    if (j==i)
                    {
                        continue;
                    }

                    var x1 = Math.Round(CarCollection[j].CarImage.RenderTransform.Value.OffsetX);
                    var y1 = Math.Round(CarCollection[j].CarImage.RenderTransform.Value.OffsetY);

                

                    if (((x1 - x) <= 60 && (x1 - x) >= 0) && (((y1 - y) <= 110|| (y - y1) <= 110) && (y1 - y) >= 0))
                    {

                        if (CarCollection[j].CarSpeed < CarCollection[i].CarSpeed)
                        {

                            CarCollection[i].CarSpeed = CarCollection[j].CarSpeed - CarCollection[j].CarSpeed / 20;
                            CarCollection[i].Animator.SetSpeedRatio(mainWindow,CarCollection[i].CarSpeed);
                            
                        }


                    }


                }


            }
        }

        public void LoadNewCar()
        {
            
            if (CarCollection.Count>=20)
            {
                return;
            }

            //5,201 //Delete pointer in car
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

        public void LoadNewTrain()
        {

            if (TrainCollection.Count >= 3)
            {
                return;
            }


            Train train = new Train();
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
