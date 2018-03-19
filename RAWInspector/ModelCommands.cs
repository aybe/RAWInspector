using System;
using JetBrains.Annotations;

namespace RAWInspector
{
    internal sealed class ModelCommands
    {
        public ModelCommands([NotNull] Model model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        private Model Model { get; }
    }
}