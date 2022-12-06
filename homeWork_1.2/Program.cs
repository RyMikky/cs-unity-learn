using System;

namespace homeWork_1._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();

            int[] a = new int[100];                    // создаём массив

            for (int i = 0; i < a.Length; ++i)
            {
                a[i] = rnd.Next(1, 1000);              // заполняем массив
            }

            PrintArrayInLine(a);                       // печатаем до сортировки
            ArrayBoubleSort(a);                        // сортируем массив
            PrintArrayInLine(a);                       // печатаем после сортировки
        }

        static void PrintArray(int[] a)                // печать построчно в столбик
        {
            foreach (int e in a)
            {
                Console.WriteLine(e);
            }
        }

        static void PrintArrayInLine(int[] a)          // печать в линию через пробел
        {
            foreach (int e in a)
            {
                Console.Write(e);                      // вывод элемента
                Console.Write(" ");                    // пробел между элементами
            }
            Console.WriteLine('\n');                   // перенос строки 
        }

        static void ArrayBoubleSort(int[] a)           // сортировка массива пузырьком от меньшего к большему
        {
            for (int i = 0; i < a.Length; ++i)
            {
                for (int j = i; j < a.Length; ++j)
                {

                    if (a[i] > a[j])
                    {
                        int tmp = a[i];
                        a[i] = a[j];
                        a[j] = tmp;
                    }
                }
            }
        }
    }
}
