﻿using System.Collections.Concurrent;

namespace CarsCrawler.Infrastructure.CefCrawler
{
    public sealed class SingleThreadSynchronizationContext : SynchronizationContext
    {
        private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object?>> _queue =
            new BlockingCollection<KeyValuePair<SendOrPostCallback, object?>>();

        public override void Post(SendOrPostCallback d, object? state)
        {
            _queue.Add(new KeyValuePair<SendOrPostCallback, object?>(d, state));
        }

        public void RunOnCurrentThread()
        {
            while (_queue.TryTake(out var workItem, Timeout.Infinite))
            {
                workItem.Key(workItem.Value);
            }
        }

        public void Complete()
        {
            _queue.CompleteAdding();
        }
    }
}
