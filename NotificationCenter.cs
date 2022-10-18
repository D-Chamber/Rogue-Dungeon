using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarterGame
{
    public class NotificationCenter
    {
        private Dictionary<String, EventContainer> observers;
        private static NotificationCenter _instance;
        public static NotificationCenter Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new NotificationCenter();
                }
                return _instance;
            }
        }
        public NotificationCenter()
        {
            observers = new Dictionary<String, EventContainer>();
        }

        private class EventContainer
        {
            private event Action<Notification> Observer;
            public EventContainer()
            {
            }

            public void AddObserver(Action<Notification> observer)
            {
                Observer += observer;
            }

            public void RemoveObserver(Action<Notification> observer)
            {
                Observer -= observer;
            }

            public void SendNotification(Notification notification)
            {
                Observer(notification);
            }

            public bool IsEmpty()
            {
                return Observer == null;
            }
        }

        public void AddObserver(String notificationName, Action<Notification> observer)
        {
            if(!observers.ContainsKey(notificationName))
            {
                observers[notificationName] = new EventContainer();
            }
            observers[notificationName].AddObserver(observer);
        }

        public void RemoveObserver(String notificationName, Action<Notification> observer)
        {
            if(observers.ContainsKey(notificationName))
            {
                observers[notificationName].RemoveObserver(observer);
                if(observers[notificationName].IsEmpty())
                {
                    observers.Remove(notificationName);
                }
            }
        }

        public void PostNotification(Notification notification)
        {
            if(observers.ContainsKey(notification.Name))
            {
                observers[notification.Name].SendNotification(notification);
            }
        }
    }
}
