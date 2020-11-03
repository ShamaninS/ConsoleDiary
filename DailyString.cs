using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07
{
    /// <summary>
    /// Структура определяет состав одной записи ежедневника и наследует свойства от интерфейса IComparable для выполнения сравнения
    /// </summary>
    public struct DailyString : IComparable<DailyString>
    {
        #region Поля

        public DateTime Date;
        public string Event;
        public uint Duration;
        public string Place;
        public string Note;

        #endregion

        #region конструкторы
        /// <summary>
        /// Общий конструктор
        /// </summary>
        /// <param name="Date">Дата события</param>
        /// <param name="Event">Название события</param>
        /// <param name="Duration">Продолжительность</param>
        /// <param name="Place">Место события</param>
        /// <param name="Note">Примечание</param>
        public DailyString(DateTime Date,string Event, uint Duration, string Place, string Note)
        {
            this.Date = Date;
            this.Event = Event;
            this.Duration = Duration;
            this.Place = Place;
            this.Note = Note;
        }

        /// <summary>
        /// Конструктор с предустановленными значениями для быстрого создания события
        /// </summary>
        /// <param name="Event">Название события</param>
        /// <param name="Place">Место события</param>
        /// <param name="Note">Примечание</param>
        public DailyString(string Event, string Place, string Note):
             this(DateTime.Now.AddHours(1), Event, 1, Place, Note)
        {

        }

        #endregion

        #region Методы
        /// <summary>
        /// Метод сравнивает значения массива структур по указанному параметру, используя интерфейс IComporable
        /// </summary>
        /// <param name="ds">Элемент массива для сравнения</param>
        /// <returns>-1,0 или 1 в зависимости от результата сравнения</returns>
        public int CompareTo (DailyString ds)
        {
            if (Planner.input == 6) return this.Date.CompareTo(ds.Date);
            else return this.Duration.CompareTo(ds.Duration);
        }

        #endregion
    }
}
