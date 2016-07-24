using System.Speech.Synthesis;

namespace TalkerLibrary
{
    public class Voice
    {
        private static SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        public static void Read(int type, string message)
        {
            synthesizer.Rate = 1;
            switch (type)
            {
                case 2:
                    synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Child);
                    break;
                case 1:
                    synthesizer.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Senior);
                    break;
            }
            synthesizer.Volume = 100;
            synthesizer.Speak(message);
        }
    }
}
