using System.Collections.Generic;

namespace SkipUnsentProperties
{
    public class BaseDataTransferObject
    {
        /// <summary>
        /// Коллекция, содержащая названия свойств в которых была выполнена установка значения
        /// </summary>
        private HashSet<string> _propertiesWithSetValue = new HashSet<string>();
        
        /// <summary>
        /// Флаг, определяющий будет ли отслеживаться установка значений в свойствах
        /// </summary>
        public bool TrackingValueChange { get; set; } = true;

        /// <summary>
        /// Метод для установки значения в свойство
        /// Фиксирует факт установки значения в свойство для дальнейшей обработки
        /// </summary>
        /// <typeparam name="T">Тип свойства</typeparam>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="property">Держатель значения свойства</param>
        /// <param name="value">Значение свойства</param>
        protected void SetValue<T>(string propertyName, ref T property, T value)
        {
            if (TrackingValueChange)
                _propertiesWithSetValue.Add(propertyName);

            property = value;
        }

        /// <summary>
        /// Определить устанавливалось ли значение в свойство
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <returns></returns>
        public bool IsValueChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return false;
            }

            return _propertiesWithSetValue.Contains(propertyName);
        }
    }
}