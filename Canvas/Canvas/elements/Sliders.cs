using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using canvas = System.Windows.Controls.Canvas;
namespace OSU
{
    public class Sliders: IElement
    {
        public int PosX { get; }
        public int PosY { get; }
        public int DelayTime { get; }
        public int DeathTime { get; }
        public int Dx { get; }
        public int Dy { get; }

        public int Type { get; }
        private bool isMousePressed;  
        private MainWindow mw;
        private double constY = 50;
        private Ellipse slider = new Ellipse()                               
                {
                    Width = 50,
                    Height = 50,
                    Stroke = Brushes.White,
                    StrokeThickness = 6,
                    Fill = Brushes.Green
                    
                };
        private  Ellipse target = new Ellipse()                                
                {
                    Width = 50,
                    Height = 50,
                    Stroke = Brushes.White,
                    StrokeThickness = 6,
                    Fill = Brushes.Black
                };
        public Sliders(MainWindow m, int pX, int pY, int dT, int deT, int delX, int delY, int type)
        {
            Type = type;
            PosX = pX;
            PosY = pY;
            DelayTime = dT;
            DeathTime = deT;
            Dx = delX;
            Dy = delY;
            mw = m;
        }

        private void MakeSlider(int type)
        {
            mw.Canva.Children.Add(slider);
            mw.Canva.Children.Add(target);
            canvas.SetLeft(slider, PosX);
            canvas.SetTop(slider, PosY);
            canvas.SetLeft(target, PosX + Dx);
            canvas.SetTop(target, PosY + Dy);
            slider.MouseLeftButtonDown += (sender, args) =>
                isMousePressed = true;
            slider.MouseLeftButtonUp += (sender, args) =>
                isMousePressed = false;
            slider.MouseMove += (sender, args) =>
            {
                
                if (isMousePressed)
                    switch (type)
                    {
                        case 0:
                            if (IsOutOfRange(slider, PosX, PosY, Dx, Dy, 0))
                                MoveSlider(slider, args.GetPosition(null).X - 25, PosY);
                            else MoveSlider(slider, PosX, PosY);
                            break;
                        case 1:
                            if (IsOutOfRange(slider, PosX, PosY, Dx, Dy, 1))
                                MoveSlider(slider, PosX, args.GetPosition(null).Y - 25);
                            else MoveSlider(slider, PosX, PosY);
                            break;
                        case 2:
                            if (IsOutOfRange(slider, PosX, PosY, Dx, Dy, 0))
                                MoveSlider(slider, args.GetPosition(null).X - 25, 
                            canvas.GetTop(target) - constY * Math.Sin((args.GetPosition(null).X - PosX) / Dx * Math.PI));
                            else MoveSlider(slider, PosX, PosY);
                            break;
                    }
            };
            target.MouseEnter += (o, eventArgs) =>
            {
                if (isMousePressed)
                {
                    mw.Canva.Children.Remove(target);
                    mw.Canva.Children.Remove(slider);
                    mw.ChangeHealth(mw.HealthBar, 10);
                    mw.UpScore(mw.Tb, 500);
                }
            };
                
        }

        private bool IsOutOfRange (Ellipse slider, double pointX, double pointY,  double dX, double dY, int type)
        {
            bool isOutOfRange = false;
            switch (type)
            {
                case 0:
                    if (dX > 0)
                        isOutOfRange = (Canvas.GetLeft(slider) >= pointX);
                    else isOutOfRange = (Canvas.GetLeft(slider) <= pointX);
                    break;
                case 1:
                    if (dY > 0)
                        isOutOfRange = (Canvas.GetTop(slider) >= pointY);
                    else isOutOfRange = (Canvas.GetTop(slider) <= pointY);
                    break;
            }

            return isOutOfRange;
        }

        private void MoveSlider(Ellipse slider, double posX, double posY)
        {
            Canvas.SetLeft(slider, posX);
            Canvas.SetTop(slider, posY);
        }

        public void Show()
            => MakeSlider(Type);    

        public void Remove()
        {
            mw.Canva.Children.Remove(target);                        
            mw.Canva.Children.Remove(slider);
            mw.ChangeHealth(mw.HealthBar, -20);
        }
    }
}