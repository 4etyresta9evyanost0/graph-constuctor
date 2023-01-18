using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicalGraph
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DesignerWindow designerWindow;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (x, ev) =>
            {
                designerWindow = new DesignerWindow();
                designerWindow.Owner = this;
                optionsButton.Click += (y, eve) => designerWindow.Show();
            };
            delAllButton.Click += DelAllVerts;
            CanvGrid.MouseDown += ClickEvent;
            CanvGrid.MouseMove += MouseMovingEvent;
            CanvGrid.MouseUp += UnClickEvent;
            this.SizeChanged += (x, ev) => CanvGrid.MinHeight = this.Height - this.UIGrid.Height;

            AddingRB.Tag = MouseCanvasAction.Adding;
            MovingRB.Tag = MouseCanvasAction.Moving;
            SelectingRB.Tag = MouseCanvasAction.Selecting;
            BindingRB.Tag = MouseCanvasAction.Binding;
            //Positioning.IsPointInRect(Mouse.GetPosition(this), MainGrid);
        }

        private void DelAllVerts(object sender, EventArgs e)
        {
            CanvGrid.Children.Clear();
        }

        public enum MouseCanvasAction
        {
            Adding = 0,
            Moving = 1,
            Selecting = 2,
            Binding = 3,
            //Deleting = 3
        }
        public MouseCanvasAction MouseAction = MouseCanvasAction.Adding;
        Point p = new Point(-1, -1);

        gEdge BindingLine = null;
        private void ClickEvent(object sender, MouseButtonEventArgs e)
        {
            var s = sender as Grid;
            p = e.GetPosition(s);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                switch (MouseAction)
                {
                    case MouseCanvasAction.Adding:
                        {
                            var el = new gVertex(s);
                            Binding b = new Binding("Value");
                            b.Source = designerWindow.diameterUD;
                            el.SetBinding(gVertex.WidthProperty, b);
                            el.SetBinding(gVertex.HeightProperty, b);
                            //el.Width = el.Height = 75d;
                            var r = el.Width / 2;
                            b = new Binding("SelectedColor");
                            b.Converter = new ColorConverter();
                            b.Source = designerWindow.vertColorPicker;
                            //var c = new SolidColorBrush(vertColorPicker.SelectedColor ?? Color.FromRgb(255, 255, 255));
                            el.Ellipse.SetBinding(Ellipse.FillProperty, b);
                            b = new Binding("SelectedColor");
                            b.Converter = new ColorConverter();
                            b.Source = designerWindow.strokeVertColorPicker;
                            el.Ellipse.SetBinding(Ellipse.StrokeProperty, b);
                            b = new Binding("Value");
                            b.Source = designerWindow.strokeVertUD;
                            el.Ellipse.SetBinding(Ellipse.StrokeThicknessProperty, b);
                            el.HorizontalAlignment = HorizontalAlignment.Left;
                            el.VerticalAlignment = VerticalAlignment.Top;
                            el.Margin = new Thickness(p.X - r, p.Y - r, 0, 0);

                            Point mP = new Point(-1,-1);
                            el.MouseDown += (x, ev) => {
                                if (e.LeftButton == MouseButtonState.Pressed)
                                {
                                    var gV = (gVertex)x;
                                    switch (MouseAction)
                                    {
                                        case MouseCanvasAction.Moving:
                                            {
                                                mP = Mouse.GetPosition(gV);
                                                gV.CaptureMouse();
                                                break;
                                            }
                                        case MouseCanvasAction.Binding:
                                            {
                                                BindingLine = new gEdge();
                                                
                                                Binding bind = new Binding("CenterX");
                                                bind.Source = gV;
                                                BindingLine.SetBinding(gEdge.X1Property, bind);

                                                bind = new Binding("CenterY");
                                                bind.Source = gV;
                                                BindingLine.SetBinding(gEdge.Y1Property, bind);

                                                //BindingLine.X1 = gV.Margin.Left + gV.Width / 2;
                                                //BindingLine.Y1 = gV.Margin.Top + gV.Height / 2;
                                                Panel.SetZIndex(BindingLine, -1);

                                                Point canvPoint = Mouse.GetPosition(CanvGrid);
                                                BindingLine.X2 = canvPoint.X;
                                                BindingLine.Y2 = canvPoint.Y;

                                                BindingLine.Stroke = Brushes.Black;
                                                BindingLine.StrokeThickness = 2;


                                                CanvGrid.Children.Add(BindingLine);
                                                break;
                                            }
                                    }

                                }
                            }; //ElMouseDown;
                            el.MouseMove += (x, ev) => {
                                if (e.LeftButton == MouseButtonState.Pressed)
                                {
                                    var gV = (gVertex)x;
                                    if (!(VisualTreeHelper.GetParent(gV) is UIElement container))
                                        return;
                                    switch (MouseAction)
                                    {
                                        case MouseCanvasAction.Moving:
                                            {
                                                if (gV.IsMouseCaptured)
                                                {
                                                    var mousePosition = e.GetPosition(container);
                                                    gV.Margin = new Thickness(mousePosition.X - mP.X, mousePosition.Y - mP.Y, 0, 0);
                                                }
                                                break;
                                            }
                                        case MouseCanvasAction.Binding:
                                            {
                                                //
                                                break;
                                            }
                                    }
                                }
                            }; //ElMouseMove;
                            el.MouseUp += (x, ev) => {
                                if (e.LeftButton == MouseButtonState.Released)
                                {
                                    var gV = (gVertex)x;
                                    gV.ReleaseMouseCapture();
                                    switch (MouseAction)
                                    {
                                        case MouseCanvasAction.Moving:
                                            {
                                                break;
                                            }
                                        case MouseCanvasAction.Binding:
                                            {
                                                Binding bind = new Binding("CenterX");
                                                bind.Source = gV;
                                                BindingLine.SetBinding(gEdge.X2Property, bind);

                                                bind = new Binding("CenterY");
                                                bind.Source = gV;
                                                BindingLine.SetBinding(gEdge.Y2Property, bind);

                                                BindingLine = null;
                                                break;
                                            }
                                    }
                                }
                            }; //ElMouseUp;

                            s.Children.Add(el);

                            if (Keyboard.Modifiers != ModifierKeys.Shift)
                                MovingRB.IsChecked = true;
                            break;
                        }
                    case MouseCanvasAction.Moving:
                        {
                            //var list = s.Children.Cast<FrameworkElement>().ToList();
                            //var el = list.FindAll(x => Positioning.IsPointInRect(p, x)).First();
                            //if (el == null)
                            //    return;
                            //s.CaptureMouse();
                            break;
                        }
                    case MouseCanvasAction.Selecting:
                        {

                            break;
                        }
                    case MouseCanvasAction.Binding:
                        {

                            break;
                        }
                }
                
            }


            //else if (e.RightButton == MouseButtonState.Pressed)
            //{
            //    var list = CanvGrid.Children.Cast<FrameworkElement>().ToList();
            //    var toDel = list.FindAll(x => Positioning.IsPointInRect(p, x));
            //    foreach (var el in toDel)
            //    {
            //        CanvGrid.Children.Remove(el);
            //    }
            //}
        }

        private void MouseMovingEvent(object sender, MouseEventArgs e)
        {
            var s = sender as Grid;
            p = e.GetPosition(s);

            switch (MouseAction)
            {
                case MouseCanvasAction.Adding:
                    {

                        break;
                    }
                case MouseCanvasAction.Moving:
                    {

                        break;
                    }
                case MouseCanvasAction.Selecting:
                    {

                        break;
                    }
                case MouseCanvasAction.Binding:
                    {
                        if (BindingLine == null)
                            return;

                        Point canvPoint = Mouse.GetPosition(CanvGrid);
                        BindingLine.X2 = canvPoint.X;
                        BindingLine.Y2 = canvPoint.Y;
                        break;
                    }
            }

        }

        private void UnClickEvent(object sender, MouseButtonEventArgs e)
        {
            var s = sender as Grid;
            p = e.GetPosition(s);
            if (e.LeftButton == MouseButtonState.Released)
            {
                switch (MouseAction)
                {
                    case MouseCanvasAction.Adding:
                        {
                           
                            break;
                        }
                    case MouseCanvasAction.Moving:
                        {
                            
                            break;
                        }
                    case MouseCanvasAction.Selecting:
                        {

                            break;
                        }
                    case MouseCanvasAction.Binding:
                        {
                            if (BindingLine == null)
                                return;
                            CanvGrid.Children.Remove(BindingLine);
                            BindingLine = null;
                            break;
                        }
                }

            }
        }

        [ValueConversion(typeof(Color?), typeof(SolidColorBrush))]
        public class ColorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Color? col = value as Color?;
                return new SolidColorBrush(col ?? Color.FromRgb(255,255,255));
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var brush = (value as SolidColorBrush);
                return brush.Color;
            }
        }

        private void MouseActionChanged(object sender, RoutedEventArgs e)
        {
            if (!((sender as RadioButton).Tag is MouseCanvasAction action))
                return;
            MouseAction = action;
        }
        //[ValueConversion(typeof(string), typeof(double))]
        //public class TextToNumConverter : IValueConverter
        //{
        //    public TextToNumConverter()
        //    { }

        //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //    {
        //        string strValue = value as string;
        //        double res = double.NaN;
        //        if (double.TryParse(strValue, out res))
        //        {
        //            return res;
        //        }
        //        return DependencyProperty.UnsetValue;
        //    }

        //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //    {
        //        double num = (double)value;
        //        return num.ToString();
        //    }
        //}
    }
}
