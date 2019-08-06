using System.Collections.Generic;
using Doods.Framework.Ssh.Std.Enums;
using Doods.Framework.Std;

namespace Doods.Framework.Ssh.Std.Beans
{
    public class OsMemoryBean : NotifyPropertyChangedBase
    {
        private MemoryBean _totalMemory;
        private MemoryBean _totalUsed;
        private MemoryBean _totalFree;
        private float _percentageUsed;
        private string _errorMessage;
        public readonly Dictionary<string, long> MemoryDataValues;
        public MemoryBean TotalMemory
        {
            get => _totalMemory;
            internal set => SetProperty(ref _totalMemory, value);
        }

        public MemoryBean TotalUsed
        {
            get => _totalUsed;
            internal set => SetProperty(ref _totalUsed, value);
        }

        public MemoryBean TotalFree
        {
            get => _totalFree;
            internal set => SetProperty(ref _totalFree, value);
        }

        public float PercentageUsed
        {
            get => _percentageUsed;
            internal set => SetProperty(ref _percentageUsed, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            internal set => SetProperty(ref _errorMessage, value);
        }

        public OsMemoryBean(long totalMemory, long totalUsed, Dictionary<string, long> memoryDataValues)
        {
            TotalMemory = MemoryBean.From(Memory.KB, totalMemory);
            TotalUsed = MemoryBean.From(Memory.KB, totalUsed);
            TotalFree = MemoryBean.From(Memory.KB, totalMemory - totalUsed);
            PercentageUsed = (float) totalUsed / (float) totalMemory;
            MemoryDataValues = memoryDataValues;
        }

        public OsMemoryBean(string str, Dictionary<string, long> memoryDataValues)
        {
            ErrorMessage = str;
            MemoryDataValues = memoryDataValues;
        }
    }
}