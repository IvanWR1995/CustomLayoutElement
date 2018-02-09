using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CircleControl
{
    class FirstPanel:Panel
    {
        public FirstPanel():base()
        { 
            
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = 100, y = 100, angle = 0 ;
            byte tmp = 100;
            foreach (UIElement child in InternalChildren)
            {
                var PiceCircleObj = ((PiceCircle)child);
                PiceCircleObj.AngleRotate = angle;
                child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
                PiceCircleObj.Background = new SolidColorBrush(Color.FromRgb(tmp, (byte)(tmp+10), (byte)(tmp+10)));
                angle += PiceCircleObj.Angle;
                tmp += 10;
                
            }
            return finalSize; 
           
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            Size panelDesiredSize = new Size();
            double Angle = Math.Round(360.0 / InternalChildren.Count,1);
            foreach (UIElement child in InternalChildren)
            {
                ((PiceCircle)child).Angle = Angle;
                child.Measure(availableSize);
                panelDesiredSize = child.DesiredSize;
            }

            return panelDesiredSize;

          
        }
    }
}
