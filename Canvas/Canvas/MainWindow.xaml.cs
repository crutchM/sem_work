using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        private MediaPlayer _mp;
        private int playerScore = 0;
        public int playerHealth { get; private set; } = 100;
        public TextBlock tb { get;}= new TextBlock();
        public TextBlock healthBar { get;} = new TextBlock();
        public MainWindow()
        {
            InitializeComponent();
            SetComponents();
            foreach (var point in new LevelPoints("points2.txt"))
            {
                 ImageBrush brush = new ImageBrush();
                 if (LevelPoints.backgroundPath != null && LevelPoints.musicPath != null)
                 {
                 brush.ImageSource = new BitmapImage(new Uri(LevelPoints.backgroundPath));
                 Canva.Background = brush;
                 _mp.Open(new Uri(LevelPoints.musicPath));
                 _mp.Play();
                 }
                Sliders sl = new Sliders(this);
                switch ((PointType) point.Item1)
                {
                    case PointType.dot:
                        Dot dot = new Dot(this);
                        dot.MakeDot(point.Item2, point.Item3, point.Item4, point.Item5);
                        break;
                    case PointType.horizontalSlider:
                            sl.MakeSlider(point.Item4, point.Item2, point.Item3, 
                                point.Item6, point.Item7, 0, point.Item5);
                         break; 
                    case PointType.verticalSlider:
                            sl.MakeSlider(point.Item4, point.Item2, point.Item3,
                            point.Item6, point.Item7, 1, point.Item5);
                            break;
                    case PointType.sliderSin:
                        sl.MakeSlider(point.Item4, point.Item2, point.Item3,
                            point.Item6, point.Item7, 2, point.Item5);
                        break;
                }
            }
        }

        private void SetComponents()
        {
            Canva.Background = Brushes.Black;
            tb.Text = "Score: " + playerScore;
            tb.FontSize = 12;
            tb.Background = Brushes.Azure;
            Canva.Children.Add(tb);
            canvas.SetTop(tb,  10);
            canvas.SetLeft(tb, 10);
            healthBar.Text = "hp" + playerHealth;
            healthBar.FontSize = 12;
            healthBar.Background = Brushes.Azure;
            canvas.SetRight(healthBar, 10);
            canvas.SetTop(healthBar, 10);
            Canva.Children.Add(healthBar);
        }
        public void changeHealth(TextBlock healthBar, int changeCount)
        {
            if (playerHealth == 100)
                playerHealth += 0;
            else playerHealth += changeCount;
            if (playerHealth <= 0)
                MessageBox.Show("сдох как лох");
            healthBar.Text = "hp" + playerHealth;
        }
        public void upScore(TextBlock tb, int count)
        {
            playerScore += count;
            tb.Text = "Score: " + playerScore;
        }
    }

    
}