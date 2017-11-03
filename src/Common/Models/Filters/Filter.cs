using System.IO;
using System.Linq.Expressions;
using Serialization;
using Serialization.Deserializer;
using Serialization.Serializer;
using Serialize.Linq.Extensions;
using Serialize.Linq.Serializers;

namespace Common.Models.Filters
{
    public abstract class Filter<T> : Message
    {
        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            var stream = new MemoryStream();
            new ExpressionSerializer(new BinarySerializer()).Serialize(stream, GetExpression());
            serializer.WriteByteArray(stream.ToArray());
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            var expression = new ExpressionSerializer(new BinarySerializer()).Deserialize(new MemoryStream(deserializer.ReadByteArray()));
            SetExpression(expression);
        }

        protected abstract Expression GetExpression();
        protected abstract void SetExpression(Expression expression);
        public abstract T[] Execute(T[] items);
    }
}