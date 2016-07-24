using System;
using System.Drawing;
using System.Windows.Forms;
using TalkerLibrary;

namespace FormsTalker
{
    
    public partial class Form1 : Form
    {
        AnswerBuilder ab = new AnswerBuilder();
        SQLiteHelper con = new SQLiteHelper();

        public Form1()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            SendMessage();
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

        private void SendMessage()
        {
            string query = InputRichTextBox.Text;
            InputRichTextBox.Text = string.Empty;
            AppendText(richTextBox1, Color.Black, ">> " + query);
            con.Save(true, "me", query);
            string answer = ab.Answer(query);
            AppendText(richTextBox1, Color.Blue, "<< " + answer + "\n");
            con.Save(false, "boot", answer);
        }

        private void InputRichTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void InputRichTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
                e.Handled = true;
            }
        }
    }
}
