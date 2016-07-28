using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TalkerLibrary;

namespace FormsTalker
{
    
    public partial class Form1 : Form
    {
        SQLiteHelper con = new SQLiteHelper();
        AnswerBuilder ab;
        List<string> history = new List<string>();

        public Form1()
        {
            InitializeComponent();
            ab = new AnswerBuilder(con);
            ab.Rules = con.LoadRules();
            InputRichTextBox.Focus();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            SendMessage(InputRichTextBox.Text);
        }

        private void InputRichTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               // SendMessage();
            }
        }

        // Append text of the given color.
        void AppendText(RichTextBox box, Color color, string text)
        {
            int start = box.TextLength;
            box.AppendText(text);
            int end = box.TextLength;

            // Textbox may transform chars, so (end-start) != text.Length
            box.Select(start, 3);
            {
                box.SelectionColor = Color.Green;
                // could set box.SelectionBackColor, box.SelectionFont too.
            }
            box.SelectionLength = 0; // clear
                                     // Textbox may transform chars, so (end-start) != text.Length
            box.Select(start+3, end - start -3);
            {
                box.SelectionColor = color;
                // could set box.SelectionBackColor, box.SelectionFont too.
            }
            box.SelectionLength = 0; // clear
        }

        private void SendMessage(string query)
        {
            InputRichTextBox.Text = string.Empty;
            Voice.Read(1, query);
            AppendText(richTextBox1, Color.Black, ">> " + query);
            con.Save(true, "me", query);
            string answer = ab.Answer(query);
            AppendText(richTextBox1, Color.Blue, "<< " + answer + "\n");
            Voice.Read(2, answer);
            con.Save(false, "boot", answer);
            if(Autoplay)
            {
                string answer2 = ab.Answer(answer);
                InputRichTextBox.AppendText(answer2);
                Thread.Sleep(1);
                SendMessage(answer2 + "\n");
            }
        }

        private void InputRichTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        int i = 0;

        private void InputRichTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                history.Add(InputRichTextBox.Text);
                SendMessage(InputRichTextBox.Text);
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                if (i == 0)
                {
                   history.Add(InputRichTextBox.Text);
                }
                if (history.Count > i)
                {
                    i++;
                    InputRichTextBox.Text = history[i];
                }
            }
            if (e.KeyCode == Keys.F2)
            {
                SwitchViews();
            }
            if (e.KeyCode == Keys.Down)
            {
                if(i>0)
                {
                    i--;
                    InputRichTextBox.Text = history[i];
                }
            }
        }
        bool isChating = true;

        public bool Autoplay { get; private set; }

        private void SwitchViews()
        {

            if (isChating)
            {
                richTextBox1.Text = con.ShowRulesForEdit(ab.Rules);
            }
            else
            {
                ab.Rules.Clear();
                con.SaveEditedRules(richTextBox1.Text);
                richTextBox1.Text = "";
                ab.Rules = con.LoadRules();
            }
            isChating = !isChating;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                SwitchViews();
            }
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                SwitchViews();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Autoplay = checkBox1.Checked;
            SendMessage("Hi Angelica!" + "\n");
        }
    }
}
