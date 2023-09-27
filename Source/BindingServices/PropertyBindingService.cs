using System.ComponentModel;
using System.Linq.Expressions;

namespace BindingServices;

public static class PropertyBindingService
{
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

    private static readonly IList<PropertyBinding> _propertyBindings = new List<PropertyBinding>();

    #region Public methods 
    #region Registering
    public static void OneWay<TTarget, TSource, TProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TProperty>> sourcePropertyGetterExpr
    )
    where TTarget : class where TSource : INotifyPropertyChanged
    {
        CheckRegistrationArgs(context, targetPropertyGetterExpr, targetPropertyGetterExpr, source, sourcePropertyGetterExpr);
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

    public static void OneWay<TTarget, TSource, TTargetProperty, TSourceProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TTargetProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TSourceProperty>> sourcePropertyGetterExpr,
        Func<TSourceProperty, TTargetProperty> converter
    )
    where TTarget : class where TSource : INotifyPropertyChanged
    {
        CheckRegistrationArgs(context, targetPropertyGetterExpr, targetPropertyGetterExpr, source, sourcePropertyGetterExpr);
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

    public static void OneWayToSource<TTarget, TSource, TProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TProperty>> sourcePropertyGetterExpr
    )
    where TTarget : INotifyPropertyChanged where TSource : class
    {
        OneWay(context, source, sourcePropertyGetterExpr, target, targetPropertyGetterExpr);
    }

    public static void OneWayToSource<TTarget, TSource, TTargetProperty, TSourceProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TTargetProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TSourceProperty>> sourcePropertyGetterExpr,
        Func<TTargetProperty, TSourceProperty> converter
    )
    where TTarget : INotifyPropertyChanged where TSource : class
    {
        OneWay(context, source, sourcePropertyGetterExpr, target, targetPropertyGetterExpr, converter);
    }

    public static void TwoWay<TTarget, TSource, TProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TProperty>> sourcePropertyGetterExpr
    )
    where TTarget : class, INotifyPropertyChanged where TSource : class, INotifyPropertyChanged
    {
        OneWay(context, target, targetPropertyGetterExpr, source, sourcePropertyGetterExpr);
        OneWay(context, source, sourcePropertyGetterExpr, target, targetPropertyGetterExpr);
    }

    public static void TwoWay<TTarget, TSource, TTargetProperty, TSourceProperty>
    (
        object context,
        TTarget target, Expression<Func<TTarget, TTargetProperty>> targetPropertyGetterExpr,
        TSource source, Expression<Func<TSource, TSourceProperty>> sourcePropertyGetterExpr,
        Func<TTargetProperty, TSourceProperty> targetValueConverter,
        Func<TSourceProperty, TTargetProperty> sourceValueConverter
    )
    where TTarget : class, INotifyPropertyChanged where TSource : class, INotifyPropertyChanged
    {
        OneWay(context, target, targetPropertyGetterExpr, source, sourcePropertyGetterExpr, sourceValueConverter);
        OneWay(context, source, sourcePropertyGetterExpr, target, targetPropertyGetterExpr, targetValueConverter);
    }
    #endregion

    #region Unregistering
    public static void UnregisterFromTarget<T, TProperty>(object context, object target, Expression<Func<T, TProperty>> targetPropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(targetPropertyGetterExpr, nameof(targetPropertyGetterExpr));

        var targetPropertyName = GetPropertyName(targetPropertyGetterExpr);

        _propertyBindings.Where(pb => pb.Context == context && pb.Target == target && pb.TargetPropertyName == targetPropertyName)
            .ToList().ForEach(Unregister);
    }

    public static void UnregisterFromTarget(object context, object target)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        _propertyBindings.Where(pb => pb.Context == context && pb.Target == target).ToList().ForEach(Unregister);
    }

    public static void UnregisterFromSource<T, TProperty>(object context, object source, Expression<Func<T, TProperty>> sourcePropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(sourcePropertyGetterExpr, nameof(sourcePropertyGetterExpr));

        var sourcePropertyName = GetPropertyName(sourcePropertyGetterExpr);

        _propertyBindings.Where(pb => pb.Context == context && pb.Source == source && pb.SourcePropertyName == sourcePropertyName)
            .ToList().ForEach(Unregister);
    }

    public static void UnregisterFromSource(object context, object source)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        _propertyBindings.Where(pb => pb.Context == context && pb.Source == source).ToList().ForEach(Unregister);
    }

    public static void Unregister(object context) => _propertyBindings.Where(pb => pb.Context == context).ToList().ForEach(Unregister);

    private static void Unregister(PropertyBinding propertyBinding)
    {
        if (propertyBinding.Target is INotifyPropertyChanged observableTarget)
            observableTarget.PropertyChanged -= propertyBinding.TargetPropertyChangedEventHandler;
        else if (propertyBinding.Source is INotifyPropertyChanged observableSource)
            observableSource.PropertyChanged -= propertyBinding.SourcePropertyChangedEventHandler;

        _propertyBindings.Remove(propertyBinding);
    }
    #endregion
    #endregion

    #region Private methods 
    private static void CheckRegistrationArgs
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
    #endregion
}