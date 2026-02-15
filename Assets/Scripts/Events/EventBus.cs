using System;
using System.Collections.Generic;

namespace DoubleTactics.Events
{
    public enum EventTypes
    {
        InputClick,
        StartGame,
        CardsGenerated,
        BoardFinished,
    }

    public static class EventBus
    {
        private static Dictionary<EventTypes, List<Action<IEventData>>> _subscribers = new();

        public static void Subscribe(EventTypes type, Action<IEventData> handler)
        {
            if (!_subscribers.TryGetValue(type, out var actions))
            {
                actions = new List<Action<IEventData>>();
                _subscribers[type] = actions;
            }

            actions.Add(handler);
        }

        public static void Unsubscribe(EventTypes type, Action<IEventData> handler)
        {
            if (_subscribers.TryGetValue(type, out var actions))
            {
                actions.Remove(handler);

                if (actions.Count <= 0)
                {
                    _subscribers.Remove(type);
                }
            }
        }

        public static void Invoke(EventTypes type, IEventData data = null)
        {
            if (_subscribers.TryGetValue(type, out var actions))
            {
                foreach (var action in actions)
                {
                    action.Invoke(data);
                }
            }
        }
    }
}
