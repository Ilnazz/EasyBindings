﻿using System.ComponentModel;
using System.Linq.Expressions;

namespace EasyBindings;

/// <summary>
/// Allows you to bind the properties of one object to a <see cref="INotifyPropertyChanged"/> event of a property of another object.
/// </summary>
public static class PropertyBinder
{
    private static readonly IList<PropertyBinding> _propertyBindings = new List<PropertyBinding>();

    #region Public methods 
    #region Binding
    /// <summary>
    /// Binds <paramref name="target"/> object's property to changes of <paramref name="source"/> object's property in a given context.
    /// </summary>
    /// <typeparam name="TTarget">The type of the <paramref name="target"/> object.</typeparam>
    /// <typeparam name="TSource">The type of the <paramref name="source"/> object.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="target">The object to which the property is being bound.</param>
    /// <param name="targetPropertyGetterExpr">An expression that identifies the property of the <paramref name="target"/> object.</param>
    /// <param name="source">The object from which the property changes are being observed.</param>
    /// <param name="sourcePropertyGetterExpr">An expression that identifies the property of the <paramref name="source"/> object.</param>
    public static void BindOneWay<TTarget, TSource, TProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TProperty>> sourcePropertyGetterExpr
    )
    where TTarget : class where TSource : INotifyPropertyChanged
    {
        CheckBindingArgs(context, targetPropertyGetterExpr, targetPropertyGetterExpr, source, sourcePropertyGetterExpr);
        var targetPropertyName = GetPropertyName(targetPropertyGetterExpr);
        var sourcePropertyName = GetPropertyName(sourcePropertyGetterExpr);

        var sourcePropertyGetter = sourcePropertyGetterExpr.Compile();
        var targetPropertyGetter = targetPropertyGetterExpr.Compile();
        var targetPropertySetter = CreatePropertySetter(targetPropertyGetterExpr);

        void sourcePropertyChangedEventHandler(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == sourcePropertyName)
            {
                var oldValue = targetPropertyGetter(target);
                var newValue = sourcePropertyGetter(source);

                if (EqualityComparer<TProperty>.Default.Equals(oldValue, newValue) == false)
                    targetPropertySetter(target, newValue);
            }
        }

        source.PropertyChanged += sourcePropertyChangedEventHandler;
        _propertyBindings.Add(new PropertyBinding
        (
            context,
            target, targetPropertyName, null,
            source, sourcePropertyName, sourcePropertyChangedEventHandler
        ));

        // Initialize target property by source property value
        sourcePropertyChangedEventHandler(null, new PropertyChangedEventArgs(sourcePropertyName));
    }

    /// <summary>
    /// Binds <paramref name="target"/> object's property to changes of <paramref name="source"/> object's property in a given context.
    /// </summary>
    /// <typeparam name="TTarget">The type of the <paramref name="target"/> object.</typeparam>
    /// <typeparam name="TSource">The type of the <paramref name="source"/> object.</typeparam>
    /// <typeparam name="TTargetProperty">The type of the property of the <paramref name="target"/> object.</typeparam>
    /// <typeparam name="TSourceProperty">The type of the property of the <paramref name="source"/> object.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="target">The object to which the property is being bound.</param>
    /// <param name="targetPropertyGetterExpr">An expression that identifies the property of the <paramref name="target"/> object.</param>
    /// <param name="source">The object from which the property changes are being observed.</param>
    /// <param name="sourcePropertyGetterExpr">An expression that identifies the property of the <paramref name="source"/> object.</param>
    /// <param name="converter">A function that converts the <paramref name="source"/> property value to the <paramref name="target"/> property value.</param>
    public static void BindOneWay<TTarget, TSource, TTargetProperty, TSourceProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TTargetProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TSourceProperty>> sourcePropertyGetterExpr,
        Func<TSourceProperty, TTargetProperty> converter
    )
    where TTarget : class where TSource : INotifyPropertyChanged
    {
        CheckBindingArgs(context, targetPropertyGetterExpr, targetPropertyGetterExpr, source, sourcePropertyGetterExpr);
        ArgumentNullException.ThrowIfNull(converter, nameof(converter));
        var targetPropertyName = GetPropertyName(targetPropertyGetterExpr);
        var sourcePropertyName = GetPropertyName(sourcePropertyGetterExpr);

        var sourcePropertyGetter = sourcePropertyGetterExpr.Compile();
        var targetPropertyGetter = targetPropertyGetterExpr.Compile();
        var targetPropertySetter = CreatePropertySetter(targetPropertyGetterExpr);

        void sourcePropertyChangedEventHandler(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == sourcePropertyName)
            {
                var oldValue = targetPropertyGetter(target);
                var newValue = converter(sourcePropertyGetter(source));

                if (EqualityComparer<TTargetProperty>.Default.Equals(oldValue, newValue) == false)
                    targetPropertySetter(target, newValue);
            }
        }

        source.PropertyChanged += sourcePropertyChangedEventHandler;
        _propertyBindings.Add(new PropertyBinding
        (
            context,
            target, targetPropertyName, null,
            source, sourcePropertyName, sourcePropertyChangedEventHandler
        ));

        // Initialize target property by source property value
        sourcePropertyChangedEventHandler(null, new PropertyChangedEventArgs(sourcePropertyName));
    }

    /// <summary>
    /// Binds <paramref name="source"/> object's property to changes of <paramref name="target"/> object's property in a given context.
    /// </summary>
    /// <typeparam name="TTarget">The type of the <paramref name="target"/> object.</typeparam>
    /// <typeparam name="TSource">The type of the <paramref name="source"/> object.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="target">The object from which the property changes are being observed.</param>
    /// <param name="targetPropertyGetterExpr">An expression that identifies the property of the <paramref name="target"/> object.</param>
    /// <param name="source">The object to which the property is being bound.</param>
    /// <param name="sourcePropertyGetterExpr">An expression that identifies the property of the <paramref name="source"/> object.</param>
    public static void BindOneWayToSource<TTarget, TSource, TProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TProperty>> sourcePropertyGetterExpr
    )
    where TTarget : INotifyPropertyChanged where TSource : class
    {
        BindOneWay(context, source, sourcePropertyGetterExpr, target, targetPropertyGetterExpr);
    }

    /// <summary>
    /// Binds <paramref name="source"/> object's property to changes of <paramref name="target"/> object's property in a given context.
    /// </summary>
    /// <typeparam name="TTarget">The type of the <paramref name="target"/> object.</typeparam>
    /// <typeparam name="TSource">The type of the <paramref name="source"/> object.</typeparam>
    /// <typeparam name="TTargetProperty">The type of the property of the <paramref name="target"/> object.</typeparam>
    /// <typeparam name="TSourceProperty">The type of the property of the <paramref name="source"/> object.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="target">The object from which the property changes are being observed.</param>
    /// <param name="targetPropertyGetterExpr">An expression that identifies the property of the <paramref name="target"/> object.</param>
    /// <param name="source">The object to which the property is being bound.</param>
    /// <param name="sourcePropertyGetterExpr">An expression that identifies the property of the <paramref name="source"/> object.</param>
    /// <param name="converter">A function that converts the <paramref name="target"/> property value to the <paramref name="source"/> property value.</param>
    public static void BindOneWayToSource<TTarget, TSource, TTargetProperty, TSourceProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TTargetProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TSourceProperty>> sourcePropertyGetterExpr,
        Func<TTargetProperty, TSourceProperty> converter
    )
    where TTarget : INotifyPropertyChanged where TSource : class
    {
        BindOneWay(context, source, sourcePropertyGetterExpr, target, targetPropertyGetterExpr, converter);
    }

    /// <summary>
    /// Binds <paramref name="target"/> object's property to changes of <paramref name="source"/> object's property and vice versa in a given context.
    /// </summary>
    /// <typeparam name="TTarget">The type of the <paramref name="target"/> object.</typeparam>
    /// <typeparam name="TSource">The type of the <paramref name="source"/> object.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="target">The first binding participating object.</param>
    /// <param name="targetPropertyGetterExpr">An expression that identifies the property of the <paramref name="target"/> object.</param>
    /// <param name="source">The second binding participating object.</param>
    /// <param name="sourcePropertyGetterExpr">An expression that identifies the property of the <paramref name="source"/> object.</param>
    public static void BindTwoWay<TTarget, TSource, TProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TProperty>> sourcePropertyGetterExpr
    )
    where TTarget : class, INotifyPropertyChanged where TSource : class, INotifyPropertyChanged
    {
        BindOneWay(context, target, targetPropertyGetterExpr, source, sourcePropertyGetterExpr);
        BindOneWay(context, source, sourcePropertyGetterExpr, target, targetPropertyGetterExpr);
    }

    /// <summary>
    /// Binds <paramref name="target"/> object's property to changes of <paramref name="source"/> object's property and vice versa in a given context.
    /// </summary>
    /// <typeparam name="TTarget">The type of the <paramref name="target"/> object.</typeparam>
    /// <typeparam name="TSource">The type of the <paramref name="source"/> object.</typeparam>
    /// <typeparam name="TTargetProperty">The type of the property of the <paramref name="target"/> object.</typeparam>
    /// <typeparam name="TSourceProperty">The type of the property of the <paramref name="source"/> object.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="target">The first binding participating object.</param>
    /// <param name="targetPropertyGetterExpr">An expression that identifies the property of the <paramref name="target"/> object.</param>
    /// <param name="source">The second binding participating object.</param>
    /// <param name="sourcePropertyGetterExpr">An expression that identifies the property of the <paramref name="source"/> object.</param>
    /// <param name="targetValueConverter">A function that converts the <paramref name="target"/> property value to the <paramref name="source"/> property value.</param>
    /// <param name="sourceValueConverter">A function that converts the <paramref name="source"/> property value to the <paramref name="target"/> property value.</param>
    public static void BindTwoWay<TTarget, TSource, TTargetProperty, TSourceProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TTargetProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TSourceProperty>> sourcePropertyGetterExpr,
        Func<TTargetProperty, TSourceProperty> targetValueConverter,
        Func<TSourceProperty, TTargetProperty> sourceValueConverter
    )
    where TTarget : class, INotifyPropertyChanged where TSource : class, INotifyPropertyChanged
    {
        BindOneWay(context, target, targetPropertyGetterExpr, source, sourcePropertyGetterExpr, sourceValueConverter);
        BindOneWay(context, source, sourcePropertyGetterExpr, target, targetPropertyGetterExpr, targetValueConverter);
    }
    #endregion

    #region Unbinding
    /// <summary>
    /// Removes bindings from <paramref name="target"/> object's property.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="target"/> object.</typeparam>
    /// <typeparam name="TProperty">The of the <paramref name="target"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="target">The object whose property was bound.</param>
    /// <param name="targetPropertyGetterExpr">An expression that identifies the property of the <paramref name="target"/> object.</param>
    public static void UnbindFromTarget<T, TProperty>(object context, object target, Expression<Func<T, TProperty>> targetPropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(targetPropertyGetterExpr, nameof(targetPropertyGetterExpr));

        var targetPropertyName = GetPropertyName(targetPropertyGetterExpr);

        _propertyBindings.Where(pb => pb.Context == context && pb.Target == target && pb.TargetPropertyName == targetPropertyName)
            .ToList().ForEach(Unbind);
    }

    /// <summary>
    /// Removes bindings from <paramref name="target"/> object.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="target">The object whose properties were bound.</param>
    public static void UnbindFromTarget(object context, object target)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        _propertyBindings.Where(pb => pb.Context == context && pb.Target == target).ToList().ForEach(Unbind);
    }

    /// <summary>
    /// Removes bindings from <paramref name="source"/> object's property.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="source"/> object.</typeparam>
    /// <typeparam name="TProperty">The of the <paramref name="source"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="source">The object whose property was bound.</param>
    /// <param name="sourcePropertyGetterExpr">An expression that identifies the property of the <paramref name="source"/> object.</param>
    public static void UnbindFromSource<T, TProperty>(object context, object source, Expression<Func<T, TProperty>> sourcePropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(sourcePropertyGetterExpr, nameof(sourcePropertyGetterExpr));

        var sourcePropertyName = GetPropertyName(sourcePropertyGetterExpr);

        _propertyBindings.Where(pb => pb.Context == context && pb.Source == source && pb.SourcePropertyName == sourcePropertyName)
            .ToList().ForEach(Unbind);
    }

    /// <summary>
    /// Removes bindings from <paramref name="source"/> object.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="source">The object whose properties were bound.</param>
    public static void UnbindFromSource(object context, object source)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        _propertyBindings.Where(pb => pb.Context == context && pb.Source == source).ToList().ForEach(Unbind);
    }

    /// <summary>
    /// Unbinds all bindings made in a given context.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    public static void Unbind(object context) => _propertyBindings.Where(pb => pb.Context == context).ToList().ForEach(Unbind);
    #endregion
    #endregion

    #region Private methods 
    private static void CheckBindingArgs
    (
        object context,
        object target, object targetPropertyGetterExpr,
        object source, object sourcePropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(targetPropertyGetterExpr, nameof(targetPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(sourcePropertyGetterExpr, nameof(sourcePropertyGetterExpr));
    }

    private static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertyGetterExpr) =>
        ((MemberExpression)propertyGetterExpr.Body).Member.Name;

    private static Action<T, TProperty> CreatePropertySetter<T, TProperty>(Expression<Func<T, TProperty>> propertyGetter)
    {
        var propertyName = ((MemberExpression)propertyGetter.Body).Member.Name;
        var propertyInfo = typeof(T).GetProperty(propertyName)!;
        var propertySetter = (T target, TProperty newValue) => propertyInfo.SetValue(target, newValue);
        return propertySetter;
    }

    private static void Unbind(PropertyBinding propertyBinding)
    {
        if (propertyBinding.Target is INotifyPropertyChanged observableTarget)
            observableTarget.PropertyChanged -= propertyBinding.TargetPropertyChangedEventHandler;
        else if (propertyBinding.Source is INotifyPropertyChanged observableSource)
            observableSource.PropertyChanged -= propertyBinding.SourcePropertyChangedEventHandler;

        _propertyBindings.Remove(propertyBinding);
    }
    #endregion

    private class PropertyBinding
    {
        public object Context { get; init; }

        public dynamic? Target { get; init; }

        public string TargetPropertyName { get; init; }

        public PropertyChangedEventHandler? TargetPropertyChangedEventHandler { get; init; }

        public dynamic? Source { get; init; }

        public string SourcePropertyName { get; init; }

        public PropertyChangedEventHandler? SourcePropertyChangedEventHandler { get; init; }

        public PropertyBinding
        (
            object context, dynamic? target, string targetPropertyName,
            PropertyChangedEventHandler? targetPropertyChangedEventHandler,
            dynamic? source, string sourcePropertyName,
            PropertyChangedEventHandler sourcePropertyChangedEventHandler)
        {
            Context = context;
            Target = target;
            TargetPropertyName = targetPropertyName;
            TargetPropertyChangedEventHandler = targetPropertyChangedEventHandler;
            Source = source;
            SourcePropertyName = sourcePropertyName;
            SourcePropertyChangedEventHandler = sourcePropertyChangedEventHandler;
        }
    }
}