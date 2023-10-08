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
        CheckPropertyChangedOrChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);

        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;
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
        CheckPropertyChangedOrChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);

        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;
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
        CheckPropertyChangedOrChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);

        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;
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
        CheckPropertyChangedOrChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);
        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;
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
        CheckPropertyChangedOrChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);
        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;
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
        CheckPropertyChangedOrChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);
        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;
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
    #region First group
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangedTrigger, eventHandler);
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangedTrigger, eventHandler);
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangedTrigger, eventHandler);
    }
    #endregion

    #region Second group
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangedTrigger, eventHandler);
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangedTrigger, eventHandler);
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangedTrigger, eventHandler);
    }
    #endregion

    #region Third group
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangedTrigger, eventHandler);
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangedTrigger, eventHandler);
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangedInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangedTrigger, eventHandler);
    }
    #endregion
    #endregion

    #region CollectionChangedAndItemPropertyChanging
    #region First group
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangingTrigger, eventHandler);
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangingTrigger, eventHandler);
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangingTrigger, eventHandler);
    }
    #endregion

    #region Second group
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangingTrigger, eventHandler);
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangingTrigger, eventHandler);
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangingTrigger, eventHandler);
    }
    #endregion

    #region Third group
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangingTrigger, eventHandler);
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangingTrigger, eventHandler);
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
        CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionChangedAndItemPropertyChangingInternal(context, observableCollection,
            collectionChangedTrigger, itemPropertyName, itemPropertyChangingTrigger, eventHandler);
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
        CheckCollectionItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionItemPropertyChangedInternal(context,
            observableCollection, itemPropertyName, itemPropertyChangedTrigger, eventHandler);
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
        CheckCollectionItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionItemPropertyChangedInternal(context,
            observableCollection, itemPropertyName, itemPropertyChangedTrigger, eventHandler);
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
        CheckCollectionItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionItemPropertyChangedInternal(context,
            observableCollection, itemPropertyName, itemPropertyChangedTrigger, eventHandler);
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
        CheckCollectionItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionItemPropertyChangingInternal(context,
            observableCollection, itemPropertyName, itemPropertyChangingTrigger, eventHandler);
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
        CheckCollectionItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionItemPropertyChangingInternal(context,
            observableCollection, itemPropertyName, itemPropertyChangingTrigger, eventHandler);
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
        CheckCollectionItemPropertyChangedOrChangingBindingArgs(context,
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;
        OnCollectionItemPropertyChangingInternal(context,
            observableCollection, itemPropertyName, itemPropertyChangingTrigger, eventHandler);
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

        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;

        _propertyChangedTriggerBindings.Where(pb =>
            pb.Context == context &&
            pb.Observable == (INotifyPropertyChanged)observable &&
            pb.PropertyName == propertyName &&
            pb.Trigger == trigger).ToList().ForEach(UnbindPropertyChangedInternal);
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

        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;

        _propertyChangedTriggerBindings.Where(pb =>
            pb.Context == context &&
            pb.Observable == (INotifyPropertyChanged)observable &&
            pb.PropertyName == propertyName).ToList().ForEach(UnbindPropertyChangedInternal);
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

        _propertyChangedTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.Observable == observable)
            .ToList().ForEach(UnbindPropertyChangedInternal);
    }

    /// <summary>
    /// Unbinds all trigger bindings made in a given context
    /// </summary>
    /// <param name="context"></param>
    public static void UnbindPropertyChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _propertyChangedTriggerBindings.Where(tb => tb.Context == context)
            .ToList().ForEach(UnbindPropertyChangedInternal);
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

        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;

        _propertyChangingTriggerBindings.Where(pb =>
            pb.Context == context &&
            pb.Observable == (INotifyPropertyChanging)observable &&
            pb.PropertyName == propertyName &&
            pb.Trigger == trigger).ToList().ForEach(UnbindPropertyChangingInternal);
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

        var propertyName = ((MemberExpression)observablePropertyGetterExpr.Body).Member.Name;

        _propertyChangingTriggerBindings.Where(pb =>
            pb.Context == context &&
            pb.Observable == (INotifyPropertyChanging)observable &&
            pb.PropertyName == propertyName).ToList().ForEach(UnbindPropertyChangingInternal);
    }

    public static void UnbindPropertyChanging
    (
        object context,
        INotifyPropertyChanging observable)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));

        _propertyChangingTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.Observable == observable).ToList().ForEach(UnbindPropertyChangingInternal);
    }

    public static void UnbindPropertyChanging(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _propertyChangingTriggerBindings.Where(tb =>
            tb.Context == context).ToList().ForEach(UnbindPropertyChangingInternal);
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

        _collectionChangedTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection &&
            tb.Trigger == trigger).ToList().ForEach(UnbindCollectionChangedInternal);
    }

    public static void UnbindCollectionChanged(object context, INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        _collectionChangedTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection).ToList().ForEach(UnbindCollectionChangedInternal);
    }

    public static void UnbindCollectionChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _collectionChangedTriggerBindings.Where(tb =>
            tb.Context == context).ToList().ForEach(UnbindCollectionChangedInternal);
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;

        _collectionChangedAndItemPropertyChangedTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection &&
            tb.CollectionChangedTrigger == collectionChangedTrigger &&
            tb.ItemPropertyName == itemPropertyName &&
            tb.ItemPropertyChangedTrigger == itemPropertyChangedTrigger)
            .ToList().ForEach(UnbindCollectionChangedAndItemPropertyChangedInternal);
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;

        _collectionChangedAndItemPropertyChangedTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection &&
            tb.CollectionChangedTrigger == collectionChangedTrigger &&
            tb.ItemPropertyName == itemPropertyName)
            .ToList().ForEach(UnbindCollectionChangedAndItemPropertyChangedInternal);
    }

    public static void UnbindCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object collectionChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));

        _collectionChangedAndItemPropertyChangedTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection &&
            tb.CollectionChangedTrigger == collectionChangedTrigger)
            .ToList().ForEach(UnbindCollectionChangedAndItemPropertyChangedInternal);
    }

    public static void UnbindCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        _collectionChangedAndItemPropertyChangedTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection)
            .ToList().ForEach(UnbindCollectionChangedAndItemPropertyChangedInternal);
    }

    public static void UnbindCollectionChangedAndItemPropertyChanged<TItem, TItemProperty>(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _collectionChangedAndItemPropertyChangedTriggerBindings.Where(tb =>
            tb.Context == context)
            .ToList().ForEach(UnbindCollectionChangedAndItemPropertyChangedInternal);
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;

        _collectionChangedAndItemPropertyChangingTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection &&
            tb.CollectionChangedTrigger == collectionChangedTrigger &&
            tb.ItemPropertyName == itemPropertyName &&
            tb.ItemPropertyChangingTrigger == itemPropertyChangingTrigger)
            .ToList().ForEach(UnbindCollectionChangedAndItemPropertyChangingInternal);
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;

        _collectionChangedAndItemPropertyChangingTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection &&
            tb.CollectionChangedTrigger == collectionChangedTrigger &&
            tb.ItemPropertyName == itemPropertyName)
            .ToList().ForEach(UnbindCollectionChangedAndItemPropertyChangingInternal);
    }

    public static void UnbindCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object collectionChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));

        _collectionChangedAndItemPropertyChangingTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection &&
            tb.CollectionChangedTrigger == collectionChangedTrigger)
            .ToList().ForEach(UnbindCollectionChangedAndItemPropertyChangingInternal);
    }

    public static void UnbindCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        _collectionChangedAndItemPropertyChangingTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection)
            .ToList().ForEach(UnbindCollectionChangedAndItemPropertyChangingInternal);
    }

    public static void UnbindCollectionChangedAndItemPropertyChanging<TItem, TItemProperty>(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _collectionChangedAndItemPropertyChangingTriggerBindings.Where(tb =>
            tb.Context == context)
            .ToList().ForEach(UnbindCollectionChangedAndItemPropertyChangingInternal);
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;

        _collectionItemPropertyChangedTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection &&
            tb.ItemPropertyName == itemPropertyName &&
            tb.ItemPropertyChangedTrigger == itemPropertyChangedTrigger)
            .ToList().ForEach(UnbindCollectionItemPropertyChangedInternal);
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;

        _collectionItemPropertyChangedTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection &&
            tb.ItemPropertyName == itemPropertyName)
            .ToList().ForEach(UnbindCollectionItemPropertyChangedInternal);
    }

    public static void UnbindCollectionItemPropertyChanged<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        _collectionItemPropertyChangedTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection)
            .ToList().ForEach(UnbindCollectionItemPropertyChangedInternal);
    }

    public static void UnbindCollectionItemPropertyChanged<TItem, TItemProperty>(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _collectionItemPropertyChangedTriggerBindings.Where(tb =>
            tb.Context == context)
            .ToList().ForEach(UnbindCollectionItemPropertyChangedInternal);
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;

        _collectionItemPropertyChangingTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection &&
            tb.ItemPropertyName == itemPropertyName &&
            tb.ItemPropertyChangingTrigger == itemPropertyChangingTrigger)
            .ToList().ForEach(UnbindCollectionItemPropertyChangingInternal);
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

        var itemPropertyName = ((MemberExpression)itemPropertyGetterExpr.Body).Member.Name;

        _collectionItemPropertyChangingTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection &&
            tb.ItemPropertyName == itemPropertyName)
            .ToList().ForEach(UnbindCollectionItemPropertyChangingInternal);
    }

    public static void UnbindCollectionItemPropertyChanging<TItem, TItemProperty>
    (
        object context,
        INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        _collectionItemPropertyChangingTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection)
            .ToList().ForEach(UnbindCollectionItemPropertyChangingInternal);
    }

    public static void UnbindCollectionItemPropertyChanging<TItem, TItemProperty>(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _collectionItemPropertyChangingTriggerBindings.Where(tb =>
            tb.Context == context)
            .ToList().ForEach(UnbindCollectionItemPropertyChangingInternal);
    }
    #endregion

    public static void Unbind(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        UnbindPropertyChanged(context);
        UnbindPropertyChanging(context);
        UnbindCollectionChanged(context);
    }
    #endregion
    #endregion

    #region Private methods
    #region Argument checking methods
    private static void CheckPropertyChangedOrChangingBindingArgs
    (
        object context,
        object observable,
        object propertyGetterExpr,
        object trigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observable, nameof(observable));
        ArgumentNullException.ThrowIfNull(propertyGetterExpr, nameof(propertyGetterExpr));
        ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));
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
    }

    private static void CheckCollectionChangedAndItemPropertyChangedOrChangingBindingArgs
    (
        object context,
        object observableCollection,
        object collectionChangedTrigger,
        object itemPropertyGetterExpr,
        object itemPropertyChangedTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(collectionChangedTrigger, nameof(collectionChangedTrigger));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangedTrigger, nameof(itemPropertyChangedTrigger));
    }

    private static void CheckCollectionItemPropertyChangedOrChangingBindingArgs
    (
        object context,
        object observableCollection,
        object itemPropertyGetterExpr,
        object itemPropertyChangedOrChangingTrigger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));
        ArgumentNullException.ThrowIfNull(itemPropertyGetterExpr, nameof(itemPropertyGetterExpr));
        ArgumentNullException.ThrowIfNull(itemPropertyChangedOrChangingTrigger, nameof(itemPropertyChangedOrChangingTrigger));
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
        _propertyChangedTriggerBindings.Add
        (
            new PropertyChangedTriggerBinding
            (
                context,
                observable,
                propertyName,
                trigger, eventHandler
            )
        );
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
        _propertyChangingTriggerBindings.Add
        (
            new PropertyChangingTriggerBinding
            (
                context,
                observable,
                propertyName,
                trigger,
                eventHandler
            )
        );
    }

    private static void OnCollectionChangedInternal
    (
        object context,
        INotifyCollectionChanged observableCollection,
        object trigger,
        NotifyCollectionChangedEventHandler eventHandler)
    {
        observableCollection.CollectionChanged += eventHandler;
        _collectionChangedTriggerBindings.Add
        (
            new CollectionChangedTriggerBinding
            (
                context,
                observableCollection,
                trigger,
                eventHandler
            )
        );
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
        _collectionChangedAndItemPropertyChangedTriggerBindings.Add
        (
            new CollectionChangedAndItemPropertyChangedTriggerBinding
            (
                context,
                observableCollection,
                collectionChangedTrigger,
                itemPropertyName,
                itemPropertyChangedTrigger,
                eventHandler
            )
        );
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
        _collectionChangedAndItemPropertyChangingTriggerBindings.Add
        (
            new CollectionChangedAndItemPropertyChangingTriggerBinding
            (
                context,
                observableCollection,
                collectionChangedTrigger,
                itemPropertyName,
                itemPropertyChangingTrigger,
                eventHandler
            )
        );
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
        _collectionItemPropertyChangedTriggerBindings.Add
        (
            new CollectionItemPropertyChangedTriggerBinding
            (
                context,
                observableCollection,
                itemPropertyName,
                itemPropertyChangedTrigger,
                eventHandler
            )
        );
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
        _collectionItemPropertyChangingTriggerBindings.Add
        (
            new CollectionItemPropertyChangingTriggerBinding
            (
                context,
                observableCollection,
                itemPropertyName,
                itemPropertyChangingTrigger,
                eventHandler
            )
        );
    }
    #endregion

    #region Unbinding methods
    private static void UnbindPropertyChangedInternal(PropertyChangedTriggerBinding triggerBinding)
    {
        triggerBinding.Observable.PropertyChanged -= triggerBinding.EventHandler;
        _propertyChangedTriggerBindings.Remove(triggerBinding);
    }

    private static void UnbindPropertyChangingInternal(PropertyChangingTriggerBinding triggerBinding)
    {
        triggerBinding.Observable.PropertyChanging -= triggerBinding.EventHandler;
        _propertyChangingTriggerBindings.Remove(triggerBinding);
    }

    private static void UnbindCollectionChangedInternal(CollectionChangedTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;
        _collectionChangedTriggerBindings.Remove(triggerBinding);
    }

    private static void UnbindCollectionChangedAndItemPropertyChangedInternal(CollectionChangedAndItemPropertyChangedTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;

        _propertyChangedTriggerBindings.Where(pb =>
            pb.Context == triggerBinding.Context &&
            pb.PropertyName == triggerBinding.ItemPropertyName &&
            pb.Trigger == triggerBinding.ItemPropertyChangedTrigger).ToList().ForEach(UnbindPropertyChangedInternal);

        _collectionChangedAndItemPropertyChangedTriggerBindings.Remove(triggerBinding);
    }

    private static void UnbindCollectionChangedAndItemPropertyChangingInternal(CollectionChangedAndItemPropertyChangingTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;

        _propertyChangingTriggerBindings.Where(pb =>
            pb.Context == triggerBinding.Context &&
            pb.PropertyName == triggerBinding.ItemPropertyName &&
            pb.Trigger == triggerBinding.ItemPropertyChangingTrigger).ToList().ForEach(UnbindPropertyChangingInternal);

        _collectionChangedAndItemPropertyChangingTriggerBindings.Remove(triggerBinding);
    }

    private static void UnbindCollectionItemPropertyChangedInternal(CollectionItemPropertyChangedTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;

        _propertyChangedTriggerBindings.Where(pb =>
            pb.Context == triggerBinding.Context &&
            pb.PropertyName == triggerBinding.ItemPropertyName &&
            pb.Trigger == triggerBinding.ItemPropertyChangedTrigger).ToList().ForEach(UnbindPropertyChangedInternal);

        _collectionItemPropertyChangedTriggerBindings.Remove(triggerBinding);
    }

    private static void UnbindCollectionItemPropertyChangingInternal(CollectionItemPropertyChangingTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;

        _propertyChangingTriggerBindings.Where(pb =>
            pb.Context == triggerBinding.Context &&
            pb.PropertyName == triggerBinding.ItemPropertyName &&
            pb.Trigger == triggerBinding.ItemPropertyChangingTrigger).ToList().ForEach(UnbindPropertyChangingInternal);

        _collectionItemPropertyChangingTriggerBindings.Remove(triggerBinding);
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