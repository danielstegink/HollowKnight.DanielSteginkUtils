using System.Reflection;

namespace DanielSteginkUtils.Utilities
{
    /// <summary>
    /// Library for methods used to access fields, properties and methods that cannot be accessed conventionally.
    /// 
    /// This is usually for private fields, but it can also be for classes that are referenced as objects or IMod 
    /// instead of their proper type.
    /// </summary>
    public static class ClassIntegrations
    {
        /// <summary>
        /// Gets the value of a field from the given input class.
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="O"></typeparam>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static O GetField<I, O>(I input, string fieldName, 
                                        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
        {
            FieldInfo fieldInfo = input.GetType()
                                       .GetField(fieldName, flags);
            return (O)fieldInfo.GetValue(input);
        }

        /// <summary>
        /// Sets the value of a field from the given input class
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        public static void SetField<I>(I input, string fieldName, object value,
                                        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
        {
            FieldInfo fieldInfo = input.GetType()
                                       .GetField(fieldName, flags);
            fieldInfo.SetValue(input, value);
        }

        /// <summary>
        /// Gets the value of a property from the given input class.
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="O"></typeparam>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static O GetProperty<I, O>(I input, string fieldName, 
                                        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
        {
            PropertyInfo propertyInfo = input.GetType()
                                                .GetProperty(fieldName, flags);
            return (O)propertyInfo.GetValue(input);
        }

        /// <summary>
        /// Sets the value of a property from the given input class.
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static void SetProperty<I>(I input, string fieldName, object value,
                                        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
        {
            PropertyInfo propertyInfo = input.GetType()
                                                .GetProperty(fieldName, flags);
            propertyInfo.SetValue(input, value);
        }

        /// <summary>
        /// Calls a function from the given input class
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="O"></typeparam>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <param name="parameters"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static O CallFunction<I, O>(I input, string fieldName, object[] parameters,
                                            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
        {
            MethodInfo methodInfo = input.GetType()
                                            .GetMethod(fieldName, flags);
            return (O)methodInfo.Invoke(input, parameters);
        }
    }
}
