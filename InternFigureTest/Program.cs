using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace InternFigureTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Circle circle = new Circle(12);

            Console.WriteLine($"Площадь круга {circle.CountArea()}");

            Figure figure = new Figure()
            {
                Points = circle.Points
            };
            
            Console.WriteLine($"Площадь круга, в compile-time {figure.CountArea()} \n \n");

            Triangle triangle = new Triangle(3, 4, 5);

            figure.Points = triangle.Points;

            Console.WriteLine($"Площадь треугольника {triangle.CountArea()}" +
                $"\nЯвляется ли треугольник прямоугольным: {triangle.IsRightAngled()}" +
                $"\nПлощадь треугольника в compile-time {figure.CountArea()}");

            Console.ReadLine();
        }
    }
    class Figure
    {
        public List<Vector2> Points = new List<Vector2>();
        public virtual float CountArea()
        {
            int n = Points.Count;
            float area = 0;


            for (int i = 0; i < n; i++)
            {
                Vector2 current = Points[i];
                Vector2 next = Points[(i + 1) % n];
                area += current.X * next.Y - next.X * current.Y;
            }

            return Math.Abs(area) / 2;
        }
    }
    class Circle : Figure
    {
        public float Radius { get; set; }
        public float Area { get; set; }
        public Circle(float radius) : base()
        {
            List<Vector2> points = new List<Vector2>();
            int pointCount = (int) radius * 10;

            for (int i = 0; i < pointCount; i++)
            {
                float angle = i * (2 * MathF.PI / pointCount);
                float x = radius * MathF.Cos(angle);
                float y = radius * MathF.Sin(angle);
                points.Add(new Vector2(x, y));
            }

            this.Radius = radius;
            this.Points = points;
            this.Area = (float)Math.PI * (float)Math.Pow(Radius, 2);
        }
        public override float CountArea() => Area;
    }
    class Triangle : Figure
    {
        float side1;
        float side2;
        float side3;

        public Triangle(float side1, float side2, float side3)
        {
            if (side1 + side2 <= side3 || side1 + side3 <= side2 || side2 + side3 <= side1)
            {
                throw new ArgumentException("Из данных сторон нельзя построить треугольник.");
            }

            this.side1 = side1;
            this.side2 = side2;
            this.side3 = side3;


            Points.Add(new Vector2(0, 0));
            Points.Add(new Vector2(side1, 0));

            float cosAngleC = (side1 * side1 + side2 * side2 - side3 * side3) / (2 * side1 * side2);
            float angleC = MathF.Acos(cosAngleC);         // Угол при вершине C

            float xC = side2 * MathF.Cos(angleC);
            float yC = side2 * MathF.Sin(angleC);

            Points.Add(new Vector2(xC, yC));
        }
        public override float CountArea()
        {
            float s = (side1 + side2 + side3) / 2.0f;

            float area = (float)Math.Sqrt(s * (s - side1) * (s - side2) * (s - side3));

            return area;
        }
        public bool IsRightAngled()
        {
            float hypotenuse = Math.Max(Math.Max(side1, side2), side3);
            float leg1, leg2;

            if (hypotenuse == side1)
            {
                leg1 = side2;
                leg2 = side3;
            }
            else if (hypotenuse == side2)
            {
                leg1 = side1;
                leg2 = side3;
            }
            else
            {
                leg1 = side1;
                leg2 = side2;
            }

            return hypotenuse * hypotenuse == (leg1 * leg1 + leg2 * leg2);
        }
    }
}
