using System;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface ITranscriptionControl
    {
        public Func<string, string, Task> AddTextTranscriptionCallback { get; set; }
    }
}