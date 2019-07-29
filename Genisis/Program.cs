using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Diagnostics;

//Potentially look at switching to google api for speech reconition and synthesis

//Create webscraper for finding specific data from web search

namespace Genisis
{
    class Program
    {
        static void Main(string[] args)
        {
            Frontend frontend = new Frontend();
            frontend.Listen();
            Console.ReadLine();
        }
    }

    class Frontend
    {
        private SpeechRecognitionEngine listener = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
        private SpeechSynthesizer talker = new SpeechSynthesizer();
        

        public Frontend()
        {
            listener.SetInputToDefaultAudioDevice();
            listener.LoadGrammar(new Grammar(Backend.grammar));
            talker.SetOutputToDefaultAudioDevice();
        }

        public void Listen()
        {
            listener.SpeechRecognized += Listener_SpeechRecognized;
            listener.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void Listener_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Backend.CheckCommand(e.Result);
        }
    }



    static class Backend
    {
        public static string userName = "Case";
        public static string assistantName = "Genisis";
        public static GrammarBuilder grammar = new GrammarBuilder();

        static Backend()
        {
            grammar.Append(assistantName);
            GrammarBuilder commandWords = new GrammarBuilder(new Choices(new string[]
            {
                "search",
                "what",
                "when",
                "how",
                "who",
                "why",
            }));
            grammar.Append(commandWords);
            grammar.AppendDictation();
        }

        public static void CheckCommand(RecognitionResult command)
        {
            if (command.Confidence < 0.8)
            {
                return;
            }
            else
            {
                HandleCommand(command);
            }
        }

        public static void HandleCommand(RecognitionResult command)
        {
            Process.Start("https://www.google.co.nz/search?q=", command.Text);
        }
    }
}
