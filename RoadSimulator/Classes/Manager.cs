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
    /// <summary>
    /// klasa zarzadzajaca wszystkimi wydarzeniami majacymi miejsce na MainWindow
    /// </summary>
    public class Manager
    {
        //generator liczb losowych
        private Random rnd = new Random();
        public Canvas MapCanvas { get; set; }
        public MainWindow mainWindow { get; set;}
        private PathBuilder pathBuilder { get; set; }

        //kolekcja obiektow typu Car
        public static ObservableCollection<Car> CarCollection { get; set; }

        //kolekcja obiektow typu Train
        public static ObservableCollection<Train> TrainCollection { get; set; }

        //timera dla tworzenia nowych obiektow klasy Car
        public DispatcherTimer CarTimer;
        
        //mutex  by tylko jeden watek na raz mogl uzywac metody DisplayCar 
        private static Mutex CarMutex = new Mutex();

        //polozenie samochodziku na Canvasie - procent ukonczenia przewidzianej dla niego trasy        
        public static double [] state= new double [6];

        //zmienna przyjmujaca wartosc true jezeli obiekt klasy Car znajduje sie na przejezdzie kolejowym
        public static Boolean onThePass = false;

        //by zapobiec wyrzuceniu Exception
        public static Boolean shutDownApp = false;


        /// <summary>
        /// konstruktor klasy Manager przyjmujacy dwa argumenty
        /// </summary>
        /// <param name="ImageCanvas"></param>
        /// <param name="mainWindow"></param>
        public Manager(Canvas ImageCanvas, MainWindow mainWindow)
        {
            MapCanvas = ImageCanvas;
            this.mainWindow = mainWindow;

            pathBuilder = new PathBuilder(mainWindow,ImageCanvas);
            //stworzenie obiektu kolekcji obiektow typu Car
            CarCollection = new ObservableCollection<Car>();
            //dodanie obiektow do kolekcji
            CarCollection.CollectionChanged += CarCollection_CollectionChanged;

            //stworzenie obiektu kolekcji obiektow typu Train
            TrainCollection = new ObservableCollection<Train>();
            //dodanie obiektow do kolekcji
            TrainCollection.CollectionChanged += TrainCollection_CollectionChanged;
            
            //stworzenie timera sprawdzajacych stan animowanych obiektow
            CarTimer = new DispatcherTimer();
            //ustawienie dlugosci ticku CarTimera
            CarTimer.Interval = TimeSpan.FromMilliseconds(8); //ustawione na predkosc samochodzikow
            CarTimer.Tick += CarTimer_Tick;
        }

        /// <summary>
        /// metoda wywolywana gdy stan kolekcji obiekt klasy Train ulega zmianie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //jezeli kolekcja jest pusta rogatki sie otwieraja,
            //i tworzony jest nowy watek z pociagiem.            
            if (TrainCollection.Count == 0)
            {
                RailwayGates.OpenGates();
                new Thread(DisplayTrain).Start();
            }           
        }

        /// <summary>
        /// metoda wywolywana gdy stan kolekcji obiekt klasy Car ulega zmianie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //jezeli kolekcja jest pusta timer dla obiektow klasy Car sie zatrzymuje.  
            //gdy do kolekcji dodany jest pierwszy samochod timer dla obiektow klasy Car startuje
            if (CarCollection.Count==0)
            {                
                CarTimer.Stop();
                
                for (int i = 0; i < 6; i++)
                {
                    Thread car = new Thread(DisplayCar);
                    car.Start();
                }
                
            }
            else if(CarCollection.Count==1)
            {
                CarTimer.Start();
            }
        }

        /// <summary>
        /// metoda wywolywana przy kazdym ticku timera dla obiektow klasy Car
        /// odpowiadajaca za polozenie obiektow na Canvasie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarTimer_Tick(object sender, EventArgs e)
        {
            //akcje wykonywane dla kazdego obiektu w kolekcji
            for (int i = 0; i < CarCollection.Count; i++)
            {
                //wspolrzedne samochodziku na Canvasie                
                var x =Math.Round(CarCollection[i].CarImage.RenderTransform.Value.OffsetX);
                var y = Math.Round(CarCollection[i].CarImage.RenderTransform.Value.OffsetY);

                //poloznie obiektow na Canvasie w danej chwili
                //polozenie obiektow to ich progres w pokonywaniu przez nie trasy, czyli wartosc od 0 do 1                              
                state[i] = (double)CarCollection[i].Animator.GetCurrentProgress(mainWindow);

                //obiekt klasy Car zatrzymuje sie, gdy jest wystarczajaco blisko przejazdu i rogatki sa opuszczone                          
                if (RailwayGates.IsClosed && state[i] < 0.28 && state[i] > 0.26)
                {
                    CarCollection[i].Animator.SetSpeedRatio(mainWindow, 0);
                    CarCollection[i].Stopped = true;
                }                            
                else
                {
                    //gdy w danym ticku timera rogatki sa podniesione, a pojazd zatrzymany,
                    //rusza on ze swoja predkoscia przed zatrzymaniem
                    if (CarCollection[i].Stopped == true)
                    {
                        CarCollection[i].Stopped = false;
                        CarCollection[i].Animator.SetSpeedRatio(mainWindow, CarCollection[i].CarSpeed);
                    }

                    //jezeli samochodzik pokonuje zakret jego aktualna grafika zostaje zmieniona
                    //na obrazek bedacy jej lustrzanym odbiciem 
                    //if (state.Value > 0.27 && state.Value < 0.31 && CarCollection[i].IsRight)
                    if (state[i] > 0.27 && state[i] < 0.31 && CarCollection[i].IsRight)
                    {
                        CarCollection[i].FlipCar();
                    }
                    //else if (state.Value > 0.57 && !CarCollection[i].IsRight)
                    else if (state[i] > 0.57 && !CarCollection[i].IsRight)
                    {
                        CarCollection[i].FlipCar();
                    }

                    for (int j = 0; j < CarCollection.Count; j++)
                    {
                        //wyrazenie warunkowe uniemozliwiajace programowi probe pobrania wspolrzednych kolejnego obiektu,
                        //w momencie gdy na Canvasie istnieje tylko jeden obiekt
                        if (j == i)
                        {
                            continue;
                        }

                        //wspolrzedne obiektu z kolekcji
                        var x1 = Math.Round(CarCollection[j].CarImage.RenderTransform.Value.OffsetX);
                        var y1 = Math.Round(CarCollection[j].CarImage.RenderTransform.Value.OffsetY);

                        //jezeli wspolrzedne mlodszego obiektu w kolekcji (o indeksie i) beda na tyle zblizone do obiektu bezposrednio go poprzedzajcego(indeks j),
                        //ze powodowalyby nakladanie sie na siebie obrazkow dwoch samochodzikow                        
                        if (((x1 - x) <= 50 && (x1 - x) >= 0) && (((y1 - y) <= 110 || (y - y1) <= 110) && (y1 - y) >= 0))
                        {
                            // predkosc obiektu o indeksie i jest wieksza od predkosci obiektu go poprzedzajacego(indeks j)                   
                            if (CarCollection[j].CarSpeed < CarCollection[i].CarSpeed || CarCollection[j].Stopped)
                            {
                                //predkosc obiektu o indeksie i przyjmuje jako swoja predkosci odrobine pomniejszona wartosc predkosci obiektu go poprzedzajacego
                                CarCollection[i].CarSpeed = CarCollection[j].CarSpeed - CarCollection[j].CarSpeed / 20;
                                //jezeli obiekt poprzedzajacy obiekt o indeksie i zatrzymal sie przed rogatkami to ten rowniez sie zatrzymuje,
                                //w przeciwnym razie przyjmuje nowa predkosc
                                CarCollection[i].Animator.SetSpeedRatio(mainWindow, CarCollection[j].Stopped ? 0 : CarCollection[i].CarSpeed);
                                
                                //samochodzikowi po zmianie predkosci ustawia sie wlasciowsc Stopped na true, by w przypadku przyjecia jako nowa predkosc
                                //wartosci 0, samochodzik zanim, ktory porusza sie szybciej przyjal jako swoja nowa predkosc 0, a nie wynik dzialania 0 - 0/20                                                      
                                CarCollection[i].Stopped = true;                               
                            }
                        }
                    }
                }
                
                //jezeli zaden samochodzik nie znajduje sie na przejezdzie kolejowym, a pociag jest gotowy do drogi, rogatki zostaja zamkniete
                if (CarCollection.Count > 0)
                {
                    foreach (double element in state)
                    {
                        if (element > 0.28 && element < 0.31)
                        {
                            onThePass = true;
                            break;
                        }
                    }
                }
                if (RailwayGates.IsClosed && !onThePass)
                    RailwayGates.CloseGates();

                onThePass = false;
            }
        }

        /// <summary>
        /// metoda tworzy nowy obiekt klasy Car i dodaje go do kolekcji CarCollection
        /// </summary>
        public void LoadCar()
        {
            //utworzenie instancji klasy Car
            Car car = new Car();
            //dodanie instancji klasy Car do Canvasu
            AddToCanvas(car);

            //losowanie predkosci dla utworzonego obiektu klasy Car
            car.CarSpeed = rnd.NextDouble();
            //predkosc obiektu klasy Car musi byc co najmniej rowna 0,4
            if (car.CarSpeed < 0.4)
            {
                car.CarSpeed += 0.4;
            }

            //sprawdzenie czy kolekcja zawiera przynajmniej 1 item oraz czy zaden z jej itemow nie posiada juz przypisanej ktoremus obiektowi predkosci  
            if (CarCollection.Any(x => x.CarSpeed == car.CarSpeed) && CarCollection.Count != 0)
            {
                while (CarCollection.Any(x => x.CarSpeed == car.CarSpeed))
                {
                    //jezeli ktorys obiekt w kolekcji posiada ta sama predkosc co dodawany samochodzik
                    //predkosc nowego samochodziku jest losowana ponownie tak dlugo az bedzie unikalna
                    car.CarSpeed = rnd.NextDouble();
                    if (car.CarSpeed<0.4)
                    {
                        car.CarSpeed += 0.4;
                    }
                }
            }
            //dodanie obiektu klasy Car do kolekcji
            CarCollection.Add(car);
            //wywolanie metody poruszajacej samochodzik po Canvasie
            pathBuilder.MoveCarMatrix(car);

        }

        /// <summary>
        /// metoda tworzy nowy obiekt klasy Train i dodaje go do kolekcji TrainCollection
        /// </summary>
        public void LoadTrain()
        {
            //utworzenie pociagu i dodanie go na Canvas
            Train train = new Train();            
            AddToCanvas(train);

            //losowanie predkosci do nadania pociagowi, ktora musi byc nie mniejsza niz 0,7
            train.TrainSpeed = rnd.NextDouble();
            if (train.TrainSpeed < 0.7)
            {
                train.TrainSpeed += 0.7;
            }            
           
            //dodanie pociagu do kolekcji obiektow typu Train
            TrainCollection.Add(train);
            //wywolanie metody poruszajacej pociag po Canvasie
            pathBuilder.MoveTrainMatrix(train);
        }

        /// <summary>
        /// //dodanie i ustawienie obiektu klasy Car na Canvasie
        /// </summary>
        /// <param name="car"></param>
        public void AddToCanvas(Car car)
        { 
            MapCanvas.Children.Add(car.CarImage);
            Canvas.SetTop(car.CarImage,  0);
            Canvas.SetLeft(car.CarImage, 0);
        }

        /// <summary>
        /// //dodanie i ustawienie pociagu na Canvasie
        /// </summary>
        /// <param name="train"></param>        
        public void AddToCanvas(Train train)
        { 
            MapCanvas.Children.Add(train.TrainImage);
            Canvas.SetTop(train.TrainImage,  0);
            Canvas.SetZIndex(train.TrainImage, 1);
            Canvas.SetLeft(train.TrainImage, 0);
        }

        /// <summary>
        /// metoda wywoluje metode tworzaca nowy samochodzik sterowany przez osobny watek oraz startuje dla niego timer
        /// </summary>
        public void DisplayCar()
        { 
            CarMutex.WaitOne();
            
            if (!shutDownApp)
            { 
                mainWindow.Dispatcher.Invoke(() =>
                {
                    LoadCar();
                });
            }
            Thread.Sleep(1000);

            //pozwala nastepnemu watkowi wejsc do metody
            CarMutex.ReleaseMutex();
        }

        /// <summary>
        /// metoda wywoluje metode zamykajaca rogatki 
        /// po czym wywoluje metode tworzaca pociag sterowany przez osobny watek oraz startuje dla niego timer
        /// </summary>
        public void DisplayTrain()
        {            
            Thread.Sleep(4000);
            RailwayGates.IsClosed = true;            
            Thread.Sleep(1500);  
            
            if (!shutDownApp)
            {
                //metoda zostaje wykonana przez watek bedacy pociagiem 
                mainWindow.Dispatcher.Invoke(() =>
                {
                    LoadTrain();
                });
            }
           
        }
    }
}
