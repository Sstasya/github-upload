using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace ConsoleApp3
{
    abstract class Set
    {
        protected int count_elem;

        public int count
        {
            get { return count_elem; }
        }

        public abstract void Add(int x);

        public abstract void Del(int x);

        public abstract bool Find(int x);

        public void Create(string s)
        {
            string[] str = s.Split(new char[] { ' ' });
            foreach (string t in str)
            {
                try
                {
                    Add(Convert.ToInt32(t));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //return;
                }
            }
        }

        public void Create(int[] array)
        {
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                try
                {
                    Add(array[i]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //return;
                }

            }
        }

        public void Create(int min, int max)
        {
            Random rand = new Random();
            int n = count_elem;
            for (int i = 0; i < n - 1; i++)
            {
                try
                {
                    Add(rand.Next(min, max));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //return;
                }

            }
        }

        public void Print()
        {
            for (int i = 1; i <= count_elem; i++)
            {
                if (Find(i))
                    Console.Write(i + " ");
            }
            Console.WriteLine();
        }
    }

    class SimpleSet : Set
    {
        protected bool[] elem;

        public SimpleSet(int max_num)
        {
            elem = new bool[max_num + 1];
            for (int i = 1; i <= max_num; i++)
                elem[i] = false;
            count_elem = max_num;
        }

        public override bool Find(int x)
        {
            if (x > count_elem || x <= 0)
            {
                throw new OversizeSetException("Выход за пределы множества");
                //return false;
            }
            return elem[x];
        }

        public override void Del(int x)
        {
            if (x > count_elem || x <= 0)
            {
                //Console.WriteLine("Выход за пределы множества");
                //return;
                throw new OversizeSetException("Выход за пределы множества");
            }
            if (elem[x])
                elem[x] = false;
            else Console.WriteLine("Элемента нет во множестве");
        }

        public override void Add(int x)
        {

            if (x > elem.Length || x <= 0)
            {
                throw new OversizeSetException("Выход за пределы множества");
            }

            if (elem[x])
            {
                Console.WriteLine("Элемент во множестве уже есть");
                return;
            }
            elem[x] = true;
        }

        public static SimpleSet operator +(SimpleSet x, SimpleSet y)
        {
            SimpleSet new_set = new SimpleSet(Math.Max(x.count,y.count));

            for (int i = 1; i <= x.count; i++)
            {
                if (x.Find(i))
                    new_set.Add(i);
            }

            for (int i = 1; i <= y.count; i++)
            {
                if (y.Find(i) && !new_set.Find(i))
                    new_set.Add(i);
            }

            return new_set;
        }

        public static SimpleSet operator *(SimpleSet x, SimpleSet y)
        {
            SimpleSet new_set = new SimpleSet(Math.Min(x.count, y.count));

            for (int i = 1; i <= new_set.count; i++)
            {
                if (x.Find(i) && y.Find(i))
                    new_set.Add(i);
            }

            return new_set;
        }
    }

    class BitSet : Set
    {
        int[] elem;

        public BitSet(int a)
        {
            elem = new int[a / 32 + 1];
            count_elem = a;
        }

        public override void Add(int x)
        {

            if (x > count_elem || x <= 0)
                throw new OversizeSetException("Выход за пределы множества");

            int bit = x / 32;

            if ((elem[bit] & (1 << x % 32)) != 0)
            {
                //Console.WriteLine("Элемент во множестве уже есть");
                throw new OversizeSetException("Выход за пределы множества");
                //return;
            }
            elem[bit] = elem[bit] | (1 << x % 32);
        }

        public override void Del(int x)
        {
            if (x > count_elem || x <= 0)
            {
                //Console.WriteLine("Выход за пределы множества");
                throw new OversizeSetException("Выход за пределы множества");
                //return;
            }
            int bit = x / 32;

            if ((elem[bit] & (1 << x % 32)) != 0)
                elem[bit] = elem[bit] & ~(1 << x % 32);
            else Console.WriteLine("Элемента нет во множестве");
        }

        public override bool Find(int x)
        {
            if (x > count_elem || x <= 0)
            {
                throw new OversizeSetException("Выход за пределы множества");
                //return false;
            }
            int bit = x / 32;

            if ((elem[bit] & (1 << x % 32)) != 0)
                return true;
            return false;
        }

        public static BitSet operator +(BitSet x, BitSet y)
        {
            BitSet new_set;

            if (x.count > y.count)
                new_set = new BitSet(x.count);
            else
                new_set = new BitSet(y.count);

            for (int i = 1; i <= x.count; i++)
                if (x.Find(i))
                    new_set.Add(i);

            for (int i = 1; i <= y.count; i++)
                if (y.Find(i) && !new_set.Find(i))
                    new_set.Add(i);
            return new_set;
        }

        public static BitSet operator *(BitSet x, BitSet y)
        {
            BitSet new_set;

            if (x.count > y.count)
                new_set = new BitSet(x.count);
            else new_set = new BitSet(y.count);

            for (int i = 1; i <= new_set.count; i++)
            {
                if (x.Find(i) && y.Find(i))
                    new_set.Add(i);
            }

            return new_set;
        }
    }

    class MultiSet : Set
    {
        protected int[] elem;

        public MultiSet(int max_num)
        {
            elem = new int[max_num + 1];
            for (int i = 1; i <= max_num; i++)
                elem[i] = 0;
            count_elem = max_num;
        }
        public override void Add(int x)
        {
            if (x > count_elem || x <= 0)
                throw new OversizeSetException("Выход за пределы множества");
            elem[x] += 1;
        }

        public override void Del(int x)
        {
            if (x > count_elem || x <= 0)
            {
                //Console.WriteLine("Выход за пределы множества");
                throw new OversizeSetException("Выход за пределы множества");
                //return;
            }

            if (elem[x] > 0)
                elem[x] -= 1;
            else Console.WriteLine("Элемента нет во множестве");
        }

        public override bool Find(int x)
        {
            if (x > count_elem || x <= 0)
            {
                throw new OversizeSetException("Выход за пределы множества");
                //return false;
            }

            if (elem[x] > 0)
                return true;
            return false;
        }
    }

    //[Serializable()]
    public class OversizeSetException : System.Exception
    {
        public OversizeSetException() { }
        public OversizeSetException(string message) : base(message) { }
        public OversizeSetException(string message, Exception inner) : base(message, inner) { }
        protected OversizeSetException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Выбирeте представление множества: 1 - битовое множество, 2 - логическое множество, 3 - мультимножество");
            int type_set = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите максимальное значение");
            int size = Convert.ToInt32(Console.ReadLine());
            Set set;
            switch (type_set)
            {
                case 1:
                    set = new BitSet(size);
                    break;
                case 2:
                    set = new SimpleSet(size);
                    break;
                case 3:
                    set = new MultiSet(size);
                    break;
                default:
                    Console.WriteLine("Ошибка");
                    return;
            }

            Console.WriteLine("Выбирите способ заполнения множества: 1 - из файла, 2 - из строки, 3 - заполнение случайными числами");
            int fill = Convert.ToInt32(Console.ReadLine());
            string s;
            switch (fill)
            {
                case 1:
                    string[] input = File.ReadAllLines("file.txt");
                    int[] array = new int[input.Length+1];
                    for (int i = 0; i < input.Length; i++)
                        array[i] = Convert.ToInt32(input[i]);
                    set.Create(array);
                    break;
                case 2:
                    Console.WriteLine("Введите числа");
                    s = Console.ReadLine();
                    set.Create(s);
                    break;
                case 3:
                    Console.WriteLine("Введите минимальное чиcло");
                    int min_n = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Введите максимальное чиcло");
                    int max_n = Convert.ToInt32(Console.ReadLine());
                    set.Create(min_n, max_n);
                    break;
                default:
                    Console.WriteLine("Ошибка");
                    return;
            }

            int act;
            bool flag = true;
            while (flag)
            {
                set.Print();
                Console.WriteLine("Выберите действие над множеством: 1 - добавить элемент, 2 - удалить элемент, 3 - проверить наличие элемента, 4 - выход");
                act = Convert.ToInt32(Console.ReadLine());
                switch (act)
                {
                    case 1:
                        Console.WriteLine("Введите элемент");
                        act = Convert.ToInt32(Console.ReadLine());
                        try
                        {
                            set.Add(act);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case 2:
                        Console.WriteLine("Введите элемент");
                        act = Convert.ToInt32(Console.ReadLine());
                        try
                        {
                            set.Del(act);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case 3:
                        Console.WriteLine("Введите элемент");
                        act = Convert.ToInt32(Console.ReadLine());
                        bool f;
                        try
                        {
                            f = set.Find(act);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            break;
                        }
                        if (f)
                            Console.WriteLine("Элемент есть в множестве");
                        else
                            Console.WriteLine("Элемента нет в множестве");
                        break;
                    case 4:
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Ошибка");
                        return;

                }
            }

            //while (true)
           // {
                Console.WriteLine("Операции над множествами: 1 - да, 2 - нет");
                s = Console.ReadLine();
                if (s == "1")
                {
                    Console.WriteLine("Введите максимальное значение первого множества");
                    int set1 = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Введите максимальное значение второго множества");
                    int set2 = Convert.ToInt32(Console.ReadLine());

                    SimpleSet SimpleSet_x = new SimpleSet(set1);
                    SimpleSet SimpleSet_y = new SimpleSet(set2);

                    BitSet BitSet_x = new BitSet(set1);
                    BitSet BitSet_y = new BitSet(set2);

                    Console.WriteLine("Введите множество X");
                    s = Console.ReadLine();

                    SimpleSet_x.Create(s);
                    BitSet_x.Create(s);

                    Console.WriteLine("Введите множество Y");
                    s = Console.ReadLine();

                    SimpleSet_y.Create(s);
                    BitSet_y.Create(s);

                    int set3;
                    if (set1 > set2)
                        set3 = set1;
                    set3 = set2;

                    SimpleSet SimpleSet_U = new SimpleSet(set3);
                    SimpleSet SimpleSet_I = new SimpleSet(set3);

                    BitSet BitSet_U = new BitSet(set3);
                    BitSet BitSet_I = new BitSet(set3);

                    SimpleSet_U = SimpleSet_x + SimpleSet_y;
                    SimpleSet_I = SimpleSet_x * SimpleSet_y;

                    BitSet_U = BitSet_x + BitSet_y;
                    BitSet_I = BitSet_x * BitSet_y;


                    Console.WriteLine("Объединение логических множеств");
                    SimpleSet_U.Print();

                    Console.WriteLine("Пересечение логических множеств");
                    SimpleSet_I.Print();


                    Console.WriteLine("Объединение битовых множеств");
                    BitSet_U.Print();

                    Console.WriteLine("Пересечение битовых множеств");
                    BitSet_I.Print();

                    Console.ReadKey();
                }
            //}
        }
    }
}
