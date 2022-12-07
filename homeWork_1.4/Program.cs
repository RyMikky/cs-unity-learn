using System;
using System.Collections.Generic;
using RPG_Data;

namespace homeWork_1._4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Dictionary<string, Player> user_data = new Dictionary<string, Player>();
            user_data.Add("HyppperWarrior", new Warrior(200, 2, 20, 10, 0, 0));
            user_data.Add("UnstopableMage", new Mage(150, 1, 25, 2, 1, 3));
            user_data.Add("RogueVasiliysa", new Rogue(175, 4, 15, 5, 1, 3));

            PrintPlayerData(ref user_data);                                    // выводим в консоль данные о персонажах

            LinkedList<Enemy> enemy_data = new LinkedList<Enemy>();            // создаем список врагов
            MakeRandomEnemy(ref enemy_data, 10, 20);                           // заполняем список врагов
            PrintEnemyList(ref enemy_data);                                    // выводим в консоль список врагов

            DeadlyPotion(ref enemy_data, 2);                                   // запускаем отравление противников ядом
        }

        // отравление всех потивников ядом, на вход подается список врагов и урон от яда
        // выводит в консоль типы павших противнико и оставшегося наиболее живучего врага
        static void DeadlyPotion(ref LinkedList<Enemy> enemy_data, int damage)
        {
            List<Enemy> enemy_corpse = new List<Enemy>();                      // список для умерших врагов если окажется больше одного
            bool _NoCorpse = true;                                             // флаг того, что пока ни один противник не умер

            Enemy max_hp_enemy = enemy_data.First.Value;                       // противник с самым "жирным" хитпоинтом, по умолчанию первый в списке

            while (_NoCorpse)
            {
                foreach(Enemy enemy in enemy_data)
                {
                    if (enemy._unit_health > damage)
                    {
                        enemy._unit_health -= damage;                          // если здоровья больше чем урон от яда, просто вычитаем урон

                        if (enemy._unit_health > max_hp_enemy._unit_health)
                        {
                            max_hp_enemy = enemy;                              // если количество здоровья текущего элемента после вычитания больше
                        }

                    }
                    else
                    {
                        enemy._unit_health = 0;                                // если здоровья меньше или равно урону от яда - обнуляем hp
                        enemy_corpse.Add(enemy);                               // записываем в список павших врагов
                        _NoCorpse = false;                                     // обновляем флаг того, что появился труп
                    }
                }
            }

            foreach(Enemy enemy in enemy_corpse)
            {
                Console.WriteLine("Павший враг - " + enemy.GetUnitType());
            }

            Console.WriteLine();

            Console.WriteLine("Враг с наибольшим количество здоровья");
            Console.WriteLine(max_hp_enemy.GetUnitType() + " | Здоровье - " + max_hp_enemy._unit_health);
        }

        // выводит в печать таблицу игровых персонажей в формате ник | класс
        static void PrintPlayerData(ref Dictionary<string, Player> dict)
        {
            foreach(KeyValuePair<string, Player> pair in dict) 
            { 
                Console.WriteLine(pair.Key + " | " + pair.Value.GetUnitType());
            }

            Console.WriteLine();
        }

        // добавляет вражеских персонажей, принимает список врагов, количество какое требуется добавить и максимум здоровья
        static void MakeRandomEnemy(ref LinkedList<Enemy> enemy_data, int enemy_count, int max_hp)
        {
            Random rand = new Random();

            for(int i = 0; i != enemy_count; ++i) 
            {

                int type = i % 3;

                if (type == 0) 
                {
                    enemy_data.AddLast(new Goblin(rand.Next(1, max_hp), 2, 5, 1, rand.Next(0, 50), rand.Next(0, 50)));
                }
                else if (type == 1)
                {
                    enemy_data.AddLast(new Skeleton(rand.Next(1, max_hp), 2, 5, 3, rand.Next(0, 50), rand.Next(0, 50)));
                }
                else if (type == 2)
                {
                    enemy_data.AddLast(new Tarantula(rand.Next(1, max_hp), 1, 8, 5, rand.Next(0, 50), rand.Next(0, 50)));
                }
                else
                {
                    continue;
                }

            }
        }

        // печатает список врагов в консоль
        static void PrintEnemyList(ref LinkedList<Enemy> enemy_data)
        {
            foreach (Enemy enemy in enemy_data)
            {
                enemy.PrintUnitType();
            }
            Console.WriteLine();
        }

        // функция теста подгрузки данных из модуля RPG_Data
        static void RPG_Data_Test()
        {
            // По сути вывод того, что было в мейне ДЗ 1.3.2 

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
    }
}