using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace System_Programing_Critical_Section_Homework4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Car> cars;
        public ObservableCollection<Car> Cars
        {
            get { return cars; }
            set
            {
                cars = value;
                OnPropertyChanged();
            }
        }

        private string milliSecond;
        public string MilliSecond
        {
            get { return milliSecond; }
            set
            {
                milliSecond = value;
                OnPropertyChanged();
            }
        }

        private string second;
        public string Second
        {
            get { return second; }
            set
            {
                second = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            MilliSecond = "00";
            Second = "00 : ";

            DataContext = this;

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
                    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void Start(object sender, RoutedEventArgs e)
        {
            if (Cars != null)
            {
                Application.Current.Dispatcher.Invoke(() => { Cars.Clear(); });
            }
            if (singleTextBox.IsChecked == true)
            {
                MilliSecond = "00";
                Second = "00 : ";

                Thread thread = new Thread(() =>
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    SingleThread();
                    watch.Stop();
                    MilliSecond = watch.Elapsed.Milliseconds.ToString() + " milli";
                    Second = watch.Elapsed.Seconds.ToString() + " san : ";
                });
                thread.Start();

            }
            else
            {
                MilliSecond = "00";
                Second = "00 : ";
                Thread thread = new Thread(() =>
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    MultiThread();
                    watch.Stop();
                    MilliSecond = watch.Elapsed.Milliseconds.ToString() + " milli";
                    Second = watch.Elapsed.Seconds.ToString() + " san : ";

                });
                
                thread.Start();
            }

        }

        public void SingleThread()
        {
            Thread thread = new Thread(() =>
            {
                object _lock = new object();
                lock (_lock)
                {
                    var AllCarss = new List<Car>();
                    for (int i = 1; i < 10; i++)
                    {
                        string jsonName = "Cars" + i.ToString() + ".json";
                        if (File.Exists(jsonName))
                        {
                            var JsonText = File.ReadAllText(jsonName);
                            var carss = JsonSerializer.Deserialize<List<Car>>(JsonText);
                            AllCarss.AddRange(carss!);
                        }
                    }
                    Application.Current.Dispatcher.Invoke(() => { Cars = new ObservableCollection<Car>(AllCarss!); });
                }
            });
            thread.Start();
            Thread.Sleep(1000);
            //App.Current.Dispatcher.Invoke(() => { Cars = new ObservableCollection<Car>(AllCarss!); });

        }

        public void MultiThread()
        {
            var AllCars = new List<Car>();
            object _lock = new object();
            for (int i = 1; i < 10; i++)
            {

                string jsonName = "Cars" + i.ToString() + ".json";
                if (File.Exists(jsonName))
                {
                    Thread thread = new Thread(() =>
                    {
                        lock (_lock)
                        {
                            var JsonText = File.ReadAllText(jsonName);
                            var carss = JsonSerializer.Deserialize<List<Car>>(JsonText);
                            AllCars.AddRange(carss!);
                            Application.Current.Dispatcher.Invoke(() => { Cars = new ObservableCollection<Car>(AllCars!); });
                        }
                    });
                    thread.Start();
                }
            }

            Thread.Sleep(1000);
            //MessageBox.Show(AllCars.Count.ToString());


            //MessageBox.Show(AllCars.Count.ToString());


        }
    }
}