﻿using System;
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
         *  Залупа
         *
         */






        public string Name { get; set; } // Имя существа
        public string Race { get; set; } // Рассовая принадлежность
        public int LVL { get; set; } // Полный уровень существа
        public int Exp {  get; set; } // Опыт существа
        public int ExpMax { get; set; } // Максимум опыта, при котором происходит повышение уровня
        public double LVLinc { get; set; } // Коэффициент, на который увеличивается количество опыта, необходимое для повышения опыта
        public int Age { get; set; } // Возраст существа
        public int AgeDays { get; set; } // Число дней со дня рождения
        public bool Corr {  get; set; } // Факт искажённого состояния существа
        public bool Stasys { get; set; } // Факт стазиса (бонусы возраста не начисляются)



        public Create(string Name, string Race) : this (Name, Race, 0, false) { }
        public Create(string Name, string Race, int LVL) : this(Name, Race, LVL, false) { } // Конструкторы класса


        public Create(string Name, string Race, int LVL, bool Corr) // ===== Конструктор класса второго порядка
        {
            this.Name = Name;
            this.Race = Race;
            this.LVL = LVL;
            this.Exp = 0;
            this.ExpMax = 1000;
            this.LVLinc = 1.2; // Стандартный коэффициент прогрессии опыта
            this.Age = 0;
            this.AgeDays = 0;
            this.Corr = Corr;
            this.Stasys = false;


            Console.WriteLine($"Было рождено {(Corr ? "искажённое" : "незапятнанное")} существо рассы {this.Race} носящее имя {this.Name}"); // Типа лог

            if (this.LVL > 0) // Расчёт максимального опыта для текущего уровня (выполняется только при спавне)
            {
                int templLVL = 0;
                while (templLVL < this.LVL)
                {
                    templLVL++;
                    this.ExpMax = this.ExpMax * this.LVLinc;
                }

            }

            Console.WriteLine($"Его уровень: {this.LVL}"); // Типа лог 2
        }


        public void DayP(int Day) // ===== Для реализащии возможности перемотки времени возраст принимает на вход количество пройденных дней
        // При реализации високосного года можно просто не прибавлять возраст существам в дополнительные дни
        // Годы так-же прибавляются по одному, чтобы при резкой перемотке не было кнб пиздеца, превышеный возраст останется в днях, и каждый день будет перетекать в года и рассовые бонусы
        {
            this.AgeDays += Day;

            if (AgeDays >= 364) 
            {
                AgeDays -= 364;

                if (!Stasys) 
                {
                    this.Exp += Day;
                    Console.WriteLine(    
                } // Получение бонуса за возраст
            }

        }

        public void giveExp(int Exp) // ====== Получение опыта
        { 
            this.Exp += Exp;

            while (this.Exp >= this.ExpMax) 
            {
                this.Exp -= this.ExpMax;
                this.LVL++;
                this.ExpMax = this.ExpMax * this.LVLinc
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
