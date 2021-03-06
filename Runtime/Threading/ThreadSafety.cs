﻿using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Aquiris.SQLite.Threading
{
    internal sealed class ThreadSafety : MonoBehaviour
    {
        private static ThreadSafety _shared = default;
        private static readonly ConcurrentQueue<Action> _concurrentQueue = new ConcurrentQueue<Action>();

        internal static bool isPlaying { get; private set; }

        public static void Initialize()
        {
            isPlaying = Application.isPlaying;
            if (!isPlaying) return;
            
            if (_shared) return;
            _shared = new GameObject("SQLiteThreadSafety").AddComponent<ThreadSafety>();
            DontDestroyOnLoad(_shared.gameObject);
        }
        
        public static void RunOnMainThread(Action action)
        {
            if (isPlaying)
            {
                _concurrentQueue.Enqueue(action);
                return;
            }
            action.Invoke();
        }

        private void Update()
        {
            if (_concurrentQueue.Count == 0) return;
            while (_concurrentQueue.TryDequeue(out Action action)) action.Invoke();
        }
    }
}
