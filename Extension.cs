using AutoMapper;
using AutoMapper.Internal;
using System.Linq;
using System.Reflection;

namespace SkipUnsentProperties
{
    public static class Extension
    {
        /// <summary>
        /// Метод для получения свойства источника (SourceMember)
        /// по свойству назначения (DestinationMember) для маппинга указанных типов
        /// </summary>
        /// <typeparam name="TSource">Тип источника</typeparam>
        /// <typeparam name="TDestination">Тип назначения</typeparam>
        /// <param name="mapperConfiguration">Конфигурация AutoMapper-а</param>
        /// <param name="destinationMember">Свойство назначения (DestinationMember)</param>
        /// <returns>Свойство источника (SourceMember)</returns>
        public static PropertyMap GetSourcePropertyInfo<TSource, TDestination>(this MapperConfiguration mapperConfiguration, MemberInfo destinationMember)
        {
            if (destinationMember != null
                && mapperConfiguration is IGlobalConfiguration globalConfiguration)
            {
                var map = globalConfiguration.FindTypeMapFor<TSource, TDestination>();
                return map?.PropertyMaps?.FirstOrDefault(pm => pm.DestinationMember.Equals(destinationMember));
            }

            return null;
        }
    }
}