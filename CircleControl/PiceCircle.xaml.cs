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

namespace CircleControl
{
    /// <summary>
    /// Interaction logic for PiceCircle.xaml
    /// </summary>
    public partial class PiceCircle : UserControl
    {
        public static readonly DependencyProperty RadiusProperty;
        public static readonly DependencyProperty AngleProperty;
        public static readonly DependencyProperty AngleRotateProperty;
        public static readonly DependencyProperty TextProperty;
        public double AngleText { get; set; }
        static double Lambda;
        static PiceCircle()
        {
          
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata((Double)0, FrameworkPropertyMetadataOptions.AffectsMeasure);
            metadata.CoerceValueCallback = new CoerceValueCallback(RadiusChangeCallback);
            RadiusProperty = DependencyProperty.Register("RadiusSz", typeof(Double), typeof(PiceCircle), metadata);

            metadata = new FrameworkPropertyMetadata((Double)0, FrameworkPropertyMetadataOptions.AffectsMeasure);
            metadata.CoerceValueCallback = new CoerceValueCallback(AngleChangeCallback);
            AngleProperty = DependencyProperty.Register("Angle", typeof(Double), typeof(PiceCircle), metadata);

            metadata = new FrameworkPropertyMetadata((String)"Введите текст", FrameworkPropertyMetadataOptions.AffectsMeasure);
            metadata.CoerceValueCallback = new CoerceValueCallback(TextValidate);
            TextProperty = DependencyProperty.Register("TextContent", typeof(string), typeof(PiceCircle), metadata);

            metadata = new FrameworkPropertyMetadata((Double)0, FrameworkPropertyMetadataOptions.AffectsMeasure);
            metadata.CoerceValueCallback = new CoerceValueCallback(AngleRotateCallback);
            AngleRotateProperty = DependencyProperty.Register("AngleRotate", typeof(Double), typeof(PiceCircle), metadata);
            

        }
        
         static object AngleRotateCallback( DependencyObject obj, Object value)
        {
         
            PiceCircle PiceCircleObj = (PiceCircle)obj;
            var angle = (double)value;
            Point begin = (Point)PiceCircleObj.Resources["Begin"];
            Point end = (Point)PiceCircleObj.Resources["End"];
           
            CoordinateEndCompute(PiceCircleObj, PiceCircleObj.Angle, PiceCircleObj.Radius);
            UEParametrs Parametrs  = RotateUIElement(PiceCircleObj.Angle, angle, begin, end, PiceCircleObj.Radius);
           
            var text = (TextBox)PiceCircleObj.Resources["ContentText"];
            text.Height = Parametrs.Heigth;
            text.Width = Parametrs.Width;
            text.RenderTransform = new RotateTransform(Parametrs.AngleElement, text.Width / 2, text.Height / 2);
            PiceCircleObj.RenderTransform = new RotateTransform(angle, PiceCircleObj.Radius, PiceCircleObj.Radius);
            return value;

        }
       
        static object RadiusChangeCallback( DependencyObject obj, Object value)
        {
            PiceCircle PiceCircleObj = (PiceCircle)obj;
            var radius = (double)value;
            CoordinateChange(PiceCircleObj, radius);
            CoordinateEndCompute(PiceCircleObj, PiceCircleObj.Angle, radius);
            return value;

        }
        static void CoordinateChange(PiceCircle PiceCircleObj, Double radius)
        {

            PiceCircleObj.Height = 2 * radius;
            PiceCircleObj.Width = 2 * radius;
            PiceCircleObj.Resources["Center"] = new Point(radius, radius);
            PiceCircleObj.Resources["Begin"] = new Point(2 * radius, radius);
            PiceCircleObj.Resources["RadiusSz"] = new Size(radius, radius);
        }
        static void CoordinateEndCompute(PiceCircle PiceCircleObj, double AngleGrad, double Radius)
        {
            
           
            Point begin = (Point)PiceCircleObj.Resources["Begin"];
            double angle = AngleGrad * Math.PI / 180;
            Point End = new Point();
            End.X = Math.Abs(Radius + (begin.X - Radius) * Math.Cos(angle) + (Radius - begin.Y) * Math.Sin(angle));
            End.Y = Math.Abs(Radius + (begin.Y - Radius) * Math.Cos(angle) + (begin.X - Radius) * Math.Sin(angle));
            PiceCircleObj.Resources["End"] = End;


            UEParametrs Parameters = RotateUIElement(AngleGrad, PiceCircleObj.AngleRotate, begin, End, Radius); ;
            var text = (TextBox)PiceCircleObj.Resources["ContentText"];
           
            if (AngleGrad == 360)
            {
                var Elipse = ((GeometryGroup)PiceCircleObj.Resources["Circle"]).Children[0];
                Elipse.SetValue(EllipseGeometry.RadiusXProperty, Radius);
                Elipse.SetValue(EllipseGeometry.RadiusYProperty, Radius);
                PiceCircleObj.Template = (ControlTemplate)PiceCircleObj.Resources["TemplateCircle"];



                
            }
            else if(AngleGrad != 0)
            {
                PiceCircleObj.Template = (ControlTemplate)PiceCircleObj.Resources["TemplatePice"];
            
                
            }
            text.SetValue(Canvas.TopProperty, Parameters.Top);
            text.SetValue(Canvas.LeftProperty, Parameters.Left);
            text.Width = Parameters.Width;
            text.Height = Parameters.Heigth;
            text.RenderTransform = new RotateTransform(Parameters.AngleElement,text.Width/2,text.Height/2);
        }
        static UEParametrs RotateUIElement(double AngleGrad, double AngleUIElement, Point begin, Point End, double Radius)
        {
            UEParametrs ParametrValue = new UEParametrs();
            double Top = (Radius + End.Y) / 2;
            double Left = (Radius + End.X) / 2;
            double MiddleBeginLineY = (Radius + begin.Y) / 2;
            double MiddleBeginLineX = (Radius + begin.X) / 2;
            double Side = Math.Sqrt(Math.Pow(MiddleBeginLineX - Left, 2) + Math.Pow(MiddleBeginLineY - Top, 2));
            if (AngleGrad == 360)
            {
                ParametrValue.AngleElement = 0;
                ParametrValue.Width = Radius;
                ParametrValue.Heigth = 30;
                ParametrValue.Top = 5 * Radius / 4 + ParametrValue.Heigth;
                ParametrValue.Left = Radius - ParametrValue.Width / 2;
            
            }
            else if (AngleGrad == 180)
            {


                double Y = Top - MiddleBeginLineY;
                double X = Left - MiddleBeginLineX;
                double alpha = Math.Abs(Y * 0 + X * 1) /
                    (Math.Sqrt(Math.Pow(0, 2) + Math.Pow(1, 2)) * Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)));

                ParametrValue.AngleElement = AngleUIElement == 180 ? 180: 0;
                ParametrValue.Width = Side;
                ParametrValue.Heigth = Radius / 2;
                ParametrValue.Top = Top == Radius ? Radius + (Radius - ParametrValue.Heigth) / 2 : Top;
                ParametrValue.Left = Left ;

            }
            else if(AngleGrad >= 90)
            {
                ParametrValue.AngleElement = -AngleUIElement;
                ParametrValue.Width = Radius / 2;
                ParametrValue.Heigth = Radius / 2;
                ParametrValue.Top = Top - 2*ParametrValue.Heigth/3;
                ParametrValue.Left = MiddleBeginLineX - 2*ParametrValue.Width/3;
            }
            else if(AngleGrad < 90)
            {

                ParametrValue.AngleElement = -AngleUIElement;
                ParametrValue.Width = Side;
                ParametrValue.Heigth = 2*Side/3;
                if (Lambda == 0)
                    Lambda = 2;
                double RigthTopX = 0, RigthTopY = 0;
                double LenRigthTop = 0, LenRigthBottom = 0 ; 
                double RigthBottomX = 0, RigthBottomY = 0;
                do
                {
                    ParametrValue.Top = ((Radius + Lambda * End.Y) / (1 + Lambda) + (Radius + Lambda * begin.Y) / (1 + Lambda)) / 2;
                    ParametrValue.Left = ((Radius + Lambda * End.X) / (1 + Lambda) + (Radius + Lambda * begin.X) / (1 + Lambda)) / 2;
                    RigthTopX = ParametrValue.Left + ParametrValue.Width;
                    RigthTopY = ParametrValue.Top;
                    RigthBottomX = RigthTopX;
                    RigthBottomY = ParametrValue.Top + ParametrValue.Heigth;
                    LenRigthTop = Math.Sqrt(Math.Pow(RigthTopX - Radius, 2) + Math.Pow(RigthTopY - Radius, 2));
                    LenRigthBottom = Math.Sqrt(Math.Pow(RigthBottomX - Radius, 2) + Math.Pow(RigthBottomY - Radius, 2));
                    if ((Radius < LenRigthTop || Radius < LenRigthBottom) && AngleUIElement == 0)
                        Lambda -= 0.25;
                }
                while ((Radius < LenRigthTop || Radius < LenRigthBottom) && AngleUIElement==0);
            }
            return ParametrValue;
        }
        static object TextValidate (DependencyObject obj, Object value)
        {
            PiceCircle PiceCircleObj = (PiceCircle)obj;
            ((TextBox)PiceCircleObj.Resources["ContentText"]).Text = (string)value;
            return value;
        }
        static object AngleChangeCallback(DependencyObject obj, Object value)
        {
            PiceCircle PiceCircleObj =   (PiceCircle)obj;
            CoordinateEndCompute(PiceCircleObj, (double)value, PiceCircleObj.Radius);
           
            return value;

        }
        public Double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        public Double AngleRotate
        {
            get { return (double)GetValue(AngleRotateProperty); }
            set { SetValue(AngleRotateProperty, value); }
        }
        public Double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set {  SetValue(AngleProperty, value);}
        }
        public String TextContent
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value);  }
        }
        public PiceCircle()
        {
            InitializeComponent();
        }
    }
}
