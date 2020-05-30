using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OSU
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MapBuilder
    {
        private const int TIMEWAIT = 1000;
        private bool _recording;
        private List<Entity> list = new List<Entity>();
        private Stopwatch stopwatch;
        
        
        public MapBuilder()
        {
            this.stopwatch = new Stopwatch();
            InitializeComponent();
            Canva.MouseLeftButtonDown += (sender, args) =>
            {
                Point p = Mouse.GetPosition(Canva);
                p.X += Canva.Margin.Left - 25;
                p.Y += Canva.Margin.Top - 25;
                if(_recording) list.Add(new Entity((EntityType) EntityType.SelectedIndex, p, (int) stopwatch.ElapsedMilliseconds ,TIMEWAIT));
            };
            SaveButton.Click += (sender, args) =>
            {
                foreach (var entity in list) 
                    DrawEntity(entity);
                File.WriteAllLines(FileNameForm.Text + ".txt", list.Select(x => x.ToString()));
            };
        }

        private void DrawEntity(Entity entity)
        {
            Ellipse e = new Ellipse() { Fill = Brushes.Indigo, Stroke = Brushes.Lime, Width = 50, Height = 50 };
            Canva.Children.Add(e);
            Canvas.SetLeft(e, entity.X);
            Canvas.SetTop(e, entity.Y);
        }

        private void Canva_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.R:
                    _recording = !_recording;
                    if(_recording) stopwatch.Restart();
                    else stopwatch.Stop();
                    RecordingShield.Text = _recording ? "Record..." : "Paused";
                    break;
            }
        }
    }

    public class Entity
    {
        private EntityType _type;
        private Point _point;
        private int _timeout, _timewait;
        public double X
        {
            get => _point.X;
        }
        public double Y
        {
            get => _point.Y;
        }
        
        public Entity(EntityType type, Point point, int timeout, int timewait)
        {
            this._type = type;
            this._point = point;
            this._timeout = timeout;
            this._timewait = timewait;
        }

        public override string ToString()
            => $"{(int)_type};{(int)_point.X};{(int)_point.Y};{_timeout};{_timewait}";
        
    }
    
    public enum EntityType {
        Point, SliderHorizontal, SliderVertical, SliderSin, SliderLog10
    }
}