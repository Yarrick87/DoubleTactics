using System;
using System.Collections.Generic;

public enum EventTypes
{
    Click,
}

public static class EventBus
{
    private static Dictionary<EventTypes, List<Action>> _subscribers = new ();
    
    public static void Subscribe(EventTypes type, Action handler)
    {
        if (!_subscribers.TryGetValue(type, out var actions))
        {
            actions = new List<Action>();
            _subscribers[type] = actions;
        }
        
        actions.Add(handler);
    }
    
    public static void Unsubscribe(EventTypes type, Action handler)
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
    
    public static void Invoke(EventTypes type)
    {
        if (_subscribers.TryGetValue(type, out var actions))
        {
            foreach (var action in actions)
            {
                action.Invoke();
            }
        }
    }
}
