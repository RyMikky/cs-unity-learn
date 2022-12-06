using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace homeWork_1._3._1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Puzzle puz = new Puzzle("SmartPUZZLE",
                "Самый умный пазл из всех умных пазлов, аще!", 2002, "запредельная", 18);
            puz.Message();

            RPG rpg = new RPG("Dibla",
                "Классическо-динамическая, олдово-ньюфажная экшон-RPG", 2011,
                "можно грабить корованы и стать главой Сунтаурия", 12);
            rpg.Message();

            Shooter sho = new Shooter("DackHanter",
                "Подстрели как можно больше уток мясистых уток!", 1992,
                "стебущаяся над твоими промахами псина присутствует", 6);
            sho.AddDependency("Световой пистолет");
            sho.AddDependency("Пузатый ламповый ЭЛТ-телек");
            sho.Message();

            MOBA moba = new MOBA("BOTA-фигота",
                "Чёрти что для чёрти кого, с магазином и скинчиками", 2017, 6);
            moba.Message();

            Strategy strata = new Strategy("CraftStar",
                "Стань во главе одной из трех рас и зохвати весь сектор Купралу", 1998,
                "есть редактор карт", 12);
            strata.Message();
        }

        public class Puzzle
        {
            private string _Name;            // Название продукта
            private string _Description;     // Описание продукта
            private int _Year;               // Год выпуска продукта
            private string _Difficulty;      // Сложность продукта
            private int _RARS;               // Возрастная классификация информационной продукции (Russian Age Rating System, RARS)

            public Puzzle(string name, int year, string difficulty, int rARS)
            {
                _Name = name;
                _Year = year;
                _Difficulty = difficulty;
                _RARS = rARS;
            }

            public Puzzle(string name, string description, int year, string difficulty, int rARS)
                : this(name, year, difficulty, rARS)
            {
                _Description = description;
            }

            public void Message()
            {
                Console.WriteLine($"Игра-пазл: \"{_Name}\", выпущенная в {_Year}");
                Console.WriteLine(_Description);
                Console.WriteLine($"Сложность игры: \"{_Difficulty}\", возрастное ограничение {_RARS}+ \n");
            }
        }

        public class RPG
        {
            private string _Name;            // Название продукта
            private string _Description;     // Описание продукта
            private int _Year;               // Год выпуска продукта
            private string _Feature;         // Особенность продукта
            private int _RARS;               // Возрастная классификация информационной продукции (Russian Age Rating System, RARS)

            public RPG(string name, string description, int year, string feature, int rARS)
            {
                _Name = name;
                _Description = description;
                _Year = year;
                _Feature = feature;
                _RARS = rARS;
            }

            public void Message()
            {
                Console.WriteLine($"Ролевая игра: \"{_Name}\", выпущенная в {_Year}");
                Console.WriteLine(_Description);
                Console.WriteLine($"Особенность игры: \"{_Feature}\"");
                Console.WriteLine($"Возрастное ограничение {_RARS}+ \n");
            }
        }

        public class Shooter
        {
            private string _Name;               // Название продукта
            private string _Description;        // Описание продукта
            private int _Year;                  // Год выпуска продукта
            private string _Feature;            // Особенность продукта
            private int _RARS;                  // Возрастная классификация

            private List<string> _Dependency = new List<string>();   // Зависимости продукта

            public Shooter(string name, string description, int year, string feature, int rARS)
            {
                _Name = name;
                _Description = description;
                _Year = year;
                _Feature = feature;
                _RARS = rARS;
            }

            public void AddDependency(string dependency)
            {
                if (!_Dependency.Contains(dependency))
                {
                    _Dependency.Add(dependency);
                }
            }

            public void Message()
            {
                Console.WriteLine($"Стрелялка: \"{_Name}\", выпущенная в {_Year}");
                Console.WriteLine(_Description);

                if (_Dependency.Count() > 0)
                {
                    Console.Write("Требования к игре: ");
                    bool IsFirst = true;
                    foreach (string dependency in _Dependency)
                    {
                        if (IsFirst)
                        {
                            Console.Write(dependency);
                            IsFirst = false;
                        }
                        else
                        {
                            Console.Write(", " + dependency);
                        }
                    }
                    Console.Write("\n");
                }

                Console.WriteLine($"Особенность игры: \"{_Feature}\"");
                Console.WriteLine($"Возрастное ограничение {_RARS}+ \n");
            }
        }

        public class MOBA
        {
            private string _Name;            // Название продукта
            private string _Description;     // Описание продукта
            private int _Year;               // Год выпуска продукта
            private int _RARS;               // Возрастная классификация информационной продукции (Russian Age Rating System, RARS)

            private string[] _TeamSize = new string[] { "3 на 3", "5 на 5" };   // размеры команд

            public MOBA(string name, string description, int year, int rARS)
            {
                _Name = name;
                _Description = description;
                _Year = year;
                _RARS = rARS;
            }

            public void Message()
            {
                Console.WriteLine($"Очередная МОВА: \"{_Name}\", выпущенная в {_Year}");
                Console.WriteLine(_Description);
                Console.WriteLine($"Размеры команд: \"{_TeamSize[0]}\" и \"{_TeamSize[1]}\"");
                Console.WriteLine($"Возрастное ограничение {_RARS}+ \n");
            }
        }

        public class Strategy
        {
            private string _Name;            // Название продукта
            private string _Description;     // Описание продукта
            private int _Year;               // Год выпуска продукта
            private string _Feature;         // Особенность продукта
            private int _RARS;               // Возрастная классификация информационной продукции (Russian Age Rating System, RARS)

            private string[] _GameType = new string[] { "Синглплеер", "Мультиплеер" };                  // режимы игры
            private string[] _Races = new string[] { "Человечки", "Космодервиши", "Насикомые" };       // играбельные расы

            public Strategy(string name, string description, int year, string feature, int rARS)
            {
                _Name = name;
                _Description = description;
                _Feature = feature;
                _Year = year;
                _RARS = rARS;
            }

            public void Message()
            {
                Console.WriteLine($"Стратегия: \"{_Name}\", выпущенная в {_Year}");
                Console.WriteLine(_Description);
                Console.WriteLine($"Основные режимы игры: \"{_GameType[0]}\" и \"{_GameType[1]}\"");
                Console.WriteLine($"Играбельные расы: \"{_Races[0]}\", \"{_Races[1]}\" и \"{_Races[2]}\"");
                Console.WriteLine($"Особенности игры: \"{_Feature}\"");
                Console.WriteLine($"Возрастное ограничение {_RARS}+ \n");
            }
        }
    }
}