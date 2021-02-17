using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class ReflectionLibrary
    {
        internal const BindingFlags STATIC_MEMBER_BINDING_FLAGS =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        internal const BindingFlags INSTANCE_MEMBER_BINDING_FLAGS =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static string GetReflectionName(this Type type) =>
            $"{type.FullName}, {type.Assembly.FullName}";

        public static IEnumerable<Type> GetSubclasses(this Type type,
            bool allowAbstract = false, bool allowSelf = true)
            =>
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from t in assembly.GetTypes()
                where type.IsAssignableFrom(t)
                      && (allowAbstract || (!t.IsAbstract && !t.IsInterface))
                      && (allowSelf || t != type)
                select t;

        public static IEnumerable<Type> GetSuperClasses(this Type type,
            bool allowAbstract = false, bool allowSelf = true)
            =>
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from t in assembly.GetTypes()
                where t.IsAssignableFrom(type)
                      && t != typeof(object)
                      && (allowAbstract || (!t.IsAbstract && !t.IsInterface))
                      && (allowSelf || t != type)
                select t;
        
        public static IEnumerable<Type> GetHierarchy(this Type target, bool allowAbstract = true,
            bool includeSelf = true)
            =>
                target
                    .GetSuperClasses(allowAbstract, false)
                    .Concat(target
                        .GetSubclasses(allowAbstract, includeSelf));

        public static IEnumerable<Type> GetTypesWithThisAttribute(this Type attribute, bool allowAbstract = false)
            =>
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from t in assembly.GetTypes()
            where t.IsDefined(attribute, false)
                  && (allowAbstract || (!t.IsAbstract && !t.IsInterface))
            select t;

        internal static Type[] GetTypeArray(this object[] parameters)
        {
            var length = parameters.Length;
            var types = new Type[length];
            for (var i = 0; i < length; i++) types[i] = parameters[i].GetType();
            return types;
        }

        // Get static data
        public static bool Get<T>(this Type type, string info, out T data) =>
            type.GetData(info, out data) || type.Retrieve(info, out data);

        // Get static data (non-generic)
        public static bool Get(this Type type, string info, out object data) =>
            type.GetData(info, out data) || type.Retrieve(info, out data);

        // Get instance data
        public static bool Get<T>(this object o, string info, out T data) =>
            o.GetData(info, out data) || o.Retrieve(info, out data);

        // Get instance data (non-generic)
        public static bool Get(this object o, string info, out object data) =>
            o.GetData(info, out data) || o.Retrieve(info, out data);

        // Set instance data
        public static bool Set<T>(this Type type, string info, T data) =>
            type.SetData(info, data) || type.Assign(info, data);

        // Set instance data (non-generic)
        public static bool Set(this Type type, string info, object data) =>
            type.SetData(info, data) || type.Assign(info, data);

        // Set instance data
        public static bool Set<T>(this object o, string info, T data) =>
            o.SetData(info, data) || o.Assign(info, data);

        // Set instance data (non-generic)
        public static bool Set(this object o, string info, object data) =>
            o.SetData(info, data) || o.Assign(info, data);

        // Get attribute data
        public static TAttribute[] GetAttributeData<TAttribute>(this IReadOnlyList<MemberInfo> members)
            where TAttribute : Attribute
        {
            // return members.Select(x => x.GetCustomAttribute<TAttribute>());
            var count = members.Count;
            var attributeData = new TAttribute[count];
            for (var i = 0; i < count; i++)
            {
                attributeData[i] = members[i].GetCustomAttribute<TAttribute>();
            }

            return attributeData;
        }
    }

    public static class PropertyReflectionLibrary
    {
        // Get static property
        public static bool Retrieve<T>(this Type type, string info, out T data)
        {
            var property = type.GetProperty(info, ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS);

            var valid = property != null && property.CanRead && typeof(T).IsAssignableFrom(property.PropertyType);

            data = valid ? (T) property.GetValue(null) : default;

            return valid;
        }
        
        // Get static property (non-generic)
        public static bool Retrieve(this Type type, string info, out object data)
        {
            var property = type.GetProperty(info, ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS);

            var valid = property != null && property.CanRead;

            data = valid ? property.GetValue(null) : null;

            return valid;
        }

        // Get instance property
        public static bool Retrieve<T>(this object o, string info, out T data)
        {
            var property = o.GetType().GetProperty(info, ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS);

            var valid = property != null && property.CanRead && typeof(T).IsAssignableFrom(property.PropertyType);

            data = valid ? (T) property.GetValue(o) : default;

            return valid;
        }

        // Get instance property (non-generic)
        public static bool Retrieve(this object o, string info, out object data)
        {
            var property = o.GetType().GetProperty(info, ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS);

            var valid = property != null && property.CanRead;

            data = valid ? property.GetValue(o) : null;

            return valid;
        }

        // Set static property
        public static bool Assign<T>(this Type type, string info, T data)
        {
            var property = type.GetProperty(info, ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS);

            var valid = property != null && property.CanWrite && property.PropertyType.IsInstanceOfType(data);

            if (valid) property.SetValue(null, data);

            return valid;
        }
        
        // Set static property (non-generic)
        public static bool Assign(this Type type, string info, object data)
        {
            var property = type.GetProperty(info, ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS);

            var valid = property != null && property.CanWrite && property.PropertyType.IsInstanceOfType(data);

            if (valid) property.SetValue(null, data);

            return valid;
        }

        // Set instance property
        public static bool Assign<T>(this object o, string info, T data)
        {
            var property = o.GetType().GetProperty(info, ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS);

            var valid = property != null && property.CanWrite && property.PropertyType.IsInstanceOfType(data);

            if (valid) property.SetValue(o, data);

            return valid;
        }

        // Set instance property (non-generic)
        public static bool Assign(this object o, string info, object data)
        {
            var property = o.GetType().GetProperty(info, ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS);

            var valid = property != null && property.CanWrite && property.PropertyType.IsInstanceOfType(data);

            if (valid) property.SetValue(o, data);

            return valid;
        }

        // Get static properties with attribute
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this Type type)
            where T : Attribute =>
            type
                .GetPropertiesWithAttribute<T>(ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS);

        // Get instance properties with attribute
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this object o)
            where T : Attribute =>
            o
                .GetType()
                .GetPropertiesWithAttribute<T>(ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS);

        // Get properties with attribute
        private static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this IReflect t, BindingFlags flags)
            where T : Attribute =>
            t
                .GetProperties(flags)
                .Where(p => p.IsDefined(typeof(T)));
    }

    public static class MethodReflectionLibrary
    {
        // Execute static command
        public static bool SendMessage(this Type type, string message, params object[] parameters)
        {
            var methodInfo = type.GetMethod(message, ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS, null,
                parameters.GetTypeArray(), null);

            if (methodInfo == null) return false;

            methodInfo.Invoke(null, parameters);

            return true;
        }

        // Execute instance command
        public static bool SendMessage(this object o, string message, params object[] parameters)
        {
            var methodInfo = o.GetType().GetMethod(message, ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS, null,
                parameters.GetTypeArray(), null);

            if (methodInfo == null) return false;

            methodInfo.Invoke(o, parameters);

            return true;
        }

        // Query static method
        public static bool Query<T>(this Type type, string message, out T data, params object[] parameters)
        {
            var methodInfo = type.GetMethod(message, ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS, null,
                parameters.GetTypeArray(), null);

            var valid = methodInfo != null && methodInfo.ReturnType == typeof(T);

            data = valid ? (T) methodInfo.Invoke(null, parameters) : default;

            return valid;
        }

        // Query static method (non-generic)
        public static bool Query(this Type type, string message, out object data, params object[] parameters)
        {
            var methodInfo = type.GetMethod(message, ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS, null,
                parameters.GetTypeArray(), null);

            var valid = methodInfo != null;

            data = valid ? methodInfo.Invoke(null, parameters) : null;

            return valid;
        }

        // Query instance method
        public static bool Query<T>(this object o, string message, out T data, params object[] parameters)
        {
            var methodInfo = o.GetType().GetMethod(message, ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS, null,
                parameters.GetTypeArray(), null);

            var valid = methodInfo != null && methodInfo.ReturnType == typeof(T);

            data = valid ? (T) methodInfo.Invoke(o, parameters) : default;

            return valid;
        }

        // Query instance method (non-generic)
        public static bool Query(this object o, string message, out object data, params object[] parameters)
        {
            var methodInfo = o.GetType().GetMethod(message, ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS, null,
                parameters.GetTypeArray(), null);

            var valid = methodInfo != null;

            data = valid ? methodInfo.Invoke(o, parameters) : null;

            return valid;
        }

        // Get static methods with attribute
        public static IEnumerable<MethodInfo> GetMethodsWithAttribute<T>(this Type type)
            where T : Attribute =>
            type
                .GetMethodsWithAttribute<T>(ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS);

        // Get instance methods with attribute
        public static IEnumerable<MethodInfo> GetMethodsWithAttribute<T>(this object o)
            where T : Attribute =>
            o
                .GetType()
                .GetMethodsWithAttribute<T>(ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS);

        // Get methods with attribute
        private static IEnumerable<MethodInfo> GetMethodsWithAttribute<T>(this IReflect t, BindingFlags flags)
            where T : Attribute =>
            t
                .GetMethods(flags)
                .Where(p => p.IsDefined(typeof(T)));
    }

    public static class FieldReflectionLibrary
    {
        // Get static field
        public static bool GetData<T>(this Type type, string info, out T data)
        {
            var field = type.GetField(info, ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS);

            var valid = field != null && field.FieldType == typeof(T);

            data = valid ? (T) field.GetValue(null) : default;

            return valid;
        }
        
        // Get static field (non-generic)
        public static bool GetData(this Type type, string info, out object data)
        {
            var field = type.GetField(info, ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS);

            var valid = field != null;

            data = valid ? field.GetValue(null) : null;

            return valid;
        }

        // Get instance field
        public static bool GetData<T>(this object o, string info, out T data)
        {
            var field = o.GetType().GetField(info, ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS);

            var valid = field != null && field.FieldType == typeof(T);

            data = valid ? (T) field.GetValue(o) : default;

            return valid;
        }

        // Get instance field (non-generic)
        public static bool GetData(this object o, string info, out object data)
        {
            var field = o.GetType().GetField(info, ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS);

            var valid = field != null;

            data = valid ? field.GetValue(o) : null;

            return valid;
        }

        // Set static field 
        public static bool SetData<T>(this Type type, string info, T data)
        {
            var field = type.GetField(info, ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS);

            var valid = field != null && field.FieldType == typeof(T) && !field.IsInitOnly;

            if (valid) field.SetValue(null, data);

            return valid;
        }

        // Set static field (non-generic)
        public static bool SetData(this Type type, string info, object data)
        {
            var field = type.GetField(info, ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS);

            var valid = field != null && field.FieldType.IsInstanceOfType(data) && !field.IsInitOnly;

            if (valid) field.SetValue(null, data);

            return valid;
        }

        // Set instance field
        public static bool SetData<T>(this object o, string info, T data)
        {
            var field = o.GetType().GetField(info, ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS);

            var valid = field != null && field.FieldType == typeof(T) && !field.IsInitOnly;

            if (valid) field.SetValue(o, data);

            return valid;
        }

        // Set instance field (non-generic)
        public static bool SetData(this object o, string info, object data)
        {
            var field = o.GetType().GetField(info, ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS);

            var valid = field != null && field.FieldType.IsInstanceOfType(data) && !field.IsInitOnly;

            if (valid) field.SetValue(o, data);

            return valid;
        }

        // Get static fields with attribute
        public static IEnumerable<FieldInfo> GetFieldsWithAttribute<T>(this Type type)
            where T : Attribute =>
            type
                .GetFieldsWithAttribute<T>(ReflectionLibrary.STATIC_MEMBER_BINDING_FLAGS);

        // Get instance fields with attribute
        public static IEnumerable<FieldInfo> GetFieldsWithAttribute<T>(this object o)
            where T : Attribute =>
            o
                .GetType()
                .GetFieldsWithAttribute<T>(ReflectionLibrary.INSTANCE_MEMBER_BINDING_FLAGS);

        // Get fields with attribute
        private static IEnumerable<FieldInfo> GetFieldsWithAttribute<T>(this IReflect t, BindingFlags flags)
            where T : Attribute =>
            t
                .GetFields(flags)
                .Where(p => p.IsDefined(typeof(T)));
    }
}