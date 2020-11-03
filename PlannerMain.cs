using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07
{
    class PlannerMain
    {
        /// <summary>
        /// "Пользовательский" метод содержит имя файла для чтения и хранения записей
        /// </summary>
        public static void Main()
        {
            /// Разработать ежедневник.
            /// В ежедневнике реализовать возможность 
            /// - создания +
            /// - удаления +
            /// - редактирования + 
            /// записей
            /// 
            /// В отдельной записи должно быть не менее пяти полей
            /// 
            /// Реализовать возможность 
            /// - Загрузки данных из файла +
            /// - Выгрузки данных в файл +
            /// - Добавления данных в текущий ежедневник из выбранного файла
            /// - Импорт записей по выбранному диапазону дат
            /// - Упорядочивания записей ежедневника по выбранному полю

            string path = @"Planner.txt";
            _ = new Planner(path);
            Console.ReadKey();
        }
    }
}
