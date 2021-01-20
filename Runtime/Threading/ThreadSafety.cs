﻿using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Aquiris.SQLite.Threading
{
    internal sealed class ThreadSafety : MonoBehaviour
    {
        private static ThreadSafety _shared = default;
        
        private static readonly ConcurrentQueue<Action> _concurrentQueue = new ConcurrentQueue<Action>();

        public static void Initialize()
        {
            if (_shared) return;
            _shared = new GameObject("SQLiteThreadSafety").AddComponent<ThreadSafety>();
            DontDestroyOnLoad(_shared.gameObject);
        }
        
        public static void RunOnMainThread(Action action)
        {
            _concurrentQueue.Enqueue(action);
        }

        private void Update()
        {
            if (_concurrentQueue.Count == 0) return;
            while (_concurrentQueue.TryDequeue(out Action action)) action.Invoke();
        }
    }
}