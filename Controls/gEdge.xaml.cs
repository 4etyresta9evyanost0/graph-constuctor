using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для gEdge.xaml
    /// </summary>
    public partial class gEdge : UserControl
    {
        public gEdge()
        {
            InitializeComponent();

            X1Changed += ChangedPos;
            X2Changed += ChangedPos;
            Y1Changed += ChangedPos;
            Y2Changed += ChangedPos;
        }

        private void ChangedPos(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _line.X1 = (double)GetValue(X1Property);
            _line.X2 = (double)GetValue(X2Property);
            _line.Y1 = (double)GetValue(Y1Property);
            _line.Y2 = (double)GetValue(Y2Property);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            
            if (e.Property == X1Property)
            {
                X1Changed?.Invoke(this, e);
            }

            if (e.Property == X2Property)
            {
                X2Changed?.Invoke(this, e);
            }

            if (e.Property == Y1Property)
            {
                Y1Changed?.Invoke(this, e);
            }

            if (e.Property == Y2Property)
            {
                Y2Changed?.Invoke(this, e);
            }

        }

        public static event PropertyChangedCallback X1Changed;
        public static event PropertyChangedCallback X2Changed;
        public static event PropertyChangedCallback Y1Changed;
        public static event PropertyChangedCallback Y2Changed;

        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }
        public static readonly DependencyProperty X1Property
            = DependencyProperty.Register(
                  "X1",
                  typeof(double),
                  typeof(gVertex),
                  new FrameworkPropertyMetadata(0.0, X1Changed)
              );

        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }
        public static readonly DependencyProperty Y1Property
            = DependencyProperty.Register(
                  "Y1",
                  typeof(double),
                  typeof(gVertex),
                  new FrameworkPropertyMetadata(0.0, Y1Changed)
              );

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }
        public static readonly DependencyProperty X2Property
            = DependencyProperty.RegisterAttached(
                  "X2",
                  typeof(double),
                  typeof(gVertex),
                  new FrameworkPropertyMetadata(0.0, X2Changed)
              );

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }
        public static readonly DependencyProperty Y2Property
            = DependencyProperty.Register(
                  "Y2",
                  typeof(double),
                  typeof(gVertex),
                  new FrameworkPropertyMetadata(0.0, Y2Changed)
              );

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty
            = DependencyProperty.Register(
                  "Stroke",
                  typeof(Brush),
                  typeof(gVertex),
                  new FrameworkPropertyMetadata(Brushes.Black)
              );

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty
            = DependencyProperty.Register(
                  "StrokeThickness",
                  typeof(double),
                  typeof(gVertex),
                  new FrameworkPropertyMetadata(2.0)
              );
    }
}
