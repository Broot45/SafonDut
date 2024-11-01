using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public Create(string Name, string Race) : this(Name, Race, 0, false, (new double[] { 50, 1.1, 1000, 1.2 })) { }
        public Create(string Name, string Race, int LVL) : this(Name, Race, LVL, false, (new double[] { 50, 1.1, 1000, 1.2 })) { } // Конструкторы класса
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
                              // Теперь этот фрагмент вручную набивает опыт до требуемого уровня
            {
                int templLVL = this.LVL;
                this.LVL = 0;

                while (templLVL > this.LVL)
                {
                    this.giveExp(this.ExpMax, false);
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


        public void giveExp(int Exp, bool Render = true) // ====== Получение опыта (если параметр рендер задан, и отрицателен - анимации получения опыта не будет)
        {
            this.Exp += Exp;

            while (this.Exp >= this.ExpMax)
            {
                this.Exp -= this.ExpMax;
                this.LVL++;
                this.ExpMax = (int)(this.ExpMax * this.Expinc); // Расчёт нового предела опыта
                this.DurMax = (int)(this.DurMax * this.Durinc); // Расчёт нового максимального HP 
                this.Dur = this.DurMax;

                if (Render) // Анимация получения опыта
                {
                    Console.WriteLine($"Существо {this.Name} получило {this.LVL} уровень!");
                }

                if (this.Stasys) // Возврат из стазиса   ==== Просто впадлу куда-либо ещё впихивать
                {
                    this.Stasys = false;
                    Console.WriteLine($"Повышение уровня сняло стазис существа {this.Name}!");
                }

            }
        }

        public int brokeEbalo(int hurt) // Функция получения существом по ебалу (отхил входит в комплект), возвращает оставшееся HP
        {
            this.Dur -= hurt;

            if (this.Dur > this.DurMax) // Защита от оверхила + воскрешалка
            {
                this.Dur = this.DurMax;

                if (this.Stasys) // Воскрешалка
                {
                    this.Stasys = false;
                    this.Dur = 10;
                    Console.WriteLine($"Получив оверхилл, {this.Name} воскрес, имея 10 ХП");
                }
            }
            if (this.Dur <= 0) // Убивалка
            {
                this.Dur = 0;

                if (!this.Stasys) // Чтобы не кричало на каждый пинок мертвеца
                {
                    Console.WriteLine($"{this.Name} от полученного урона попытался съёбаться {(this.Corr ? "в обьятия хаоса" : "на свет божий")}, но поскольку {(this.Corr ? "хаоса" : "бога")} здесь пока нет - он ушёл в стазис");
                    this.Stasys = true;
                }

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

        public Orc(string Name, string Breed, string Cast) : this(Name, Breed, Cast, 0) { } // Конструкторы класса, также как и в базовом, совершают цепочку вызовов
        public Orc(string Name, string Breed, string Cast, int LVL) : base(Name, "Orc", LVL, false, (new double[] { 65, 1.15, 2000, 1.05 })) // ===== Конструктор класса второго порядка, помимо своих действий, вызывает конструктор базового класса (наиболее полный)
        // balance = {65, 1.15, 2000, 1.05 } подразумевает, что орки имеют больше хп, быстрее жиреют, однако им гораздо труднее в начале, и проще в конце (много мяса, и крайне мало крайне сильных)
        // Орки не могут получить искажённое состояние
        {
            this.Breed = Breed; // Уникальные для дочернего класса задаваемые параметры
            this.Cast = Cast;


            Console.WriteLine($"Этот орк имеет титул {Cast}, и проживает в поселении {Breed}");
        }
    }

    class Fabric // Класс создатель
        // Экземпляр этого класса будет просто создавать существ различной рассы с заранее определённым уровнем, именем, искажением
    {
        public int Count { get; set; } // Порядковый номер существа
        public string NamePress { get; set; } // Пресет имени
        public int LMin { get; set; }
        public int LMax { get; set; } // Минимальный и максимальный диапазон уровней генерируемых существ
        public bool Corr { get; set; } // Факт искажённости существ
        public Random Randy { get; set; }

        public Fabric(int LMin, string NamePress = "[Empty]", bool Corr = false) : this(LMin, LMin, NamePress, Corr) { } // Без разброса уровней
        public Fabric (int LMin, int LMax, string NamePress = "[Empty]", bool Corr = false) // Конструктор
        {
            this.NamePress = NamePress;
            this.LMin = LMin;
            this.LMax = LMax;
            this.Corr = Corr;
            this.Count = 0;
            this.Randy = new Random();

            Console.WriteLine($"Шаблон для создания существ с именем {(this.NamePress == "[Empty]" ? "[Расса]" : this.NamePress)} #[Порядковый номер], успешно создан");
        }

        public Orc OrcGretch(string Breed) // Уровень гретчина не может превышать 5
        {
            this.Count++;

            int LVL = this.LMin; // Целевой уровень существа

            if ( this.LMin != this.LMax ) // Ы - оптимизация
            {
                LVL = Randy.Next(LMin, LMax + 1);
            }

            if (LVL > 5) { LVL = 5; }
            

            return new Orc($"{(this.NamePress == "[Empty]" ? "Гретчин" : this.NamePress)} #{this.Count}", Breed, "Гретчин", LVL);
        }

        public Orc OrcSnott(string Breed) // Уровень сноттлингов не может превышать 1
        {
            this.Count++;

            return new Orc($"{(this.NamePress == "[Empty]" ? "Сноттлинг" : this.NamePress)} #{this.Count}", Breed, "Сноттлинг", 1);
        }

        public Create Zoldaten() { return this.Zoldaten(new double[] { 50, 1.1, 1000, 1.2 }); }
        public Create Zoldaten(double[] balance) // (new double[] { MaxHP, HPLVLIncrement, XPtoLVLUp, XPNxtLVLIncrement })
        {
            this.Count++;

            int LVL = this.LMin; // Целевой уровень существа

            if (this.LMin != this.LMax) // Ы - оптимизация
            {
                LVL = Randy.Next(LMin, LMax + 1);
            }

            return new Create($"{(this.NamePress == "[Empty]" ? "Криговец" : this.NamePress)} #{this.Count}", "Человек", LVL, this.Corr, balance);
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

            Fabric Spore = new Fabric(4, "Член седьмого карательного отряда", true);

            Orc g = Spore.OrcGretch("Горк");
            Orc s = Spore.OrcSnott("Морк");

            g.brokeEbalo(100);
            g.log();
            s.brokeEbalo(45);
            s.log();

            Create Crig = Spore.Zoldaten(new double[] { 50, 5.5, 1000, 1.9 });

            Crig.log();


            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey(false); // false нужен, чтобы клавиша не отображалась в консоли перед закрытием
        }
    }
}
