using System;

namespace Deepwell
{
    /*
     * ObservableValue
     * An generic struct to create values that can have observers attached to them
     * Creating by e.g.:
     *   public ObservableValue<int> lives
     *   lives = new ObservableValue<int>(3);
     * Set values with:
     *   lives.value = 5;
     * Attach observer with:
     *   lives.observers += MyFunctionThatTakesAnInt;
     * Detach with -=
     * (C) 2020 Deepwell.at
     */

    public struct ObservableValue<T>
    {
        public event Action<T> observers;
        private T _value;

        public T value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value.Equals(value)) return;
                _value = value;
                observers?.Invoke(_value);
            }
        }

        public static implicit operator T(ObservableValue<T> of)
        {
            return of.value;
        }

        public static explicit operator ObservableValue<T>(T value)
        {
            return new ObservableValue<T>(value);
        }

        public ObservableValue(T v)
        {
            _value = v;
            observers = null;
        }
    }
}

