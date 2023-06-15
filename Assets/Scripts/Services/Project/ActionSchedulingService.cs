using System;
using System.Collections.Generic;
using Zenject;

namespace Asteroids.Services.Project
{
    public interface IActionSchedulingService
    {
        void Schedule(float targetTime, Action action);
    }

    public class TimedActionsComparer : IComparer<float>
    {
        public int Compare(float x, float y)
        {
            var value = x.CompareTo(y);
            return value == 0 ? 1 : value;
        }
    }

    public class ActionSchedulingService : IActionSchedulingService
    {
        [Inject] private readonly IFrameInfoService _frameInfoService;

        private readonly IUnityExecutionService _unityExecutionService;

        private SortedList<float, Action> _actions = new SortedList<float, Action>(new TimedActionsComparer());
        private IList<float> _actionsKeys;
        private IList<Action> _actionsValues;

        public ActionSchedulingService(IUnityExecutionService unityExecutionService)
        {
            _unityExecutionService = unityExecutionService;
            _unityExecutionService.SubscribeToUpdate(Update);
            _actionsKeys = _actions.Keys;
            _actionsValues = _actions.Values;
        }

        public void Schedule(float targetTime, Action action)
        {
            _actions.Add(targetTime, action);
        }

        private void Update()
        {
            while (_actions.Count != 0 && _frameInfoService.StartTime > _actionsKeys[0])
            {
                var action = _actionsValues[0];
                _actions.RemoveAt(0);
                action();
            }
        }
    }
}
