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
        PropertyChangedBindings = new List<PropertyChangedTriggerBinding>();

    private static readonly IList<PropertyChangingTriggerBinding>
        PropertyChangingBindings = new List<PropertyChangingTriggerBinding>();

    private static readonly IList<CollectionChangedTriggerBinding>
        CollectionChangedBindings = new List<CollectionChangedTriggerBinding>();

    private static readonly IList<CollectionChangedAndItemPropertyChangedTriggerBinding>
        CollectionChangedAndItemPropertyChangedBindings = new List<CollectionChangedAndItemPropertyChangedTriggerBinding>();

    private static readonly IList<CollectionChangedAndItemPropertyChangingTriggerBinding>
        CollectionChangedAndItemPropertyChangingBindings = new List<CollectionChangedAndItemPropertyChangingTriggerBinding>();

    private static readonly IList<CollectionItemPropertyChangedTriggerBinding>
        CollectionItemPropertyChangedBindings = new List<CollectionItemPropertyChangedTriggerBinding>();

    private static readonly IList<CollectionItemPropertyChangingTriggerBinding>
        CollectionItemPropertyChangingBindings = new List<CollectionItemPropertyChangingTriggerBinding>();

    #endregion

    #region Public methods
    #region Binding methods
    #region PropertyChanged
    /// <summary>
    /// Binds a <paramref name="trigger"/> to changes of a given property in the <paramref name="observable"/> object.
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

        void EventHandler(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger(observable, observablePropertyGetter(observable));
        }

        OnPropertyChangedInternal(context, observable, propertyName, trigger, EventHandler);
    }

    /// <summary>
    /// Binds a <paramref name="trigger"/> to changes of a given property in the <paramref name="observable"/> object.
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

        void EventHandler(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger(observablePropertyGetter(observable));
        }

        OnPropertyChangedInternal(context, observable, propertyName, trigger, EventHandler);
    }

    /// <summary>
    /// Binds a <paramref name="trigger"/> to changes of a given property in the <paramref name="observable"/> object.
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

        void EventHandler(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger();
        }

        OnPropertyChangedInternal(context, observable, propertyName, trigger, EventHandler);
    }
    #endregion

    #region PropertyChanging
    /// <summary>
    /// Binds a <paramref name="trigger"/> to changes of a given property in the <paramref name="observable"/> object.
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

        void EventHandler(object? _, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger(observable, observablePropertyGetter(observable));
        }

        OnPropertyChangingInternal(context, observable, propertyName, trigger, EventHandler);
    }

    /// <summary>
    /// Binds a <paramref name="trigger"/> to changes of a given property in the <paramref name="observable"/> object.
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

        void EventHandler(object? _, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger(observablePropertyGetter(observable));
        }

        OnPropertyChangingInternal(context, observable, propertyName, trigger, EventHandler);
    }

    /// <summary>
    /// Binds a <paramref name="trigger"/> to changes of a given property in the <paramref name="observable"/> object.
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

        void EventHandler(object? _, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == propertyName)
                trigger();
        }

        OnPropertyChangingInternal(context, observable, propertyName, trigger, EventHandler);
    }
    #endregion

    #region CollectionChanged
    /// <summary>
    /// Binds a <paramref name="trigger"/> to changes in the <paramref name="observableCollection"/> object.
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

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e) =>
            trigger(observableCollection, e);

        OnCollectionChangedInternal(context, observableCollection, trigger, EventHandler);
    }

    /// <summary>
    /// Binds a <paramref name="trigger"/> to changes in the <paramref name="observableCollection"/> object.
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

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e) =>
            trigger(e);

        OnCollectionChangedInternal(context, observableCollection, trigger, EventHandler);
    }

    /// <summary>
    /// Binds a <paramref name="trigger"/> to changes in the <paramref name="observableCollection"/> object.
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

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e) =>
            trigger();

        OnCollectionChangedInternal(context, observableCollection, trigger, EventHandler);
    }
    #endregion

    #region CollectionChangedAndItemPropertyChanged
    #region First group (with collectionChangedTrigger with two parameters)
    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangedTrigger"/> to property changed events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger to be executed when the property changes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangedTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanged<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action<ObservableCollection<T>, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<T, TProperty> itemPropertyChangedTrigger
    )
    where T : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(observableCollection, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangedBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangedTrigger"/> to property changed events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger to be executed when the property changes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangedTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanged<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action<ObservableCollection<T>, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<TProperty> itemPropertyChangedTrigger
    )
    where T : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(observableCollection, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangedBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangedTrigger"/> to property changed events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger to be executed when the property changes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangedTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanged<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action<ObservableCollection<T>, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangedTrigger
    )
    where T : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(observableCollection, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangedBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, EventHandler);
    }
    #endregion

    #region Second group (with collectionChangedTrigger with one parameter)
    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangedTrigger"/> to property changed events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger to be executed when the property changes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangedTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanged<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<T, TProperty> itemPropertyChangedTrigger
    )
    where T : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangedBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangedTrigger"/> to property changed events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger to be executed when the property changes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangedTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanged<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<TProperty> itemPropertyChangedTrigger
    )
    where T : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangedBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangedTrigger"/> to property changed events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger to be executed when the property changes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangedTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanged<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangedTrigger
    )
    where T : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangedBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, EventHandler);
    }
    #endregion

    #region Third group (with collectionChangedTrigger without parameters)
    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangedTrigger"/> to property changed events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger to be executed when the property changes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangedTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanged<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<T, TProperty> itemPropertyChangedTrigger
    )
    where T : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangedBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangedTrigger"/> to property changed events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger to be executed when the property changes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangedTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanged<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<TProperty> itemPropertyChangedTrigger
    )
    where T : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangedBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangedTrigger"/> to property changed events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger to be executed when the property changes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangedTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanged<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangedTrigger
    )
    where T : INotifyPropertyChanged
    {
        CheckCollectionChangedAndItemPropertyChangedBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangedBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, EventHandler);
    }
    #endregion
    #endregion

    #region CollectionChangedAndItemPropertyChanging
    #region First group (with collectionChangedTrigger with two parameters)
    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangingTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger to be executed when the property changing event will occur.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangingTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanging<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action<ObservableCollection<T>, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<T, TProperty> itemPropertyChangingTrigger
    )
    where T : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(observableCollection, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangingBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangingTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger to be executed when the property changing event will occur.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangingTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanging<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action<ObservableCollection<T>, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<TProperty> itemPropertyChangingTrigger
    )
    where T : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(observableCollection, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangingBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangingTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger to be executed when the property changing event will occur.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangingTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanging<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action<ObservableCollection<T>, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangingTrigger
    )
    where T : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(observableCollection, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangingBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, EventHandler);
    }
    #endregion

    #region Second group (with collectionChangedTrigger with one parameter)
    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangingTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger to be executed when the property changing event will occur.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangingTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanging<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<T, TProperty> itemPropertyChangingTrigger
    )
    where T : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangingBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangingTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger to be executed when the property changing event will occur.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangingTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanging<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<TProperty> itemPropertyChangingTrigger
    )
    where T : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangingBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangingTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger to be executed when the property changing event will occur.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangingTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanging<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangingTrigger
    )
    where T : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangingBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, EventHandler);
    }
    #endregion

    #region Third group (with collectionChangedTrigger without parameters)
    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangingTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger to be executed when the property changing event will occur.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangingTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanging<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<T, TProperty> itemPropertyChangingTrigger
    )
    where T : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangingBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangingTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger to be executed when the property changing event will occur.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangingTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanging<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<TProperty> itemPropertyChangingTrigger
    )
    where T : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangingBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="collectionChangedTrigger"/> to changes in the <paramref name="observableCollection"/> object.
    /// - Binds a <paramref name="itemPropertyChangingTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="collectionChangedTrigger">The method to trigger the collection changed event.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger to be executed when the property changing event will occur.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="collectionChangedTrigger"/>,
    /// <paramref name="itemPropertyGetterExpr"/> or <paramref name="itemPropertyChangingTrigger"/> is null.</exception>
    public static void OnCollectionChangedAndItemPropertyChanging<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangingTrigger
    )
    where T : INotifyPropertyChanging
    {
        CheckCollectionChangedAndItemPropertyChangingBindingArgs(context,
            observableCollection, collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            collectionChangedTrigger();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangingBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }
            }
        }

        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, EventHandler);
    }
    #endregion
    #endregion

    #region CollectionAndItemPropertyChanged
    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="itemPropertyChangedTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger to be executed when the property changes</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="itemPropertyGetterExpr"/>
    /// or <paramref name="itemPropertyChangedTrigger"/> is null.</exception>
    public static void OnCollectionItemPropertyChanged<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<T, TProperty> itemPropertyChangedTrigger
    )
    where T : INotifyPropertyChanged
    {
        CheckCollectionItemPropertyChangedBindingArgs(context,
            observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangedBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }
            }
        }

        OnCollectionItemPropertyChangedInternal(context, observableCollection,
            GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="itemPropertyChangedTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger to be executed when the property changes</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="itemPropertyGetterExpr"/>
    /// or <paramref name="itemPropertyChangedTrigger"/> is null.</exception>
    public static void OnCollectionItemPropertyChanged<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<TProperty> itemPropertyChangedTrigger
    )
    where T : INotifyPropertyChanged
    {
        CheckCollectionItemPropertyChangedBindingArgs(context,
            observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangedBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }
            }
        }

        OnCollectionItemPropertyChangedInternal(context, observableCollection,
            GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="itemPropertyChangedTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger to be executed when the property changes</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="itemPropertyGetterExpr"/>
    /// or <paramref name="itemPropertyChangedTrigger"/> is null.</exception>
    public static void OnCollectionItemPropertyChanged<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangedTrigger
    )
    where T : INotifyPropertyChanged
    {
        CheckCollectionItemPropertyChangedBindingArgs(context,
            observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangedBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            UnbindPropertyChanged(context, item, itemPropertyGetterExpr, itemPropertyChangedTrigger);
                    break;
                }
            }
        }

        OnCollectionItemPropertyChangedInternal(context, observableCollection,
            GetPropertyName(itemPropertyGetterExpr), itemPropertyChangedTrigger, EventHandler);
    }
    #endregion

    #region CollectionAndItemPropertyChanging
    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="itemPropertyChangingTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger to be executed when the property changing event occurs.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="itemPropertyGetterExpr"/>
    /// or <paramref name="itemPropertyChangingTrigger"/> is null.</exception>
    public static void OnCollectionItemPropertyChanging<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<T, TProperty> itemPropertyChangingTrigger
    )
    where T : INotifyPropertyChanging
    {
        CheckCollectionItemPropertyChangingBindingArgs(context,
            observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangingBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }
            }
        }

        OnCollectionItemPropertyChangingInternal(context, observableCollection,
            GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="itemPropertyChangingTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger to be executed when the property changing event occurs.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="itemPropertyGetterExpr"/>
    /// or <paramref name="itemPropertyChangingTrigger"/> is null.</exception>
    public static void OnCollectionItemPropertyChanging<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action<TProperty> itemPropertyChangingTrigger
    )
    where T : INotifyPropertyChanging
    {
        CheckCollectionItemPropertyChangingBindingArgs(context,
            observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangingBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }
            }
        }

        OnCollectionItemPropertyChangingInternal(context, observableCollection,
            GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, EventHandler);
    }

    /// <summary>
    /// This method performs the following operations:
    /// - Binds a <paramref name="itemPropertyChangingTrigger"/> to property changing events of every collection item.
    /// - For items that are added to the collection, new trigger bindings are created.
    /// - For items that are removed from the collection, bindings are removed.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TProperty">The type of the item property.</typeparam>
    /// <param name="context">The context object associated with the event.</param>
    /// <param name="observableCollection">The observable collection.</param>
    /// <param name="itemPropertyGetterExpr">The expression used to get the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger to be executed when the property changing event occurs.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>,
    /// <paramref name="observableCollection"/>, <paramref name="itemPropertyGetterExpr"/>
    /// or <paramref name="itemPropertyChangingTrigger"/> is null.</exception>
    public static void OnCollectionItemPropertyChanging<T, TProperty>
    (
        object context,
        ObservableCollection<T> observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangingTrigger
    )
    where T : INotifyPropertyChanging
    {
        CheckCollectionItemPropertyChangingBindingArgs(context,
            observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        foreach (var item in observableCollection)
            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

        void EventHandler(object? _, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                {
                    UnbindPropertyChangingBindingsBoundWithinObservableCollection(context,
                        observableCollection, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    foreach (var item in observableCollection)
                        OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                default:
                {
                    if (e.NewItems is not null)
                        foreach (T item in e.NewItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);

                    if (e.OldItems is not null)
                        foreach (T item in e.OldItems)
                            OnPropertyChanging(context, item, itemPropertyGetterExpr, itemPropertyChangingTrigger);
                    break;
                }
            }
        }

        OnCollectionItemPropertyChangingInternal(context, observableCollection,
            GetPropertyName(itemPropertyGetterExpr), itemPropertyChangingTrigger, EventHandler);
    }
    #endregion
    #endregion

    #region Unbinding methods
    #region PropertyChanged
    /// <summary>
    /// Unbinds a <paramref name="trigger"/> from <paramref name="observable"/> object's property in a given context.
    /// </summary>
    /// <typeparam name="T">Type of the observable object.</typeparam>
    /// <typeparam name="TProperty">Type of the <paramref name="observable"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observable">Observable object.</param>
    /// <param name="observablePropertyGetterExpr">An expression representing the getter of the observable property.</param>
    /// <param name="trigger">The trigger which was bound.</param>
    public static void UnbindPropertyChanged<T, TProperty>
    (
        object context,
        T observable,
        Expression<Func<T, TProperty>> observablePropertyGetterExpr,
        Delegate trigger
    )
    where T : INotifyPropertyChanged
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(observablePropertyGetterExpr, nameof(observablePropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        UnbindPropertyChangedBindings(
            PropertyChangedBindings
            .Where(b => b.Context == context &&
                        b.Observable.Equals(observable) &&
                        b.PropertyName == propertyName &&
                        b.Trigger == trigger));
    }

    /// <summary>
    /// Unbinds all triggers from <paramref name="observable"/> object's property in a given context.
    /// </summary>
    /// <typeparam name="T">Type of the observable object.</typeparam>
    /// <typeparam name="TProperty">Type of the <paramref name="observable"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observable">Observable object.</param>
    /// <param name="observablePropertyGetterExpr">An expression representing the getter of the observable property.</param>
    public static void UnbindPropertyChanged<T, TProperty>
    (
        object context,
        T observable,
        Expression<Func<T, TProperty>> observablePropertyGetterExpr
    )
    where T : INotifyPropertyChanged
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(observablePropertyGetterExpr, nameof(observablePropertyGetterExpr));

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        UnbindPropertyChangedBindings(
            PropertyChangedBindings
            .Where(b => b.Context == context &&
                        b.Observable.Equals(observable) &&
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
            PropertyChangedBindings.Where(b => b.Context == context && b.Observable.Equals(observable)));
    }

    /// <summary>
    /// Unbinds all trigger bindings made in a given context
    /// </summary>
    /// <param name="context"></param>
    public static void UnbindPropertyChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindPropertyChangedBindings(PropertyChangedBindings.Where(b => b.Context == context));
    }
    #endregion

    #region PropertyChanging
    /// <summary>
    /// Unbinds a <paramref name="trigger"/> from <paramref name="observable"/> object's property in a given context.
    /// </summary>
    /// <typeparam name="T">Type of the observable object.</typeparam>
    /// <typeparam name="TProperty">Type of the <paramref name="observable"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observable">Observable object.</param>
    /// <param name="observablePropertyGetterExpr">An expression representing the getter of the observable property.</param>
    /// <param name="trigger">The trigger which was bound.</param>
    public static void UnbindPropertyChanging<T, TProperty>
    (
        object context,
        T observable,
        Expression<Func<T, TProperty>> observablePropertyGetterExpr,
        Delegate trigger
    )
    where T : INotifyPropertyChanging
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(observablePropertyGetterExpr, nameof(observablePropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        UnbindPropertyChangingBindings(
            PropertyChangingBindings
            .Where(b => b.Context == context &&
                        b.Observable == (INotifyPropertyChanging)observable &&
                        b.PropertyName == propertyName &&
                        b.Trigger == trigger));
    }

    /// <summary>
    /// Unbinds all triggers from <paramref name="observable"/> object's property in a given context.
    /// </summary>
    /// <typeparam name="T">Type of the observable object.</typeparam>
    /// <typeparam name="TProperty">Type of the <paramref name="observable"/> object's property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observable">Observable object.</param>
    /// <param name="observablePropertyGetterExpr">An expression representing the getter of the observable property.</param>
    public static void UnbindPropertyChanging<T, TProperty>
    (
        object context,
        T observable,
        Expression<Func<T, TProperty>> observablePropertyGetterExpr
    )
    where T : INotifyPropertyChanging
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(observablePropertyGetterExpr, nameof(observablePropertyGetterExpr));

        var propertyName = GetPropertyName(observablePropertyGetterExpr);
        UnbindPropertyChangingBindings(
            PropertyChangingBindings
            .Where(b => b.Context == context &&
                        b.Observable == (INotifyPropertyChanging)observable &&
                        b.PropertyName == propertyName));
    }

    /// <summary>
    /// Unbinds all triggers from <paramref name="observable"/> object in a given context.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observable">Observable object.</param>
    public static void UnbindPropertyChanging
    (
        object context,
        INotifyPropertyChanging observable)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));

        UnbindPropertyChangingBindings(
            PropertyChangingBindings.Where(b => b.Context == context && b.Observable == observable));
    }

    /// <summary>
    /// Unbinds all triggers from all observable objects in a given context.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    public static void UnbindPropertyChanging(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindPropertyChangingBindings(PropertyChangingBindings.Where(b => b.Context == context));
    }
    #endregion

    #region CollectionChanged
    /// <summary>
    /// Unbinds a <paramref name="trigger"/> from <paramref name="observableCollection"/> in a given context.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    /// <param name="trigger">The trigger which was bound.</param>
    public static void UnbindCollectionChanged
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Delegate trigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));

        UnbindCollectionChangedBindings(
            CollectionChangedBindings
            .Where(b => b.Context == context &&
                        b.ObservableCollection == observableCollection &&
                        b.Trigger == trigger));
    }

    /// <summary>
    /// Unbinds all triggers from <paramref name="observableCollection"/> in a given context.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    public static void UnbindCollectionChanged(object context, INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        UnbindCollectionChangedBindings(
            CollectionChangedBindings
            .Where(b => b.Context == context && b.ObservableCollection == observableCollection));
    }

    /// <summary>
    /// Unbinds all triggers from all observable collections in a given context.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    public static void UnbindCollectionChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindCollectionChangedBindings(CollectionChangedBindings.Where(b => b.Context == context));
    }
    #endregion

    #region CollectionChangedAndItemPropertyChanged
    /// <summary>
    /// In a given context unbinds <paramref name="collectionChangedTrigger"/> from <paramref name="observableCollection"/>
    /// and unbinds <paramref name="itemPropertyChangedTrigger"/> from collection items property.
    /// </summary>
    /// <typeparam name="T">Type of the item of the observable collection.</typeparam>
    /// <typeparam name="TProperty">Type of the observable object's item property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    /// <param name="collectionChangedTrigger">The trigger which was bound to observable collection.</param>
    /// <param name="itemPropertyGetterExpr">An expression representing the getter of the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger which was bound to the observable collection items.</param>
    public static void UnbindCollectionChangedAndItemPropertyChanged<T, TProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Delegate collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Delegate itemPropertyChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangedTrigger, nameof(itemPropertyChangedTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionChangedAndItemPropertyChangedBindings(
            CollectionChangedAndItemPropertyChangedBindings
                .Where(b => b.Context == context &&
                            b.ObservableCollection == observableCollection &&
                            b.CollectionChangedTrigger == collectionChangedTrigger &&
                            b.ItemPropertyName == itemPropertyName &&
                            b.ItemPropertyChangedTrigger == itemPropertyChangedTrigger));
    }

    /// <summary>
    /// In a given context unbinds <paramref name="collectionChangedTrigger"/> from <paramref name="observableCollection"/>
    /// and unbinds all triggers from collection items properties.
    /// </summary>
    /// <typeparam name="T">Type of the item of the observable collection.</typeparam>
    /// <typeparam name="TProperty">Type of the observable object's item property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    /// <param name="collectionChangedTrigger">The trigger which was bound to observable collection.</param>
    /// <param name="itemPropertyGetterExpr">An expression representing the getter of the item property.</param>
    public static void UnbindCollectionChangedAndItemPropertyChanged<T, TProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Delegate collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionChangedAndItemPropertyChangedBindings(
            CollectionChangedAndItemPropertyChangedBindings
                .Where(b => b.Context == context &&
                            b.ObservableCollection == observableCollection &&
                            b.CollectionChangedTrigger == collectionChangedTrigger &&
                            b.ItemPropertyName == itemPropertyName));
    }

    /// <summary>
    /// In a given context unbinds <paramref name="collectionChangedTrigger"/> from <paramref name="observableCollection"/>
    /// and unbinds all triggers from all collection items properties.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    /// <param name="collectionChangedTrigger">The trigger which was bound to observable collection.</param>
    public static void UnbindCollectionChangedAndItemPropertyChanged
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Delegate collectionChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));

        UnbindCollectionChangedAndItemPropertyChangedBindings(
            CollectionChangedAndItemPropertyChangedBindings
                .Where(b => b.Context == context &&
                            b.ObservableCollection == observableCollection &&
                            b.CollectionChangedTrigger == collectionChangedTrigger));
    }

    /// <summary>
    /// In a given context unbinds all triggers from <paramref name="observableCollection"/>
    /// and unbinds all triggers from all items properties.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    public static void UnbindCollectionChangedAndItemPropertyChanged
    (
        object context,
        INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        UnbindCollectionChangedAndItemPropertyChangedBindings(
            CollectionChangedAndItemPropertyChangedBindings
                .Where(b => b.Context == context && b.ObservableCollection == observableCollection));
    }

    /// <summary>
    /// In a given context unbinds all triggers from all observable collections
    /// and unbinds all triggers from all collections items properties.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    public static void UnbindCollectionChangedAndItemPropertyChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindCollectionChangedAndItemPropertyChangedBindings(
            CollectionChangedAndItemPropertyChangedBindings.Where(b => b.Context == context));
    }
    #endregion

    #region CollectionChangedAndItemPropertyChanging
    /// <summary>
    /// In a given context unbinds <paramref name="collectionChangedTrigger"/> from <paramref name="observableCollection"/>
    /// and unbinds <paramref name="itemPropertyChangingTrigger"/> from collection items property.
    /// </summary>
    /// <typeparam name="T">Type of the item of the observable collection.</typeparam>
    /// <typeparam name="TProperty">Type of the observable object's item property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    /// <param name="collectionChangedTrigger">The trigger which was bound to observable collection.</param>
    /// <param name="itemPropertyGetterExpr">An expression representing the getter of the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger which was bound to the observable collection items.</param>
    public static void UnbindCollectionChangedAndItemPropertyChanging<T, TProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Delegate collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Delegate itemPropertyChangingTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangingTrigger, nameof(itemPropertyChangingTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionChangedAndItemPropertyChangingBindings(
            CollectionChangedAndItemPropertyChangingBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.CollectionChangedTrigger == collectionChangedTrigger &&
                         tb.ItemPropertyName == itemPropertyName &&
                         tb.ItemPropertyChangingTrigger == itemPropertyChangingTrigger));
    }

    /// <summary>
    /// In a given context unbinds <paramref name="collectionChangedTrigger"/> from <paramref name="observableCollection"/>
    /// and unbinds all triggers from collection items property.
    /// </summary>
    /// <typeparam name="T">Type of the item of the observable collection.</typeparam>
    /// <typeparam name="TProperty">Type of the observable object's item property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    /// <param name="collectionChangedTrigger">The trigger which was bound to observable collection.</param>
    /// <param name="itemPropertyGetterExpr">An expression representing the getter of the item property.</param>
    public static void UnbindCollectionChangedAndItemPropertyChanging<T, TProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Delegate collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionChangedAndItemPropertyChangingBindings(
            CollectionChangedAndItemPropertyChangingBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.CollectionChangedTrigger == collectionChangedTrigger &&
                         tb.ItemPropertyName == itemPropertyName));
    }

    /// <summary>
    /// In a given context unbinds <paramref name="collectionChangedTrigger"/> from <paramref name="observableCollection"/>
    /// and unbinds all triggers from all collection items properties.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    /// <param name="collectionChangedTrigger">The trigger which was bound to observable collection.</param>
    public static void UnbindCollectionChangedAndItemPropertyChanging
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Delegate collectionChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));

        UnbindCollectionChangedAndItemPropertyChangingBindings(
            CollectionChangedAndItemPropertyChangingBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.CollectionChangedTrigger == collectionChangedTrigger));
    }

    /// <summary>
    /// In a given context unbinds all triggers from <paramref name="observableCollection"/>
    /// and unbinds all triggers from all collection items properties.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    public static void UnbindCollectionChangedAndItemPropertyChanging
    (
        object context,
        INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        UnbindCollectionChangedAndItemPropertyChangingBindings(
            CollectionChangedAndItemPropertyChangingBindings
            .Where(tb => tb.Context == context && tb.ObservableCollection == observableCollection));
    }

    /// <summary>
    /// In a given context unbinds all triggers from all observable collections
    /// and unbinds all triggers from all collections items properties.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    public static void UnbindCollectionChangedAndItemPropertyChanging(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindCollectionChangedAndItemPropertyChangingBindings(
            CollectionChangedAndItemPropertyChangingBindings.Where(tb => tb.Context == context));
    }
    #endregion

    #region CollectionItemPropertyChanged
    /// <summary>
    /// In a given context unbinds <paramref name="itemPropertyChangedTrigger"/> from collection items property.
    /// </summary>
    /// <typeparam name="T">Type of the item of the observable collection.</typeparam>
    /// <typeparam name="TProperty">Type of the observable object's item property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    /// <param name="itemPropertyGetterExpr">An expression representing the getter of the item property.</param>
    /// <param name="itemPropertyChangedTrigger">The trigger which was bound to the observable collection items.</param>
    public static void UnbindCollectionItemPropertyChanged<T, TProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Delegate itemPropertyChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangedTrigger, nameof(itemPropertyChangedTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionItemPropertyChangedBindings(
            CollectionItemPropertyChangedBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.ItemPropertyName == itemPropertyName &&
                         tb.ItemPropertyChangedTrigger == itemPropertyChangedTrigger));
    }

    /// <summary>
    /// In a given context unbinds all triggers from collection items property.
    /// </summary>
    /// <typeparam name="T">Type of the item of the observable collection.</typeparam>
    /// <typeparam name="TProperty">Type of the observable object's item property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    /// <param name="itemPropertyGetterExpr">An expression representing the getter of the item property.</param>
    public static void UnbindCollectionItemPropertyChanged<T, TProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionItemPropertyChangedBindings(
            CollectionItemPropertyChangedBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.ItemPropertyName == itemPropertyName));
    }

    /// <summary>
    /// In a given context unbinds all triggers from all collection items properties.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    public static void UnbindCollectionItemPropertyChanged
    (
        object context,
        INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        UnbindCollectionItemPropertyChangedBindings(
            CollectionItemPropertyChangedBindings
            .Where(tb => tb.Context == context && tb.ObservableCollection == observableCollection));
    }

    /// <summary>
    /// In a given context unbinds all triggers from all collection items properties.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    public static void UnbindCollectionItemPropertyChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindCollectionItemPropertyChangedBindings(
            CollectionItemPropertyChangedBindings.Where(tb => tb.Context == context));
    }
    #endregion

    #region CollectionItemPropertyChanging
    /// <summary>
    /// In a given context unbinds <paramref name="itemPropertyChangingTrigger"/> from collection items property.
    /// </summary>
    /// <typeparam name="T">Type of the item of the observable collection.</typeparam>
    /// <typeparam name="TProperty">Type of the observable object's item property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    /// <param name="itemPropertyGetterExpr">An expression representing the getter of the item property.</param>
    /// <param name="itemPropertyChangingTrigger">The trigger which was bound to the observable collection items.</param>
    public static void UnbindCollectionItemPropertyChanging<T, TProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Delegate itemPropertyChangingTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangingTrigger, nameof(itemPropertyChangingTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionItemPropertyChangingBindings(
            CollectionItemPropertyChangingBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.ItemPropertyName == itemPropertyName &&
                         tb.ItemPropertyChangingTrigger == itemPropertyChangingTrigger));
    }

    /// <summary>
    /// In a given context unbinds all triggers from collection items property.
    /// </summary>
    /// <typeparam name="T">Type of the item of the observable collection.</typeparam>
    /// <typeparam name="TProperty">Type of the observable object's item property.</typeparam>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    /// <param name="itemPropertyGetterExpr">An expression representing the getter of the item property.</param>
    public static void UnbindCollectionItemPropertyChanging<T, TProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindCollectionItemPropertyChangingBindings(
            CollectionItemPropertyChangingBindings
            .Where(tb => tb.Context == context &&
                         tb.ObservableCollection == observableCollection &&
                         tb.ItemPropertyName == itemPropertyName));
    }

    /// <summary>
    /// In a given context unbinds all triggers from all collection items properties.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    /// <param name="observableCollection">Observable collection.</param>
    public static void UnbindCollectionItemPropertyChanging
    (
        object context,
        INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        UnbindCollectionItemPropertyChangingBindings(
            CollectionItemPropertyChangingBindings
            .Where(tb => tb.Context == context && tb.ObservableCollection == observableCollection));
    }

    /// <summary>
    /// In a given context unbinds all triggers from all collections items properties.
    /// </summary>
    /// <param name="context">The context in which the binding was made.</param>
    public static void UnbindCollectionItemPropertyChanging(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindCollectionItemPropertyChangingBindings(
            CollectionItemPropertyChangingBindings.Where(tb => tb.Context == context));
    }
    #endregion

    /// <summary>
    /// Unbinds all possible triggers bound to objects in a given context.
    /// </summary>
    /// <param name="context"></param>
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
    private static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertyGetterExpr) =>
        ((MemberExpression)propertyGetterExpr.Body).Member.Name;

    #region Argument checking methods
    private static void CheckPropertyChangedBindingArgs<T, TProperty>
    (
        object context,
        object observable,
        Expression<Func<T, TProperty>> propertyGetterExpr,
        Delegate trigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(propertyGetterExpr, nameof(propertyGetterExpr));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));

        var propertyName = GetPropertyName(propertyGetterExpr);
        var isBindingExist = PropertyChangedBindings.Any(b =>
            b.Context == context &&
            b.Observable.Equals(observable) &&
            b.PropertyName == propertyName &&
            b.Trigger == trigger);

        if (isBindingExist)
            throw new BindingException("This trigger is already bound to the property of the observable object in this context.");
    }

    private static void CheckPropertyChangingBindingArgs<T, TProperty>
    (
        object context,
        object observable,
        Expression<Func<T, TProperty>> propertyGetterExpr,
        Delegate trigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(propertyGetterExpr, nameof(propertyGetterExpr));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));

        var propertyName = GetPropertyName(propertyGetterExpr);
        var isBindingExist = PropertyChangingBindings.Any(b =>
            b.Context == context &&
            b.Observable == observable &&
            b.PropertyName == propertyName &&
            b.Trigger == trigger);

        if (isBindingExist)
            throw new BindingException("This trigger is already bound to the property of the observable object in this context.");
    }

    private static void CheckCollectionChangedBindingArgs
    (
        object context,
        object observableCollection,
        Delegate trigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));

        var isBindingExist = CollectionChangedBindings.Any(b =>
            b.Context == context &&
            b.ObservableCollection == observableCollection &&
            b.Trigger == trigger);

        if (isBindingExist)
            throw new BindingException("This trigger is already bound to the observable collection in this context.");
    }

    private static void CheckCollectionChangedAndItemPropertyChangedBindingArgs<T, TProperty>
    (
        object context,
        object observableCollection,
        Delegate collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Delegate itemPropertyChangedTrigger)
    {
        CheckCollectionChangedBindingArgs(context, observableCollection, collectionChangedTrigger);
        
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangedTrigger, nameof(itemPropertyChangedTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        var isBindingExist = CollectionChangedAndItemPropertyChangedBindings.Any(b =>
            b.Context == context &&
            b.ObservableCollection == observableCollection &&
            b.ItemPropertyName == itemPropertyName &&
            b.ItemPropertyChangedTrigger == itemPropertyChangedTrigger);

        if (isBindingExist)
            throw new BindingException("This trigger is already bound to the property of an item of the observable collection in this context.");
    }

    private static void CheckCollectionChangedAndItemPropertyChangingBindingArgs<T, TProperty>
    (
        object context,
        object observableCollection,
        Delegate collectionChangedTrigger,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Delegate itemPropertyChangingTrigger)
    {
        CheckCollectionChangedBindingArgs(context, observableCollection, collectionChangedTrigger);

        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangingTrigger, nameof(itemPropertyChangingTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        var isBindingExist = CollectionChangedAndItemPropertyChangingBindings.Any(b =>
            b.Context == context &&
            b.ObservableCollection == observableCollection &&
            b.ItemPropertyName == itemPropertyName &&
            b.ItemPropertyChangingTrigger == itemPropertyChangingTrigger);

        if (isBindingExist)
            throw new BindingException("This trigger is already bound to the property of an item of the observable collection in this context.");
    }

    private static void CheckCollectionItemPropertyChangedBindingArgs<T, TProperty>
    (
        object context,
        object observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Delegate itemPropertyChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangedTrigger, nameof(itemPropertyChangedTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        var isBindingExist = CollectionItemPropertyChangedBindings.Any(b =>
            b.Context == context &&
            b.ObservableCollection == observableCollection &&
            b.ItemPropertyName == itemPropertyName &&
            b.ItemPropertyChangedTrigger == itemPropertyChangedTrigger);

        if (isBindingExist)
            throw new BindingException("This trigger is already bound to the property of an item of the observable collection in this context.");
    }

    private static void CheckCollectionItemPropertyChangingBindingArgs<T, TProperty>
    (
        object context,
        object observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Delegate itemPropertyChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangedTrigger, nameof(itemPropertyChangedTrigger));

        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        var isBindingExist = CollectionItemPropertyChangingBindings.Any(b =>
            b.Context == context &&
            b.ObservableCollection == observableCollection &&
            b.ItemPropertyName == itemPropertyName &&
            b.ItemPropertyChangingTrigger == itemPropertyChangedTrigger);

        if (isBindingExist)
            throw new BindingException("This trigger is already bound to the property of an item of the observable collection in this context.");
    }
    #endregion

    #region Binding methods
    private static void OnPropertyChangedInternal
    (
        object context,
        INotifyPropertyChanged observable,
        string propertyName,
        Delegate trigger,
        PropertyChangedEventHandler eventHandler)
    {
        observable.PropertyChanged += eventHandler;
        PropertyChangedBindings.Add(new PropertyChangedTriggerBinding
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
        Delegate trigger,
        PropertyChangingEventHandler eventHandler)
    {
        observable.PropertyChanging += eventHandler;
        PropertyChangingBindings.Add(new PropertyChangingTriggerBinding
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
        Delegate trigger,
        NotifyCollectionChangedEventHandler eventHandler)
    {
        observableCollection.CollectionChanged += eventHandler;
        CollectionChangedBindings.Add(new CollectionChangedTriggerBinding
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
        Delegate collectionChangedTrigger,
        string itemPropertyName,
        Delegate itemPropertyChangedTrigger,
        NotifyCollectionChangedEventHandler eventHandler)
    {
        observableCollection.CollectionChanged += eventHandler;
        CollectionChangedAndItemPropertyChangedBindings.Add(new CollectionChangedAndItemPropertyChangedTriggerBinding
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
        Delegate collectionChangedTrigger,
        string itemPropertyName,
        Delegate itemPropertyChangingTrigger,
        NotifyCollectionChangedEventHandler eventHandler)
    {
        observableCollection.CollectionChanged += eventHandler;
        CollectionChangedAndItemPropertyChangingBindings.Add(new CollectionChangedAndItemPropertyChangingTriggerBinding
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
        Delegate itemPropertyChangedTrigger,
        NotifyCollectionChangedEventHandler eventHandler)
    {
        observableCollection.CollectionChanged += eventHandler;
        CollectionItemPropertyChangedBindings.Add(new CollectionItemPropertyChangedTriggerBinding
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
        Delegate itemPropertyChangingTrigger,
        NotifyCollectionChangedEventHandler eventHandler)
    {
        observableCollection.CollectionChanged += eventHandler;
        CollectionItemPropertyChangingBindings.Add(new CollectionItemPropertyChangingTriggerBinding
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
            CollectionChangedAndItemPropertyChangedBindings
            .Select(b => (IEnumerable<INotifyPropertyChanged>)b.ObservableCollection)
            .Concat(CollectionItemPropertyChangedBindings
            .Select(b => (IEnumerable<INotifyPropertyChanged>)b.ObservableCollection))
            .SelectMany(e => e)
            .Any(item => item.Equals(binding.Observable));

        if (isObservableObjectPartOfCollectionChangedBinding)
            return;
        
        binding.Observable.PropertyChanged -= binding.EventHandler;
        PropertyChangedBindings.Remove(binding);
    }

    private static void UnbindPropertyChangedBindings(IEnumerable<PropertyChangedTriggerBinding> bindings)
    {
        var bindingsCopy = bindings.ToArray();
        foreach (var binding in bindingsCopy)
            UnbindPropertyChangedBinding(binding);
    }

    private static void UnbindPropertyChangingBinding(PropertyChangingTriggerBinding binding)
    {
        var isObservableObjectPartOfCollectionChangedBinding =
            CollectionChangedAndItemPropertyChangingBindings
            .Select(b => (IEnumerable<INotifyPropertyChanging>)b.ObservableCollection)
            .Concat(CollectionItemPropertyChangingBindings
            .Select(b => (IEnumerable<INotifyPropertyChanging>)b.ObservableCollection))
            .SelectMany(e => e)
            .Any(item => item == binding.Observable);

        if (isObservableObjectPartOfCollectionChangedBinding)
            return;

        binding.Observable.PropertyChanging -= binding.EventHandler;
        PropertyChangingBindings.Remove(binding);
    }

    private static void UnbindPropertyChangingBindings(IEnumerable<PropertyChangingTriggerBinding> bindings)
    {
        var bindingsCopy = bindings.ToArray();
        foreach (var binding in bindingsCopy)
            UnbindPropertyChangingBinding(binding);
    }

    private static void UnbindCollectionChangedBinding(CollectionChangedTriggerBinding binding)
    {
        binding.ObservableCollection.CollectionChanged -= binding.EventHandler;
        CollectionChangedBindings.Remove(binding);
    }

    private static void UnbindCollectionChangedBindings(IEnumerable<CollectionChangedTriggerBinding> bindings)
    {
        var bindingsCopy = bindings.ToArray();
        foreach (var binding in bindingsCopy)
            UnbindCollectionChangedBinding(binding);
    }

    private static void UnbindCollectionChangedAndItemPropertyChangedBinding
    (
        CollectionChangedAndItemPropertyChangedTriggerBinding binding)
    {
        binding.ObservableCollection.CollectionChanged -= binding.EventHandler;
        CollectionChangedAndItemPropertyChangedBindings.Remove(binding);

        UnbindPropertyChangedBindings(
            PropertyChangedBindings
                .IntersectBy((IEnumerable<INotifyPropertyChanged>)binding.ObservableCollection, pb => pb.Observable)
                .Where(pb => pb.Context == binding.Context &&
                             pb.PropertyName == binding.ItemPropertyName &&
                             pb.Trigger == binding.ItemPropertyChangedTrigger));
    }

    private static void UnbindCollectionChangedAndItemPropertyChangedBindings
    (
        IEnumerable<CollectionChangedAndItemPropertyChangedTriggerBinding> bindings)
    {
        var bindingsCopy = bindings.ToArray();
        foreach (var binding in bindingsCopy)
            UnbindCollectionChangedAndItemPropertyChangedBinding(binding);
    }

    private static void UnbindCollectionChangedAndItemPropertyChangingBinding
    (
        CollectionChangedAndItemPropertyChangingTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;
        CollectionChangedAndItemPropertyChangingBindings.Remove(triggerBinding);

        UnbindPropertyChangingBindings(
            PropertyChangingBindings
                .IntersectBy((IEnumerable<INotifyPropertyChanging>)triggerBinding.ObservableCollection, pb => pb.Observable)
                .Where(pb => pb.Context == triggerBinding.Context &&
                             pb.PropertyName == triggerBinding.ItemPropertyName &&
                             pb.Trigger == triggerBinding.ItemPropertyChangingTrigger));
    }

    private static void UnbindCollectionChangedAndItemPropertyChangingBindings
    (
        IEnumerable<CollectionChangedAndItemPropertyChangingTriggerBinding> bindings)
    {
        var bindingsCopy = bindings.ToArray();
        foreach (var binding in bindingsCopy)
            UnbindCollectionChangedAndItemPropertyChangingBinding(binding);
    }

    private static void UnbindCollectionItemPropertyChangedBinding
    (
        CollectionItemPropertyChangedTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;
        CollectionItemPropertyChangedBindings.Remove(triggerBinding);

        UnbindPropertyChangedBindings(
            PropertyChangedBindings
                .IntersectBy((IEnumerable<INotifyPropertyChanged>)triggerBinding.ObservableCollection, pb => pb.Observable)
                .Where(pb => pb.Context == triggerBinding.Context &&
                             pb.PropertyName == triggerBinding.ItemPropertyName &&
                             pb.Trigger == triggerBinding.ItemPropertyChangedTrigger));
    }

    private static void UnbindCollectionItemPropertyChangedBindings
    (
        IEnumerable<CollectionItemPropertyChangedTriggerBinding> bindings)
    {
        var bindingsCopy = bindings.ToArray();
        foreach (var binding in bindingsCopy)
            UnbindCollectionItemPropertyChangedBinding(binding);
    }

    private static void UnbindPropertyChangedBindingsBoundWithinObservableCollection<T, TProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Delegate itemPropertyChangedTrigger)
    {
        // Unbind all triggers bound to items
        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindPropertyChangedBindings(
            PropertyChangedBindings
                .IntersectBy((IEnumerable<INotifyPropertyChanged>)observableCollection, pb => pb.Observable)
                .Where(pb => pb.Context == context &&
                             pb.PropertyName == itemPropertyName &&
                             pb.Trigger == itemPropertyChangedTrigger));
    }

    private static void UnbindCollectionItemPropertyChangingBinding
    (
        CollectionItemPropertyChangingTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;
        CollectionItemPropertyChangingBindings.Remove(triggerBinding);

        UnbindPropertyChangingBindings(
            PropertyChangingBindings
                .IntersectBy((IEnumerable<INotifyPropertyChanging>)triggerBinding.ObservableCollection, pb => pb.Observable)
                .Where(pb => pb.Context == triggerBinding.Context &&
                             pb.PropertyName == triggerBinding.ItemPropertyName &&
                             pb.Trigger == triggerBinding.ItemPropertyChangingTrigger));
    }

    private static void UnbindCollectionItemPropertyChangingBindings
    (
        IEnumerable<CollectionItemPropertyChangingTriggerBinding> bindings)
    {
        var bindingsCopy = bindings.ToArray();
        foreach (var binding in bindingsCopy)
            UnbindCollectionItemPropertyChangingBinding(binding);
    }

    private static void UnbindPropertyChangingBindingsBoundWithinObservableCollection<T, TProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        Expression<Func<T, TProperty>> itemPropertyGetterExpr,
        Delegate itemPropertyChangingTrigger)
    {
        // Unbind all triggers bound to items
        var itemPropertyName = GetPropertyName(itemPropertyGetterExpr);
        UnbindPropertyChangingBindings(
            PropertyChangingBindings
                .IntersectBy((IEnumerable<INotifyPropertyChanging>)observableCollection, pb => pb.Observable)
                .Where(pb => pb.Context == context &&
                             pb.PropertyName == itemPropertyName &&
                             pb.Trigger == itemPropertyChangingTrigger));
    }
    #endregion
    #endregion

    #region Private data types
    private class PropertyChangedTriggerBinding
    {
        public object Context { get; }

        public INotifyPropertyChanged Observable { get; }

        public string PropertyName { get; } 

        public Delegate Trigger { get; }

        public PropertyChangedEventHandler EventHandler { get; }

        public PropertyChangedTriggerBinding
        (
            object context,
            INotifyPropertyChanged observable,
            string propertyName,
            Delegate trigger,
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
        public object Context { get; }

        public INotifyPropertyChanging Observable { get; }

        public string PropertyName { get; }

        public Delegate Trigger { get; }

        public PropertyChangingEventHandler EventHandler { get; }

        public PropertyChangingTriggerBinding
        (
            object context,
            INotifyPropertyChanging observable,
            string propertyName,
            Delegate trigger,
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
        public object Context { get; }

        public INotifyCollectionChanged ObservableCollection { get; }

        public NotifyCollectionChangedEventHandler EventHandler { get; }

        public Delegate Trigger { get; }

        public CollectionChangedTriggerBinding
        (
            object context,
            INotifyCollectionChanged observableCollection,
            Delegate trigger,
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
        public object Context { get; }

        public INotifyCollectionChanged ObservableCollection { get; }

        public Delegate CollectionChangedTrigger { get; }

        public string ItemPropertyName { get; }

        public Delegate ItemPropertyChangedTrigger { get; }

        public NotifyCollectionChangedEventHandler EventHandler { get; }

        public CollectionChangedAndItemPropertyChangedTriggerBinding
        (
            object context,
            INotifyCollectionChanged observableCollection,
            Delegate collectionChangedTrigger,
            string itemPropertyName,
            Delegate itemPropertyChangedTrigger,
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
        public object Context { get; }

        public INotifyCollectionChanged ObservableCollection { get; }

        public Delegate CollectionChangedTrigger { get; }

        public string ItemPropertyName { get; }

        public Delegate ItemPropertyChangingTrigger { get; }

        public NotifyCollectionChangedEventHandler EventHandler { get; }

        public CollectionChangedAndItemPropertyChangingTriggerBinding
        (
            object context,
            INotifyCollectionChanged observableCollection,
            Delegate collectionChangedTrigger,
            string itemPropertyName,
            Delegate itemPropertyChangingTrigger,
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
        public object Context { get; }

        public INotifyCollectionChanged ObservableCollection { get; }

        public string ItemPropertyName { get; }

        public Delegate ItemPropertyChangedTrigger { get; }

        public NotifyCollectionChangedEventHandler EventHandler { get; }

        public CollectionItemPropertyChangedTriggerBinding
        (
            object context,
            INotifyCollectionChanged observableCollection,
            string itemPropertyName,
            Delegate itemPropertyChangedTrigger,
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
        public object Context { get; }

        public INotifyCollectionChanged ObservableCollection { get; }

        public string ItemPropertyName { get; }

        public Delegate ItemPropertyChangingTrigger { get; }

        public NotifyCollectionChangedEventHandler EventHandler { get; }

        public CollectionItemPropertyChangingTriggerBinding
        (
            object context,
            INotifyCollectionChanged observableCollection,
            string itemPropertyName,
            Delegate itemPropertyChangedTrigger,
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