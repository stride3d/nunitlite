using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NUnit.Framework
{
    public struct TypeInfoEx
    {
        /// <summary>
        /// An empty type array.
        /// </summary>
        public static readonly Type[] EmptyTypes = new Type[0];

#if NETFX_CORE
        private readonly TypeInfo thisInfo;

        internal TypeInfoEx(TypeInfo info)
        {
            this.thisInfo = info;
        }

        internal TypeInfoEx(Type type)
        {
            this.thisInfo = type == null ? null : type.GetTypeInfo();
        }
#else
        private readonly Type thisInfo;

        internal TypeInfoEx(Type type)
        {
            this.thisInfo = type;
        }
#endif

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return thisInfo.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return thisInfo.Equals(obj);
        }

        #region MemberInfo
        // MemberInfo
#if !NETFX_CORE
        public IEnumerable<CustomAttributeData> CustomAttributes { get { return CustomAttributeData.GetCustomAttributes(thisInfo); } }
#else
        public IEnumerable<CustomAttributeData> CustomAttributes { get { return thisInfo.CustomAttributes; } }
#endif
        /// <summary>
        /// Gets the declaring type (for nested type or generic type parameter).
        /// </summary>
        /// <value>
        /// The declaring type.
        /// </value>
        public Type DeclaringType { get { return thisInfo.DeclaringType; } }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <value>
        /// The module.
        /// </value>
        public Module Module { get { return thisInfo.Module; } }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get { return thisInfo.Name; } }
        #endregion

        #region TypeInfo Methods (.NET 4.5)
        /// <summary>
        /// Converts as Type.
        /// </summary>
        /// <returns></returns>
        public Type AsType()
        {
#if !WinRT
            return thisInfo;
#else
            return thisInfo.AsType();
#endif
        }

        /// <summary>
        /// Gets the array rank.
        /// </summary>
        /// <returns></returns>
        public int GetArrayRank()
        {
            return thisInfo.GetArrayRank();
        }

        /// <summary>
        /// Gets the event declared in this type with a given name.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <returns>If found, the specified event; otherwise, <c>null</c>.</returns>
        public EventInfo GetDeclaredEvent(string name)
        {
#if !WinRT
            return thisInfo.GetEvent(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
#else
            return thisInfo.GetDeclaredEvent(name);
#endif
        }

        /// <summary>
        /// Gets the field declared in this type with a given name.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <returns>If found, the specified field; otherwise, <c>null</c>.</returns>
        public FieldInfo GetDeclaredField(string name)
        {
#if !WinRT
            return thisInfo.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
#else
            return thisInfo.GetDeclaredField(name);
#endif
        }

        /// <summary>
        /// Gets the method declared in this type with a given name.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <returns>If found, the specified method; otherwise, <c>null</c>.</returns>
        public MethodInfo GetDeclaredMethod(string name)
        {
#if !WinRT
            return thisInfo.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
#else
            return thisInfo.GetDeclaredMethod(name);
#endif
        }

        /// <summary>
        /// Gets the methods declared in this type with a given name.
        /// </summary>
        /// <param name="name">The name of the methods.</param>
        /// <returns>An enumeration of the matching methods.</returns>
        public IEnumerable<MethodInfo> GetDeclaredMethods(string name)
        {
#if !WinRT
            return thisInfo
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(x => x.Name == name);
#else
            return thisInfo.GetDeclaredMethods(name);
#endif
        }

        /// <summary>
        /// Gets the nested type declared in this type with a given name.
        /// </summary>
        /// <param name="name">The name of the nested type.</param>
        /// <returns>If found, the specified nested type; otherwise, <c>null</c>.</returns>
        public TypeInfoEx GetDeclaredNestedType(string name)
        {
#if !WinRT
            var nestedType = thisInfo.GetNestedType(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            return nestedType != null ? new TypeInfoEx(nestedType) : new TypeInfoEx();
#else
            return new TypeInfoEx(thisInfo.GetDeclaredNestedType(name));
#endif
        }

        /// <summary>
        /// Gets the property declared in this type with a given name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>If found, the specified property; otherwise, <c>null</c>.</returns>
        public PropertyInfo GetDeclaredProperty(string name)
        {
#if !WinRT
            return thisInfo.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
#else
            return thisInfo.GetDeclaredProperty(name);
#endif
        }

        /// <summary>
        /// Gets the element type if the current type is an array, pointer or reference type.
        /// </summary>
        /// <returns>If the current type is an array, pointer or reference type, the element type; otherwise <c>null</c>.</returns>
        public Type GetElementType()
        {
            return thisInfo.GetElementType();
        }

        /// <summary>
        /// Gets the generic parameter constraints of the current <see cref="Type"/>.
        /// </summary>
        /// <returns>An array of <see cref="Type"/> that are the constraints on the current generic type parameter.</returns>
        public Type[] GetGenericParameterConstraints()
        {
            return thisInfo.GetGenericParameterConstraints();
        }

        /// <summary>
        /// Gets the generic type definition of the current <see cref="Type"/>.
        /// </summary>
        /// <returns>The generic type definition.</returns>
        public Type GetGenericTypeDefinition()
        {
            return thisInfo.GetGenericTypeDefinition();
        }

        /// <summary>
        /// Determines whether an instance of the current <see cref="Type"/> is assignable from an instance of the specified type info.
        /// </summary>
        /// <param name="typeInfo">The type info.</param>
        /// <returns>
        ///   <c>true</c> if an instance of the current <see cref="Type"/> is assignable from an instance of the specified type info; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAssignableFrom(TypeInfoEx typeInfo)
        {
            return thisInfo.IsAssignableFrom(typeInfo.thisInfo);
        }

        /// <summary>
        /// Determines whether the current <see cref="Type"/> is subclass of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>
        ///   <c>true</c> if the current <see cref="Type"/> is subclass of the specified <see cref="Type"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSubclassOf(Type c)
        {
            return thisInfo.IsSubclassOf(c);
        }

        /// <summary>
        /// Makes an array type with the current <see cref="Type"/> as an element type.
        /// </summary>
        /// <returns>The array type of the current <see cref="Type"/>.</returns>
        public Type MakeArrayType()
        {
            return thisInfo.MakeArrayType();
        }

        /// <summary>
        /// Makes an array type with the current <see cref="Type"/> as an element type and the specified number of dimensions.
        /// </summary>
        /// <returns>The array type of the current <see cref="Type"/> and the specificed number of dimensions.</returns>
        public Type MakeArrayType(int rank)
        {
            return thisInfo.MakeArrayType(rank);
        }

        /// <summary>
        /// Makes a byref type with the current <see cref="Type"/>.
        /// </summary>
        /// <returns>The byref type of the current <see cref="Type"/>.</returns>
        public Type MakeByRefType()
        {
            return thisInfo.MakeByRefType();
        }

        /// <summary>
        /// Makes a generic type using the current <see cref="Type"/> as generic definition and the given type arguments as generic arguments.
        /// </summary>
        /// <param name="typeArguments">The generic type arguments.</param>
        /// <returns>The constructed generic type.</returns>
        public Type MakeGenericType(params Type[] typeArguments)
        {
            return thisInfo.MakeGenericType(typeArguments);
        }

        /// <summary>
        /// Makes a pointer type with the current <see cref="Type"/>.
        /// </summary>
        /// <returns>A pointer type of the current <see cref="Type"/>.</returns>
        public Type MakePointerType()
        {
            return thisInfo.MakePointerType();
        }
        #endregion

        #region TypeInfo Properties (.NET 4.5)
        public Assembly Assembly { get { return thisInfo.Assembly; } }
        public string AssemblyQualifiedName { get { return thisInfo.AssemblyQualifiedName; } }
        public TypeAttributes Attributes { get { return thisInfo.Attributes; } }
        public Type BaseType { get { return thisInfo.BaseType; } }
        public bool ContainsGenericParameters { get { return thisInfo.ContainsGenericParameters; } }
#if !WinRT
        public IEnumerable<ConstructorInfo> DeclaredConstructors { get { return thisInfo.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly); } }
        public IEnumerable<EventInfo> DeclaredEvents { get { return thisInfo.GetEvents(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly); } }
        public IEnumerable<FieldInfo> DeclaredFields { get { return thisInfo.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly); } }
        public IEnumerable<MemberInfo> DeclaredMembers { get { return thisInfo.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly); } }
        public IEnumerable<MethodInfo> DeclaredMethods { get { return thisInfo.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly); } }
        public IEnumerable<TypeInfoEx> DeclaredNestedTypes
        {
            get
            {
                return thisInfo
                    .GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Select(x => new TypeInfoEx(x));
            }
        }
        public IEnumerable<PropertyInfo> DeclaredProperties { get { return thisInfo.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly); } }
        public IEnumerable<Type> ImplementedInterfaces { get { return thisInfo.GetInterfaces(); } }
        public Type[] GenericTypeArguments { get { return thisInfo.GetGenericArguments(); } }
        public Type[] GenericTypeParameters
        {
            get
            {
                if (thisInfo.IsGenericTypeDefinition)
                    return thisInfo.GetGenericArguments();

                return EmptyTypes;
            }
        }
#else
        /// <summary>
        /// Gets the declared constructors.
        /// </summary>
        /// <value>
        /// The declared constructors.
        /// </value>
        public IEnumerable<ConstructorInfo> DeclaredConstructors { get { return thisInfo.DeclaredConstructors; } }

        /// <summary>
        /// Gets the declared events.
        /// </summary>
        /// <value>
        /// The declared events.
        /// </value>
        public IEnumerable<EventInfo> DeclaredEvents { get { return thisInfo.DeclaredEvents; } }

        /// <summary>
        /// Gets the declared fields.
        /// </summary>
        /// <value>
        /// The declared fields.
        /// </value>
        public IEnumerable<FieldInfo> DeclaredFields { get { return thisInfo.DeclaredFields; } }

        /// <summary>
        /// Gets the declared members.
        /// </summary>
        /// <value>
        /// The declared members.
        /// </value>
        public IEnumerable<MemberInfo> DeclaredMembers { get { return thisInfo.DeclaredMembers; } }

        /// <summary>
        /// Gets the declared methods.
        /// </summary>
        /// <value>
        /// The declared methods.
        /// </value>
        public IEnumerable<MethodInfo> DeclaredMethods { get { return thisInfo.DeclaredMethods; } }

        /// <summary>
        /// Gets the declared nested types.
        /// </summary>
        /// <value>
        /// The declared nested types.
        /// </value>
        public IEnumerable<TypeInfoEx> DeclaredNestedTypes { get { return thisInfo.DeclaredNestedTypes.Select(x => new TypeInfoEx(x)); } }

        /// <summary>
        /// Gets the declared properties.
        /// </summary>
        /// <value>
        /// The declared properties.
        /// </value>
        public IEnumerable<PropertyInfo> DeclaredProperties { get { return thisInfo.DeclaredProperties; } }

        /// <summary>
        /// Gets the implemented interfaces.
        /// </summary>
        /// <value>
        /// The implemented interfaces.
        /// </value>
        public IEnumerable<Type> ImplementedInterfaces { get { return thisInfo.ImplementedInterfaces; } }

        /// <summary>
        /// Gets the generic type arguments.
        /// </summary>
        /// <value>
        /// The generic type arguments.
        /// </value>
        public Type[] GenericTypeArguments { get { return thisInfo.GenericTypeArguments; } }

        /// <summary>
        /// Gets the generic type parameters.
        /// </summary>
        /// <value>
        /// The generic type parameters.
        /// </value>
        public Type[] GenericTypeParameters { get { return thisInfo.GenericTypeParameters; } }
#endif
        public MethodBase DeclaringMethod { get { return thisInfo.DeclaringMethod; } }
        public string FullName { get { return thisInfo.FullName; } }
        public GenericParameterAttributes GenericParameterAttributes { get { return thisInfo.GenericParameterAttributes; } }
        public int GenericParameterPosition { get { return thisInfo.GenericParameterPosition; } }
        public Guid GUID { get { return thisInfo.GUID; } }
        public bool HasElementType { get { return thisInfo.HasElementType; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Type"/> is abstract.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="Type"/> is abstract; otherwise, <c>false</c>.
        /// </value>
        public bool IsAbstract { get { return thisInfo.IsAbstract; } }

        //public bool IsAnsiClass { get { return thisInfo.IsAnsiClass; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Type"/> is array.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="Type"/> is an array type; otherwise, <c>false</c>.
        /// </value>
        public bool IsArray { get { return thisInfo.IsArray; } }
        public bool IsAutoClass { get { return thisInfo.IsAutoClass; } }
        public bool IsAutoLayout { get { return thisInfo.IsAutoLayout; } }
        public bool IsByRef { get { return thisInfo.IsByRef; } }
        public bool IsClass { get { return thisInfo.IsClass; } }
        public bool IsEnum { get { return thisInfo.IsEnum; } }
        public bool IsExplicitLayout { get { return thisInfo.IsExplicitLayout; } }
        public bool IsGenericParameter { get { return thisInfo.IsGenericParameter; } }
        public bool IsGenericType { get { return thisInfo.IsGenericType; } }
        public bool IsGenericTypeDefinition { get { return thisInfo.IsGenericTypeDefinition; } }
        public bool IsImport { get { return thisInfo.IsImport; } }
        public bool IsInterface { get { return thisInfo.IsInterface; } }
        public bool IsLayoutSequential { get { return thisInfo.IsLayoutSequential; } }
        public bool IsMarshalByRef { get { return thisInfo.IsMarshalByRef; } }
        public bool IsNested { get { return thisInfo.IsNested; } }
        public bool IsNestedAssembly { get { return thisInfo.IsNestedAssembly; } }
        public bool IsNestedFamANDAssem { get { return thisInfo.IsNestedFamANDAssem; } }
        public bool IsNestedFamily { get { return thisInfo.IsNestedFamily; } }
        public bool IsNestedFamORAssem { get { return thisInfo.IsNestedFamORAssem; } }
        public bool IsNestedPrivate { get { return thisInfo.IsNestedPrivate; } }
        public bool IsNestedPublic { get { return thisInfo.IsNestedPublic; } }
        public bool IsNotPublic { get { return thisInfo.IsNotPublic; } }
        public bool IsPointer { get { return thisInfo.IsPointer; } }
        public bool IsPrimitive { get { return thisInfo.IsPrimitive; } }
        public bool IsPublic { get { return thisInfo.IsPublic; } }
        public bool IsSealed { get { return thisInfo.IsSealed; } }
        public bool IsSerializable { get { return thisInfo.IsSerializable; } }
        public bool IsSpecialName { get { return thisInfo.IsSpecialName; } }
        public bool IsUnicodeClass { get { return thisInfo.IsUnicodeClass; } }
        public bool IsValueType { get { return thisInfo.IsValueType; } }
        public bool IsVisible { get { return thisInfo.IsVisible; } }
        public string Namespace { get { return thisInfo.Namespace; } }
        #endregion

        #region System.Reflection.Extensions extension methods
        public InterfaceMapping GetRuntimeInterfaceMap(Type interfaceType)
        {
#if !WinRT
            return thisInfo.GetInterfaceMap(interfaceType);
#else
            return thisInfo.GetRuntimeInterfaceMap(interfaceType);
#endif
        }
        #endregion

        #region Old Type methods
        public IEnumerable<ConstructorInfo> GetConstructors(BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
        {
#if NETFX_CORE
            return thisInfo.DeclaredConstructors.Where(constructor => MatchBindingFlags(constructor, bindingAttr));
#else
            return thisInfo.GetConstructors(bindingAttr);
#endif
        }

        public ConstructorInfo GetConstructor(Type[] types, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance)
        {
#if NETFX_CORE
            return thisInfo.DeclaredConstructors.FirstOrDefault(constructor => MatchBindingFlags(constructor, bindingAttr) && CompareArgumentTypes(types, constructor.GetParameters()));
#else
            return thisInfo.GetConstructor(bindingAttr, null, types, null);
#endif
        }

        public IEnumerable<MethodInfo> GetMethods(BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
        {
#if NETFX_CORE
            return ForEachBaseTypes(bindingAttr).SelectMany(type => type.DeclaredMethods.Where(method => MatchBindingFlags(method, bindingAttr)));
#else
            return thisInfo.GetMethods(bindingAttr);
#endif
        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
        {
#if NETFX_CORE
            return ForEachBaseTypes(bindingAttr).SelectMany(type => type.DeclaredMethods.Where(method => MatchBindingFlags(method, bindingAttr) && method.Name == name)).FirstOrDefault();
#else
            return thisInfo.GetMethod(name, bindingAttr);
#endif
        }

        public MethodInfo GetMethod(string name, Type[] types, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
        {
#if NETFX_CORE
            return ForEachBaseTypes(bindingAttr).SelectMany(type => type.DeclaredMethods.Where(method => MatchBindingFlags(method, bindingAttr) && method.Name == name && CompareArgumentTypes(types, method.GetParameters()))).FirstOrDefault();

#else
            return thisInfo.GetMethod(name, bindingAttr, null, types, null);
#endif
        }

        public FieldInfo GetField(string name, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
        {
#if NETFX_CORE
            return ForEachBaseTypes(bindingAttr).SelectMany(type => type.DeclaredFields.Where(field => MatchBindingFlags(field, bindingAttr) && field.Name == name)).FirstOrDefault();
#else
            return thisInfo.GetField(name, bindingAttr);
#endif
        }

        public IEnumerable<PropertyInfo> GetProperties(BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
        {
#if NETFX_CORE
            return ForEachBaseTypes(bindingAttr).SelectMany(type => (from property in type.DeclaredProperties where MatchBindingFlags(property, bindingAttr) select property));
#else
            return thisInfo.GetProperties(bindingAttr);
#endif
        }

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
        {
#if NETFX_CORE
            return ForEachBaseTypes(bindingAttr).SelectMany(type => (from property in type.DeclaredProperties where MatchBindingFlags(property, bindingAttr) && property.Name == name select property)).FirstOrDefault();

#else
            return thisInfo.GetProperty(name, bindingAttr);
#endif
        }

#if NETFX_CORE
        private static bool CompareArgumentTypes(Type[] types, ParameterInfo[] methodParameters)
        {
            bool parameterMatches = true;
            if (types.Length != methodParameters.Length)
                return false;
            for (int i = 0; i < types.Length; ++i)
            {
                if (types[i] != methodParameters[i].ParameterType)
                {
                    parameterMatches = false;
                    break;
                }
            }

            return parameterMatches;
        }

        private IEnumerable<TypeInfo> ForEachBaseTypes(BindingFlags bindingFlags)
        {
            var type = thisInfo;
            while (type != null)
            {
                yield return type;

                if ((bindingFlags & BindingFlags.DeclaredOnly) == BindingFlags.DeclaredOnly)
                    break;

                type = type.BaseType != null ? type.BaseType.GetTypeInfo() : null;
            }
        }

        private static bool MatchBindingFlags(ConstructorInfo constructorInfo, BindingFlags bindingAttr)
        {
            return (
                (((bindingAttr & BindingFlags.Instance) == BindingFlags.Instance && !constructorInfo.IsStatic) ||
                 ((bindingAttr & BindingFlags.Static) == BindingFlags.Static && constructorInfo.IsStatic)) &&
                (((bindingAttr & BindingFlags.Public) == BindingFlags.Public && constructorInfo.IsPublic) ||
                 ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.NonPublic && constructorInfo.IsPrivate)));
        }

        private static bool MatchBindingFlags(FieldInfo fieldInfo, BindingFlags bindingAttr)
        {
            return (
                (((bindingAttr & BindingFlags.Instance) == BindingFlags.Instance && !fieldInfo.IsStatic) ||
                 ((bindingAttr & BindingFlags.Static) == BindingFlags.Static && fieldInfo.IsStatic)) &&
                (((bindingAttr & BindingFlags.Public) == BindingFlags.Public && fieldInfo.IsPublic) ||
                 ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.NonPublic && fieldInfo.IsPrivate)));
        }

        private static bool MatchBindingFlags(MethodInfo methodInfo, BindingFlags bindingAttr)
        {
            return (
                (((bindingAttr & BindingFlags.Instance) == BindingFlags.Instance && !methodInfo.IsStatic) ||
                 ((bindingAttr & BindingFlags.Static) == BindingFlags.Static && methodInfo.IsStatic)) &&
                (((bindingAttr & BindingFlags.Public) == BindingFlags.Public && methodInfo.IsPublic) ||
                 ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.NonPublic && methodInfo.IsPrivate)));
        }

        private static bool MatchBindingFlags(PropertyInfo propertyInfo, BindingFlags bindingAttr)
        {
            var methodInfo = propertyInfo.GetMethod ?? propertyInfo.SetMethod;
            return (
                (((bindingAttr & BindingFlags.Instance) == BindingFlags.Instance && !methodInfo.IsStatic) ||
                 ((bindingAttr & BindingFlags.Static) == BindingFlags.Static && methodInfo.IsStatic)) &&
                (((bindingAttr & BindingFlags.Public) == BindingFlags.Public && methodInfo.IsPublic) ||
                 ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.NonPublic && methodInfo.IsPrivate)) &&
                (((bindingAttr & BindingFlags.GetProperty) == BindingFlags.GetProperty && propertyInfo.GetMethod != null) ||
                 ((bindingAttr & BindingFlags.SetProperty) != BindingFlags.SetProperty && propertyInfo.SetMethod != null)));
        }
#endif
        #endregion

        #region GetCustomAttribute extensions

        public T GetCustomAttribute<T>() where T : Attribute
        {
            return (T)GetCustomAttribute(typeof(T));
        }

        public Attribute GetCustomAttribute(Type attributeType)
        {
#if MONODROID || NET_4_0 || NET_3_5
            return Attribute.GetCustomAttribute(thisInfo, attributeType);
#else
            return thisInfo.GetCustomAttribute(attributeType);
#endif
        }

        public IEnumerable<Attribute> GetCustomAttributes(Type attributeType)
        {
#if MONODROID || NET_4_0 || NET_3_5
            return Attribute.GetCustomAttributes(thisInfo, attributeType);
#else
            return thisInfo.GetCustomAttributes(attributeType);
#endif
        }

        public IEnumerable<T> GetCustomAttributes<T>() where T : Attribute
        {
            return GetCustomAttributes(typeof(T)).Cast<T>();
        }

        public T GetCustomAttribute<T>(bool inherit) where T : Attribute
        {
            return (T)GetCustomAttribute(typeof(T), inherit);
        }

        public Attribute GetCustomAttribute(Type attributeType, bool inherit)
        {
#if MONODROID || NET_4_0 || NET_3_5
            return Attribute.GetCustomAttribute(thisInfo, attributeType, inherit);
#else
            return thisInfo.GetCustomAttribute(attributeType, inherit);
#endif
        }

        public IEnumerable<Attribute> GetCustomAttributes(Type attributeType, bool inherit)
        {
            return thisInfo.GetCustomAttributes(attributeType, inherit).OfType<Attribute>();
        }

        public IEnumerable<T> GetCustomAttributes<T>(bool inherit) where T : Attribute
        {
            return GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }

        public bool IsInstanceOfType(object obj)
        {
            if (obj == null)
            {
                return false;
            }

#if NETFX_CORE
            return thisInfo.IsAssignableFrom(obj.GetType().GetTypeInfo());
#else
            return thisInfo.IsAssignableFrom(obj.GetType());
#endif
        }

        #endregion

        #region MemberInfo extensions

        public bool IsDefined(Type attributeType)
        {
            return IsDefined(attributeType, true);
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return thisInfo.IsDefined(attributeType, inherit);
        }

        #endregion
    }
}