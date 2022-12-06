using System;
using System.Collections.Generic;

namespace homeWork_1._3._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player mage = new Mage(300, 2, 20, 10, 0, 0);
            mage.PrintUnitType();
            mage.PrintUnitStatus();

            Mage m = (Mage)mage;
            m.LearnNewSkill("Fireball").LearnNewSkill("IceLance").LearnNewSkill("Recovery");
            m.PrintMageSkills();

            m.Damage(35);
            mage.PrintUnitType();
            mage.PrintUnitStatus();

            mage.DrinkHealPotion();
            m.PrintUnitType();
            m.PrintUnitStatus();
        }

        public interface ICombat
        {
            void Run(int x, int y);                  // перемещениt на клетку по координатам x и y
            void Attack(int x, int y);               // нанести атаку по юниту на клетке с координатами х и у
            void Damage(int damage);                 // получить урон юнитом в размере damage (вводит в состояние боя)
            void Sleep();                            // пропустить ход
        }

        public abstract class Enemy
        {
            protected Enemy(int health, int speed, int power, int armor, int x, int y)
            {
                _unit_health = health;
                _move_speed = speed;
                _attack_power = power;
                _armor_ratio = armor;
                _coord_x = x; _coord_y = y;
            }

            protected int _unit_health;         // запас здоровья юнита
            protected int _move_speed;          // величина перемещения юнита
            protected int _attack_power;        // сила атаки юнита
            protected int _armor_ratio;         // величина брони

            protected int _coord_x;             // координата Х
            protected int _coord_y;             // координата У

            public abstract void PrintUnitStatus();          // печать в консоль статуса по количеству хитпоинтов и положение на доске
            public abstract void PrintUnitType();            // печать в консоль типа юнита
            public abstract void Regeneration();             // регенерация здоровья юнита в период неактивности, для упрощения для всех единица
        }

        public class Goblin : Enemy, ICombat
        {
            private string _unit_type = "Goblin";            // название типа юнита для печати
            private bool _InCombat = false;                  // флаг состояния "в бою" для регенерации
            public Goblin(int health, int speed, int power, int armor, int x, int y) : base(health, speed, power, armor, x, y) { }

            public override void PrintUnitStatus()
            {
                Console.WriteLine($"Unit position is - {{ {_coord_x}, {_coord_y} }}, Unit health is - {_unit_health}");
            }
            public override void PrintUnitType()
            {
                Console.WriteLine($"Unit type is - \"{_unit_type}\"");
            }
            public override void Regeneration()
            {
                if (!_InCombat)
                {
                    ++_unit_health;   // регенимся только вне комбата, на данном этапе не заморачиваемся максимальной величиной здоровья
                }
            }

            public void Run(int x, int y)
            {
                // для простоты текущей реализации будем просто прибавлять координаты, так конечно надо писать норм логику поля и перемещения в разные стороны)))
                if (_coord_x != x || _coord_y != y)
                {
                    _coord_x += _move_speed; _coord_y += _move_speed;
                }

            }
            public void Attack(int x, int y)
            {
                // тут по идее должно быть обращение к классу отвечающему за игровое поле, которое даст нам объект для аттаки или передаст ему урон
                // GameDeck.SetDamage(x, y, _attack_power);  <- эдакий псевдокод
            }
            public void Damage(int damage)
            {
                if (!_InCombat) { _InCombat = true; }       // входим в бой при получении урона
                _unit_health -= (damage - _armor_ratio);   // получаем урон уменьшенный от брони
            }
            public void Sleep()
            {
                if (!_InCombat) Regeneration();            // если не в бою, то просто регенерировать здоровье
            }

        }

        public class Skeleton : Enemy, ICombat
        {
            private string _unit_type = "Skeleton";          // название типа юнита для печати
            private bool _InCombat = false;                  // флаг состояния "в бою" для регенерации
            public Skeleton(int health, int speed, int power, int armor, int x, int y) : base(health, speed, power, armor, x, y) { }

            public void DrainLife(int x, int y)
            {
                // для забора жизни попросим у обработчика игровой доски передать часть хитпоинтов от юнита в указанной клетке скелету
                // GameDeck.DrianLife(x, y, _attack_power / 2); int value <- эдакий псевдокод, получаем значение в int
                // _unit_health += value; <- полученное значение прибаляем к здоровью скелета
            }

            public override void PrintUnitStatus()
            {
                Console.WriteLine($"Unit position is - {{ {_coord_x}, {_coord_y} }}, Unit health is - {_unit_health}");
            }
            public override void PrintUnitType()
            {
                Console.WriteLine($"Unit type is - \"{_unit_type}\"");
            }
            public override void Regeneration()
            {
                if (!_InCombat)
                {
                    ++_unit_health;   // регенимся только вне комбата, на данном этапе не заморачиваемся максимальной величиной здоровья
                }
            }

            public void Run(int x, int y)
            {
                // для простоты текущей реализации будем просто прибавлять координаты, так конечно надо писать норм логику поля и перемещения в разные стороны)))
                if (_coord_x != x || _coord_y != y)
                {
                    _coord_x += _move_speed; _coord_y += _move_speed;
                }
            }
            public void Attack(int x, int y)
            {
                // тут по идее должно быть обращение к классу отвечающему за игровое поле, которое даст нам объект для аттаки или передаст ему урон
                // GameDeck.SetDamage(x, y, _attack_power);  <- эдакий псевдокод
            }
            public void Damage(int damage)
            {
                if (!_InCombat) { _InCombat = true; }       // входим в бой при получении урона
                _unit_health -= (damage - _armor_ratio);   // получаем урон уменьшенный от брони
            }
            public void Sleep()
            {
                if (!_InCombat) Regeneration();            // если не в бою, то просто регенерировать здоровье
            }
        }

        public class Tarantula : Enemy
        {
            private string _unit_type = "Tarantula";         // название типа юнита для печати
            private bool _InCombat = false;                  // флаг состояния "в бою" для регенерации
            public Tarantula(int health, int speed, int power, int armor, int x, int y) : base(health, speed, power, armor, x, y) { }

            public void MakeWeb()
            {
                // для реализации паутине на клете обращаемся к обработчку игровой доски
                // GameDeck.SetWeb(_coord_x, _coord_y);  <- эдакий псевдокод
            }

            public override void PrintUnitStatus()
            {
                Console.WriteLine($"Unit position is - {{ {_coord_x}, {_coord_y} }}, Unit health is - {_unit_health}");
            }
            public override void PrintUnitType()
            {
                Console.WriteLine($"Unit type is - \"{_unit_type}\"");
            }
            public override void Regeneration()
            {
                if (!_InCombat)
                {
                    ++_unit_health;   // регенимся только вне комбата, на данном этапе не заморачиваемся максимальной величиной здоровья
                }
            }

            public void Run(int x, int y)
            {
                // для простоты текущей реализации будем просто прибавлять координаты, так конечно надо писать норм логику поля и перемещения в разные стороны)))
                if (_coord_x != x || _coord_y != y)
                {
                    _coord_x += _move_speed; _coord_y += _move_speed;
                }
            }
            public void Attack(int x, int y)
            {
                // тут по идее должно быть обращение к классу отвечающему за игровое поле, которое даст нам объект для аттаки или передаст ему урон
                // GameDeck.SetDamage(x, y, _attack_power);  <- эдакий псевдокод
            }
            public void Damage(int damage)
            {
                if (!_InCombat) { _InCombat = true; }       // входим в бой при получении урона
                _unit_health -= (damage - _armor_ratio);   // получаем урон уменьшенный от брони
            }
            public void Sleep()
            {
                if (!_InCombat) Regeneration();            // если не в бою, то просто регенерировать здоровье
            }
        }

        public abstract class Player
        {
            protected Player(int health, int speed, int armor, int x, int y)
            {
                _unit_health = health;
                _move_speed = speed;
                _armor_ratio = armor;
                _coord_x = x; _coord_y = y;
            }

            protected int _unit_health;         // запас здоровья юнита
            protected int _move_speed;          // величина перемещения юнита
            protected int _armor_ratio;         // величина брони

            protected int _coord_x;             // координата Х
            protected int _coord_y;             // координата У

            protected int _health_potions;      // количество зелий восстановления здоровья
            protected int _mana_potions;        // количество зелий восстановления маны

            public abstract void PrintUnitStatus();          // печать в консоль статуса по количеству хитпоинтов и положение на доске
            public abstract void PrintUnitType();            // печать в консоль типа юнита
            public abstract void DrinkHealPotion();          // выпить зелье восстановления здоровья
        }

        public class Warrior : Player, ICombat
        {
            private int _attack_power;                        // сила атаки юнита
            private string _unit_type = "Warrior";            // название типа юнита для печати
            public Warrior(int health, int speed, int power, int armor, int x, int y) : base(health, speed, armor, x, y)
            {
                _attack_power = power;
                _health_potions = 100;     // даем воину больше зелий здоровья так как он на пердовой)
                _mana_potions = 0;         // восполнять ману воину не надо
            }

            public void FuryCharge(int x, int y)
            {
                // для дикого рывка на указанную клетку запрашиваем возможно у доски возможность совершить прыжок
                // if (GameDeck.ChargeMove(x, y)); bool value <- эдакий псевдокод, получаем значение в bool
                // _coord_x = x; _coord_y = y; 
            }

            public override void PrintUnitStatus()
            {
                Console.WriteLine($"Unit position is - {{ {_coord_x}, {_coord_y} }}, Unit health is - {_unit_health}");
            }
            public override void PrintUnitType()
            {
                Console.WriteLine($"Unit type is - \"{_unit_type}\"");
            }
            public override void DrinkHealPotion()
            {
                if (_health_potions != 0)
                {
                    _unit_health += 25;
                    --_health_potions;
                }
            }

            public void Run(int x, int y)
            {
                // для простоты текущей реализации будем просто прибавлять координаты, так конечно надо писать норм логику поля и перемещения в разные стороны)))
                if (_coord_x != x || _coord_y != y)
                {
                    _coord_x += _move_speed; _coord_y += _move_speed;
                }

            }
            public void Attack(int x, int y)
            {
                // тут по идее должно быть обращение к классу отвечающему за игровое поле, которое даст нам объект для аттаки или передаст ему урон
                // GameDeck.SetDamage(x, y, _attack_power);  <- эдакий псевдокод
            }
            public void Damage(int damage)
            {
                _unit_health -= (damage - _armor_ratio);   // получаем урон уменьшенный от брони
            }
            public void Sleep()
            {
                // ничего не делаем
            }
        }

        public class Rogue : Player, ICombat
        {
            private int _range_power;                       // сила атаки дальнего боя
            private string _unit_type = "Rogue";            // название типа юнита для печати
            private bool _IsStelth = false;                 // состояние персонажа в режиме невидимости
            public Rogue(int health, int speed, int power, int armor, int x, int y) : base(health, speed, armor, x, y)
            {
                _range_power = power;
                _health_potions = 75;      // даем разбойнику меньше зелий так как он на расстоянии
                _mana_potions = 0;         // восполнять ману разбойнику не надо
            }

            public void Banish()
            {
                _IsStelth = true;  // безусловно входим в состояние невидимости, разбойник - читер =^_^=
            }

            public override void PrintUnitStatus()
            {
                Console.WriteLine($"Unit position is - {{ {_coord_x}, {_coord_y} }}, Unit health is - {_unit_health}");
            }
            public override void PrintUnitType()
            {
                Console.WriteLine($"Unit type is - \"{_unit_type}\"");
            }
            public override void DrinkHealPotion()
            {
                if (_health_potions != 0)
                {
                    _unit_health += 25;
                    --_health_potions;
                }
            }

            public void Run(int x, int y)
            {
                // для простоты текущей реализации будем просто прибавлять координаты, так конечно надо писать норм логику поля и перемещения в разные стороны)))
                if (_coord_x != x || _coord_y != y)
                {
                    _coord_x += _move_speed; _coord_y += _move_speed;
                }

            }
            public void Attack(int x, int y)
            {
                // тут по идее должно быть обращение к классу отвечающему за игровое поле, которое даст нам объект для аттаки или передаст ему урон
                // GameDeck.SetDamage(x, y, _attack_power);  <- эдакий псевдокод
            }
            public void Damage(int damage)
            {
                _unit_health -= (damage - _armor_ratio);   // получаем урон уменьшенный от брони
            }
            public void Sleep()
            {
                // ничего не делаем
            }
        }

        public class Mage : Player, ICombat
        {
            private int _mana_power;                        // сила иагической атаки
            private int _unit_mana;                         // количество очков маны персонажа
            private string _unit_type = "Mage";             // название типа юнита для печати
            private List<string> _skills = new List<string>();            // лист умений мага
            public Mage(int health, int speed, int power, int armor, int x, int y) : base(health, speed, armor, x, y)
            {
                _mana_power = power;
                _unit_mana = 200;
                _health_potions = 50;
                _mana_potions = 50;
            }

            public void MaximaInferno()
            {
                // если у нас полные запасы маны и есть соответствующий скил
                if (_unit_mana == 200 && _skills.Contains("MaximaInferno"))
                {
                    // для заклинания уничтожения всех врагов на карте запрашиваем это у оператора игрового поля
                    // GameDeck.KillAllEnemy(); 
                }
            }
            public Mage LearnNewSkill(string skill)
            {
                // если скилла нет в списке, то можем выучить
                if (!_skills.Contains(skill))
                {
                    _skills.Add(skill);
                }
                return this;
            }
            public void PrintMageSkills()
            {
                Console.Write("Unit skills - ");
                foreach (string skill in _skills)
                {
                    Console.Write(skill + " ");
                }
                Console.Write("\n");
            }
            public void DrinkManaPotion()
            {
                if (_mana_potions != 0)
                {
                    _unit_mana += 25;
                    --_mana_potions;
                }
            }

            public override void PrintUnitStatus()
            {
                Console.WriteLine($"Unit position is - {{ {_coord_x}, {_coord_y} }}, Unit health is - {_unit_health}");
            }
            public override void PrintUnitType()
            {
                Console.WriteLine($"Unit type is - \"{_unit_type}\"");
            }
            public override void DrinkHealPotion()
            {
                if (_health_potions != 0)
                {
                    _unit_health += 25;
                    --_health_potions;
                }
            }

            public void Run(int x, int y)
            {
                // для простоты текущей реализации будем просто прибавлять координаты, так конечно надо писать норм логику поля и перемещения в разные стороны)))
                if (_coord_x != x || _coord_y != y)
                {
                    _coord_x += _move_speed; _coord_y += _move_speed;
                }

            }
            public void Attack(int x, int y)
            {
                // тут по идее должно быть обращение к классу отвечающему за игровое поле, которое даст нам объект для аттаки или передаст ему урон
                // GameDeck.SetDamage(x, y, _attack_power);  <- эдакий псевдокод
            }
            public void Damage(int damage)
            {
                _unit_health -= (damage - _armor_ratio);   // получаем урон уменьшенный от брони
            }
            public void Sleep()
            {
                // ничего не делаем
            }

        }
    }
}