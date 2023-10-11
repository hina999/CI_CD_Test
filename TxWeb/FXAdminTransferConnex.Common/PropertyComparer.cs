//-----------------------------------------------------------------------
// <copyright file="PropertyComparer.cs" company="Premiere Digital Services">
//     Copyright Premiere Digital Services. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;

namespace FXAdminTransferConnex.Common
{
    /// <summary>
    /// Class PropertyComparer.
    /// </summary>
    /// <typeparam name="T">T entity</typeparam>
    public class PropertyComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// The property to compare
        /// </summary>
        private readonly PropertyInfo propertyToCompare;

        /// <summary>
        /// The property to compare1
        /// </summary>
        private readonly PropertyInfo propertyToCompare1;

        /// <summary>
        /// The property to compare2
        /// </summary>
        private readonly PropertyInfo propertyToCompare2;

        /// <summary>
        /// The property to compare3
        /// </summary>
        private readonly PropertyInfo propertyToCompare3;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyComparer{T}"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyName1">The property name1.</param>
        /// <param name="propertyName2">The property name2.</param>
        /// <param name="propertyName3">The property name3.</param>
        public PropertyComparer(string propertyName, string propertyName1 = null, string propertyName2 = null, string propertyName3 = null)
        {
            this.propertyToCompare = typeof(T).GetProperty(propertyName);
            if (!string.IsNullOrEmpty(propertyName1))
            {
                this.propertyToCompare1 = typeof(T).GetProperty(propertyName1);
            }

            if (!string.IsNullOrEmpty(propertyName1))
            {
                this.propertyToCompare2 = typeof(T).GetProperty(propertyName2);
            }

            if (!string.IsNullOrEmpty(propertyName1))
            {
                this.propertyToCompare3 = typeof(T).GetProperty(propertyName3);
            }
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        public bool Equals(T x, T y)
        {
            object xvalue1 = new object();
            object xvalue2 = new object();
            object xvalue3 = new object();
            object yvalue1 = new object();
            object yvalue2 = new object();
            object yvalue3 = new object();

            object xvalue = this.propertyToCompare.GetValue(x, null);
            object yvalue = this.propertyToCompare.GetValue(y, null);

            if (this.propertyToCompare3 != null)
            {
                xvalue1 = this.propertyToCompare1.GetValue(x, null);
                yvalue1 = this.propertyToCompare1.GetValue(y, null);
            }

            if (this.propertyToCompare2 != null)
            {
                xvalue2 = this.propertyToCompare2.GetValue(x, null);
                yvalue2 = this.propertyToCompare2.GetValue(y, null);
            }

            if (this.propertyToCompare3 != null)
            {
                xvalue3 = this.propertyToCompare3.GetValue(x, null);
                yvalue3 = this.propertyToCompare3.GetValue(y, null);
            }

            return xvalue.Equals(yvalue) && xvalue1.Equals(yvalue1) && xvalue2.Equals(yvalue2) && xvalue3.Equals(yvalue3);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(T obj)
        {
            object objValue = this.propertyToCompare.GetValue(obj, null);
            return objValue.GetHashCode();
        }
    }
}
