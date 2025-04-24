using System;
using System.Collections.Generic;
using System.Linq;
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
        private Blackboard blackboard = new Blackboard();
        
        [SerializeField] private List<PlayerBehavior> initialBehavior = new();

        private List<PlayerBehavior> activeBehaviors = new();

        private void Awake()
        {
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
        public void Initialize(Blackboard blackboard)
        {
            this.blackboard = blackboard;
            LoadGimmicks();
        }

        protected abstract void LoadGimmicks();
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
        }

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
        [SerializeField] private bool active;
        [SerializeField] private int[] priorities;
        private List<Action> actions;
        public List<PrioritizedAction> GetActions()
        {
            List<PrioritizedAction> prioritizedActions = new();
            for (int i = 0; i < priorities.Length; i++)
            {
                prioritizedActions.Add(new PrioritizedAction(actions[i], priorities[i]));
            }
            return prioritizedActions;
        }

        public void SetActions(List<Action> actions)
        {
            this.actions = actions;
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class PrioritizedAttribute : Attribute
    {
        
    }
}
