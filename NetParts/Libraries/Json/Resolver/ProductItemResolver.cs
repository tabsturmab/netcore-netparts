using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NetParts.Libraries.Json.Resolver
{
    public class ProductItemResolver<T> : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member,
            MemberSerialization
                memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            property.Ignored = false;
            property.ShouldSerialize = propInstance => property.DeclaringType != typeof(T);
            return property;
        }
    }
}
