using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterController.Platformer
{
    public class Controller : MonoBehaviour
    {
        private Blackboard data;
        [SerializeField] private List<PlayerFunctionality> functionality = new();

        private List<PlayerFunctionality> funcs = new();

        private void Awake()
        {
            foreach (var f in functionality)
            {
                AddFunctionality(f);
            }
        }
        public void AddFunctionality(PlayerFunctionality f)
        {
            funcs.Add(f);
            f.Initialize(data);
        }
        public void RemoveFunctionality(PlayerFunctionality f) => funcs.Remove(f);

        private List<Actionable> _actions = new();
        private void Update()
        {
            _actions.Clear();
            foreach (var f in funcs)
            {
                if(!f.enabled) continue;
                _actions.AddRange(f.GetConditions());
            }
            _actions.OrderBy((x) => x.Priority).ToList().ForEach((x) => x.Play());
        }
    }

    public abstract class PlayerFunctionality : MonoBehaviour
    {
        protected Blackboard blackboard;
        public void Initialize(Blackboard blackboard) => this.blackboard = blackboard;
        private void Awake()
        {
            GenerateConditions();
        }
        protected abstract void GenerateConditions();
        protected List<Actionable> actionables = new();
        public List<Actionable> GetConditions() => actionables;
        protected void AddCondition(Actionable c) => actionables.Add(c);
        protected void IfDo(Func<bool> @if, Action @do, int priority) => AddCondition(new Actionable(@if, @do, priority));

    }

    public class Actionable
    {
        private Func<bool> Check { get; }
        private Action Action { get; }
        public int Priority { get; }
        public Actionable(Func<bool> check, Action action, int priority)
        {
            this.Check = check;
            this.Action = action;
            this.Priority = priority;
        }

        public void Play()
        {
            if(Check.Invoke()) Action.Invoke();
        }
        
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
            value = default;
            return false;
        }

        public void SetVariable<T>(string name, T value)
        {
            BlackboardEntry entry = new BlackboardEntry(name, typeof(T), value);
            int key = entry.Key;
            entries.Add(key, entry);
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
}
