using System;

namespace Chinchillada.Grid
{
    public interface IIntWindowSampler : IWindowSampler<int>
    {
    }
    
    [Serializable]
    public abstract class IntWindowSequenceSampler : WindowSequenceSampler<int> {}
}