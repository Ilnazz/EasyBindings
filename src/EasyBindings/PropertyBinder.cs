using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace EasyBindings;

/// <summary>
/// Allows you to bind the properties of one object to a <see cref="INotifyPropertyChanged"/> event of a property of another object.
/// </summary>
public static class PropertyBinder
{
    private static readonly IList<PropertyBinding> _bindings = new List<PropertyBinding>();

    #region Public methods 
    #region Binding
    /// <summary>
    /// Binds a <paramref name="target"/> object's property to changes of a <paramref name="source"/> object's property in a given context.
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
    where TSource : INotifyPropertyChanged
    {
        CheckBindingArgs(context, target, targetPropertyGetterExpr, source, sourcePropertyGetterExpr);

        var targetPropertyName = GetPropertyName(targetPropertyGetterExpr);
        var sourcePropertyName = GetPropertyName(sourcePropertyGetterExpr);

        var sourcePropertyGetter = sourcePropertyGetterExpr.Compile();
        var targetPropertyGetter = targetPropertyGetterExpr.Compile();
        var targetPropertySetter = CreatePropertySetter(targetPropertyGetterExpr);

        void sourcePropertyChangedEventHandler(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == sourcePropertyName)
            {
                TProperty oldValue = targetPropertyGetter(target),
                          newValue = sourcePropertyGetter(source);

                if (EqualityComparer<TProperty>.Default.Equals(oldValue, newValue) == false)
                    targetPropertySetter(target, newValue);
            }
        }

        source.PropertyChanged += sourcePropertyChangedEventHandler;
        _bindings.Add(new PropertyBinding
        (
            context,
            target!, targetPropertyName, null,
            source, sourcePropertyName, sourcePropertyChangedEventHandler
        ));

        // Initialize target property by source property value
        sourcePropertyChangedEventHandler(null, new PropertyChangedEventArgs(sourcePropertyName));
    }

    /// <summary>
    /// Binds a <paramref name="target"/> object's property to changes of a <paramref name="source"/> object's property in a given context.
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
    where TSource : INotifyPropertyChanged
    {
        CheckBindingArgs(context, target, targetPropertyGetterExpr, source, sourcePropertyGetterExpr);
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
                TTargetProperty oldValue = targetPropertyGetter(target),
                                newValue = converter(sourcePropertyGetter(source));

                if (EqualityComparer<TTargetProperty>.Default.Equals(oldValue, newValue) == false)
                    targetPropertySetter(target, newValue);
            }
        }

        source.PropertyChanged += sourcePropertyChangedEventHandler;
        _bindings.Add(new PropertyBinding
        (
            context,
            target!, targetPropertyName, null,
            source, sourcePropertyName, sourcePropertyChangedEventHandler
        ));

        // Initialize target property by source property value
        sourcePropertyChangedEventHandler(null, new PropertyChangedEventArgs(sourcePropertyName));
    }

    /// <summary>
    /// Binds a <paramref name="source"/> object's property to changes of a <paramref name="target"/> object's property in a given context.
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
    where TTarget : INotifyPropertyChanged
    {
        BindOneWay(context, source, sourcePropertyGetterExpr, target, targetPropertyGetterExpr);
    }

    /// <summary>
    /// Binds a <paramref name="source"/> object's property to changes of a <paramref name="target"/> object's property in a given context.
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
    where TTarget : INotifyPropertyChanged
    {
        BindOneWay(context, source, sourcePropertyGetterExpr, target, targetPropertyGetterExpr, converter);
    }

    /// <summary>
    /// Binds a <paramref name="target"/> object's property to changes of a <paramref name="source"/> object's property and vice versa in a given context.
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
    where TTarget : INotifyPropertyChanged where TSource : INotifyPropertyChanged
    {
        BindOneWay(context, target, targetPropertyGetterExpr, source, sourcePropertyGetterExpr);
        BindOneWay(context, source, sourcePropertyGetterExpr, target, targetPropertyGetterExpr);
    }

    /// <summary>
    /// Binds a <paramref name="target"/> object's property to changes of a <paramref name="source"/> object's property and vice versa in a given context.
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
    where TTarget : INotifyPropertyChanged where TSource : INotifyPropertyChanged
    {
        BindOneWay(context, target, targetPropertyGetterExpr, source, sourcePropertyGetterExpr, sourceValueConverter);
        BindOneWay(context, source, sourcePropertyGetterExpr, target, targetPropertyGetterExpr, targetValueConverter);
    }
    #endregion

    #region Unbinding
    /// <summary>
    /// Unbinds all bindigns from <paramref name="target"/> object's property.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="target"/> object.</typeparam>
    /// <typeparam name="TProperty">The of the <paramref name="target"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="target">The object whose property was bound.</param>
    /// <param name="targetPropertyGetterExpr">An expression that identifies the property of the <paramref name="target"/> object.</param>
    public static void UnbindFromTarget<T, TProperty>(object context, T target, Expression<Func<T, TProperty>> targetPropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(targetPropertyGetterExpr, nameof(targetPropertyGetterExpr));

        var targetPropertyName = GetPropertyName(targetPropertyGetterExpr);
        UnbindPropertyBindings(
            _bindings.Where(b =>
                b.Context == context &&
                b.Target == (object)target &&
                b.TargetPropertyName == targetPropertyName));
    }

    /// <summary>
    /// Unbinds all property bindigns from all <paramref name="target"/> object's properties.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="target">The object whose properties were bound.</param>
    public static void UnbindFromTarget(object context, object target)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        UnbindPropertyBindings(
            _bindings.Where(b => b.Context == context && b.Target == target));
    }

    /// <summary>
    /// Unbinds all bindigns from <paramref name="source"/> object's property.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="source"/> object.</typeparam>
    /// <typeparam name="TProperty">The of the <paramref name="source"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="source">The object whose property was bound.</param>
    /// <param name="sourcePropertyGetterExpr">An expression that identifies the property of the <paramref name="source"/> object.</param>
    public static void UnbindFromSource<T, TProperty>(object context, T source, Expression<Func<T, TProperty>> sourcePropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(sourcePropertyGetterExpr, nameof(sourcePropertyGetterExpr));

        var sourcePropertyName = GetPropertyName(sourcePropertyGetterExpr);
        UnbindPropertyBindings(
            _bindings.Where(b =>
                b.Context == context &&
                b.Source == (object)source &&
                b.SourcePropertyName == sourcePropertyName));
    }

    /// <summary>
    /// Unbinds all property bindigns from all <paramref name="source"/> object's properties.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="source">The object whose properties were bound.</param>
    public static void UnbindFromSource(object context, object source)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        UnbindPropertyBindings(
            _bindings.Where(b => b.Context == context && b.Source == source));
    }

    /// <summary>
    /// Unbinds all bindings made in a given context.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    public static void Unbind(object context)
    {
        UnbindPropertyBindings(_bindings.Where(b => b.Context == context));
    }
    #endregion
    #endregion

    #region Private methods
    private static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertyGetterExpr) =>
        ((MemberExpression)propertyGetterExpr.Body).Member.Name;

    private static Action<T, TProperty> CreatePropertySetter<T, TProperty>(Expression<Func<T, TProperty>> propertyGetter)
    {
        var propertyName = ((MemberExpression)propertyGetter.Body).Member.Name;
        var propertyInfo = typeof(T).GetProperty(propertyName)!;

        void propertySetter(T target, TProperty newValue) =>
            propertyInfo.SetValue(target, newValue);

        return propertySetter;
    }

    private static void CheckBindingArgs<TTarget, TSource, TTargetProperty, TSourceProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TTargetProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TSourceProperty>> sourcePropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(targetPropertyGetterExpr, nameof(targetPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(sourcePropertyGetterExpr, nameof(sourcePropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(sourcePropertyGetterExpr, nameof(sourcePropertyGetterExpr));

        string targetPropertyName = GetPropertyName(targetPropertyGetterExpr),
               sourcePropertyName = GetPropertyName(sourcePropertyGetterExpr);

        var isBindingExist =  _bindings.Any(b =>
            b.Context == context &&
            b.Target == (object)target &&
            b.TargetPropertyName == targetPropertyName &&
            b.Source == (object)source &&
            b.SourcePropertyName == sourcePropertyName);

        if (isBindingExist)
            throw new BindingException($"The property \"{targetPropertyName}\" of the target object \"{target}\" " +
                $"is already bound to the property \"{sourcePropertyName}\" of the source object \"{source}\".");
    }

    private static void UnbindPropertyBindings(IEnumerable<PropertyBinding> bindings)
    {
        var bindingsCopy = bindings.ToArray();
        foreach (var binding in bindingsCopy)
            UnbindPropertyBinding(binding);
    }

    private static void UnbindPropertyBinding(PropertyBinding propertyBinding)
    {
        if (propertyBinding.Target is INotifyPropertyChanged observableTarget)
            observableTarget.PropertyChanged -= propertyBinding.TargetPropertyChangedEventHandler;
        else if (propertyBinding.Source is INotifyPropertyChanged observableSource)
            observableSource.PropertyChanged -= propertyBinding.SourcePropertyChangedEventHandler;

        _bindings.Remove(propertyBinding);
    }
    #endregion

    private class PropertyBinding
    {
        public object Context { get; init; }

        public object Target { get; init; }

        public string TargetPropertyName { get; init; }

        public PropertyChangedEventHandler? TargetPropertyChangedEventHandler { get; init; }

        public object Source { get; init; }

        public string SourcePropertyName { get; init; }

        public PropertyChangedEventHandler? SourcePropertyChangedEventHandler { get; init; }

        public PropertyBinding
        (
            object context,
            object target, string targetPropertyName,
            PropertyChangedEventHandler? targetPropertyChangedEventHandler,
            object source, string sourcePropertyName,
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