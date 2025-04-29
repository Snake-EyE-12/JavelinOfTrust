using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterController.Platformer
{
    public static class PlayerBehaviorUtilities
    {
        public static int Sign(float value) => value > 0 ? 1 : value < 0 ? -1 : 0;
    }
    public class Controller : MonoBehaviour
    {
        private Blackboard blackboard = new Blackboard("Data");
        
        [SerializeField] private List<PlayerBehavior> initialBehavior = new();

        private List<PlayerBehavior> activeBehaviors = new();

        private void Awake()
        {
            var data = new CustomCharacterSettingsData();
            data.Transform = transform;
            blackboard.SetVariable("Data", data);
            foreach (var behavior in initialBehavior)
            {
                AddBehavior(behavior);
            }
        }

        private bool behaviorChainUpdated;
        public void AddBehavior(PlayerBehavior behavior)
        {
            activeBehaviors.Add(behavior);
            behavior.Initialize(blackboard);
            behaviorChainUpdated = true;
        }
        public void RemoveBehavior(PlayerBehavior behavior)
        {
            activeBehaviors.Remove(behavior);
            behaviorChainUpdated = true;
        }

        private List<PrioritizedAction> actions = new();
        private void Update()
        {
            if (behaviorChainUpdated)
            {
                RefreshOrder();
            }
            actions.ForEach((x) => x.Invoke());
        }

        private void RefreshOrder()
        {
            actions.Clear();
            foreach (var behavior in activeBehaviors)
            {
                if(!behavior.enabled) continue;
                actions.AddRange(behavior.GetActions());
            }
            actions = actions.OrderBy((x) => x.Priority).ToList();
        }
    }

    public abstract class PlayerBehavior : MonoBehaviour
    {
        protected Blackboard blackboard;
        public void Initialize(Blackboard board)
        {
            blackboard = board;
            OnLoad();
        }

        /// <summary>
        /// Use Load() to load all prioritized methods in list
        /// </summary>
        protected abstract void OnLoad();
        protected List<Gimmick> gimmicks = new();

        public List<PrioritizedAction> GetActions()
        {
            List<PrioritizedAction> actions = new();
            foreach (var gimmick in gimmicks)
            {
                actions.AddRange(gimmick.GetActions());
            }
            return actions;
        }
        protected void Load(Gimmick gimmick)
        {
            gimmick.board = blackboard;
            gimmicks.Add(gimmick);
            gimmick.Load();
        }
    }

    public abstract class PlatformerPlayerBehavior : PlayerBehavior
    {
    }
    public abstract class TopDownPlayerBehavior : PlayerBehavior
    {
    }
    public abstract class ThreeDPlayerBehavior : PlayerBehavior
    {
    }

    public class PrioritizedAction
    {
        private Action Action { get; }
        public int Priority { get; }
        public PrioritizedAction(Action action, int priority)
        {
            this.Action = action;
            this.Priority = priority;
        }
        public void Invoke() => Action.Invoke();
    }

    public class Blackboard
    {
        public Blackboard(string defaultKey = null) => stringKey = defaultKey;
        [CanBeNull] private string stringKey;
        private Dictionary<int, BlackboardEntry> entries = new();
        public bool GetVariable<T>(string name, out T value)
        {
            BlackboardEntry entry = new BlackboardEntry(name, typeof(T));
            int key = entry.Key;
            if (entries.ContainsKey(key))
            {
                value = (T)entries[key].Value;
                return true;
            }

            Debug.LogWarning("Missing variable: " + name);
            value = default;
            return false;
        }
        public T GetVariable<T>(string name) => GetVariable<T>(name, out T value) ? value : default(T);
        public T GetOrSetDefaultVariable<T>(string name)
        {
            if (GetVariable(name, out T value))
            {
                return value;
            }
            T defaultValue = default;
            SetVariable(name, defaultValue);
            return defaultValue;
        }
        public T Value<T>() => stringKey == null ? default(T) :GetOrSetDefaultVariable<T>(stringKey);

        public void SetVariable<T>(string name, T value)
        {
            BlackboardEntry entry = new BlackboardEntry(name, typeof(T), value);
            int key = entry.Key;
            if (entries.ContainsKey(key))
            {
                entries[key] = entry;
            }
            else entries.Add(entry.Key, entry);
        }

        private class BlackboardEntry
        {
            public BlackboardEntry(string name, Type type, object value = null)
            {
                stringKey = name;
                this.type = type;
                this.value = value;
            }
            private string stringKey;
            private Type type;
            private object value;
            public object Value => value;
            public int Key => stringKey.GetHashCode() + type.GetHashCode();
        }
    }
    [Serializable]
    public abstract class Gimmick
    {
        public Blackboard board { get; set; }
        public bool Active => active;
        [SerializeField] private bool active;
        [SerializeField] private int[] priorities;
        private List<Action> actionList;
        public List<PrioritizedAction> GetActions()
        {
            List<PrioritizedAction> prioritizedActions = new();
            for (int i = 0; i < priorities.Length; i++)
            {
                prioritizedActions.Add(new PrioritizedAction(actionList[i], priorities[i]));
            }
            return prioritizedActions;
        }

        public void SetActions(List<Action> action)
        {
            actionList = action;
        }

        public virtual void Load()
        {
            
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class PrioritizedAttribute : Attribute
    {
        
    }
}
