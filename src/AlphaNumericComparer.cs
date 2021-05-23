namespace Roydl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Security;

    /// <summary>
    ///     Defines a method that performs the string transformation for the comparer.
    /// </summary>
    public interface IStringObjectComparer : IComparer
    {
        /// <summary>
        ///     Retrieves the string of the object that is used for comparison.
        /// </summary>
        /// <param name="value">
        ///     The object to compare.
        /// </param>
        string GetString(object value);
    }

    /// <inheritdoc cref="IStringObjectComparer"/>
    /// <typeparam name="T">
    ///     The type to be compared or which contains the string to be compared.
    /// </typeparam>
    public interface IStringObjectComparer<in T> : IStringObjectComparer, IComparer<T> { }

    /// <summary>
    ///     Provides a base class for alphanumeric comparison.
    /// </summary>
    /// <remarks>
    ///     Although this type can be serialized, but <see cref="ISerializable"/> is
    ///     missing as this led to conflicts in some cases where it was no longer
    ///     possible to use this class as <see cref="IComparer"/>.
    /// </remarks>
    [Serializable]
    public class AlphaNumericComparer : IStringObjectComparer
    {
        /// <summary>
        ///     Gets the value that determines whether the order is descended.
        /// </summary>
        protected bool Descended { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaNumericComparer"/> class.
        ///     A parameter specifies whether the order is descended.
        /// </summary>
        /// <param name="descended">
        ///     <see langword="true"/> to enable the descending order; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public AlphaNumericComparer(bool descended) =>
            Descended = descended;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaNumericComparer"/> class.
        /// </summary>
        public AlphaNumericComparer() : this(false) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaNumericComparer"/> class
        ///     with serialized data.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        protected AlphaNumericComparer(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            switch (context.State)
            {
                case StreamingContextStates.CrossProcess:
                case StreamingContextStates.CrossMachine:
                case StreamingContextStates.File:
                case StreamingContextStates.Persistence:
                case StreamingContextStates.Remoting:
                case StreamingContextStates.Other:
                case StreamingContextStates.Clone:
                case StreamingContextStates.CrossAppDomain:
                case StreamingContextStates.All:
                    Descended = info.GetBoolean(nameof(Descended));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(context));
            }
        }

        /// <inheritdoc/>
        public virtual int Compare(object x, object y)
        {
            var s1 = GetString(x);
            var s2 = GetString(y);
            return Compare(s1, s2);
        }

        /// <summary>
        ///     Sets the <see cref="SerializationInfo"/> object for this instance.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        [SecurityCritical]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(nameof(Descended), Descended);
        }

        /// <remarks>
        ///     The first thing to try is to use <paramref name="value"/> as a string,
        ///     which should work with all <see cref="IEnumerable"/>&lt;<see cref="char"/>
        ///     &gt; types. If <paramref name="value"/> is not a string type, however, a
        ///     check is made to see whether <paramref name="value"/> has a public string
        ///     field called <see langword="Text"/> or <see langword="Name"/> that can be
        ///     used for comparison. If the <see langword="Text"/> field is found, it will
        ///     be used, even if it contains an empty string.
        /// </remarks>
        /// <inheritdoc/>
        public virtual string GetString(object value)
        {
            if (value is IEnumerable<char> item)
                return item as string ?? new string(item.ToArray());
            try
            {
                var type = value?.GetType();
                if (type == null)
                    return null;
                foreach (var name in new[] { "Text", "Name" })
                {
                    var field = type.GetField(name, BindingFlags.Public | BindingFlags.GetField);
                    if (field?.GetValue(null) is string result)
                        return result;
                }
            }
            catch
            {
                // This is a fallback feature so we don't want an exception. 
            }
            return null;
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public new virtual bool Equals(object other)
        {
            if (other is not AlphaNumericComparer comparer)
                return false;
            return Descended != comparer.Descended;
        }

        /// <inheritdoc cref="Type.GetHashCode()"/>
        public new virtual int GetHashCode() =>
            GetType().GetHashCode();

        /// <summary>
        ///     Compare two specified strings and returns an integer that indicates their
        ///     relative position in the sort order.
        /// </summary>
        /// <param name="x">
        ///     The first string to compare.
        /// </param>
        /// <param name="y">
        ///     The second string to compare.
        /// </param>
        /// <inheritdoc cref="Compare(object, object)"/>
        protected int Compare(string x, string y)
        {
            var s1 = !Descended ? x : y;
            var s2 = !Descended ? y : x;
            if (s1 == null)
                return s2 == null ? 0 : -1;
            if (s2 == null)
                return 1;
            var i1 = 0;
            var i2 = 0;
            while (i1 < s1.Length && i2 < s2.Length)
            {
                var c1 = GetChunk(s1, ref i1);
                var c2 = GetChunk(s2, ref i2);
                int r;
                if (!char.IsDigit(c1[0]) || !char.IsDigit(c2[0]))
                    r = string.Compare(c1, c2, StringComparison.CurrentCulture);
                else
                {
                    var n1 = int.Parse(c1, CultureInfo.CurrentCulture);
                    var n2 = int.Parse(c2, CultureInfo.CurrentCulture);
                    r = n1.CompareTo(n2);
                }
                if (r != 0)
                    return r;
            }
            return s1.Length - s2.Length;
        }

        private static string GetChunk(string str, ref int i)
        {
            var pos = 0;
            var len = str.Length;
            var ca = new char[len];
            do ca[pos++] = str[i];
            while (++i < len && char.IsDigit(str[i]) == char.IsDigit(ca[0]));
            return new string(ca);
        }
    }

    /// <inheritdoc cref="AlphaNumericComparer"/>
    /// <inheritdoc cref="IStringObjectComparer{T}"/>
    [Serializable]
    public class AlphaNumericComparer<T> : AlphaNumericComparer, IStringObjectComparer<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaNumericComparer{T}"/>
        ///     class. A parameter specifies whether the order is descended.
        /// </summary>
        /// <inheritdoc cref="AlphaNumericComparer(bool)"/>
        public AlphaNumericComparer(bool descended) : base(descended) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaNumericComparer{T}"/>
        ///     class.
        /// </summary>
        public AlphaNumericComparer() : base(false) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaNumericComparer{T}"/>
        ///     class with serialized data.
        /// </summary>
        /// <inheritdoc cref="AlphaNumericComparer(SerializationInfo, StreamingContext)"/>
        protected AlphaNumericComparer(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <inheritdoc/>
        public int Compare(T x, T y) => base.Compare(x, y);

        /// <inheritdoc cref="AlphaNumericComparer.GetObjectData(SerializationInfo, StreamingContext)"/>
        [SecurityCritical]
        public new virtual void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);

        /// <inheritdoc cref="object.Equals(object)"/>
        public new virtual bool Equals(object other)
        {
            if (other is not AlphaNumericComparer<T> comparer)
                return false;
            return Descended != comparer.Descended;
        }

        /// <inheritdoc cref="Type.GetHashCode()"/>
        public new virtual int GetHashCode() =>
            HashCode.Combine(GetType(), typeof(T));
    }
}
