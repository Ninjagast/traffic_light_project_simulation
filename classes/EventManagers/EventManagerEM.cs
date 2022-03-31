﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.Xna.Framework.Graphics;

namespace traffic_light_simulation.classes.EventManagers
{
    public class EventManagerEm
    {
        private static EventManagerEm _instance;
        private List<IEventManager> _subscribed = new List<IEventManager>();
        private static readonly object Padlock = new object();

        private EventManagerEm() {}
        
        public static EventManagerEm Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new EventManagerEm();
                    }
                    return _instance;
                }
            }
        }

        public void Subscribe(IEventManager inputObject)
        {
            _subscribed.Add(inputObject);
        }

        public void OnStateChange(IEventManager manager, int id, Enum state)
        {
            manager.OnStateChange(id, state);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var subbed in _subscribed)
            {
                subbed.Draw(spriteBatch);
            }
        }

        public void Update()
        {
            foreach (var subbed in _subscribed)
            {
                subbed.Update();
            }
        }
    }
}