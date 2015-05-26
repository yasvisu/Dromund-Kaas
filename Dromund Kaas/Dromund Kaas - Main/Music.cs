using System;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace DromundKaas
{
    class VoiceOver
    {
        public string Name;
        private SpeechSynthesizer WalkieTalkie;

        public VoiceOver(string Name)
        {
            this.Name = Name;
            this.WalkieTalkie = new SpeechSynthesizer();
        }

        public void UtterAsync(string Words)
        {
            this.WalkieTalkie.SpeakAsync(Words);
        }

        public void Utter(string Words)
        {
            this.WalkieTalkie.Speak(Words);
        }

    }
}
