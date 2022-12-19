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
    public static class Positioning
    {
        public static Point MarginToPoint(FrameworkElement el)
        {
            return new Point(el.Margin.Left, el.Margin.Top);
        }

        public static bool IsPointInRect(Point p, FrameworkElement r)
        {
            var x = p.X;
            var y = p.Y;

            var _x = r.Margin.Left;
            var _y = r.Margin.Top;

            //Point TopLeft = MarginToPoint(r);
            //Point BottomRight = TopLeft + new Vector(r.Width, r.Height);
            //Point b = new Point(TopLeft.X + r.Width, TopLeft.Y + r.Height);

            if (x >= _x && x - r.Width <= _x && y >= _y)
            {
                return y - r.Height <= _y;
            }
            return false;
        }
    }
}
