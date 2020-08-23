using System;

namespace Deepwell
{
    /*
     * DWEventQueue
     * An generic action event queue
     * To be used similar to regular "event Action", but with a delegate that firer whenever an action is added
     * This offers the possibility for typical value change events to perform an initial setting of the value when the action is registered
     * e.g.:
     * DWEventQueue<int> SomeEventQueue = new DWEventQueue<int>();
     * SomeEventQueue.ActionAddedDelegate = (Action<int> aDel) => { aDel(_someValue); };
     * (C) 2020 Deepwell.at
     */

    public class EventQueue<T>
    {
        public delegate void ActionAddedDelegateType(Action<T> a);
        public ActionAddedDelegateType ActionAddedDelegate = null;
        private event Action<T> _queue;

        public static EventQueue<T> operator +(EventQueue<T> q, Action<T> a)
        {
            q._queue += a;
            q.ActionAddedDelegate?.Invoke(a);
            return q;
        }

        public static EventQueue<T> operator -(EventQueue<T> q, Action<T> a)
        {
            q._queue -= a;
            return q;
        }

        public void Invoke(T v)
        {
            _queue?.Invoke(v);
        }
    }    
}
