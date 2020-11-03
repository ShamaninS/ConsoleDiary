using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Homework_07
{
    public struct EditPlanner
    {
        public static int coin = 0;
        public static int trigger = 0;
        
        /// <summary>
        /// Метод добавляет записи введеные с консоли
        /// </summary>
        public static void Add()
        {
            Console.Clear();
            
            int lastindex = Planner.index+1;
            
            Console.WriteLine("МЕНЮ ДОБАВЛЕНИЯ НОВОЙ ЗАПИСИ (нажмите ESC для возврата в главное Меню)");
            Console.WriteLine("\nПример полной записи: 12.12.12 12:00,Встреча с Аленой,2,Аквамарин,Обсудить цветы");
            Console.WriteLine("Пример сокращенной записи: Встреча с Аленой,Аквамарин,Обсудить цветы");
            
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) PlannerMain.Main();
            Console.SetCursorPosition(29, 0);
            Console.Write($"{" ",42}");
            Console.SetCursorPosition(0, 5);

            Console.Write("Новое событие: ");
            string[] text = Console.ReadLine().Split(',');

            while (text.Length!=3 && text.Length!=5)
            {
                Console.Write("Неразборчиво написано! Давай еще разок, только теперь как на примере выше");
                Thread.Sleep(4000);
                Add();
            }
            
            try
            {
                switch (text.Length)
                {
                    case 3:
                        Planner.daystring[lastindex] = new DailyString(text[0], text[1], text[2]);
                        break;
                    case 5:
                        Planner.daystring[lastindex] = new DailyString(Convert.ToDateTime(text[0]),
                                                                                          text[1],
                                                                                          Convert.ToUInt32(text[2]),
                                                                                          text[3],
                                                                                          text[4]);
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Неразборчиво написано! Давай еще разок, только теперь как на примере выше");
                Thread.Sleep(4000);
                Add();
                try
                {
                    throw;
                }
                catch (Exception)
                {
                    PlannerMain.Main();
                    throw;
                }
            }

            SaveInFile(lastindex, Planner.path);
            
            Console.Write("Событие успешно добавлено!");
            Thread.Sleep(4000);
        }
        
        /// <summary>
        /// Метод удаляет записи
        /// </summary>
        public static void Remove()
        {
            Console.Clear();
            Planner.Print();

            Console.WriteLine("\n\nМЕНЮ УДАЛЕНИЯ ЗАПИСЕЙ (нажмите ESC для возврата в главное Меню)\n");
            
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) PlannerMain.Main();
            Console.SetCursorPosition(21, Planner.index+2);
            Console.Write($"{" ",42}");
            Console.SetCursorPosition(0, Planner.index + 4);
            
            Console.Write("Введите номер события или интервал для удаления(например: 4 или 1-5): ");
            string[] index = Console.ReadLine().Split('-');

            try
            {
                uint i1 = Convert.ToUInt32(index[0]);
                
                if (i1 == 0) i1 = 1 / i1;
                if (i1 > Planner.index) i1 = 1 / (i1 - i1);
                
                uint i2;

                if (index.Length==1) i2 = i1;
                else i2 = Convert.ToUInt32(index[1]);

                if (i1 > i2) i1 = 1 / (i1 - i1);

                File.Delete(Planner.path);

                for (uint i = 0; i < Planner.index; i++)
                {
                    if (i == i1 - 1)
                    {
                        i = i2;
                    }
                    if (i == Planner.index) break;
                    
                    SaveInFile((int)i, Planner.path);
                }
                
                if (i1 != i2) Console.WriteLine("События успешно удалены!");
                else Console.WriteLine("Событие успешно удалено!");
                Thread.Sleep(4000);
            }
            catch (Exception)
            {
                Console.WriteLine("Неразборчиво написано! Давай еще разок, только теперь как на примере выше");
                Thread.Sleep(4000);
                Remove();
                try
                {
                    throw;
                }
                catch (Exception)
                {
                    PlannerMain.Main();
                    throw;
                }
            }
        }

        /// <summary>
        /// Метод редактирует существующие записи
        /// </summary>
        public static void Replace()
        {
            Console.Clear();
            Planner.Print();

            Console.WriteLine("\n\nМЕНЮ РЕДАКТИРОВАНИЯ ЗАПИСЕЙ (нажмите ESC для возврата в главное Меню)\n");
            
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) PlannerMain.Main();
            Console.SetCursorPosition(28, Planner.index + 2);
            Console.Write($"{" ",42}");
            Console.SetCursorPosition(0, Planner.index + 4);
            
            Console.Write($"Введите номер записи для изменения (1-{Planner.index}): ");
            uint index;
            try
            {
                index = Convert.ToUInt32(Console.ReadLine()) - 1;
                if (index >= Planner.index) throw new OverflowException();
            }
            catch (Exception)
            {
                Console.WriteLine("Неразборчиво написано! Давай еще разок, только теперь как на примере выше");
                Thread.Sleep(4000);
                Replace();
                try
                {
                    throw;
                }
                catch (Exception)
                {
                    PlannerMain.Main();
                    throw;
                }
            }

        interNumber:

            Console.Write("Введите номер столбца для изменения (1-6): ");

            uint number;
            try
            {
                number = Convert.ToUInt32(Console.ReadLine());
                if (number<1||number>6) throw new OverflowException();
            }
            catch (Exception)
            {
                Console.WriteLine("Неразборчиво написано! Давай еще разок, только теперь как на примере выше");
                Thread.Sleep(4000);
                
                Console.Clear();
                Planner.Print();

                Console.WriteLine("\n\nМЕНЮ РЕДАКТИРОВАНИЯ ЗАПИСЕЙ\n");
                Console.WriteLine($"Введите номер записи для изменения (1-{Planner.index}): {index+1}");
                goto interNumber;
            }
        
        internewData:
        
            Console.Write("Введите новые данные: ");

            string newData = Console.ReadLine();

            switch (number)
            {
                case 1:
                    DateTime date;
                    try
                    {
                        date = Convert.ToDateTime(newData).Add(Planner.daystring[index].Date.TimeOfDay);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Неразборчиво написано! Давай еще разок, только теперь как на примере выше");
                        Thread.Sleep(4000);

                        Console.Clear();
                        Planner.Print();

                        Console.WriteLine("\n\nМЕНЮ РЕДАКТИРОВАНИЯ ЗАПИСЕЙ\n");
                        Console.WriteLine($"Введите номер записи для изменения (1-{Planner.index}): {index + 1}");
                        Console.WriteLine($"Введите номер столбца для изменения (1-6): {number}");
                        goto internewData;
                    }
                    Planner.daystring[index].Date = date;
                    File.Delete(Planner.path);
                    for (uint i = 0; i < Planner.index; i++)
                    {
                        SaveInFile((int)i, Planner.path);
                    }
                    break;
                case 2:
                    try
                    {
                        date = Planner.daystring[index].Date.Date.Add(Convert.ToDateTime(newData).TimeOfDay);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Неразборчиво написано! Давай еще разок, только теперь как на примере выше");
                        Thread.Sleep(4000);

                        Console.Clear();
                        Planner.Print();

                        Console.WriteLine("\n\nМЕНЮ РЕДАКТИРОВАНИЯ ЗАПИСЕЙ\n");
                        Console.WriteLine($"Введите номер записи для изменения (1-{Planner.index}): {index + 1}");
                        Console.WriteLine($"Введите номер столбца для изменения (1-6): {number}");
                        goto internewData;
                    }
                    Planner.daystring[index].Date = date;
                    File.Delete(Planner.path);
                    for (uint i = 0; i < Planner.index; i++)
                    {
                        SaveInFile((int)i, Planner.path);
                    }
                    break;

                case 3:
                    Planner.daystring[index].Event = newData;
                    File.Delete(Planner.path);
                    for (uint i = 0; i < Planner.index; i++)
                    {
                        SaveInFile((int)i, Planner.path);
                    }
                    break;
                case 4:
                    try
                    {
                        Planner.daystring[index].Duration = Convert.ToUInt32(newData);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Неразборчиво написано! Давай еще разок, только теперь как на примере выше");
                        Thread.Sleep(4000);

                        Console.Clear();
                        Planner.Print();

                        Console.WriteLine("\n\nМЕНЮ РЕДАКТИРОВАНИЯ ЗАПИСЕЙ\n");
                        Console.WriteLine($"Введите номер записи для изменения (1-{Planner.index}): {index + 1}");
                        Console.WriteLine($"Введите номер столбца для изменения (1-6): {number}");
                        goto internewData;
                    }
                    File.Delete(Planner.path);
                    for (uint i = 0; i < Planner.index; i++)
                    {
                        SaveInFile((int)i, Planner.path);
                    }
                    break;
                case 5:
                    Planner.daystring[index].Place = newData;
                    File.Delete(Planner.path);
                    for (uint i = 0; i < Planner.index; i++)
                    {
                        SaveInFile((int)i, Planner.path);
                    }
                    break;
                case 6:
                    Planner.daystring[index].Note = newData;
                    File.Delete(Planner.path);
                    for (uint i = 0; i < Planner.index; i++)
                    {
                        SaveInFile((int)i, Planner.path);
                    }
                    break;
            }

        }

        /// <summary>
        /// Метод сортиурует записи
        /// </summary>
        public static void SortBy()
        {
            DailyString[] dailyStrings = new DailyString[Planner.index];
            Array.Copy(Planner.daystring, 0, dailyStrings, 0, Planner.index);
            
            if (coin == 0)
            {
                coin++;
                Array.Sort(dailyStrings);
            }
            else
            {
                coin--;
                Array.Sort(dailyStrings);
                Array.Reverse(dailyStrings);
            }
            Array.Copy(dailyStrings, 0, Planner.daystring, 0, Planner.index);
            
            File.Delete(Planner.path);

            for (int i = 0; i < Planner.index; i++)
            {
                SaveInFile(i, Planner.path);
            }
        }
        
        /// <summary>
        /// Метод сохраняет записи из массива структур daystring[] в файл
        /// </summary>
        /// <param name="index">Номер элемента массива структур daystring[]</param>
        /// <param name="path">Наименование файла для сохранения</param>
        public static void SaveInFile(int index, string path)
        {
            string output = Convert.ToString(Planner.daystring[index].Date) + "," +
                                             Planner.daystring[index].Event + "," +
                            Convert.ToString(Planner.daystring[index].Duration) + "," +
                                             Planner.daystring[index].Place + "," +
                                             Planner.daystring[index].Note;
            using (StreamWriter sw = new StreamWriter(path, true, Encoding.Unicode))
            sw.WriteLine(output);
        }

        /// <summary>
        /// Метод добавляет записи из указанного файла к текущим записям из ежедневника
        /// </summary>
        public static void AddFromFile() 
        {
            Console.Clear();
            Console.WriteLine("МЕНЮ ДОБАВЛЕНИЯ ЗАПИСЕЙ ИЗ ФАЙЛА (нажмите ESC для возврата в главное Меню)");
            
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) PlannerMain.Main();
            Console.SetCursorPosition(33, 0);
            Console.Write($"{" ",42}");
            Console.SetCursorPosition(0, 2);
            
            Console.Write("Введите адрес файла для экспорта записей в текущий ежедневник: ");
            string newpath = Console.ReadLine();
            //C:\Users\alien\Documents\Planner.txt
            try
            {
                StreamReader sr = new StreamReader(newpath, Encoding.Unicode);
                while (!sr.EndOfStream)
                {
                    string[] text = sr.ReadLine().Split(',');
                    Planner.daystring[Planner.index] = new DailyString(Convert.ToDateTime(text[0]),
                                                                          text[1],
                                                                          Convert.ToUInt32(text[2]),
                                                                          text[3],
                                                                          text[4]);
                    Planner.index++;
                }
                sr.Close();

                File.Delete(Planner.path);

                for (int i = 0; i < Planner.index; i++)
                {
                    SaveInFile(i, Planner.path);
                }
                Console.WriteLine("Записи успешно добавлены!");
                Thread.Sleep(4000);
            }
            catch (Exception)
            {
                Console.WriteLine("Файл не найден!");
                Thread.Sleep(4000);
                try
                {
                    throw;
                }
                catch (Exception)
                {
                    PlannerMain.Main();
                    throw;
                }
            }

        }

        /// <summary>
        /// Метод импортирует записи за указанный промежуток времени в указанный файл
        /// </summary>
        public static void ImportToFile() 
        {
            Console.Clear();
            Console.WriteLine("МЕНЮ ИМПОРТИРОВАНИЯ ЗАПИСЕЙ В ФАЙЛ (нажмите ESC для возврата в главное Меню)");
            
            if (Console.ReadKey(true).Key == ConsoleKey.Escape) PlannerMain.Main();
            Console.SetCursorPosition(35, 0);
            Console.Write($"{" ",42}");
            Console.SetCursorPosition(0, 2);
            
            Console.Write($"Введите диапазон дат для импорта: от \n{"дд.мм.гггг",47}");
            Console.SetCursorPosition(37, 2);
            string tempDate1 = Console.ReadLine();
            DateTime start;
            try
            {

                start = Convert.ToDateTime(tempDate1);
                Console.SetCursorPosition(37+tempDate1.Length, 2);
                Console.Write("(V)");
            }
            catch (Exception)
            {
                try
                {
                    Console.SetCursorPosition(37 + tempDate1.Length, 2);
                }
                catch (Exception)
                {
                    Console.WriteLine("-10 очков Гриффиндору! Теперь вводи все заново");
                    Thread.Sleep(4000);
                    ImportToFile();

                }
                Console.Write("(X)");
                Thread.Sleep(4000);
                ImportToFile();
                try
                {
                    throw;
                }
                catch (Exception)
                {
                    PlannerMain.Main();
                    throw;
                }
            }
            
            Console.SetCursorPosition(41 + tempDate1.Length, 2); Console.Write($"до ");

        InterSecondDate:

            Console.SetCursorPosition(44 + tempDate1.Length, 3); Console.Write("дд.мм.гггг");
            Console.SetCursorPosition(44 + tempDate1.Length, 2);
            string tempDate2 = Console.ReadLine();
            DateTime end;
            try
            {
                end = Convert.ToDateTime(tempDate2);
                Console.SetCursorPosition(44+tempDate2.Length+ tempDate1.Length, 2);
                Console.Write("(V)");
            }
            catch (Exception)
            {
                try 
                {
                    Console.SetCursorPosition(44 + tempDate2.Length + tempDate1.Length, 2);
                }
                catch (Exception) 
                {
                    Console.WriteLine("-10 очков Гриффиндору! Теперь вводи все заново");
                    Thread.Sleep(4000);
                    ImportToFile();
                    try
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        PlannerMain.Main();
                        throw;
                    }

                }
                Console.Write("(X)");
                Thread.Sleep(4000);
                Console.Clear();
                Console.WriteLine("МЕНЮ ИМПОРТИРОВАНИЯ ЗАПИСЕЙ В ФАЙЛ\n");
                Console.Write($"Введите диапазон дат для импорта: от {tempDate1}(V) до ");
                goto InterSecondDate;
            }
            var example = "C:\\Users\\Public\\Documents\\Calendar.txt";
        
        enterPath:

            Console.Write($"\nВведите путь к файлу для сохранения (например - {example}): ");
            string newpath = Console.ReadLine();

            try
            {
                for (int i = 0; i < Planner.index; i++)
                {
                    if (Planner.daystring[i].Date >= start && Planner.daystring[i].Date <= end.AddDays(1))
                    {
                        SaveInFile(i, newpath);
                    }
                }
                Console.WriteLine("Записи успешно импортированы!");
                Thread.Sleep(4000);

            }
            catch (Exception)
            {
                Console.WriteLine("Кажется указанный путь не подходит для записи. Попробуй еще раз!");
                Thread.Sleep(4000);
                Console.Clear();
                Console.Write($"Введите диапазон дат для импорта: от {tempDate1}(V) до {tempDate2}(V)");
                goto enterPath;
            }
        }

        /// <summary>
        /// Метод содержит инструкцию к ежедневнику
        /// </summary>
        public static void Instruction()
        {
            Console.Clear();
            Console.SetWindowSize(105, 56);

            Console.WriteLine("Добро пожаловать в инструкцию к ежедневнику!\n");
            
            Console.WriteLine("1 - Добавление записей из консоли:\n");
            Console.WriteLine("Чтобы добавить запись в ежедневник введите цифру 1 в Меню. Отобразится меню добавления записи." +
                "\nВы можете воспользоваться полной или сокращенной формой записи. \nПолная форма записи предполагает ввод всех полей " +
                "ежедневника вручную через запятую. \nПример ввода: 31.12.2019 16:00,Каток,2,Динамо,Заточить коньки " +
                "\nСокращенная форма записи поможет быстро записать события на сегодня. Вам необходимо ввести " +
                "только \nназвание события, место и примечание, а дата, время и длительность события установятся по умолчанию -" +
                "\nсегодняшнее число, текущее время + 1 час и длительность - 1 час. \nПример ввода: Каток,Динамо,Заточить коньки");
            Console.WriteLine("\nP.S. После запятой рекомендуется не делать отступов, так как после занесения записи отступы " +
                "проставятся \nавтоматически. Если Вы не хотите заполнять какое-либо текстовое поле, то просто пропустите его при вводе.Например, для " +
                "сокращенного варианта запись будет выглядеть так: Свидание,,");

            Console.WriteLine("\n2 - Удаление записей из ежедневника:\n");
            Console.WriteLine("Чтобы удалить запись из ежедневника введите цифру 2 в Меню. Отобразится меню удаления записей." +
                "\nЧтобы удалить одну запись - введите номер этой записи, а чтобы удалить интервал записей - введите \n2 числа через дефис " +
                "(например 1-5). По умолчанию записи не сортируются, поэтому прежде чем удалить \nинтервал событий, посмотрите" +
                " не требуется ли сначала их отсортировать.");

            Console.WriteLine("\n3 - Редактирование записей:\n");
            Console.WriteLine("Чтобы отредактировать запись введите цифру 3 в Меню. Отобразится меню редактирования записей." +
                "\nПри редактировании даты или времени соблюдайте формат ввода (для времени - чч:мм, для даты - дд.мм.гггг).");

            Console.WriteLine("\n4 - Прочитать инструкцию:\n");
            Console.WriteLine("Чтобы ознакомиться с инструкцией введите цифру 4 в Меню.");

            Console.WriteLine("\n5-6 - Сортировать список по дате или длительности:\n");
            Console.WriteLine("Чтобы отсортировать список по длительности введите цифру 5 в Меню. Для обратной сортировки еще раз \nвведите цифру 5." +
                "Чтобы отсортировать список по дате введите цифру 6 в Меню. Для обратной сортировки \nеще раз введите цифру 6.");

            Console.WriteLine("\n7 - Добавить записи из нового файла:\n");
            Console.WriteLine("Чтобы добавить записи из нового файла введите цифру 7 в Меню. Отобразится меню добавления записей." +
                "\nВы можете указать путь до файла целиком (например: C:\\Users\\alien\\Documents\\Spisok.txt) или " +
                "только \nнаименование файла (например: Spisok.txt). Во втором случае, файл должен находиться в папке с исполняемым файлом " +
                "Homework_07.exe. Новые записи будут добавлены после старых.");

            Console.WriteLine("\n8 - Импортировать записи в новый файл:\n");
            Console.Write("Чтобы импортировать записи в новый файл введите цифру 8 в Меню. Отобразится меню импортирования записей. " +
                "Введите временной интервал, за который вы хотите импортировать записи. Придерживайтесь формата даты - \nдд.мм.гггг. " +
                "Если вы хотите получить записи за один день, то в полях \"от\" и \"до\" укажите одну и ту же дату" +
                "Вы можете указать путь до файла целиком (например: C:\\Users\\alien\\Documents\\Spisok.txt) или " +
                "только \nнаименование файла (например: Spisok.txt). Во втором случае, файл будет сохранен в папке с исполняемым \nфайлом " +
                "Homework_07.exe.");

            Console.ReadKey();

        }
    }
}
