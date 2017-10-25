using Domain.Infrastructure.Mapping;

namespace Data.Mappers
{
    public class MappersPull
    {
        private static MappersPull _instance;
        public static MappersPull Instance => _instance ?? (_instance = new MappersPull());
     
        private readonly Pull<object> _pull;

        private MappersPull()
        {
            _pull = new ObjectPullBuilder<object>()
//                .For(typeof(IMapper<SubscribeMessage, PersistenceSubscription>)).Use(typeof(SubscribeMessageToPersistenceSubscription))
                .Build();
        }

        public TR Map<TM, TR>(TM objecToMapp)
        {
            var mapper = (IMapper<TM, TR>) _pull.GetObject(typeof(IMapper<TM, TR>));
            return mapper.Map(objecToMapp);
        }
    }
}