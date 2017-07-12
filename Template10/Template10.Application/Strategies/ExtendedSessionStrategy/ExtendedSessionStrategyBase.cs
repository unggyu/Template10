﻿using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Strategies.ExtendedSessionStrategy
{
    public abstract class ExtendedSessionStrategyBase : IExtendedSessionStrategyInternal
    {
        ExtendedSessionManager _manager;
        public ExtendedSessionStrategyBase()
        {
            _manager = new ExtendedSessionManager();
        }

        public abstract Task StartupAsync(StartupInfo e);
        public abstract Task SuspendingAsync();

        public virtual void Dispose()
        {
            _manager.Dispose();
        }

        SessionKinds IExtendedSessionStrategyInternal.CurrentKind => _manager.CurrentKind;

        bool IExtendedSessionStrategyInternal.IsActive => _manager.IsActive;

        bool IExtendedSessionStrategyInternal.IsStarted => _manager.IsStarted;

        bool IExtendedSessionStrategyInternal.IsRevoked => _manager.IsRevoked;

        int IExtendedSessionStrategyInternal.Progress => _manager.Progress;

        async Task<bool> IExtendedSessionStrategyInternal.StartUnspecifiedAsync()
        {
            if (_manager.IsActive)
            {
                if (_manager.CurrentKind == SessionKinds.Unspecified)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return await _manager.StartAsync(SessionKinds.Unspecified);
            }
        }

        async Task<bool> IExtendedSessionStrategyInternal.StartSaveDataAsync()
        {
            if (_manager.IsActive)
            {
                if (_manager.CurrentKind == SessionKinds.SavingData)
                {
                    return true;
                }
                _manager.Recreate();
            }
            return await _manager.StartAsync(SessionKinds.SavingData);
        }
    }
}