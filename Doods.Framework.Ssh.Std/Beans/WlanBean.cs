﻿using Doods.Framework.Std;

namespace Doods.Framework.Ssh.Std.Beans
{
    public class WlanBean : NotifyPropertyChangedBase
    {
        private int _linkQuality;

        private int _signalLevel;

        public int LinkQuality
        {
            get => _linkQuality;
            set => SetProperty(ref _linkQuality, value);
        }

        public int SignalLevel
        {
            get => _signalLevel;
            set => SetProperty(ref _signalLevel, value);
        }
    }
}