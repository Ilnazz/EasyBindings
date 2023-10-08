﻿using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace EasyBindings;

/// <summary>
/// Allows you to bind event handlers (triggers) to a <see cref="INotifyPropertyChanged"/>, <see cref="INotifyPropertyChanging"/> and <see cref="INotifyCollectionChanged"/> events.
/// </summary>
public static class TriggerBinder
{
    #region Fields
    private static readonly IList<PropertyChangedTriggerBinding> _propertyChangedTriggerBindings = new List<PropertyChangedTriggerBinding>();

    private static readonly IList<PropertyChangingTriggerBinding> _propertyChangingTriggerBindings = new List<PropertyChangingTriggerBinding>();

    private static readonly IList<CollectionChangedTriggerBinding> _collectionChangedTriggerBindings = new List<CollectionChangedTriggerBinding>();
    #endregion

    #region Public methods
    #region Binding
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
        CheckPropertyChangedOrPropertyChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);

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
        CheckPropertyChangedOrPropertyChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);

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
        CheckPropertyChangedOrPropertyChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);

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
        CheckPropertyChangedOrPropertyChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);
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
        CheckPropertyChangedOrPropertyChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);
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
        CheckPropertyChangedOrPropertyChangingBindingArgs(context, observable, observablePropertyGetterExpr, trigger);
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

    #region CollectionChangedDeep
    #region First
    public static void OnCollectionChangedDeep<TObservableCollection, TItem, TItemProperty>
    (
        object context,
        TObservableCollection observableCollection,
        Action<TObservableCollection, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItem, TItemProperty> itemPropertyChangedTrigger
    )
    where TObservableCollection : INotifyCollectionChanged
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedDeepBindingArgs(context, observableCollection,
            collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        if (observableCollection is IEnumerable<TItem> initialItems)
            foreach (TItem item in initialItems)
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

        OnCollectionChangedInternal(context, observableCollection, collectionChangedTrigger, eventHandler);
    }

    public static void OnCollectionChangedDeep<TObservableCollection, TItem, TItemProperty>
    (
        object context,
        TObservableCollection observableCollection,
        Action<TObservableCollection, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItemProperty> itemPropertyChangedTrigger
    )
    where TObservableCollection : INotifyCollectionChanged
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedDeepBindingArgs(context, observableCollection,
            collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        if (observableCollection is IEnumerable<TItem> initialItems)
            foreach (TItem item in initialItems)
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

        OnCollectionChangedInternal(context, observableCollection, collectionChangedTrigger, eventHandler);
    }

    public static void OnCollectionChangedDeep<TObservableCollection, TItem, TItemProperty>
    (
        object context,
        TObservableCollection observableCollection,
        Action<TObservableCollection, NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangedTrigger
    )
    where TObservableCollection : INotifyCollectionChanged
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedDeepBindingArgs(context, observableCollection,
            collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        if (observableCollection is IEnumerable<TItem> initialItems)
            foreach (TItem item in initialItems)
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

        OnCollectionChangedInternal(context, observableCollection, collectionChangedTrigger, eventHandler);
    }
    #endregion

    #region Second
    public static void OnCollectionChangedDeep<TObservableCollection, TItem, TItemProperty>
    (
        object context,
        TObservableCollection observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItem, TItemProperty> itemPropertyChangedTrigger
    )
    where TObservableCollection : INotifyCollectionChanged
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedDeepBindingArgs(context, observableCollection,
            collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        if (observableCollection is IEnumerable<TItem> initialItems)
            foreach (TItem item in initialItems)
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

        OnCollectionChangedInternal(context, observableCollection, collectionChangedTrigger, eventHandler);
    }

    public static void OnCollectionChangedDeep<TObservableCollection, TItem, TItemProperty>
    (
        object context,
        TObservableCollection observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItemProperty> itemPropertyChangedTrigger
    )
    where TObservableCollection : INotifyCollectionChanged
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedDeepBindingArgs(context, observableCollection,
            collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        if (observableCollection is IEnumerable<TItem> initialItems)
            foreach (TItem item in initialItems)
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

        OnCollectionChangedInternal(context, observableCollection, collectionChangedTrigger, eventHandler);
    }

    public static void OnCollectionChangedDeep<TObservableCollection, TItem, TItemProperty>
    (
        object context,
        TObservableCollection observableCollection,
        Action<NotifyCollectionChangedEventArgs> collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangedTrigger
    )
    where TObservableCollection : INotifyCollectionChanged
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedDeepBindingArgs(context, observableCollection,
            collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        if (observableCollection is IEnumerable<TItem> initialItems)
            foreach (TItem item in initialItems)
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

        OnCollectionChangedInternal(context, observableCollection, collectionChangedTrigger, eventHandler);
    }
    #endregion

    #region Third
    public static void OnCollectionChangedDeep<TObservableCollection, TItem, TItemProperty>
    (
        object context,
        TObservableCollection observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItem, TItemProperty> itemPropertyChangedTrigger
    )
    where TObservableCollection : INotifyCollectionChanged
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedDeepBindingArgs(context, observableCollection,
            collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        if (observableCollection is IEnumerable<TItem> initialItems)
            foreach (TItem item in initialItems)
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

        OnCollectionChangedInternal(context, observableCollection, collectionChangedTrigger, eventHandler);
    }

    public static void OnCollectionChangedDeep<TObservableCollection, TItem, TItemProperty>
    (
        object context,
        TObservableCollection observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItemProperty> itemPropertyChangedTrigger
    )
    where TObservableCollection : INotifyCollectionChanged
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedDeepBindingArgs(context, observableCollection,
            collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        if (observableCollection is IEnumerable<TItem> initialItems)
            foreach (TItem item in initialItems)
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

        OnCollectionChangedInternal(context, observableCollection, collectionChangedTrigger, eventHandler);
    }

    public static void OnCollectionChangedDeep<TObservableCollection, TItem, TItemProperty>
    (
        object context,
        TObservableCollection observableCollection,
        Action collectionChangedTrigger,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action itemPropertyChangedTrigger
    )
    where TObservableCollection : INotifyCollectionChanged
    where TItem : INotifyPropertyChanged
    {
        CheckCollectionChangedDeepBindingArgs(context, observableCollection,
            collectionChangedTrigger, itemPropertyGetterExpr, itemPropertyChangedTrigger);

        if (observableCollection is IEnumerable<TItem> initialItems)
            foreach (TItem item in initialItems)
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

        OnCollectionChangedInternal(context, observableCollection, collectionChangedTrigger, eventHandler);
    }
    #endregion

    #region Fourth
    // ЗДЕСЬ Я ПОНЯЛ, ЧТО ДЛЯ КАЖДОГО ТИПА (INotifyProperty[Changed|Changing] НУЖЕН ОТДЕЛЬНЫЙ МЕТОД - СДЕЛАТЬ ЭТО...
    public static void OnCollectionChangedDeep<TObservableCollection, TItem, TItemProperty>
    (
        object context,
        TObservableCollection observableCollection,
        Expression<Func<TItem, TItemProperty>> itemPropertyGetterExpr,
        Action<TItem, TItemProperty> itemPropertyChangedTrigger
    )
    where TObservableCollection : INotifyCollectionChanged
    where TItem : INotifyPropertyChanged
    {
        // Checkings ...

        if (observableCollection is IEnumerable<TItem> initialItems)
            foreach (TItem item in initialItems)
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

        OnCollectionChangedInternal(context, observableCollection, collectionChangedTrigger, eventHandler);
    }
    #endregion
    #endregion
    #endregion

    #region Unbinding
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
            tb.Trigger == trigger).ToList().ForEach(UnbindCollectionChanged);
    }

    public static void UnbindCollectionChanged(object context, INotifyCollectionChanged observableCollection)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(observableCollection, nameof(observableCollection));

        _collectionChangedTriggerBindings.Where(tb =>
            tb.Context == context &&
            tb.ObservableCollection == observableCollection).ToList().ForEach(UnbindCollectionChanged);
    }

    public static void UnbindCollectionChanged(object context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _collectionChangedTriggerBindings.Where(tb =>
            tb.Context == context).ToList().ForEach(UnbindCollectionChanged);
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
    #region Checkings
    private static void CheckPropertyChangedOrPropertyChangingBindingArgs
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

    private static void CheckCollectionChangedDeepBindingArgs
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
    #endregion

    #region Binding
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
    #endregion

    #region Unbinding
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

    private static void UnbindCollectionChanged(CollectionChangedTriggerBinding triggerBinding)
    {
        triggerBinding.ObservableCollection.CollectionChanged -= triggerBinding.EventHandler;
        _collectionChangedTriggerBindings.Remove(triggerBinding);
    }
    #endregion
    #endregion

    #region Data types
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
    #endregion
}