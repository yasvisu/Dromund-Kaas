using System.Speech.Synthesis;

namespace DromundKaas
{
    /// <summary>
    /// A SpeechSynthesizer wrapper.
    /// </summary>
    class VoiceOver
    {
        /// <summary>
        /// The Name of this VoiceOver.
        /// </summary>
        public string Name;
        private SpeechSynthesizer WalkieTalkie;

        /// <summary>
        /// Default constructor for VoiceOvers.
        /// </summary>
        /// <param name="Name">The Name of the VoiceOver.</param>
        public VoiceOver(string Name)
        {
            this.Name = Name;
            this.WalkieTalkie = new SpeechSynthesizer();
        }

        /// <summary>
        /// Let the VoiceOver say something asynchronously.
        /// </summary>
        /// <param name="Words">The words to utter.</param>
        public void UtterAsync(string Words)
        {
            this.WalkieTalkie.SpeakAsync(Words);
        }

        /// <summary>
        /// Let the VoiceOver say something synchronously.
        /// </summary>
        /// <param name="Words">The words to utter.</param>
        public void Utter(string Words)
        {
            this.WalkieTalkie.Speak(Words);
        }

    }
}
