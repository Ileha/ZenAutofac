using ZenAutofac.Interfaces;

namespace ZenAutofac.Entities.Factories
{
    //generated amount 10

    public abstract class PlaceholderFactory<TInstance> : Disposer, IFactory<TInstance>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal IFactory<TInstance> Nested { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public virtual TInstance Create()
        {
            return Nested.Create();
        }
    }

    public abstract class PlaceholderFactory<TP0, TInstance> : Disposer, IFactory<TP0, TInstance>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal IFactory<TP0, TInstance> Nested { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public virtual TInstance Create(TP0 param0)
        {
            return Nested.Create(param0);
        }
    }

    public abstract class PlaceholderFactory<TP0, TP1, TInstance> : Disposer, IFactory<TP0, TP1, TInstance>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal IFactory<TP0, TP1, TInstance> Nested { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public virtual TInstance Create(TP0 param0, TP1 param1)
        {
            return Nested.Create(param0, param1);
        }
    }

    public abstract class PlaceholderFactory<TP0, TP1, TP2, TInstance> : Disposer, IFactory<TP0, TP1, TP2, TInstance>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal IFactory<TP0, TP1, TP2, TInstance> Nested { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public virtual TInstance Create(TP0 param0, TP1 param1, TP2 param2)
        {
            return Nested.Create(param0, param1, param2);
        }
    }

    public abstract class PlaceholderFactory<TP0, TP1, TP2, TP3, TInstance> : Disposer, IFactory<TP0, TP1, TP2, TP3, TInstance>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal IFactory<TP0, TP1, TP2, TP3, TInstance> Nested { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public virtual TInstance Create(TP0 param0, TP1 param1, TP2 param2, TP3 param3)
        {
            return Nested.Create(param0, param1, param2, param3);
        }
    }

    public abstract class PlaceholderFactory<TP0, TP1, TP2, TP3, TP4, TInstance> : Disposer, IFactory<TP0, TP1, TP2, TP3, TP4, TInstance>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal IFactory<TP0, TP1, TP2, TP3, TP4, TInstance> Nested { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public virtual TInstance Create(TP0 param0, TP1 param1, TP2 param2, TP3 param3, TP4 param4)
        {
            return Nested.Create(param0, param1, param2, param3, param4);
        }
    }

    public abstract class PlaceholderFactory<TP0, TP1, TP2, TP3, TP4, TP5, TInstance> : Disposer, IFactory<TP0, TP1, TP2, TP3, TP4, TP5, TInstance>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal IFactory<TP0, TP1, TP2, TP3, TP4, TP5, TInstance> Nested { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public virtual TInstance Create(TP0 param0, TP1 param1, TP2 param2, TP3 param3, TP4 param4, TP5 param5)
        {
            return Nested.Create(param0, param1, param2, param3, param4, param5);
        }
    }

    public abstract class PlaceholderFactory<TP0, TP1, TP2, TP3, TP4, TP5, TP6, TInstance> : Disposer, IFactory<TP0, TP1, TP2, TP3, TP4, TP5, TP6, TInstance>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal IFactory<TP0, TP1, TP2, TP3, TP4, TP5, TP6, TInstance> Nested { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public virtual TInstance Create(TP0 param0, TP1 param1, TP2 param2, TP3 param3, TP4 param4, TP5 param5, TP6 param6)
        {
            return Nested.Create(param0, param1, param2, param3, param4, param5, param6);
        }
    }

    public abstract class PlaceholderFactory<TP0, TP1, TP2, TP3, TP4, TP5, TP6, TP7, TInstance> : Disposer, IFactory<TP0, TP1, TP2, TP3, TP4, TP5, TP6, TP7, TInstance>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal IFactory<TP0, TP1, TP2, TP3, TP4, TP5, TP6, TP7, TInstance> Nested { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public virtual TInstance Create(TP0 param0, TP1 param1, TP2 param2, TP3 param3, TP4 param4, TP5 param5, TP6 param6, TP7 param7)
        {
            return Nested.Create(param0, param1, param2, param3, param4, param5, param6, param7);
        }
    }

    public abstract class PlaceholderFactory<TP0, TP1, TP2, TP3, TP4, TP5, TP6, TP7, TP8, TInstance> : Disposer, IFactory<TP0, TP1, TP2, TP3, TP4, TP5, TP6, TP7, TP8, TInstance>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal IFactory<TP0, TP1, TP2, TP3, TP4, TP5, TP6, TP7, TP8, TInstance> Nested { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public virtual TInstance Create(TP0 param0, TP1 param1, TP2 param2, TP3 param3, TP4 param4, TP5 param5, TP6 param6, TP7 param7, TP8 param8)
        {
            return Nested.Create(param0, param1, param2, param3, param4, param5, param6, param7, param8);
        }
    }

    public abstract class PlaceholderFactory<TP0, TP1, TP2, TP3, TP4, TP5, TP6, TP7, TP8, TP9, TInstance> : Disposer, IFactory<TP0, TP1, TP2, TP3, TP4, TP5, TP6, TP7, TP8, TP9, TInstance>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal IFactory<TP0, TP1, TP2, TP3, TP4, TP5, TP6, TP7, TP8, TP9, TInstance> Nested { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public virtual TInstance Create(TP0 param0, TP1 param1, TP2 param2, TP3 param3, TP4 param4, TP5 param5, TP6 param6, TP7 param7, TP8 param8, TP9 param9)
        {
            return Nested.Create(param0, param1, param2, param3, param4, param5, param6, param7, param8, param9);
        }
    }

}
