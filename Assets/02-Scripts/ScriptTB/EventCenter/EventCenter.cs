using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ns
{

    public interface IEventInfo
    { }

    public class EventInfo<T> :IEventInfo
    {
        public UnityAction<T> actions;
        
        public EventInfo(UnityAction<T> action)
        {
            actions += action;
        }

    }

    public class EventInfo : IEventInfo 
    {
        public UnityAction actions;

        public EventInfo(UnityAction action)
        {
            actions += action;
        }
    }
    

    ///<summary>
    ///1.Dictionary
    ///
    ///<summary>
	public class EventCenter : BaseManager<EventCenter>
    {
        private Dictionary<string, IEventInfo> eventDic =
            new Dictionary<string, IEventInfo>();


        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="name">事件名字</param>
        /// <param name="action"> 事件 的委托函数</param>
        public void AddEventListener<T>(string name, UnityAction<T> action)
        {
            if(eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T>).actions += action;//将委托事件添加进去
            }
            else
            {
                eventDic.Add(name, new EventInfo<T>(action));
            }
        }
        public void AddEventListener(string name, UnityAction action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo).actions += action;//将委托事件添加进去
            }
            else
            {
                eventDic.Add(name, new EventInfo(action));
            }
        }

        /// <summary>
        /// 移出事件监听
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void RemoveEnvetListener<T>(string name, UnityAction<T> action)
        {
            if(eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T>).actions -= action;
            }
        }
        public void RemoveEnvetListener(string name, UnityAction action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo).actions -= action;
            }
        }
        /// <summary>
        /// 执行事件触发
        /// </summary>
        /// <param name="name"> 事件名字</param>
        public void EventTrigger<T>(string name, T info)
        {
            if(eventDic.ContainsKey(name))
            {

                //eventDic[name]();
                //eventDic[name].Invoke();
                if((eventDic[name] as EventInfo<T>).actions != null)
                (eventDic[name] as EventInfo<T>).actions.Invoke(info);
            }
        }
        public void EventTrigger(string name)
        {
            if (eventDic.ContainsKey(name))
            {

                //eventDic[name]();
                //eventDic[name].Invoke();
                if ((eventDic[name] as EventInfo).actions != null)
                    (eventDic[name] as EventInfo).actions.Invoke();
            }
        }

        /// <summary>
        /// 清空事件监听
        /// </summary>
        public void Clear()
        {
            eventDic.Clear();
        }
    }
}

