using System;
using JetBrains.Annotations;

namespace RAWInspector
{
    internal sealed class ModelCommands
    {
        public ModelCommands([NotNull] Model model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));

            Close = new ModelCommand<EventArgs>(s => { Model.OnCloseRequest(); });
        }

        private Model Model { get; }

        public ModelCommand<EventArgs> Close { get; }
    }
}