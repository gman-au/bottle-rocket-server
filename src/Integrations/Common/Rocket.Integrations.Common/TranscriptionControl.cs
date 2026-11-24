using System;
using System.Threading.Tasks;
using Rocket.Interfaces;

namespace Rocket.Integrations.Common
{
    public class TranscriptionControl : ITranscriptionControl
    {
        public Func<string, string, Task> AddTextTranscriptionCallback { get; set; }
    }
}