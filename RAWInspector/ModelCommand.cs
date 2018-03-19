using System;
using GalaSoft.MvvmLight.CommandWpf;

namespace RAWInspector
{
    internal sealed class ModelCommand : RelayCommand
    {
        public ModelCommand(Action execute, bool keepTargetAlive = false)
            : base(execute, keepTargetAlive)
        {
        }

        public ModelCommand(Action execute, Func<bool> canExecute, bool keepTargetAlive = false)
            : base(execute, canExecute, keepTargetAlive)
        {
        }
    }

    internal sealed class ModelCommand<T> : RelayCommand<T>
    {
        public ModelCommand(Action<T> execute, bool keepTargetAlive = false)
            : base(execute, keepTargetAlive)
        {
        }

        public ModelCommand(Action<T> execute, Func<T, bool> canExecute, bool keepTargetAlive = false)
            : base(execute, canExecute, keepTargetAlive)
        {
        }
    }
}