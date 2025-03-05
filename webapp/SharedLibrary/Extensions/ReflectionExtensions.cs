﻿using K9.SharedLibrary.Attributes;
using K9.SharedLibrary.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace K9.SharedLibrary.Extensions
{
    public static class ReflectionExtensions
    {

        /// <summary>
        /// Copies all property values from one object to another
        /// </summary>
        /// <param name="objectToUpdate"></param>
        /// <param name="newObject"></param>
        public static void MapTo(this object newObject, object objectToUpdate)
        {
            foreach (var propInfo in objectToUpdate.GetType().GetProperties())
            {
                try
                {
                    objectToUpdate.SetProperty(propInfo, newObject.GetProperty(propInfo.Name));
                }
                catch (Exception)
                {
                }
            }
        }

        public static List<PropertyInfo> GetProperties(this Object item)
        {
            return item.GetType().GetProperties().ToList();
        }

        /// <summary>
        /// Return a list of properties which are decorated with the specified attribute
        /// </summary>
        /// <param name="item"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertiesWithAttribute(this Object item, Type attributeType)
        {
            return (from prop in item.GetType().GetProperties() let attributes = prop.GetCustomAttributes(attributeType, true) where attributes.Any() select prop).ToList();
        }

        public static Dictionary<PropertyInfo, T> GetPropertiesAndAttributesWithAttribute<T>(this IEnumerable<PropertyInfo> propertyInfos) 
            where T : Attribute
        {
            return propertyInfos
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T
                })
                .Where(x => x.Attribute != null)
                .ToDictionary(x => x.Property, x => x.Attribute);
        }


        public static Dictionary<PropertyInfo, T> GetPropertiesAndAttributesWithAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetProperties().GetPropertiesAndAttributesWithAttribute<T>();
        }

        public static Dictionary<PropertyInfo, T> GetPropertiesAndAttributesWithAttribute<T>(this Object item) where T : Attribute
        {
            return item.GetType().GetProperties().GetPropertiesAndAttributesWithAttribute<T>();
        }

        public static List<PropertyInfo> GetCollectionProperties(this Object item)
        {
            return item.GetType().GetProperties()
                .Where(p =>
                    (
                        p.PropertyType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(p.PropertyType)
                    )).ToList();
        }

        /// <summary>
        /// Return a list of properties which are decorated with the specified attribute
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="attributeTypes"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertiesWithAttributes(this List<PropertyInfo> properties, params Type[] attributeTypes)
        {
            var items = new List<PropertyInfo>();
            foreach (var attributeType in attributeTypes)
            {
                items.AddRange(from prop in properties let attributes = prop.GetCustomAttributes(attributeType, true) where attributes.Any() select prop);
            }
            return items.ToList();
        }

        public static object GetProperty(this object obj, PropertyInfo propertyInfo)
        {
            return propertyInfo.GetValue(obj, null);
        }

        public static object GetProperty(this object obj, string propertyName)
        {
            return obj.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, obj, new object[] { });
        }

        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetProperty(propertyName) != null;
        }

        public static bool HasAttribute(this Type type, Type attributeType)
        {
            return type.GetCustomAttribute(attributeType) != null;
        }

        public static void SetProperty(this object obj, string propertyName, object value)
        {
            var propInfo = obj.GetType().GetProperty(propertyName);
            SetProperty(obj, propInfo, value);
        }

        public static void SetProperty(this object obj, PropertyInfo propertyInfo, object value)
        {
            if (propertyInfo != null)
            {
                object formattedValue;

                // Check if the type is Nullable
                if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    // Get underlying type, e.g. "int"
                    formattedValue = value == null ? null : Convert.ChangeType(value, propertyInfo.PropertyType.GetGenericArguments()[0]);
                }
                else
                {
                    formattedValue = Convert.ChangeType(value, propertyInfo.PropertyType);
                }

                propertyInfo.SetValue(obj, formattedValue, null);
            }
        }

        public static bool IsPrimaryKey(this PropertyInfo info)
        {
            return info.GetCustomAttribute(typeof(KeyAttribute)) != null;
        }

        public static bool IsForeignKey(this PropertyInfo info)
        {
            return info.GetCustomAttribute(typeof(ForeignKeyAttribute)) != null;
        }

        public static bool IsVirtualCollection(this PropertyInfo info)
        {
            return info.GetGetMethod().IsVirtual && info.PropertyType.IsGenericType &&
                   info.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>);
        }

        public static bool IsVirtual(this PropertyInfo info)
        {
            var methodInfo = info.GetGetMethod();
            return methodInfo.IsVirtual && !methodInfo.IsFinal;
        }

        public static int GetStringLength(this PropertyInfo info)
        {
            var attr = info.GetCustomAttribute(typeof(StringLengthAttribute), false);
            if (attr != null)
            {
                return ((StringLengthAttribute)attr).MaximumLength;
            }

            return 0;
        }

        /// <summary>
        /// If the property has a DisplayName attribute, return the value of this, else return the property name
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetDisplayName(this PropertyInfo info)
        {
            var attr = info.GetCustomAttribute(typeof(DisplayAttribute), true);
            return attr == null ? info.Name : ((DisplayAttribute)attr).GetName();
        }

        public static bool IsDataBound(this PropertyInfo info)
        {
            return (info.GetCustomAttribute(typeof(NotMappedAttribute), false) == null || info.GetCustomAttribute(typeof(LinkedColumnAttribute), false) != null) && info.CanWrite;
        }

        public static Type GetLinkedPropertyType(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName).PropertyType;
        }

        public static string GetLinkedForeignTableName(this Type type, string foreignKeyColumn)
        {
            var property = type.GetProperty(foreignKeyColumn);
            if (property != null)
            {
                var attribute = property.GetCustomAttribute(typeof(ForeignKeyAttribute), true) as ForeignKeyAttribute;
                if (attribute == null)
                {
                    throw new Exception($"No ForeignKey attribute is set on property {foreignKeyColumn}");
                }
                return type.GetLinkedPropertyType(attribute.Name).Name;
            }
            throw new Exception($"Invalid property name {foreignKeyColumn}");
        }

        public static bool LimitedByUser(this Type type)
        {
            return type.GetCustomAttribute(typeof(LimitByUserIdAttribute), true) != null;
        }

        public static List<PropertyInfo> GetUserIdProperties(this Type type)
        {
            return type.GetProperties().Where(p => p.PropertyType == typeof(int) && p.Name.EndsWith("UserId")).ToList();
        }

        public static bool ImplementsIUserData(this Type type)
        {
            return typeof(IUserData).IsAssignableFrom(type);
        }

        public static bool FilterByParent(this Type type)
        {
            return type.GetCustomAttribute(typeof(LimitByUserIdAttribute), true) != null;
        }

        public static bool ImplementsIUserData(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetAttribute<UserDataAttribute>() != null;
        }

        public static string GetForeignKeyName(this Type type)
        {
            return $"{type.Name}Id";
        }

        public static T GetAttribute<T>(this Type type)
            where T : Attribute
        {
            return type.GetCustomAttribute(typeof(T), true) as T;
        }

        public static List<T> GetAttributes<T>(this Type type)
            where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), true).Cast<T>().ToList();
        }

        public static T GetAttribute<T>(this PropertyInfo propertyInfo)
            where T : Attribute
        {
            return propertyInfo.GetCustomAttribute(typeof(T), true) as T;
        }

        public static T GetAttribute<T>(this Enum value)
        where T : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<T>()
                .SingleOrDefault();
        }

    }
}
