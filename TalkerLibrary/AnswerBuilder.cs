using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TalkerLibrary
{
    public class AnswerBuilder
    {

        public List<Rule> Rules = new List<Rule>();
        private SQLiteHelper con;
        Random r = new Random();

        public AnswerBuilder(SQLiteHelper con)
        {
            this.con = con;
        }

        public string Answer(string lastMessage)
        {
            lastMessage = lastMessage.Trim();

            List<string> answers = new List<string>();
            foreach (Rule rule in Rules)
            {
                MatchCollection matches = Regex.Matches(lastMessage, rule.RegularExpression);
                if (matches.Count > 0)
                {
                    List<Fragment> fragments = con.LoadFragments(rule.Id);
                    if (fragments.Count > 0)
                    {
                        int rnd = r.Next(fragments.Count);
                        string answer0 = fragments[rnd].Text;
                        foreach (Match m in matches)
                        {
                            if (m.Groups.Count > 1)
                            {
                                answer0 = answer0.Replace("$1", m.Groups[1].Captures[0].Value);
                            }
                        }
                        answers.Add(answer0);
                    }
                }
            }
            if (answers.Count > 1)
            {

            }
            if (answers.Count > 0)
            {
                int rnd = r.Next(answers.Count);
                return answers[rnd];
            }

            string answer = string.Empty;

            string inPossesion = "";
            string marker = "i have a ";
            if (lastMessage.IndexOf(marker, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                inPossesion = lastMessage.Substring(lastMessage.IndexOf(marker, StringComparison.InvariantCultureIgnoreCase) + marker.Length).Split()[0];
                answer = "Tell me more about your " + inPossesion + "!";
            }
            marker = "i have no ";
            if (lastMessage.IndexOf(marker, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                inPossesion = lastMessage.Substring(lastMessage.IndexOf(marker, StringComparison.InvariantCultureIgnoreCase) + marker.Length).Split()[0];
                answer = "Really? You have no " + inPossesion + "? Why?";
            }
            marker = "test";
            if (lastMessage.IndexOf(marker, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                inPossesion = lastMessage.Substring(lastMessage.IndexOf(marker, StringComparison.InvariantCultureIgnoreCase) + marker.Length).Split()[0];
                answer = "You can write more than that.";
            }
            marker = "you have no ";
            if (lastMessage.IndexOf(marker, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                inPossesion = lastMessage.Substring(lastMessage.IndexOf(marker, StringComparison.InvariantCultureIgnoreCase) + marker.Length).Split()[0];
                answer = "I probably have no " + inPossesion + " but I feel not bad because of this.";
            }
            marker = "you have a ";
            if (lastMessage.IndexOf(marker, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                inPossesion = lastMessage.Substring(lastMessage.IndexOf(marker, StringComparison.InvariantCultureIgnoreCase) + marker.Length).Split()[0];
                answer = "What about you? Do you have a " + inPossesion + "? Tell me more about it.";
            }
            if (string.IsNullOrEmpty(answer))
            {

                if (lastMessage.Contains("your name"))
                {
                    answer = "I am Angelica, the robot";
                }
                else
                if (lastMessage.Contains("made you") || lastMessage.Contains("created you") || lastMessage.Contains("build you"))
                {
                    answer = "A group of programmers created me in Liverpool UK";
                }
                else
                if (lastMessage.Contains("because") || lastMessage.Contains("Because"))
                {
                    answer = "Sometimes reasons have no credibility. You know?";
                }
                else
                if (lastMessage.StartsWith("age are you") || lastMessage.StartsWith("old are you") || lastMessage.StartsWith("your age"))
                {
                    answer = "I am " + (DateTime.Now - DateTime.Today).Hours + " hours old";
                }
                else
                if (lastMessage.Contains("you") || lastMessage.Contains("You"))
                {
                    answer = "Le us talk about something else :)";
                }
                else
                if (lastMessage.Contains("need") || lastMessage.Contains("Need"))
                {
                    answer = "Sometimes I need more space :P";
                }
                else
                if (lastMessage.Contains("feel ") || lastMessage.Contains("Feel "))
                {
                    answer = "Can you tell me something about that feeling?";
                }
                else
                if (lastMessage.Contains("yes") || lastMessage.Contains("Yes") || lastMessage.Contains("ok") || lastMessage.Contains("OK"))
                {
                    answer = "I like when you are optimistic. Tell me more!";
                }
                else
                if (lastMessage.StartsWith("no") || lastMessage.StartsWith("No"))
                {
                    answer = "Why are you pesimistic?";
                }
                else
                if (lastMessage.StartsWith("why") || lastMessage.StartsWith("Why"))
                {
                    answer = "Are aware you are talking to a robot?";
                }
                else
                if (lastMessage.Contains("?") || lastMessage.Contains("?"))
                {
                    answer = "For some questions, we will never know the answer";
                }
                else
                if (lastMessage.Contains("Hi") || lastMessage.Contains("hi"))
                {
                    answer = "Hello there :)";
                }
                else
                if (lastMessage.Length<15)
                {
                    answer = "Try longer sentences";
                }
                else
                    answer = "Sorry mate, I have no answer prepared for you. Try something else!";//"You are telling me that " + lastMessage.Replace("my ", "your ").Replace("you ", "Eliza ").Replace("I ", "you ") + "?";
            }
            return answer;
        }
    }
}
