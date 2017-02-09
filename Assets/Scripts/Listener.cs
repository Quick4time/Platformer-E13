using UnityEngine;
using System.Collections;
// 146 Интерфейс Listener

    public enum EVENT_TYPE { SPEED_CHANGE}

    public interface IListener
    {
        void OnEvent (EVENT_TYPE Event_Type, Component Sender, object Param = null);
    }


