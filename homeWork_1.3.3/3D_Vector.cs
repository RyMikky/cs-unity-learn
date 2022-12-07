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
            _x = x; _y = y; _z = z;
        }

        public void Print_3D_Vector()
        {
            Console.WriteLine($"_3D_Vector with coords {{ {_x}, {_y}, {_z} }}");
        }

        public void Add_3D_Vector(ref _3D_Vector other)
        {
            _x += other._x;
            _y += other._y;
            _z += other._z;
        }

        public void Add_3D_Vector(double x, double y, double z)
        {
            _x += x;
            _y += y;
            _z += z;
        }

        public void Sub_3D_Vector(ref _3D_Vector other)
        {
            _x -= other._x;
            _y -= other._y;
            _z -= other._z;
        }

        public void Sub_3D_Vector(double x, double y, double z)
        {
            _x -= x;
            _y -= y;
            _z -= z;
        }

        public void Mul_3D_Vector(double scalar)
        {
            _x *= scalar;
            _y *= scalar;
            _z *= scalar;
        }

        public void Dev_3D_Vector(double scalar)
        {
            Mul_3D_Vector(1 / scalar);
        }
    }
}
