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

/*
- projekt musi być wielowątkowy
- wykorzystywanie mechanizmów kontroli i synchronizacji (lock itp.)
- komentarze w kodzie
- samochodziki(min. 6 na planszy jednocześnie) o różnych losowych prędkościach
     Nie wyprzedzają się
     Nie zderzają się 
     Jadą w jednym kierunku
- szlabany i światła na przejeździe kolejowym
     Nie mogą się zamknąć z samochodem na przejedzie
     Szlabany muszą się odpowiednio szybko zamknąć przed pociągiem
- pociąg z losową prędkością i losowym kierunkiem
*/
namespace RoadSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //zmienna boolowska okreslajaca czy animacja jest zatrzymana. Jezeli jest zatrzymana zmienna ma wartosc false
        bool pauseresume =true;        
        Manager _Manager;

        public MainWindow()
        {
            InitializeComponent();
            //zdarzenie przesuniecia kursoru myszy po canvasie okna MainWindow
            this.MouseMove += MainWindow_MouseMove;
            //stworzenie instancji klasy Manager, przyjmujac MainWindow jako parametr//
            _Manager = new Manager(MapCanvas, this);
            MapCanvas.Loaded += MapCanvas_Loaded;
            //zdarzenie nacisniecia przycisku space
            this.KeyDown += MainWindow_KeyDown;                       
            //metody klas TrafficLights i RailwayGates beda mialy odnosily sie do okna MainWindow
            TrafficLights.MainWindow = this;
            RailwayGates.MainWindow = this;
        }

        // TODO: usunac metode KeyDown 
        /// <summary>
        /// metoda dodaje nowy samochod sterowany przez osobny watek na canvas 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                new Thread(_Manager.DisplayCar).Start();
            }
        }

        // TODO: usunac metode KMouseMove
        /// <summary>
        /// metoda wyswietla wspolrzedne polozenia kursora na canvasie jako wlasciwosc MainWindow.Title
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            this.Title = $"X:{e.GetPosition(this).X}   Y:{e.GetPosition(this).Y}";
        }

        // TODO: usunac metode mouseDown 
        /// <summary>
        /// metoda zatrzymuje lub wznawia animacje wszystkich obiektow istniejacych na canvasie w zaleznosci od wartosci zmiennej pauseresume
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// metoda tworzy w petli 6 obiektow klasy Car, a nastepnie jeden obiekt typu Train. Wszystkie obiekty sa osobnymi watkami
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
/*
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
*/

        /// <summary>
        /// rozwiazanie wyrzucania Exception przy konczeniu dzialania aplikacji przez dispatchery samochodzikow i pociagu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Manager.ShutingDown = true;                        
        }
    }
}
