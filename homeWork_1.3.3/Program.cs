using System;
using MyMath;

namespace homeWork_1._3._3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            _3D_Vector myVector = new _3D_Vector(1, 5, 0);
            myVector.Add_3D_Vector(4, 0, 6);

            myVector.Dev_3D_Vector(2);

            _3D_Vector myVector2 = new _3D_Vector(0, 2.5, 3.4);

            myVector.Add_3D_Vector(ref myVector2);
        }
    }
}