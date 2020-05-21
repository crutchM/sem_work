using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using canvas = System.Windows.Controls.Canvas;

namespace Canvas
{
    enum PointType
    {
        dot = 0,
        horizontalSlider,
        verticalSlider
    }
    
    public partial class MainWindow
    {
        private int playerScore = 0;
        private int playerHealth = 100;
        public TextBlock tb { get;}= new TextBlock();
        public TextBlock healthBar { get;} = new TextBlock();
        public MainWindow()
        {
            InitializeComponent();
            SetComponents();
            
            //Canva.MouseLeftButtonDown += (sender, args) =>
              //  {
                //    changeHealth(healthBar, -20);
                  //  if (playerHealth <= 0)
                    //    MessageBox.Show("чекай мать");
                //};
            foreach (var point in new LevelPoints("points2.txt"))
            {
                
                switch ((PointType) point.Item1)
                {
                    case PointType.dot:
                        DispatcherTimer determinate = new DispatcherTimer();
                        DispatcherTimer invalidate = new DispatcherTimer();

                        Ellipse target = new Ellipse()
                        {
                            Width = 50,
                            Height = 50,
                            Stroke = Brushes.White,
                            StrokeThickness = 6,
                            Fill = Brushes.Black
                        };
                        target.MouseLeftButtonDown += (sender, args) =>
                        {
                            upScore(tb, 500);
                            Canva.Children.Remove(target);
                            if (playerHealth > 100)
                                changeHealth(healthBar, 0);
                            else changeHealth(healthBar, 10);
                        };
                        determinate.Interval = TimeSpan.FromMilliseconds(point.Item4);
                        determinate.Tick += (o, e) => {
                            Canva.Children.Add(target);
                            canvas.SetLeft(target, point.Item2);
                            canvas.SetTop(target, point.Item3);
                            invalidate.Start();
                            determinate.Stop();
                        };
                        determinate.Start();
                        invalidate.Interval = TimeSpan.FromMilliseconds(point.Item5);
                        invalidate.Tick += (o, e) => {
                            Canva.Children.Remove(target);
                            changeHealth(healthBar, -20);
                            invalidate.Stop();
                        };
                        break;
                    case PointType.horizontalSlider:
                            Sliders sl = new Sliders(this);
                            sl.MakeSlider(point.Item4, point.Item2, point.Item3, 100, 0, point.Item5);
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
            playerHealth += changeCount;
            healthBar.Text = "hp" + playerHealth;
        }
        public void upScore(TextBlock tb, int count)
        {
            playerScore += count;
            tb.Text = "Score: " + playerScore;
        }
    }

    
}