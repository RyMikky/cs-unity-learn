using System;
using System.Collections.Generic;
using System.Text;

namespace MyMath
{
    internal struct _3D_Vector
    {
        public double _x { get; set; }
        public double _y { get; set; }
        public double _z { get; set; }

        public _3D_Vector(double x, double y, double z)
        {
            Console.WriteLine($"_3D_Vector CTor with args {{ {x}, {y}, {z}}} \n");
            _x = x; _y = y; _z = z;
        }

        public void Add_3D_Vector(ref _3D_Vector other)
        {
            Console.WriteLine("Using method { \"void Add_3D_Vector(ref _3D_Vector other)\" }");
            Console.WriteLine($"On _3D_Vector with coords {{ {_x}, {_y}, {_z} }}");
            _x += other._x;
            _y += other._y;
            _z += other._z;
            Console.WriteLine($"Result _3D_Vector with coords {{ {_x}, {_y}, {_z} }} \n");
        }

        public void Add_3D_Vector(double x, double y, double z)
        {
            Console.WriteLine("Using method { \"void Add_3D_Vector(double x, double y, double z)\" }");
            Console.WriteLine($"On _3D_Vector with coords {{ {_x}, {_y}, {_z} }}");
            _x += x;
            _y += y;
            _z += z;
            Console.WriteLine($"Result _3D_Vector with coords {{ {_x}, {_y}, {_z} }} \n");
        }

        public void Sub_3D_Vector(ref _3D_Vector other)
        {
            Console.WriteLine("Using method { \"void Sub_3D_Vector(ref _3D_Vector other)\" }");
            Console.WriteLine($"On _3D_Vector with coords {{ {_x}, {_y}, {_z} }}");
            _x -= other._x;
            _y -= other._y;
            _z -= other._z;
            Console.WriteLine($"Result _3D_Vector with coords {{ {_x}, {_y}, {_z} }} \n");
        }

        public void Sub_3D_Vector(double x, double y, double z)
        {
            Console.WriteLine("Using method { \"void Sub_3D_Vector(double x, double y, double z)\" }");
            Console.WriteLine($"On _3D_Vector with coords {{ {_x}, {_y}, {_z} }}");
            _x -= x;
            _y -= y;
            _z -= z;
            Console.WriteLine($"Result _3D_Vector with coords {{ {_x}, {_y}, {_z} }} \n");
        }

        public void Mul_3D_Vector(double scalar)
        {
            Console.WriteLine("Using method { \"void Mul_3D_Vector(double scalar)\" }");
            Console.WriteLine($"On _3D_Vector with coords {{ {_x}, {_y}, {_z} }}");
            _x *= scalar;
            _y *= scalar;
            _z *= scalar;
            Console.WriteLine($"Result _3D_Vector with coords {{ {_x}, {_y}, {_z} }} \n");
        }

        public void Dev_3D_Vector(double scalar)
        {
            Console.WriteLine("Using method { \"void Dev_3D_Vector(double scalar)\" }");
            Console.WriteLine($"On _3D_Vector with coords {{ {_x}, {_y}, {_z} }}");
            // Было бы логично вызвать умножение на скаляр от значение 1 / scalar
            // Mul_3D_Vector(1 / scalar);
            // Но чтобы удовлетворить заданию по выводу результатов в консоль сделаем иначе

            _x *= 1 / scalar;
            _y *= 1 / scalar;
            _z *= 1 / scalar;

            Console.WriteLine($"Result _3D_Vector with coords {{ {_x}, {_y}, {_z} }} \n");
        }
    }

    //public void Add_3D_Vector(ref _3D_Vector lhs, ref _3D_Vector rhs)
    //{

    //}
}
