using AutoMapper;
using System;
using System.Reflection;

namespace SkipUnsentProperties
{
    public class Program
    {
        /// <summary>
        /// Конфигурация AutoMapper-а
        /// Используется методом GetSourceMember() для нахождения свойства источника (SourceMember) по свойству назначения (DestinationMember)
        /// </summary>
        public static MapperConfiguration MapperConfiguration { get; set; } = new MapperConfiguration(m => Configurate(m));

        public static void Main(string[] args)
        {
            Mapper mapper = new Mapper(MapperConfiguration);

            BusinessObject businessObject = new BusinessObject();
            businessObject.One = "Sample one";
            businessObject.Two = "Sample two";

            DataTransferObject dto = System.Text.Json.JsonSerializer.Deserialize<DataTransferObject>("{ \"Two\": \"Hello world!\" }");
            // BusinessObjectDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObjectDTO>("{ \"Two\": \"Hello world!\" }");

            mapper.Map(dto, businessObject);

            Console.WriteLine($"{nameof(businessObject.One)} = {businessObject.One ?? "null"}");
            Console.WriteLine($"{nameof(businessObject.Two)} = {businessObject.Two ?? "null"}");
            Console.ReadKey();
        }

        public static void Configurate(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DataTransferObject, BusinessObject>()
                // .ForMember(destination => destination.Two, option => option.Ignore())
                // .ForMember(destination => destination.One, option => option.MapFrom(c => c.Two))

                .ForAllMembers(option => option.Condition((source, destination, sourceMemberValue) => IsValueSetCondition(source, destination, sourceMemberValue, option.DestinationMember)));
        }

        /// <summary>
        /// Метод, определяющий устанавливали ли значение свойства
        /// </summary>
        /// <typeparam name="TSource">Тип источника</typeparam>
        /// <typeparam name="TDestination">Тип назначения</typeparam>
        /// <param name="source">Объект-источник</param>
        /// <param name="destination">Объект назначения</param>
        /// <param name="sourceMemberValue">Значение свойства источника (SourceMember)</param>
        /// <param name="destinationMember">Свойство назначения (DestinationMember)</param>
        /// <returns>true - значение свойства установили, false - значение свойства не установили</returns>
        public static bool IsValueSetCondition<TSource, TDestination>(TSource source, TDestination destination, object sourceMemberValue, MemberInfo destinationMember) where TSource : BaseDataTransferObject
        {
            // Необходимо определять устанавливалось ли значения в свойство в DTO-классе не по свойству назначения (DestinationMember),
            // а по свойству источника (SourceMember) для этого взываем метод GetSourcePropertyInfo(...)
            var propertyInfo = MapperConfiguration.GetSourcePropertyInfo<TSource, TDestination>(destinationMember);

            // Если мы не нашли свойство из которого будет происходить маппинг,
            // необходимо вернуть true, иначе значение не обновится, даже если они не null т.к. source.IsValueSet(null) - всегда false
            // Такое может происходить, например, когда к полю применяется Resolver
            // (в этом случае обработка значения null лежит на разработчике)
            if (propertyInfo?.SourceMember == null)
                return true;
            else
                // Обращаемся к объекту DTO для того, чтобы узнать, устанавливали ли значение свойства
                return source.IsValueChanged(propertyInfo.SourceMember.Name);
        }
    }
}