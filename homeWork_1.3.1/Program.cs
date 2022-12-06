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
                "Самый умный пазл из всех умных пазлов, аще!", 2002, 
                "запредельная", 18, true, @"http:://www.puzzel-ladder.ashe");
            puz.Message();

            RPG rpg = new RPG("Dibla",
                "Классическо-динамическая, олдово-ньюфажная экшон-RPG", 2011,
                "можно грабить корованы и стать главой Сунтаурия", 12, true);
            rpg.Message();

            Shooter sho = new Shooter("DackHanter",
                "Подстрели как можно больше уток мясистых уток!", 1992,
                "стебущаяся над твоими промахами псина присутствует", 6, ShooterType.RetroGame);
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

        internal enum ShooterType
        {
            FirstPerson, ThirdPerson, RetroGame
        }

        public class Puzzle
        {
            private string _Name;            // Название продукта
            private string _Description;     // Описание продукта
            private int _Year;               // Год выпуска продукта
            private string _Difficulty;      // Сложность продукта
            private int _RARS;               // Возрастная классификация информационной продукции (Russian Age Rating System, RARS)
            public bool _HasOnlineLadder;    // Есть ли у игры таблица онлайн рейтинга
            public string _webLadderURL;     // Адрес страницы в интернете с онлайн рейтингом

            public Puzzle(string name, int year, string difficulty, int rARS)
            {
                _Name = name;
                _Year = year;
                _Difficulty = difficulty;
                _RARS = rARS;
            }

            public Puzzle(string name, string description, int year, string difficulty, int rARS, bool ladder, string web)
                : this(name, year, difficulty, rARS)
            {
                _Description = description;
                _HasOnlineLadder = ladder;
                _webLadderURL = web;
            }

            public void Message()
            {
                Console.WriteLine($"Игра-пазл: \"{_Name}\", выпущенная в {_Year}");
                Console.WriteLine(_Description);
                Console.WriteLine($"Сложность игры: \"{_Difficulty}\", возрастное ограничение {_RARS}+");
                if (_HasOnlineLadder)
                {
                    Console.WriteLine($"Таблица лучших игроков расположенна по адресу: \"{_webLadderURL}\"\n");
                }
            }
        }

        public class RPG
        {
            private string _Name;            // Название продукта
            private string _Description;     // Описание продукта
            private int _Year;               // Год выпуска продукта
            private string _Feature;         // Особенность продукта
            private int _RARS;               // Возрастная классификация информационной продукции (Russian Age Rating System, RARS)
            public bool _IsActionRPG;        // Флаг ActionRPG

            public RPG(string name, string description, int year, string feature, int rARS, bool action)
            {
                _Name = name;
                _Description = description;
                _Year = year;
                _Feature = feature;
                _RARS = rARS;
                _IsActionRPG = action;
            }

            public void Message()
            {
                if (_IsActionRPG)
                {
                    Console.Write($"ActionRPG: \"{_Name}\"");
                }
                else
                {
                    Console.Write($"Ролевая игра: \"{_Name}\"");
                }

                Console.Write($", выпущенная в {_Year}\n");
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

            protected List<string> _Dependency = new List<string>();   // Зависимости продукта

            internal ShooterType _gameType;     // тип шутера, например находится в доп файле Commons.cs

            public Shooter(string name, string description, int year, string feature, int rARS, ShooterType type)
            {
                _Name = name;
                _Description = description;
                _Year = year;
                _Feature = feature;
                _RARS = rARS;
                _gameType = type;
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

            public int _HeroesCount;         // Количество героев
            public string[] _TeamSize = new string[] { "3 на 3", "5 на 5" };   // размеры команд

            public MOBA(string name, string description, int year, int rARS)
            {
                _Name = name;
                _Description = description;
                _Year = year;
                _RARS = rARS;
            }

            public MOBA(string name, string description, int year, int rARS, int heroes) 
                : this(name, description, year, rARS)
            {
                _HeroesCount = heroes;
            }

            public void Message()
            {
                Console.WriteLine($"Очередная МОВА: \"{_Name}\", выпущенная в {_Year}");
                Console.WriteLine(_Description);
                Console.WriteLine($"Размеры команд: \"{_TeamSize[0]}\" и \"{_TeamSize[1]}\"");
                Console.WriteLine($"Доступно: \"{_HeroesCount}\" уникальных персонажей");
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

            public string[] _GameType = new string[] { "Синглплеер", "Мультиплеер" };                  // режимы игры
            public string[] _Races = new string[] { "Человечки", "Космодервиши", "Насикомые" };        // играбельные расы

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