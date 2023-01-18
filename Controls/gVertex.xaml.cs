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
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class gVertex : UserControl
    {
        public Grid ParentGrid { get; private set; }
        public Label Label { get => _label;  }
        public Ellipse Ellipse { get => _ellipse; }

        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }
        public static readonly DependencyProperty CenterXProperty
            = DependencyProperty.Register(
                  "CenterX",
                  typeof(double),
                  typeof(gVertex),
                  new PropertyMetadata()
              );

        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }
        public static readonly DependencyProperty CenterYProperty
            = DependencyProperty.Register(
                  "CenterY",
                  typeof(double),
                  typeof(gVertex),
                  new PropertyMetadata()
              );
        //public Vertex Vertex { get; private set; }

        //public delegate void del(object sender, MouseButtonEventArgs e);

        public delegate void MarginChangedEventHandler(object sender, EventArgs e);
        public event MarginChangedEventHandler MarginChanged;

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == MarginProperty)
            {
                MarginChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public gVertex(Grid parent/*, Vertex v*/)
        {
            InitializeComponent();
            this.SizeChanged += (x,ev) => _textBox.FontSize = Label.FontSize = ev.NewSize.Height * (8d / 35d);

            void setCenter(object sender, EventArgs e)
            {
                CenterY = this.Margin.Top + this.Height / 2;
                CenterX = this.Margin.Left + this.Width / 2;
            };

            this.SizeChanged += setCenter;
            this.MarginChanged += setCenter;


            ParentGrid = parent;
            Label.MouseDoubleClick += (x, ev) =>
            {
                _textBox.Visibility = Visibility.Visible;
                Label.Visibility = Visibility.Hidden;
                _textBox.Text = (string)Label.Content;
            };

            _textBox.AcceptsReturn = _textBox.AcceptsTab = false;
            _textBox.KeyDown += (x, ev) =>
            {
                if (ev.Key == Key.Enter)
                {
                    _textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            };
            _textBox.LostFocus += (x, ev) =>
            {
                _textBox.Visibility = Visibility.Hidden;
                Label.Visibility = Visibility.Visible;
                Label.Content = _textBox.Text;
            };

            ContextMenu = new ContextMenu();

            var mi = new MenuItem();
            mi.Header = "Переименовать";
            mi.Click += (x, ev) =>
            {
                if (_textBox.Visibility != Visibility.Visible)
                {
                    _textBox.Visibility = Visibility.Visible;
                    Label.Visibility = Visibility.Hidden;
                    _textBox.Text = (string)Label.Content;
                }
                else
                {
                    _textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            };
            
            this.ContextMenu.Items.Add(mi);

            mi = new MenuItem();
            mi.Header = "Удалить";
            mi.Click += (x, ev) => ParentGrid.Children.Remove(this); 
            this.ContextMenu.Items.Add(mi);



        }
    }
}
