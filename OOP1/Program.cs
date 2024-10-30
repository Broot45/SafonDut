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
         *  Залупа
         *
         */

        public string Name { get; set; } // Имя существа
        public int Dur { get; set; } // HP 
        public int DurMax { get; set; } // Максимальное HP
        public double Durinc { get; set; } // Прогрессия дюра от уровня
        public string Race { get; set; } // Рассовая принадлежность
        public int LVL { get; set; } // Полный уровень существа
        public int Exp { get; set; } // Опыт существа
        public int ExpMax { get; set; } // Максимум опыта, при котором происходит повышение уровня
        public double Expinc { get; set; } // Коэффициент, на который увеличивается количество опыта, необходимое для повышения опыта
        public int Age { get; set; } // Возраст существа
        public int AgeDays { get; set; } // Число дней со дня рождения
        public bool Corr { get; set; } // Факт искажённого состояния существа
        public bool Stasys { get; set; } // Факт стазиса (бонусы возраста не начисляются)

        public Create(string Name, string Race) : this(Name, Race, 0, false, (new double[] { 50, 1.1, 1000, 1.2})) { }
        public Create(string Name, string Race, int LVL) : this(Name, Race, LVL, false, (new double[] { 50, 1.1, 1000, 1.2})) { } // Конструкторы класса
        public Create(string Name, string Race, int LVL, bool Corr) : this(Name, Race, LVL, Corr, (new double[] { 50, 1.1, 1000, 1.2 })) { }
        public Create(string Name, string Race, int LVL, bool Corr, double[] balance) // ===== Конструктор класса второго порядка
        {
            // double[] balance  = {DurMax, Durinc, Expmax, Expinc} То есть для стандартного существа это - (new double[] { 50, 1.1, 1000, 1.2})
            this.Name = Name;
            this.Dur = (int)balance[0];
            this.DurMax = (int)balance[0]; 
            this.Durinc = balance[1];
            this.Race = Race;
            this.LVL = LVL;
            this.Exp = 0;
            this.ExpMax = (int)balance[2];
            this.Expinc = balance[3]; // Стандартный коэффициент прогрессии опыта
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
                    this.ExpMax = (int)(this.ExpMax * this.Expinc);
                }
            }
            Console.WriteLine($"Его уровень: {this.LVL} \n"); // Типа лог 2
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
                    Console.WriteLine($"{this.Name} пережил ещё один год, прибавка к опыту получена");
                    this.giveExp(1000);
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
                this.ExpMax = (int)(this.ExpMax * this.Expinc); // Расчёт нового предела опыта
                Console.WriteLine($"Существо {this.Name} получило {this.LVL} уровень!");
                this.DurMax = (int)(this.DurMax * this.Durinc); // РАсчёт нового максимального HP 
                this.Dur = this.DurMax;
            }
        }

        public int brokeEbalo(int hurt) // Функция получения существом по ебалу (отхил входит в комплект), возвращает оставшееся HP
        {
            this.Dur -= hurt;

            if (this.Dur > this.DurMax) { this.Dur = this.DurMax; } // Защита от оверхила
            if (this.Dur <= 0) 
            {
                this.Dur = 0;
                this.Stasys = true;
                Console.WriteLine($"{this.Name} от полученного урона попытался съёбаться {(this.Corr ? "в обьятия хаоса" : "на свет божий")}, но поскольку {(this.Corr ? "хаоса" : "бога")} здесь пока нет - он ушёл в стазис");
            }

            return this.Dur;
        }

        public void log()
        {
            Console.WriteLine("Имя: " + this.Name);
            Console.WriteLine($"Опыт: {this.Exp}/{this.ExpMax}");
            Console.WriteLine("Уровень: " + this.LVL);
            Console.WriteLine($"Durability: {this.Dur}/{this.DurMax}\n");
        }
    }

    class Orc : Create
    {
        public string Breed { get; set; } // Племя орка
        public string Cast { get; set; } // Племенная каста, привязана к работе

        public Orc(string Name, string Breed, string Cast) : this(Name, Breed, Cast, 0, false) { }
        public Orc(string Name, string Breed, string Cast, int LVL) : this(Name, Breed, Cast, LVL, false) { } // Конструкторы класса, также как и в базовом, совершают цепочку вызовов
        public Orc(string Name, string Breed, string Cast, int LVL, bool Corr) : base(Name, "Orc", 0, false, (new double[] {65, 1.15, 2000, 1.05 })) // ===== Конструктор класса второго порядка, помимо своих действий, вызывает конструктор базового класса (наиболее полный)
            // balance = {65, 1.15, 2000, 1.05 } подразумевает, что орки имеют больше хп, быстрее жиреют, однако им гораздо труднее в начале, и проще в конце (много мяса, и крайне мало крайне сильных)
        {
            this.Breed = Breed; // Уникальные для дочернего класса задаваемые параметры
            this.Cast = Cast;


            Console.WriteLine($"Этот орк имеет титул {Cast}, и проживает в поселении {Breed}");
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world");

            Create[] Cr = new Create[4];

            Cr[0] = new Create("Sample", "Crete");
            Cr[1] = new Create("John", "Orc");
            Cr[2] = new Create("John, but better", "Oni (chan)", 3);
            Cr[3] = new Create("John from onlyfans", "Ogr", 27, true);

            for (int i = 0; i < Cr.Length; i++) 
            {
                Cr[i].log();
            }

            Orc O1 = new Orc("Банан", "Умпалумпы", "Жёздъ");




            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey(false); // false нужен, чтобы клавиша не отображалась в консоли перед закрытием
        }
    }
}
