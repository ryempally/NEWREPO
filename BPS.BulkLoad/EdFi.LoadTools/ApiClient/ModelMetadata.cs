using System.Collections.Generic;
using System.Linq;
using EdFi.LoadTools.Engine;

namespace EdFi.LoadTools.ApiClient
{
    public class ModelMetadata : IModelMetadata
    {
        public string Model { get; set; }
        public string Property { get; set; }
        public string Type { get; set; }
        public bool IsArray { get; set; }
        public bool IsRequired { get; set; }
        public virtual bool IsSimpleType { get; set; }
        public virtual bool IsAttribute { get; set; }

        /// <summary>
        /// Type.property.property.property for nested properties...Used for matching
        /// </summary>
        public string PropertyPath { get; set; }
    }

    public class XmlModelMetadata : ModelMetadata
    {
        public override bool IsSimpleType => Constants.XmlAtomicTypes.Contains(Type);
    }

    public class JsonModelMetadata : ModelMetadata
    {
        public string Category { get; set; }
        public string Resource { get; set; }
        public string Description { get; set; }
        public override bool IsAttribute => false;
        public override bool IsSimpleType => Constants.JsonAtomicTypes.Contains(Type);
    }

    public class ModelMetadataEqualityComparer<T> :
        IEqualityComparer<T> where T : IModelMetadata
    {
        public bool Equals(T x, T y)
        {
            return x.Model == y.Model &&
                   x.Property == y.Property &&
                   x.Type == y.Type;
        }

        public int GetHashCode(T obj)
        {
            var text = $"{obj.Model},{obj.Property},{obj.Type}";
            return text.GetHashCode();
        }
    }
}