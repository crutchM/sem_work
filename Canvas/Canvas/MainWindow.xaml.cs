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
        slider
    }
    
    public partial class MainWindow
    {
        private int playerScore = 0;
        TextBlock tb = new TextBlock();
        
        public MainWindow()
        {
            InitializeComponent();
            Canva.Background = Brushes.Black;
            tb.Text = "Score: " + playerScore;
            tb.FontSize = 12;
            tb.Background = Brushes.Azure;
            Canva.Children.Add(tb);
            canvas.SetTop(tb, 10);
            canvas.SetLeft(tb, 10);
            foreach (var point in new LevelPoints("points.txt"))
            {
                switch ((PointType) point.Item1)
                {
                    case PointType.dot:
                        DispatcherTimer determinate = new DispatcherTimer();
                        DispatcherTimer invalidate = new DispatcherTimer();

                        Ellipse b = new Ellipse()
                        {
                            Width = 50,
                            Height = 50,
                            Stroke = Brushes.White,
                            StrokeThickness = 6,
                            Fill = Brushes.Black
                        };
                        b.MouseLeftButtonDown += (sender, args) => upScore(tb, 500);
                        determinate.Interval = TimeSpan.FromMilliseconds(point.Item4);
                        determinate.Tick += (o, e) => {
                            Canva.Children.Add(b);
                            canvas.SetLeft(b, point.Item2);
                            canvas.SetTop(b, point.Item3);
                            invalidate.Start();
                            determinate.Stop();
                        };
                        determinate.Start();
                        invalidate.Interval = TimeSpan.FromMilliseconds(point.Item5);
                        invalidate.Tick += (o, e) => {
                            Canva.Children.Remove(b);
                            invalidate.Stop();
                        };
                        break;
                }
            }
        }

        private void upScore(TextBlock tb, int count)
        {
            playerScore += count;
            tb.Text = "Score: " + playerScore;
        }
    }

    public class LevelPoints : IEnumerable<Tuple<int, int, int, int, int>>
    {

        private string filePath;
        private StreamReader sr;

        public LevelPoints(string fileName)
        {
            this.filePath = fileName;
            openFile();
        }

        private void openFile()
        {
            try
            {
                this.sr = new StreamReader(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("[E]: " + e);
                if (this.sr == null)
                {
                    MessageBox.Show(
                        "Возникла непредвиденная ошибка: отсутствует один или более файлов уровней",
                        "Ошибка!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
        
        public IEnumerator<Tuple<int, int, int, int, int>> GetEnumerator()
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if(line[0] == '#') continue;
                string[] values = line.Split(';');
                if(int.TryParse(values[0], out int type)
                   && int.TryParse(values[1], out int x)
                   && int.TryParse(values[2], out int y)
                   && int.TryParse(values[3], out int when)
                   && int.TryParse(values[4], out int duration))
                    yield return Tuple.Create(type, x, y, when, duration);
                else
                    yield break;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}