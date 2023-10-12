using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace EasyBindings;

/// <summary>
/// Allows you to bind event handlers (triggers) to a <see cref="INotifyPropertyChanged"/>, <see cref="INotifyPropertyChanging"/> and <see cref="INotifyCollectionChanged"/> events.
/// </summary>
public static class TriggerBinder
{
    #region Fields
    private static readonly IList<PropertyChangedTriggerBinding>
        _propertyChangedTriggerBindings = new List<PropertyChangedTriggerBinding>();

    private static readonly IList<PropertyChangingTriggerBinding>
        _propertyChangingTriggerBindings = new List<PropertyChangingTriggerBinding>();

    private static readonly IList<CollectionChangedTriggerBinding>
        _collectionChangedTriggerBindings = new List<CollectionChangedTriggerBinding>();

    private static readonly IList<CollectionChangedAndItemPropertyChangedTriggerBinding>
        _collectionChangedAndItemPropertyChangedTriggerBindings = new List<CollectionChangedAndItemPropertyChangedTriggerBinding>();

    private static readonly IList<CollectionChangedAndItemPropertyChangingTriggerBinding>
        _collectionChangedAndItemPropertyChangingTriggerBindings = new List<CollectionChangedAndItemPropertyChangingTriggerBinding>();

    private static readonly IList<CollectionItemPropertyChangedTriggerBinding>
        _collectionItemPropertyChangedTriggerBindings = new List<CollectionItemPropertyChangedTriggerBinding>();

    private static readonly IList<CollectionItemPropertyChangingTriggerBinding>
        _collectionItemPropertyChangingTriggerBindings = new List<CollectionItemPropertyChangingTriggerBinding>();

    #endregion

    #region Public methods
    #region Binding methods
    #region PropertyChanged
    /// <summary>
    /// Binds a trigger to changes of a given property in the <paramref name="observable"/> object.
    /// </summary>
    /// <typeparam name="TObservable">The type of the observable object.</typeparam>
    /// <typeparam name="TProperty">The type of the <paramref name="observable"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="observable">The observable object.</param>
    /// <param name="observablePropertyGetterExpr">An expression representing the getter of the observable property.</param>
    /// <param name="trigger">The trigger to be executed when the property changes.</param>
    public static void OnPropertyChanged<TObservable, TProperty>
    (
        object context,
        TObservable observable, Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        Action<TObservable, TProperty> trigger
    )
    where TObservable : INotifyPropertyChanged
    {
        CheckPropertyChangedBindingArgs(context, observable, observablePropertyGetterExpr, trigger);

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        var observablePropertyGetter = observablePropertyGetterExpr.Compile();

        void eventHandler(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger(observable, observablePropertyGetter(observable));
        }

        OnPropertyChangedInternal(context, observable, propertyName, trigger, eventHandler);
    }

    /// <summary>
    /// Binds a trigger to changes of a given property in the <paramref name="observable"/> object.
    /// </summary>
    /// <typeparam name="TObservable">The type of the observable object.</typeparam>
    /// <typeparam name="TProperty">The type of the <paramref name="observable"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="observable">The observable object.</param>
    /// <param name="observablePropertyGetterExpr">An expression representing the getter of the observable property.</param>
    /// <param name="trigger">The trigger to be executed when the property changes.</param>
    public static void OnPropertyChanged<TObservable, TProperty>
    (
        object context,
        TObservable observable, Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        Action<TProperty> trigger
    )
    where TObservable : INotifyPropertyChanged
    {
        CheckPropertyChangedBindingArgs(context, observable, observablePropertyGetterExpr, trigger);

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        var observablePropertyGetter = observablePropertyGetterExpr.Compile();

        void eventHandler(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger(observablePropertyGetter(observable));
        }

        OnPropertyChangedInternal(context, observable, propertyName, trigger, eventHandler);
    }

    /// <summary>
    /// Binds a trigger to changes of a given property in the <paramref name="observable"/> object.
    /// </summary>
    /// <typeparam name="TObservable">The type of the observable object.</typeparam>
    /// <typeparam name="TProperty">The type of the <paramref name="observable"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="observable">The observable object.</param>
    /// <param name="observablePropertyGetterExpr">An expression representing the getter of the observable property.</param>
    /// <param name="trigger">The trigger to be executed when the property changes.</param>
    public static void OnPropertyChanged<TObservable, TProperty>
    (
        object context,
        TObservable observable, Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        Action trigger
    )
    where TObservable : INotifyPropertyChanged
    {
        CheckPropertyChangedBindingArgs(context, observable, observablePropertyGetterExpr, trigger);

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        var observablePropertyGetter = observablePropertyGetterExpr.Compile();

        void eventHandler(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger();
        }

        OnPropertyChangedInternal(context, observable, propertyName, trigger, eventHandler);
    }
    #endregion

    #region PropertyChanging
    /// <summary>
    /// Binds a trigger to changes of a given property in the <paramref name="observable"/> object.
    /// </summary>
    /// <typeparam name="TObservable">The type of the observable object.</typeparam>
    /// <typeparam name="TProperty">The type of the <paramref name="observable"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="observable">The observable object.</param>
    /// <param name="observablePropertyGetterExpr">An expression representing the getter of the observable property.</param>
    /// <param name="trigger">The trigger to be executed when the property changes.</param>
    public static void OnPropertyChanging<TObservable, TProperty>
    (
        object context,
        TObservable observable, Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        Action<TObservable, TProperty> trigger
    )
    where TObservable : INotifyPropertyChanging
    {
        CheckPropertyChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        var observablePropertyGetter = observablePropertyGetterExpr.Compile();

        void eventHandler(object? _, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger(observable, observablePropertyGetter(observable));
        }

        OnPropertyChangingInternal(context, observable, propertyName, trigger, eventHandler);
    }

    /// <summary>
    /// Binds a trigger to changes of a given property in the <paramref name="observable"/> object.
    /// </summary>
    /// <typeparam name="TObservable">The type of the observable object.</typeparam>
    /// <typeparam name="TProperty">The type of the <paramref name="observable"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="observable">The observable object.</param>
    /// <param name="observablePropertyGetterExpr">An expression representing the getter of the observable property.</param>
    /// <param name="trigger">The trigger to be executed when the property changes.</param>
    public static void OnPropertyChanging<TObservable, TProperty>
    (
        object context,
        TObservable observable, Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        Action<TProperty> trigger
    )
    where TObservable : INotifyPropertyChanging
    {
        CheckPropertyChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        var observablePropertyGetter = observablePropertyGetterExpr.Compile();

        void eventHandler(object? _, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger(observablePropertyGetter(observable));
        }

        OnPropertyChangingInternal(context, observable, propertyName, trigger, eventHandler);
    }

    /// <summary>
    /// Binds a trigger to changes of a given property in the <paramref name="observable"/> object.
    /// </summary>
    /// <typeparam name="TObservable">The type of the observable object.</typeparam>
    /// <typeparam name="TProperty">The type of the <paramref name="observable"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="observable">The observable object.</param>
    /// <param name="observablePropertyGetterExpr">An expression representing the getter of the observable property.</param>
    /// <param name="trigger">The trigger to be executed when the property changes.</param>
    public static void OnPropertyChanging<TObservable, TProperty>
    (
        object context,
        TObservable observable, Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        Action trigger
    )
    where TObservable : INotifyPropertyChanging
    {
        CheckPropertyChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        var observablePropertyGetter = observablePropertyGetterExpr.Compile();

        void eventHandler(object? _, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger();
        }

        OnPropertyChangingInternal(context, observable, propertyName, trigger, eventHandler);
    }
    #endregion

    #region CollectionChanged
    /// <summary>
    /// Binds a trigger to changes in the <paramref name="observableCollection"/> object.
    /// </summary>
    /// <typeparam name="TObservableCollection">The type of the observable object.</typeparam>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="observableCollection">The observable collection object.</param>
    /// <param name="trigger">The trigger to be executed when the collection changes.</param>
    public static void OnCollectionChanged<TObservableCollection>
    (
        object context,
        TObservableCollection observableCollection,
        Action<TObservableCollection, NotifyCollectionChangedEventArgs> trigger
    )
    where TObservableCollection : INotifyCollectionChanged
    {
        CheckCollectionChangedBindingArgs(context, observableCollection, trigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e) =>
            trigger(observableCollection, e);

        OnCollectionChangedInternal(context, observableCollection, trigger, eventHandler);
    }

    /// <summary>
    /// Binds a trigger to changes in the <paramref name="observableCollection"/> object.
    /// </summary>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="observableCollection">The observable collection object.</param>
    /// <param name="trigger">The trigger to be executed when the collection changes.</param>
    public static void OnCollectionChanged
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Action<NotifyCollectionChangedEventArgs> trigger)
    {
        CheckCollectionChangedBindingArgs(context, observableCollection, trigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e) =>
            trigger(e);

        OnCollectionChangedInternal(context, observableCollection, trigger, eventHandler);
    }

    /// <summary>
    /// Binds a trigger to changes in the <paramref name="observableCollection"/> object.
    /// </summary>
    /// <param name="context">The context in which the binding is being made.</param>
    /// <param name="observableCollection">The observable collection object.</param>
    /// <param name="trigger">The trigger to be executed when the collection changes.</param>
    public static void OnCollectionChanged
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Action trigger)
    {
        CheckCollectionChangedBindingArgs(context, observableCollection, trigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e) =>
            trigger();

        OnCollectionChangedInternal(context, observableCollection, trigger, eventHandler);
    }
    #endregion

    #region CollectionChangedAndItemPropertyChanged
    #region First group (with collectionChangedTrigger with two parameters)
    public static void OnCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action<ObservableCollection<TItem>, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItem, TItemProperty> itemPropertyChangedTrigger
    )
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(observableCollection, e);

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, eventHandler);
    }

    public static void OnCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action<ObservableCollection<TItem>, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItemProperty> itemPropertyChangedTrigger
    )
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(observableCollection, e);

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, eventHandler);
    }

    public static void OnCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action<ObservableCollection<TItem>, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangedTrigger
    )
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(observableCollection, e);

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, eventHandler);
    }
    #endregion

    #region Second group (with collectionChangedTrigger with one parameter)
    public static void OnCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItem, TItemProperty> itemPropertyChangedTrigger
    )
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(e);

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, eventHandler);
    }

    public static void OnCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItemProperty> itemPropertyChangedTrigger
    )
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(e);

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, eventHandler);
    }

    public static void OnCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangedTrigger
    )
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(e);

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, eventHandler);
    }
    #endregion

    #region Third group (with collectionChangedTrigger without parameters)
    public static void OnCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItem, TItemProperty> itemPropertyChangedTrigger
    )
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger();

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, eventHandler);
    }

    public static void OnCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItemProperty> itemPropertyChangedTrigger
    )
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger();

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, eventHandler);
    }

    public static void OnCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangedTrigger
    )
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger();

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, eventHandler);
    }
    #endregion
    #endregion

    #region CollectionChangedAndItemPropertyChanging
    #region First group (with collectionChangedTrigger with two parameters)
    public static void OnCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action<ObservableCollection<TItem>, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItem, TItemProperty> itemPropertyChangingTrigger
    )
    where TItem : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(observableCollection, e);

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, eventHandler);
    }

    public static void OnCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action<ObservableCollection<TItem>, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItemProperty> itemPropertyChangingTrigger
    )
    where TItem : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(observableCollection, e);

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, eventHandler);
    }

    public static void OnCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action<ObservableCollection<TItem>, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangingTrigger
    )
    where TItem : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(observableCollection, e);

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, eventHandler);
    }
    #endregion

    #region Second group (with collectionChangedTrigger with one parameter)
    public static void OnCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItem, TItemProperty> itemPropertyChangingTrigger
    )
    where TItem : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(e);

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, eventHandler);
    }

    public static void OnCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItemProperty> itemPropertyChangingTrigger
    )
    where TItem : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(e);

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, eventHandler);
    }

    public static void OnCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangingTrigger
    )
    where TItem : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(e);

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, eventHandler);
    }
    #endregion

    #region Third group (with collectionChangedTrigger without parameters)
    public static void OnCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItem, TItemProperty> itemPropertyChangingTrigger
    )
    where TItem : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger();

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, eventHandler);
    }

    public static void OnCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItemProperty> itemPropertyChangingTrigger
    )
    where TItem : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger();

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, eventHandler);
    }

    public static void OnCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangingTrigger
    )
    where TItem : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger();

            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, eventHandler);
    }
    #endregion
    #endregion

    #region CollectionAndItemPropertyChanged
    public static void OnCollectionItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItem, TItemProperty> itemPropertyChangedTrigger
    )
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionItemPropertyChangedBindingArgs(context,
            observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
        }

        OnCollectionItemPropertyChangedInternal(context, observableCollection,
            GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, eventHandler);
    }

    public static void OnCollectionItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItemProperty> itemPropertyChangedTrigger
    )
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionItemPropertyChangedBindingArgs(context,
            observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
        }

        OnCollectionItemPropertyChangedInternal(context, observableCollection,
            GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, eventHandler);
    }

    public static void OnCollectionItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangedTrigger
    )
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionItemPropertyChangedBindingArgs(context,
            observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
        }

        OnCollectionItemPropertyChangedInternal(context, observableCollection,
            GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, eventHandler);
    }
    #endregion

    #region CollectionAndItemPropertyChanging
    public static void OnCollectionItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItem, TItemProperty> itemPropertyChangingTrigger
    )
    where TItem : INotifyPropertyChanging
    {
        CheckCollectionItemPropertyChangingBindingArgs(context,
            observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
        }

        OnCollectionItemPropertyChangingInternal(context, observableCollection,
            GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, eventHandler);
    }

    public static void OnCollectionItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItemProperty> itemPropertyChangingTrigger
    )
    where TItem : INotifyPropertyChanging
    {
        CheckCollectionItemPropertyChangingBindingArgs(context,
            observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
        }

        OnCollectionItemPropertyChangingInternal(context, observableCollection,
            GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, eventHandler);
    }

    public static void OnCollectionItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        ObservableCollection<TItem> observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangingTrigger
    )
    where TItem : INotifyPropertyChanging
    {
        CheckCollectionItemPropertyChangingBindingArgs(context,
            observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (TItem item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void eventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.NewItems is not null)
                foreach (TItem item in e.NewItems)
                    OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

            if (e.OldItems is not null)
                foreach (TItem item in e.OldItems)
                    UnbindPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
        }

        OnCollectionItemPropertyChangingInternal(context, observableCollection,
            GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, eventHandler);
    }
    #endregion
    #endregion

    #region Unbinding methods
    #region PropertyChanged
    /// <summary>
    /// Unbinds a given trigger from <paramref name="observable"/> object's property in a given context.
    /// </summary>
    /// <typeparam name="TObservable">Type of the observable object.</typeparam>
    /// <typeparam name="TProperty">Type of the <paramref name="observable"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observable">Observable object.</param>
    /// <param name="observablePropertyGetterExpr">An expression representing the getter of the observable property.</param>
    /// <param name="trigger">The trigger which was bound.</param>
    public static void UnbindPropertyChanged<TObservable, TProperty>
    (
        object context,
        TObservable observable,
        Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        object trigger
    )
    where TObservable : INotifyPropertyChanged
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(observablePropertyGetterExpr, nameof(observablePropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        UnbindPropertyChangedBindings(
            _propertyChangedTriggerBindings
            .Where(b => b.Context == context &&
                        b.Observable == (INotifyPropertyChanged)observable &&
                        b.PropertyName == propertyName &&
                        b.Trigger == trigger));
    }

    /// <summary>
    /// Unbinds all triggers from <paramref name="observable"/> object's property in a given context.
    /// </summary>
    /// <typeparam name="TObservable">Type of the observable object.</typeparam>
    /// <typeparam name="TProperty">Type of the <paramref name="observable"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observable">Observable object.</param>
    /// <param name="observablePropertyGetterExpr">An expression representing the getter of the observable property.</param>
    public static void UnbindPropertyChanged<TObservable, TProperty>
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

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        UnbindPropertyChangedBindings(
            _propertyChangedTriggerBindings
            .Where(b => b.Context == context &&
                        b.Observable == (INotifyPropertyChanged)observable &&
                        b.PropertyName == propertyName));
    }

    /// <summary>
    /// Unbinds all triggers from <paramref name="observable"/> object in the given context.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observable">Observable object.</param>
    public static void UnbindPropertyChanged
    (
        object context,
        INotifyPropertyChanged observable)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));

        UnbindPropertyChangedBindings(
            _propertyChangedTriggerBindings.Where(b => b.Context == context && b.Observable == observable));
    }

    /// <summary>
    /// Unbinds all trigger bindings made in a given context
    /// </summary>
    /// <param name="context"></param>
    public static void UnbindPropertyChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindPropertyChangedBindings(_propertyChangedTriggerBindings.Where(b => b.Context == context));
    }
    #endregion

    #region PropertyChanging
    public static void UnbindPropertyChanging<TObservable, TProperty>
    (
        object context,
        TObservable observable,
        Expression<Func<TObservable, TProperty>> observablePropertyGetterExpr,
        object trigger
    )
    where TObservable : INotifyPropertyChanging
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(observablePropertyGetterExpr, nameof(observablePropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        UnbindPropertyChangingBindings(
            _propertyChangingTriggerBindings
            .Where(b => b.Context == context &&
                        b.Observable == (INotifyPropertyChanging)observable &&
                        b.PropertyName == propertyName &&
                        b.Trigger == trigger));
    }

    public static void UnbindPropertyChanging<TObservable, TProperty>
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

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        UnbindPropertyChangingBindings(
            _propertyChangingTriggerBindings
            .Where(b => b.Context == context &&
                        b.Observable == (INotifyPropertyChanging)observable &&
                        b.PropertyName == propertyName));
    }

    public static void UnbindPropertyChanging
    (
        object context,
        INotifyPropertyChanging observable)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));

        UnbindPropertyChangingBindings(
            _propertyChangingTriggerBindings.Where(b => b.Context == context && b.Observable == observable));
    }

    public static void UnbindPropertyChanging(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindPropertyChangingBindings(_propertyChangingTriggerBindings.Where(b => b.Context == context));
    }
    #endregion

    #region CollectionChanged
    public static void UnbindCollectionChanged
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object trigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));

        UnbindCollectionChangedBindings(
            _collectionChangedTriggerBindings
            .Where(b => b.Context == context &&
                        b.ObservableCollection == observableCollection &&
                        b.Trigger == trigger));
    }

    public static void UnbindCollectionChanged(object context, INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        UnbindCollectionChangedBindings(
            _collectionChangedTriggerBindings
            .Where(b => b.Context == context && b.ObservableCollection == observableCollection));
    }

    public static void UnbindCollectionChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindCollectionChangedBindings(_collectionChangedTriggerBindings.Where(b => b.Context == context));
    }
    #endregion

    #region CollectionChangedAndItemPropertyChanged
    public static void UnbindCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        object itemPropertyChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangedTrigger, nameof(itemPropertyChangedTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionChangedAndItemPropertyChangedBindings(
            _collectionChangedAndItemPropertyChangedTriggerBindings
                .Where(b => b.Context == context &&
                            b.ObservableCollection == observableCollection &&
                            b.CollectionChangedTrigger == collectionChangedTrigger &&
                            b.ItemPropertyName == itemPropertyName &&
                            b.ItemPropertyChangedTrigger == itemPropertyChangedTrigger));
    }

    public static void UnbindCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionChangedAndItemPropertyChangedBindings(
            _collectionChangedAndItemPropertyChangedTriggerBindings
                .Where(b => b.Context == context &&
                            b.ObservableCollection == observableCollection &&
                            b.CollectionChangedTrigger == collectionChangedTrigger &&
                            b.ItemPropertyName == itemPropertyName));
    }

    public static void UnbindCollectionChangedAndItemPropertyChanged
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object collectionChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));

        UnbindCollectionChangedAndItemPropertyChangedBindings(
            _collectionChangedAndItemPropertyChangedTriggerBindings
                .Where(b => b.Context == context &&
                            b.ObservableCollection == observableCollection &&
                            b.CollectionChangedTrigger == collectionChangedTrigger));
    }

    public static void UnbindCollectionChangedAndItemPropertyChanged
    (
        object context,
        INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        UnbindCollectionChangedAndItemPropertyChangedBindings(
            _collectionChangedAndItemPropertyChangedTriggerBindings
                .Where(b => b.Context == context && b.ObservableCollection == observableCollection));
    }

    public static void UnbindCollectionChangedAndItemPropertyChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindCollectionChangedAndItemPropertyChangedBindings(
            _collectionChangedAndItemPropertyChangedTriggerBindings.Where(b => b.Context == context));
    }
    #endregion

    #region CollectionChangedAndItemPropertyChanging
    public static void UnbindCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        object itemPropertyChangingTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangingTrigger, nameof(itemPropertyChangingTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionChangedAndItemPropertyChangingBindings(
            _collectionChangedAndItemPropertyChangingTriggerBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.CollectionChangedTrigger == collectionChangedTrigger &&
                         tb.ItemPropertyName == itemPropertyName &&
                         tb.ItemPropertyChangingTrigger == itemPropertyChangingTrigger));
    }

    public static void UnbindCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionChangedAndItemPropertyChangingBindings(
            _collectionChangedAndItemPropertyChangingTriggerBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.CollectionChangedTrigger == collectionChangedTrigger &&
                         tb.ItemPropertyName == itemPropertyName));
    }

    public static void UnbindCollectionChangedAndItemPropertyChanging
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object collectionChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));

        UnbindCollectionChangedAndItemPropertyChangingBindings(
            _collectionChangedAndItemPropertyChangingTriggerBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.CollectionChangedTrigger == collectionChangedTrigger));
    }

    public static void UnbindCollectionChangedAndItemPropertyChanging
    (
        object context,
        INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        UnbindCollectionChangedAndItemPropertyChangingBindings(
            _collectionChangedAndItemPropertyChangingTriggerBindings
            .Where(tb => tb.Context == context && tb.ObservableCollection == observableCollection));
    }

    public static void UnbindCollectionChangedAndItemPropertyChanging(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindCollectionChangedAndItemPropertyChangingBindings(
            _collectionChangedAndItemPropertyChangingTriggerBindings.Where(tb => tb.Context == context));
    }
    #endregion

    #region CollectionItemPropertyChanged
    public static void UnbindCollectionItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        object itemPropertyChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangedTrigger, nameof(itemPropertyChangedTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionItemPropertyChangedBindings(
            _collectionItemPropertyChangedTriggerBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.ItemPropertyName == itemPropertyName &&
                         tb.ItemPropertyChangedTrigger == itemPropertyChangedTrigger));
    }

    public static void UnbindCollectionItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionItemPropertyChangedBindings(
            _collectionItemPropertyChangedTriggerBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.ItemPropertyName == itemPropertyName));
    }

    public static void UnbindCollectionItemPropertyChanged
    (
        object context,
        INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        UnbindCollectionItemPropertyChangedBindings(
            _collectionItemPropertyChangedTriggerBindings
            .Where(tb => tb.Context == context && tb.ObservableCollection == observableCollection));
    }

    public static void UnbindCollectionItemPropertyChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindCollectionItemPropertyChangedBindings(
            _collectionItemPropertyChangedTriggerBindings.Where(tb => tb.Context == context));
    }
    #endregion

    #region CollectionItemPropertyChanging
    public static void UnbindCollectionItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        object itemPropertyChangingTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangingTrigger, nameof(itemPropertyChangingTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionItemPropertyChangingBindings(
            _collectionItemPropertyChangingTriggerBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.ItemPropertyName == itemPropertyName &&
                         tb.ItemPropertyChangingTrigger == itemPropertyChangingTrigger));
    }

    public static void UnbindCollectionItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionItemPropertyChangingBindings(
            _collectionItemPropertyChangingTriggerBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.ItemPropertyName == itemPropertyName));
    }

    public static void UnbindCollectionItemPropertyChanging
    (
        object context,
        INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        UnbindCollectionItemPropertyChangingBindings(
            _collectionItemPropertyChangingTriggerBindings
            .Where(tb => tb.Context == context && tb.ObservableCollection == observableCollection));
    }

    public static void UnbindCollectionItemPropertyChanging(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindCollectionItemPropertyChangingBindings(
            _collectionItemPropertyChangingTriggerBindings.Where(tb => tb.Context == context));
    }
    #endregion

    public static void Unbind(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindPropertyChanged(context);
        UnbindPropertyChanging(context);
        UnbindCollectionChanged(context);
        UnbindCollectionChangedAndItemPropertyChanged(context);
        UnbindCollectionChangedAndItemPropertyChanging(context);
        UnbindCollectionItemPropertyChanged(context);
        UnbindCollectionItemPropertyChanging(context);
    }
    #endregion
    #endregion

    #region Private methods
    private static string GetPropertyName<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyGetterExpr) =>
        ((MemberExpression)propertyGetterExpr.Body).Member.Name;

    #region Argument checking methods
    private static void CheckPropertyChangedBindingArgs<TObservable, TProperty>
    (
        object context,
        object observable,
        Expression<Func<TObservable, TProperty>> propertyGetterExpr,
        object trigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(propertyGetterExpr, nameof(propertyGetterExpr));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));

        var propertyName = GetPropertyName(propertyGetterExpr);
        var isBindingExist = _propertyChangedTriggerBindings.Any(b =>
            b.Context == context &&
            b.Observable == observable &&
            b.PropertyName == propertyName &&
            b.Trigger == trigger);

        if (isBindingExist)
            throw new Exception("This trigger is already bound to the property of the observable object in this context.");
    }

    private static void CheckPropertyChangingBindingArgs<TObservable, TProperty>
    (
        object context,
        object observable,
        Expression<Func<TObservable, TProperty>> propertyGetterExpr,
        object trigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(propertyGetterExpr, nameof(propertyGetterExpr));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));

        var propertyName = GetPropertyName(propertyGetterExpr);
        var isBindingExist = _propertyChangingTriggerBindings.Any(b =>
            b.Context == context &&
            b.Observable == observable &&
            b.PropertyName == propertyName &&
            b.Trigger == trigger);

        if (isBindingExist)
            throw new Exception("This trigger is already bound to the property of the observable object in this context.");
    }

    private static void CheckCollectionChangedBindingArgs
    (
        object context,
        object observableCollection,
        object trigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));

        var isBindingExist = _collectionChangedTriggerBindings.Any(b =>
            b.Context == context &&
            b.ObservableCollection == observableCollection &&
            b.Trigger == trigger);

        if (isBindingExist)
            throw new Exception("This trigger is already bound to the observable collection in this context.");
    }

    private static void CheckCollectionChangedAndItemPropertyChangedBindingArgs<TItem, TItemProperty>
    (
        object context,
        object observableCollection,
        object collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        object itemPropertyChangedTrigger)
    {
        CheckCollectionChangedBindingArgs(context, observableCollection, collectionChangedTrigger);
        
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangedTrigger, nameof(itemPropertyChangedTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        var isBindingExist = _collectionChangedAndItemPropertyChangedTriggerBindings.Any(b =>
            b.Context == context &&
            b.ObservableCollection == observableCollection &&
            b.ItemPropertyName == itemPropertyName &&
            b.ItemPropertyChangedTrigger == itemPropertyChangedTrigger);

        if (isBindingExist)
            throw new Exception("This trigger is already bound to the property of an item of the observable collection in this context.");
    }

    private static void CheckCollectionChangedAndItemPropertyChangingBindingArgs<TItem, TItemProperty>
    (
        object context,
        object observableCollection,
        object collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        object itemPropertyChangingTrigger)
    {
        CheckCollectionChangedBindingArgs(context, observableCollection, collectionChangedTrigger);

        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangingTrigger, nameof(itemPropertyChangingTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        var isBindingExist = _collectionChangedAndItemPropertyChangingTriggerBindings.Any(b =>
            b.Context == context &&
            b.ObservableCollection == observableCollection &&
            b.ItemPropertyName == itemPropertyName &&
            b.ItemPropertyChangingTrigger == itemPropertyChangingTrigger);

        if (isBindingExist)
            throw new Exception("This trigger is already bound to the property of an item of the observable collection in this context.");
    }

    private static void CheckCollectionItemPropertyChangedBindingArgs<TItem, TItemProperty>
    (
        object context,
        object observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        object itemPropertyChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangedTrigger, nameof(itemPropertyChangedTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        var isBindingExist = _collectionItemPropertyChangedTriggerBindings.Any(b =>
            b.Context == context &&
            b.ObservableCollection == observableCollection &&
            b.ItemPropertyName == itemPropertyName &&
            b.ItemPropertyChangedTrigger == itemPropertyChangedTrigger);

        if (isBindingExist)
            throw new Exception("This trigger is already bound to the property of an item of the observable collection in this context.");
    }

    private static void CheckCollectionItemPropertyChangingBindingArgs<TItem, TItemProperty>
    (
        object context,
        object observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        object itemPropertyChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangedTrigger, nameof(itemPropertyChangedTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        var isBindingExist = _collectionItemPropertyChangingTriggerBindings.Any(b =>
            b.Context == context &&
            b.ObservableCollection == observableCollection &&
            b.ItemPropertyName == itemPropertyName &&
            b.ItemPropertyChangingTrigger == itemPropertyChangedTrigger);

        if (isBindingExist)
            throw new Exception("This trigger is already bound to the property of an item of the observable collection in this context.");
    }
    #endregion

    #region Binding methods
    private static void OnPropertyChangedInternal
    (
        object context,
        INotifyPropertyChanged observable,
        string propertyName,
        object trigger,
        PropertyChangedEventHandler eventHandler)
    {
        observable.PropertyChanged += eventHandler;
        _propertyChangedTriggerBindings.Add(new PropertyChangedTriggerBinding
        (
            context,
            observable, propertyName,
            trigger, eventHandler
        ));
    }

    private static void OnPropertyChangingInternal
    (
        object context,
        INotifyPropertyChanging observable,
        string propertyName,
        object trigger,
        PropertyChangingEventHandler eventHandler)
    {
        observable.PropertyChanging += eventHandler;
        _propertyChangingTriggerBindings.Add(new PropertyChangingTriggerBinding
        (
            context,
            observable, propertyName,
            trigger, eventHandler
        ));
    }

    private static void OnCollectionChangedInternal
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object trigger,
        NotifyCollectionChangedEventHandler eventHandler)
    {
        observableCollection.CollectionChanged += eventHandler;
        _collectionChangedTriggerBindings.Add(new CollectionChangedTriggerBinding
        (
            context,
            observableCollection, trigger,
            eventHandler
        ));
    }

    private static void OnCollectionChangedAndItemPropertyChangedInternal
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object collectionChangedTrigger,
        string itemPropertyName,
        object itemPropertyChangedTrigger,
        NotifyCollectionChangedEventHandler eventHandler)
    {
        observableCollection.CollectionChanged += eventHandler;
        _collectionChangedAndItemPropertyChangedTriggerBindings.Add(new CollectionChangedAndItemPropertyChangedTriggerBinding
        (
            context,
            observableCollection, collectionChangedTrigger,
            itemPropertyName, itemPropertyChangedTrigger,
            eventHandler
        ));
    }

    private static void OnCollectionChangedAndItemPropertyChangingInternal
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object collectionChangedTrigger,
        string itemPropertyName,
        object itemPropertyChangingTrigger,
        NotifyCollectionChangedEventHandler eventHandler)
    {
        observableCollection.CollectionChanged += eventHandler;
        _collectionChangedAndItemPropertyChangingTriggerBindings.Add(new CollectionChangedAndItemPropertyChangingTriggerBinding
        (
            context,
            observableCollection, collectionChangedTrigger,
            itemPropertyName, itemPropertyChangingTrigger,
            eventHandler
        ));
    }

    private static void OnCollectionItemPropertyChangedInternal
    (
        object context,
        INotifyCollectionChanged observableCollection,
        string itemPropertyName,
        object itemPropertyChangedTrigger,
        NotifyCollectionChangedEventHandler eventHandler)
    {
        observableCollection.CollectionChanged += eventHandler;
        _collectionItemPropertyChangedTriggerBindings.Add(new CollectionItemPropertyChangedTriggerBinding
        (
            context,
            observableCollection, itemPropertyName,
            itemPropertyChangedTrigger, eventHandler
        ));
    }

    private static void OnCollectionItemPropertyChangingInternal
    (
        object context,
        INotifyCollectionChanged observableCollection,
        string itemPropertyName,
        object itemPropertyChangingTrigger,
        NotifyCollectionChangedEventHandler eventHandler)
    {
        observableCollection.CollectionChanged += eventHandler;
        _collectionItemPropertyChangingTriggerBindings.Add(new CollectionItemPropertyChangingTriggerBinding
        (
            context,
            observableCollection,
            itemPropertyName, itemPropertyChangingTrigger,
            eventHandler
        ));
    }
    #endregion

    #region Unbinding methods
    private static void UnbindPropertyChangedBinding(PropertyChangedTriggerBinding binding)
    {
        var isObservableObjectPartOfCollectionChangedBinding =
            _collectionChangedAndItemPropertyChangedTriggerBindings
            .Select(b => (IEnumerable<INotifyPropertyChanged>)b.ObservableCollection)
            .Concat(_collectionItemPropertyChangedTriggerBindings
            .Select(b => (IEnumerable<INotifyPropertyChanged>)b.ObservableCollection))
            .SelectMany(e => e)
            .Any(item => item == binding.Observable);

        if (isObservableObjectPartOfCollectionChangedBinding)
            return;
        
        binding.Observable.PropertyChanged -= binding.EventHandler;
        _propertyChangedTriggerBindings.Remove(binding);
    }

    private static void UnbindPropertyChangedBindings(IEnumerable<PropertyChangedTriggerBinding> bindings)
    {
        foreach (var binding in bindings)
            UnbindPropertyChangedBinding(binding);
    }

    private static void UnbindPropertyChangingBinding(PropertyChangingTriggerBinding binding)
    {
        var isObservableObjectPartOfCollectionChangedBinding =
            _collectionChangedAndItemPropertyChangingTriggerBindings
            .Select(b => (IEnumerable<INotifyPropertyChanging>)b.ObservableCollection)
            .Concat(_collectionItemPropertyChangingTriggerBindings
            .Select(b => (IEnumerable<INotifyPropertyChanging>)b.ObservableCollection))
            .SelectMany(e => e)
            .Any(item => item == binding.Observable);

        if (isObservableObjectPartOfCollectionChangedBinding)
            return;

        binding.Observable.PropertyChanging -= binding.EventHandler;
        _propertyChangingTriggerBindings.Remove(binding);
    }

    private static void UnbindPropertyChangingBindings(IEnumerable<PropertyChangingTriggerBinding> bindings)
    {
        foreach (var binding in bindings)
            UnbindPropertyChangingBinding(binding);
    }

    private static void UnbindCollectionChangedBinding(CollectionChangedTriggerBinding binding)
    {
        binding.ObservableCollection.CollectionChanged -= binding.EventHandler;
        _collectionChangedTriggerBindings.Remove(binding);
    }

    private static void UnbindCollectionChangedBindings(IEnumerable<CollectionChangedTriggerBinding> bindings)
    {
        foreach (var binding in bindings)
            UnbindCollectionChangedBinding(binding);
    }

    private static void UnbindCollectionChangedAndItemPropertyChangedBinding
    (
        CollectionChangedAndItemPropertyChangedTriggerBinding binding)
    {
        binding.ObservableCollection.CollectionChanged -= binding.EventHandler;
        _collectionChangedAndItemPropertyChangedTriggerBindings.Remove(binding);

        UnbindPropertyChangedBindings(
            _propertyChangedTriggerBindings
                .IntersectBy((IEnumerable<INotifyPropertyChanged>)binding.ObservableCollection, pb => pb.Observable)
                .Where(pb => pb.Context == binding.Context &&
                             pb.PropertyName == binding.ItemPropertyName &&
                             pb.Trigger == binding.ItemPropertyChangedTrigger));
    }

    private static void UnbindCollectionChangedAndItemPropertyChangedBindings
    (
        IEnumerable<CollectionChangedAndItemPropertyChangedTriggerBinding> bindings)
    {
        foreach (var binding in bindings)
            UnbindCollectionChangedAndItemPropertyChangedBinding(binding);
    }

    private static void UnbindCollectionChangedAndItemPropertyChangingBinding
    (
        CollectionChangedAndItemPropertyChangingTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;
        _collectionChangedAndItemPropertyChangingTriggerBindings.Remove(triggerBinding);

        UnbindPropertyChangingBindings(
            _propertyChangingTriggerBindings
                .IntersectBy((IEnumerable<INotifyPropertyChanging>)triggerBinding.ObservableCollection, pb => pb.Observable)
                .Where(pb => pb.Context == triggerBinding.Context &&
                             pb.PropertyName == triggerBinding.ItemPropertyName &&
                             pb.Trigger == triggerBinding.ItemPropertyChangingTrigger));
    }

    private static void UnbindCollectionChangedAndItemPropertyChangingBindings
    (
        IEnumerable<CollectionChangedAndItemPropertyChangingTriggerBinding> bindings)
    {
        foreach (var binding in bindings)
            UnbindCollectionChangedAndItemPropertyChangingBinding(binding);
    }

    private static void UnbindCollectionItemPropertyChangedBinding
    (
        CollectionItemPropertyChangedTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;
        _collectionItemPropertyChangedTriggerBindings.Remove(triggerBinding);

        UnbindPropertyChangedBindings(
            _propertyChangedTriggerBindings
                .IntersectBy((IEnumerable<INotifyPropertyChanged>)triggerBinding.ObservableCollection, pb => pb.Observable)
                .Where(pb => pb.Context == triggerBinding.Context &&
                             pb.PropertyName == triggerBinding.ItemPropertyName &&
                             pb.Trigger == triggerBinding.ItemPropertyChangedTrigger));
    }

    private static void UnbindCollectionItemPropertyChangedBindings
    (
        IEnumerable<CollectionItemPropertyChangedTriggerBinding> bindings)
    {
        foreach (var binding in bindings)
            UnbindCollectionItemPropertyChangedBinding(binding);
    }

    private static void UnbindCollectionItemPropertyChangingBinding
    (
        CollectionItemPropertyChangingTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;
        _collectionItemPropertyChangingTriggerBindings.Remove(triggerBinding);

        UnbindPropertyChangingBindings(
            _propertyChangingTriggerBindings
                .IntersectBy((IEnumerable<INotifyPropertyChanging>)triggerBinding.ObservableCollection, pb => pb.Observable)
                .Where(pb => pb.Context == triggerBinding.Context &&
                             pb.PropertyName == triggerBinding.ItemPropertyName &&
                             pb.Trigger == triggerBinding.ItemPropertyChangingTrigger));
    }

    private static void UnbindCollectionItemPropertyChangingBindings
    (
        IEnumerable<CollectionItemPropertyChangingTriggerBinding> bindings)
    {
        foreach (var binding in bindings)
            UnbindCollectionItemPropertyChangingBinding(binding);
    }
    #endregion
    #endregion

    #region Private data types
    private class PropertyChangedTriggerBinding
    {
        public object Context { get; init; }

        public INotifyPropertyChanged Observable { get; init; }

        public string PropertyName { get; init; }

        public object Trigger { get; init; }

        public PropertyChangedEventHandler EventHandler { get; init; }

        public PropertyChangedTriggerBinding
        (
            object context,
            INotifyPropertyChanged observable,
            string propertyName,
            object trigger,
            PropertyChangedEventHandler eventHandler)
        {
            Context = context;
            Observable = observable;
            PropertyName = propertyName;
            Trigger = trigger;
            EventHandler = eventHandler;
        }
    }

    private class PropertyChangingTriggerBinding
    {
        public object Context { get; init; }

        public INotifyPropertyChanging Observable { get; init; }

        public string PropertyName { get; init; }

        public object Trigger { get; init; }

        public PropertyChangingEventHandler EventHandler { get; init; }

        public PropertyChangingTriggerBinding
        (
            object context,
            INotifyPropertyChanging observable,
            string propertyName,
            object trigger,
            PropertyChangingEventHandler eventHandler)
        {
            Context = context;
            Observable = observable;
            PropertyName = propertyName;
            Trigger = trigger;
            EventHandler = eventHandler;
        }
    }

    private class CollectionChangedTriggerBinding
    {
        public object Context { get; init; }

        public INotifyCollectionChanged ObservableCollection { get; init; }

        public NotifyCollectionChangedEventHandler EventHandler { get; init; }

        public object Trigger { get; init; }

        public CollectionChangedTriggerBinding
        (
            object context,
            INotifyCollectionChanged observableCollection,
            object trigger,
            NotifyCollectionChangedEventHandler eventHandler)
        {
            Context = context;
            ObservableCollection = observableCollection;
            Trigger = trigger;
            EventHandler = eventHandler;
        }
    }

    private class CollectionChangedAndItemPropertyChangedTriggerBinding
    {
        public object Context { get; init; }

        public INotifyCollectionChanged ObservableCollection { get; init; }

        public object CollectionChangedTrigger { get; init; }

        public string ItemPropertyName { get; init; }

        public object ItemPropertyChangedTrigger { get; init; }

        public NotifyCollectionChangedEventHandler EventHandler { get; init; }

        public CollectionChangedAndItemPropertyChangedTriggerBinding
        (
            object context,
            INotifyCollectionChanged observableCollection,
            object collectionChangedTrigger,
            string itemPropertyName,
            object itemPropertyChangedTrigger,
            NotifyCollectionChangedEventHandler eventHandler)
        {
            Context = context;
            ObservableCollection = observableCollection;
            CollectionChangedTrigger = collectionChangedTrigger;
            ItemPropertyName = itemPropertyName;
            ItemPropertyChangedTrigger = itemPropertyChangedTrigger;
            EventHandler = eventHandler;
        }
    }

    private class CollectionChangedAndItemPropertyChangingTriggerBinding
    {
        public object Context { get; init; }

        public INotifyCollectionChanged ObservableCollection { get; init; }

        public object CollectionChangedTrigger { get; init; }

        public string ItemPropertyName { get; init; }

        public object ItemPropertyChangingTrigger { get; init; }

        public NotifyCollectionChangedEventHandler EventHandler { get; init; }

        public CollectionChangedAndItemPropertyChangingTriggerBinding
        (
            object context,
            INotifyCollectionChanged observableCollection,
            object collectionChangedTrigger,
            string itemPropertyName,
            object itemPropertyChangingTrigger,
            NotifyCollectionChangedEventHandler eventHandler)
        {
            Context = context;
            ObservableCollection = observableCollection;
            CollectionChangedTrigger = collectionChangedTrigger;
            ItemPropertyName = itemPropertyName;
            ItemPropertyChangingTrigger = itemPropertyChangingTrigger;
            EventHandler = eventHandler;
        }
    }

    private class CollectionItemPropertyChangedTriggerBinding
    {
        public object Context { get; init; }

        public INotifyCollectionChanged ObservableCollection { get; init; }

        public string ItemPropertyName { get; init; }

        public object ItemPropertyChangedTrigger { get; init; }

        public NotifyCollectionChangedEventHandler EventHandler { get; init; }

        public CollectionItemPropertyChangedTriggerBinding
        (
            object context,
            INotifyCollectionChanged observableCollection,
            string itemPropertyName,
            object itemPropertyChangedTrigger,
            NotifyCollectionChangedEventHandler eventHandler)
        {
            Context = context;
            ObservableCollection = observableCollection;
            ItemPropertyName = itemPropertyName;
            ItemPropertyChangedTrigger = itemPropertyChangedTrigger;
            EventHandler = eventHandler;
        }
    }

    private class CollectionItemPropertyChangingTriggerBinding
    {
        public object Context { get; init; }

        public INotifyCollectionChanged ObservableCollection { get; init; }

        public string ItemPropertyName { get; init; }

        public object ItemPropertyChangingTrigger { get; init; }

        public NotifyCollectionChangedEventHandler EventHandler { get; init; }

        public CollectionItemPropertyChangingTriggerBinding
        (
            object context,
            INotifyCollectionChanged observableCollection,
            string itemPropertyName,
            object itemPropertyChangedTrigger,
            NotifyCollectionChangedEventHandler eventHandler)
        {
            Context = context;
            ObservableCollection = observableCollection;
            ItemPropertyName = itemPropertyName;
            ItemPropertyChangingTrigger = itemPropertyChangedTrigger;
            EventHandler = eventHandler;
        }
    }
    #endregion
}