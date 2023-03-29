using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SYS.BLL.Common
{
    /// <summary>
	/// Using Descriptor Pattern.
	/// </summary>
	/// <remarks></remarks>
	[DataContract]
    [Serializable]
    public class Descriptor<T> //: IEquatable<Descriptor<T>>
    {
        [DataMember]
        public T Value { get; private set; }

        public Descriptor()
        {
        }

        protected Descriptor(T value)
        {
            this.Value = value;
        }

        public static bool operator ==(Descriptor<T> object1, Descriptor<T> object2)
        {
            if (ReferenceEquals(null, object1)) { return false; }
            if (ReferenceEquals(null, object2)) { return false; }

            return object1.Equals(object2) && EqualityComparer<T>.Default.Equals(object1.Value, object2.Value);//) object1.Value == object2.Value;
        }

        public static bool operator ==(Descriptor<T> object1, T object2)
        {
            if (ReferenceEquals(null, object1)) { return false; }

            return EqualityComparer<T>.Default.Equals(object1.Value, object2);
        }

        public static bool operator ==(T object1, Descriptor<T> object2)
        {
            if (ReferenceEquals(null, object2)) { return false; }

            return EqualityComparer<T>.Default.Equals(object1, object2.Value);
        }

        public static bool operator !=(Descriptor<T> object1, Descriptor<T> object2)
        {
            if (ReferenceEquals(null, object1)) { return true; }
            if (ReferenceEquals(null, object2)) { return true; }

            return !object1.Equals(object2);
        }

        public static bool operator !=(Descriptor<T> object1, T object2)
        {
            if (ReferenceEquals(null, object1)) { return true; }

            return !EqualityComparer<T>.Default.Equals(object1.Value, object2);
        }

        public static bool operator !=(T object1, Descriptor<T> object2)
        {
            if (ReferenceEquals(null, object2)) { return true; }

            return !EqualityComparer<T>.Default.Equals(object1, object2.Value);
        }

        public override bool Equals(Object obj)
        {
            var objA = obj as Descriptor<T>;

            if (ReferenceEquals(null, objA)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }

            return false;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public bool Equals(Descriptor<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(other.Value, Value);
        }

        public override int GetHashCode()
        {
            return (ReferenceEquals(null, this.Value) ? 0 : Value.GetHashCode());
        }
    }
}
