using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using canvas = System.Windows.Controls.Canvas;

namespace OSU
{
    enum PointType
    {
        dot = 0,
        horizontalSlider,
        verticalSlider,
        sliderSin,
        spinner,
    }
    
    public partial class MainWindow
    {
        private ImageBrush brush = new ImageBrush();
        private DispatcherTimer _timer;
        private int _tickCount;
        private Dictionary<int, IElement> _show = new Dictionary<int, IElement>();
        private Dictionary<int, IElement> _remove = new Dictionary<int, IElement>();
        private MediaPlayer _mp = new MediaPlayer();
        private int playerScore = 0;
        public int PlayerHealth { get; private set; } = 100;
        public TextBlock Tb { get;} = new TextBlock();
        public TextBlock HealthBar { get;} = new TextBlock();

        public MainWindow()
        {
            InitializeComponent();
            SetComponents();
            ReadLevel();
            MusicAndBacground();
            SetTimer();
            Begin();
            
        }

        private void SetComponents()
        {
            Canva.Background = Brushes.Black;
            Tb.Text = "Score: " + playerScore;
            Tb.FontSize = 12;
            Tb.Background = Brushes.Azure;
            Canva.Children.Add(Tb);
            canvas.SetTop(Tb,  10);
            canvas.SetLeft(Tb, 10);
            HealthBar.Text = "hp" + PlayerHealth;
            HealthBar.FontSize = 12;
            HealthBar.Background = Brushes.Azure;
            canvas.SetRight(HealthBar, 10);
            canvas.SetTop(HealthBar, 10);
            Canva.Children.Add(HealthBar);
        }
        public void ChangeHealth(TextBlock healthBar, int changeCount)
        {
            if (PlayerHealth == 100 && changeCount > 0)
                PlayerHealth += 0;
            else PlayerHealth += changeCount;
            if (PlayerHealth <= 0)
            {
                MessageBox.Show("сдох как лох");
                _timer.Stop();
            }

            healthBar.Text = "hp" + PlayerHealth;
        }
        public void UpScore(TextBlock tb, int count)
        {
            playerScore += count;
            tb.Text = "Score: " + playerScore;
        }

        private void SetTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, args) =>
            {
                if (_show.ContainsKey(_tickCount))
                    _show[_tickCount].Show();
                if (_remove.ContainsKey(_tickCount))
                    _remove[_tickCount].Remove();
                _tickCount++;
            };
        }

        private void ReadLevel()
        {
            foreach (var point in new LevelPoints("points.txt"))
            {
                if (LevelPoints.backgroundPath != null && LevelPoints.musicPath != null && brush.ImageSource == null && _mp.Source == null)
                {
                    brush.ImageSource = new BitmapImage(new Uri($@"{LevelPoints.backgroundPath}", UriKind.Relative)); //на самом деле у Uri конструктор через дупу работает
                    _mp.Open(new Uri($@"{LevelPoints.musicPath}", UriKind.Relative));//cа цвет кноп очек  поменяю а то чет черный так себе
                }
                switch ((PointType) point.Item1)
                {
                    case PointType.dot:
                        Dot dot = new Dot(this, point.Item2, point.Item3, point.Item4, point.Item5);
                        AddToDictionary(dot);
                        break;
                    case PointType.horizontalSlider:
                        Sliders horizontal = new Sliders(this, point.Item2, point.Item3, point.Item4,
                            point.Item5, point.Item6, point.Item7, 0);
                        AddToDictionary(horizontal);
                        break;
                    case PointType.verticalSlider:
                        Sliders vertical = new Sliders(this, point.Item2, point.Item3, point.Item4,
                            point.Item5, point.Item6, point.Item7, 1);
                        AddToDictionary(vertical);
                        break;
                    case PointType.sliderSin:
                        Sliders sin = new Sliders(this, point.Item2, point.Item3, point.Item4,
                            point.Item5, point.Item6, point.Item7, 2);
                        AddToDictionary(sin);
                        break;
                    case PointType.spinner:
                        Spinner spinner = new Spinner(this, point.Item2, point.Item3, point.Item4, point.Item5);
                        AddToDictionary(spinner);
                        break;
                }
            }
        }

        private void MusicAndBacground()
        {
            if (_mp != null && brush != null)
            {
                Canva.Background = brush;
                _mp.Play();
            }
        }

        private void AddToDictionary(IElement element)
        {
            _show.Add(element.DelayTime, element);
            _remove.Add(element.DeathTime + element.DelayTime, element);
        }

        private void Begin()
            => _timer.Start();
        
    }

    
}