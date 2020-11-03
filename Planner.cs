using System;
using System.IO;
using System.Text;

namespace Homework_07
{
    /// <summary>
    /// Структура содержит основные исполняемые методы программы: меню - Menu(), чтение из файла - Load(), печать в консоль - Print();
    /// </summary>
    public struct Planner
    {
        #region Поля
        
        public static string path;
        public static DailyString[] daystring;
        public static string[] text;
        public static int index;
        public static int input;

        #endregion

        #region Конструктор
        
        public Planner(string Path)
        {
            path = Path;
            daystring = new DailyString[1000];
            text = new string[5];
            index = Load();
            input = Menu();

            Menu();
        }

        #endregion

        #region Методы
        
        /// <summary>
        /// Основной метод, который вызывает метод Load() и Print(), а также методы для работы с записями, в зависимости от ввода пользователя
        /// </summary>
        /// <returns>Возвращает 0</returns>
        public static int Menu()
        {
            Console.Clear();
            Load();
            Print();
            Console.WriteLine("\n\nМЕНЮ");
            Console.WriteLine();
            Console.WriteLine("1 - добавить запись;      5 - сортировать список по длительности;");
            Console.WriteLine("2 - удалить записи;       6 - сортировать список по дате; ");
            Console.WriteLine("3 - редактировать запись; 7 - добавить записи из нового файла;");
            Console.WriteLine("4 - прочитать инструкцию; 8 - импортировать записи в новый файл;");
            Console.Write("\nВвод: ");
            
            try
            {
                input = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                PlannerMain.Main();
                throw;
            }

            switch (input)
            {
                case 1:

                    EditPlanner.Add();
                    break;

                case 2:
                    
                    EditPlanner.Remove();
                    break;

                case 3:

                    EditPlanner.Replace();
                    break;

                case 4:

                    EditPlanner.Instruction();
                    break;

                case 5:

                    EditPlanner.SortBy();
                    break;
                
                case 6:

                    EditPlanner.SortBy();
                    break;

                case 7:

                    EditPlanner.AddFromFile();
                    break;
                
                case 8:

                    EditPlanner.ImportToFile();
                    break;

                default:
                    break;
            }

            PlannerMain.Main();
            return 0;
            
        }
        /// <summary>
        /// Метод построчно читает файл из переменной path, а затем загружает информацию в массив структур daystring[]
        /// </summary>
        /// <returns>Возвращается индекс последнего элемента массива daystring[]</returns>
        public static int Load()
        {
            try
            {
                StreamReader sr = new StreamReader(path, Encoding.Unicode);
                int index = 0;
                while (!sr.EndOfStream)
                {
                    text = sr.ReadLine().Split(',');
                    daystring[index] = new DailyString(Convert.ToDateTime(text[0]),
                                                                          text[1],
                                                                          Convert.ToUInt32(text[2]),
                                                                          text[3],
                                                                          text[4]);
                    index++;
                }
                sr.Close();
                return index;
            }
            catch (Exception)
            {
                File.Create(path).Close();
                Load();
                return 0;
            } 
            
        }

        /// <summary>
        /// Метод печатает информацию из массива структур daystring[]
        /// </summary>
        public static void Print()
        {
            string Number = "№";
            string Date = "Дата[1]";
            string Time = "Время[2]";
            string Event = "Событие[3]";
            string Duration = "Длит.[4]";
            string Place = "Место[5]";
            string Note = "Примечание[6]";
            Console.Clear();
            Console.WriteLine($"{Number,1}{Date,10}{Time,12}{Event,11}{Duration,17}{Place,9}{Note,24}");

            for (int i = 0; i < index; i++)
            {
                Console.SetCursorPosition(0, i + 1);  Console.Write($"{i+1}");
                Console.SetCursorPosition(4, i + 1);  Console.Write(daystring[i].Date.ToShortDateString());
                Console.SetCursorPosition(16, i + 1); Console.Write(daystring[i].Date.ToShortTimeString());
                Console.SetCursorPosition(24, i + 1); Console.Write(daystring[i].Event.Length<19?
                                                      daystring[i].Event:daystring[i].Event.Remove(18));
                Console.SetCursorPosition(45, i + 1); Console.Write(daystring[i].Duration);
                Console.SetCursorPosition(52, i + 1); Console.Write(daystring[i].Place.Length<19?
                                                      daystring[i].Place:daystring[i].Place.Remove(18));
                Console.SetCursorPosition(71, i + 1); Console.Write(daystring[i].Note.Length<25?
                                                      daystring[i].Note:daystring[i].Note.Remove(25));
            }
        }

        #endregion
    }
}
