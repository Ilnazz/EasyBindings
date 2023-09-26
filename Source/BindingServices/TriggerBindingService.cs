using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace BindingServices;

public static class TriggerBindingService
{
    #region Data types
    private class PropertyChangedTriggerBinding
    {
        public object Context { get; init; }

        public INotifyPropertyChanged Observable { get; init; }

        public string PropertyName { get; init; }

        public PropertyChangedEventHandler EventHandler { get; init; }

        public PropertyChangedTriggerBinding(object context, INotifyPropertyChanged observable, string propertyName, PropertyChangedEventHandler eventHandler)
        {
            Context = context;
            Observable = observable;
            PropertyName = propertyName;
            EventHandler = eventHandler;
        }
    }

    private class PropertyChangingTriggerBinding
    {
        public object Context { get; init; }

        public INotifyPropertyChanging Observable { get; init; }

        public string PropertyName { get; init; }

        public PropertyChangingEventHandler EventHandler { get; init; }

        public PropertyChangingTriggerBinding(object context, INotifyPropertyChanging observable, string propertyName, PropertyChangingEventHandler eventHandler)
        {
            Context = context;
            Observable = observable;
            PropertyName = propertyName;
            EventHandler = eventHandler;
        }
    }

    private class CollectionChangedTriggerBinding
    {
        public object Context { get; init; }

        public INotifyCollectionChanged ObservableCollection { get; init; }

        public NotifyCollectionChangedEventHandler EventHandler { get; init; }

        public CollectionChangedTriggerBinding(object context, INotifyCollectionChanged observableCollection, NotifyCollectionChangedEventHandler eventHandler)
        {
            Context = context;
            ObservableCollection = observableCollection;
            EventHandler = eventHandler;
        }
    }
    #endregion

    #region Fields
    private static readonly IList<PropertyChangedTriggerBinding> _propertyChangedTriggerBindings = new List<PropertyChangedTriggerBinding>();

    private static readonly IList<PropertyChangingTriggerBinding> _propertyChangingTriggerBindings = new List<PropertyChangingTriggerBinding>();

    private static readonly IList<CollectionChangedTriggerBinding> _collectionChangedTriggerBindings = new List<CollectionChangedTriggerBinding>();
    #endregion

    #region Public methods
    #region Registering
    #region PropertyChanged
    public static void RegisterPropertyChanged<TObservable, TProperty>
    (
        object context,
        TObservable observable, Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        Action<TObservable, TProperty> trigger
    )
    where TObservable : INotifyPropertyChanged
    {
        CheckPropertyChangedOrPropertyChangingRegistrationArgs(context, observable, observablePropertyGetterExpr, trigger);
        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;
        var observablePropertyGetter = observablePropertyGetterExpr.Compile();

        void eventHandler(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger(observable, observablePropertyGetter(observable));
        }

        RegisterPropertyChangedInternal(context, observable, propertyName, eventHandler);
    }

    public static void RegisterPropertyChanged<TObservable, TProperty>
    (
        object context,
        TObservable observable, Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        Action<TProperty> trigger
    )
    where TObservable : INotifyPropertyChanged
    {
        CheckPropertyChangedOrPropertyChangingRegistrationArgs(context, observable, observablePropertyGetterExpr, trigger);
        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;
        var observablePropertyGetter = observablePropertyGetterExpr.Compile();

        void eventHandler(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger(observablePropertyGetter(observable));
        }

        RegisterPropertyChangedInternal(context, observable, propertyName, eventHandler);
    }

    public static void RegisterPropertyChanged<TObservable, TProperty>
    (
        object context,
        TObservable observable, Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        Action trigger
    )
    where TObservable : INotifyPropertyChanged
    {
        CheckPropertyChangedOrPropertyChangingRegistrationArgs(context, observable, observablePropertyGetterExpr, trigger);
        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;
        var observablePropertyGetter = observablePropertyGetterExpr.Compile();

        void eventHandler(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger();
        }

        RegisterPropertyChangedInternal(context, observable, propertyName, eventHandler);
    }
    #endregion

    #region PropertyChanging
    public static void RegisterPropertyChanging<TObservable, TProperty>
    (
        object context,
        TObservable observable, Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        Action<TObservable, TProperty> trigger
    )
    where TObservable : INotifyPropertyChanging
    {
        CheckPropertyChangedOrPropertyChangingRegistrationArgs(context, observable, observablePropertyGetterExpr, trigger);
        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;
        var observablePropertyGetter = observablePropertyGetterExpr.Compile();

        void eventHandler(object? _, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger(observable, observablePropertyGetter(observable));
        }

        RegisterPropertyChangingInternal(context, observable, propertyName, eventHandler);
    }

    public static void RegisterPropertyChanging<TObservable, TProperty>
    (
        object context,
        TObservable observable, Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        Action<TProperty> trigger
    )
    where TObservable : INotifyPropertyChanging
    {
        CheckPropertyChangedOrPropertyChangingRegistrationArgs(context, observable, observablePropertyGetterExpr, trigger);
        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;
        var observablePropertyGetter = observablePropertyGetterExpr.Compile();

        void eventHandler(object? _, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger(observablePropertyGetter(observable));
        }

        RegisterPropertyChangingInternal(context, observable, propertyName, eventHandler);
    }

    public static void RegisterPropertyChanging<TObservable, TProperty>
    (
        object context,
        TObservable observable, Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        Action trigger
    )
    where TObservable : INotifyPropertyChanging
    {
        CheckPropertyChangedOrPropertyChangingRegistrationArgs(context, observable, observablePropertyGetterExpr, trigger);
        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;
        var observablePropertyGetter = observablePropertyGetterExpr.Compile();

        void eventHandler(object? _, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger();
        }

        RegisterPropertyChangingInternal(context, observable, propertyName, eventHandler);
    }
    #endregion

    #region CollectionChanged
    public static void RegisterCollectionChanged<TObservableCollection>
    (
        object context,
        TObservableCollection observableCollection,
        Action<TObservableCollection, NotifyCollectionChangedEventArgs> trigger
    )
    where TObservableCollection : INotifyCollectionChanged
    {
        CheckCollectionChangedRegistrationArgs(context, observableCollection, trigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e) =>
            trigger(observableCollection, e);

        RegisterCollectionChangedInternal(context, observableCollection, eventHandler);
    }

    public static void RegisterCollectionChanged
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Action<NotifyCollectionChangedEventArgs> trigger)
    {
        CheckCollectionChangedRegistrationArgs(context, observableCollection, trigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e) =>
            trigger(e);

        RegisterCollectionChangedInternal(context, observableCollection, eventHandler);
    }

    public static void RegisterCollectionChanged
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Action trigger)
    {
        CheckCollectionChangedRegistrationArgs(context, observableCollection, trigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e) =>
            trigger();

        RegisterCollectionChangedInternal(context, observableCollection, eventHandler);
    }
    #endregion
    #endregion

    #region Unregistering
    #region PropertyChanged
    public static void UnregisterPropertyChanged
    (
        object context,
        INotifyPropertyChanged observable)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));

        _propertyChangedTriggerBindings
            .Where(tb => tb.Context == context && tb.Observable == observable)
            .ToList()
            .ForEach(UnregisterPropertyChangedInternal);
    }

    public static void UnregisterPropertyChanged<TObservable, TProperty>
    (
        object context,
        TObservable observable,
        Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr
    )
    where TObservable : INotifyPropertyChanged
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(observablePropertyGetterExpr, nameof(observablePropertyGetterExpr));

        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;

        _propertyChangedTriggerBindings.Where(pb =>
            pb.Context == context &&
            pb.Observable == (INotifyPropertyChanged)observable &&
            pb.PropertyName == propertyName)
                                       .ToList()
                                       .ForEach(UnregisterPropertyChangedInternal);
    }

    public static void UnregisterPropertyChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _propertyChangedTriggerBindings
            .Where(tb => tb.Context == context)
            .ToList()
            .ForEach(UnregisterPropertyChangedInternal);
    }
    #endregion

    #region PropertyChanging
    public static void UnregisterPropertyChanging
    (
        object context,
        INotifyPropertyChanging observable)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));

        _propertyChangingTriggerBindings
            .Where(tb => tb.Context == context && tb.Observable == observable)
            .ToList()
            .ForEach(UnregisterPropertyChangingInternal);
    }

    public static void UnregisterPropertyChanging<TObservable, TProperty>
    (
        object context,
        TObservable observable,
        Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr
    )
    where TObservable : INotifyPropertyChanging
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(observablePropertyGetterExpr, nameof(observablePropertyGetterExpr));

        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;

        _propertyChangingTriggerBindings.Where(pb =>
            pb.Context == context &&
            pb.Observable == (INotifyPropertyChanging)observable &&
            pb.PropertyName == propertyName)
                                        .ToList()
                                        .ForEach(UnregisterPropertyChangingInternal);
    }

    public static void UnregisterPropertyChanging(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _propertyChangingTriggerBindings
            .Where(tb => tb.Context == context)
            .ToList()
            .ForEach(UnregisterPropertyChangingInternal);
    }
    #endregion

    #region CollectionChanged
    public static void UnregisterCollectionChanged(object context, INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        _collectionChangedTriggerBindings.Where(tb =>
            tb.Context == context && tb.ObservableCollection == observableCollection)
                                         .ToList()
                                         .ForEach(UnregisterCollectionChanged);
    }

    public static void UnregisterCollectionChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _collectionChangedTriggerBindings
            .Where(tb => tb.Context == context)
            .ToList()
            .ForEach(UnregisterCollectionChanged);
    }
    #endregion

    public static void Unregister(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnregisterPropertyChanged(context);
        UnregisterCollectionChanged(context);
    }
    #endregion
    #endregion

    #region Private methods
    #region Checking
    private static void CheckPropertyChangedOrPropertyChangingRegistrationArgs(object context, object observable, object propertyGetterExpr, object trigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(propertyGetterExpr, nameof(propertyGetterExpr));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));
    }

    private static void CheckCollectionChangedRegistrationArgs(object context, object observableCollection, object trigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));
    }
    #endregion

    #region Registering
    private static void RegisterPropertyChangedInternal(object context, INotifyPropertyChanged observable, string propertyName, PropertyChangedEventHandler eventHandler)
    {
        observable.PropertyChanged += eventHandler;
        _propertyChangedTriggerBindings.Add(new PropertyChangedTriggerBinding(context, observable, propertyName, eventHandler));
    }

    private static void RegisterPropertyChangingInternal(object context, INotifyPropertyChanging observable, string propertyName, PropertyChangingEventHandler eventHandler)
    {
        observable.PropertyChanging += eventHandler;
        _propertyChangingTriggerBindings.Add(new PropertyChangingTriggerBinding(context, observable, propertyName, eventHandler));
    }

    private static void RegisterCollectionChangedInternal(object context, INotifyCollectionChanged observableCollection, NotifyCollectionChangedEventHandler eventHandler)
    {
        observableCollection.CollectionChanged += eventHandler;
        _collectionChangedTriggerBindings.Add(new CollectionChangedTriggerBinding(context, observableCollection, eventHandler));
    }
    #endregion

    #region Unregistering
    private static void UnregisterPropertyChangedInternal(PropertyChangedTriggerBinding triggerBinding)
    {
        triggerBinding.Observable.PropertyChanged -= triggerBinding.EventHandler;
        _propertyChangedTriggerBindings.Remove(triggerBinding);
    }

    private static void UnregisterPropertyChangingInternal(PropertyChangingTriggerBinding triggerBinding)
    {
        triggerBinding.Observable.PropertyChanging -= triggerBinding.EventHandler;
        _propertyChangingTriggerBindings.Remove(triggerBinding);
    }

    private static void UnregisterCollectionChanged(CollectionChangedTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;
        _collectionChangedTriggerBindings.Remove(triggerBinding);
    }
    #endregion
    #endregion
}
