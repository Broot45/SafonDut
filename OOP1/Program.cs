using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{

    class Create // Общий класс существа
    {
        /* Общий родительский класс существ, который имеет универсальные поля, конструктор, а так-же универсальный метод прибавки возраста
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         *  
         *  Тест
         */






        public string Name { get; set; } // Имя существа
        public string Race { get; set; } // Рассовая принадлежность
        public int LVL { get; set; } // Полный уровень существа
        public int Exp {  get; set; } // Опыт существа
        public int Age { get; set; } // Возраст существа
        public int AgeDays { get; set; } // Число дней со дня рождения
        public bool Corr {  get; set; } // Факт искажённого состояния существа
        public bool Stasys { get; set; } // Факт стазиса (бонусы возраста не начисляются)



        public Create(string Name, string Race) : this(Name, Race, false) { }


        public Create(string Name, string Race, bool Corr) // Конструктор класса второго порядка
        {
            this.Name = Name;
            this.Race = Race;
            this.LVL = 0;
            this.Exp = 0;
            this.Age = 0;
            this.AgeDays = 0;
            this.Corr = Corr;
            this.Stasys = false;

            Console.WriteLine($"Было рождено {(Corr ? "искажённое" : "незапятнанное")} существо рассы {this.Race} носящее имя {this.Name}");
        }



        public void DayP(int Day) // Для реализащии возможности перемотки времени возраст принимает на вход количество пройденных дней
            // При реализации високосного года можно просто не прибавлять возраст существам в дополнительные дни
        {
            AgeDays += Day;

            if (AgeDays >= 364) 
            {
                AgeDays -= 364;

                if (!Stasys) {  } // Получение бонуса за возраст
            }
        }

       


    }

    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
