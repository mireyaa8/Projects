using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wordle_Project
{
    public partial class btnSubmit1 : Form
    {
        public btnSubmit1()
        {
            InitializeComponent();
        }

        private void btnSubmit_Load(object sender, EventArgs e)
        {

        }
    }
    public partial class WordleForm : Form
    {
        private const string WordsTextFile = @"wordsForWordle.txt";
        private const int RowLength = 5;
        private const string PlayAgainMessage = "Play again?";
        private int previousRow = 0;
        private int hintsCount = 0;
        private string currentWord = string.Empty;
        private List<TextBox> currentBoxes = new List<TextBox>();
        private bool ShouldGoToLeftTextBox(Keys pressedKey, int currentTextBoxIndex)
          => pressedKey == Keys.Left && !IsFirstTextBox(currentTextBoxIndex);
        private bool IsFirstTextBox(int currentTextBoxIndex)
            => (currentTextBoxIndex + 4) % RowLength == 0;
        private bool ShouldGoToRichtTextBox(Keys pressedKey, int currentTextBoxIndex)
            => (pressedKey == Keys.Right || IsAlphabetKeyPressed(pressedKey.ToString())) && !IsLastTextBox(currentTextBoxIndex);
        private bool IsLastTextBox(int currentTextBoxIndex) => currentTextBoxIndex % RowLength == 0;
        private bool IsAlphabetKeyPressed(string pressedKeystring)
            => pressedKeystring.Count() == 1 && char.IsLetter(pressedKeystring[0]);
        public WordleForm()
        {
            InitializeComponent();
            StartNewGame();
            foreach(TextBox tb in this.Controls.OfType<TextBox>)
            {
            tb.MouseClick += this.FocusTextBook;
             tb.KeyDown += Text.MoveCursor;
            }
            btnSubmit1.Click += btnSubmit_Click;
            btnHint.Click += btnHint_Click;
            btnReset.Click += btnReset_Click;
        }
        private void FocusTextBook(object sender, MouseEventArgs e)
        {
            if(sender is TextBox textBox)
            {
                textBox.Focus();
            }
        }
        private void MoveCursor(object sender, KeyEventArgs e)
        {
            var pressedKey = e.KeyCode;
            var senderTextBox = sender as TextBox;
            var currentTextBoxIndex = int.Parse(senderTextBox.Name.Replace("textBox", ""));
             if (ShouldGoToTheLeftBox(pressedKey, currentTextBoxIndex))
             {
                 currentTextBoxIndex--;
             }
             else if(ShouldGoToTheLeftBox(pressedKey, currentTextBoxIndex))
             {
                 currentTextBoxIndex++;
             }
             var textBox = GetTextBox(currentTextBoxIndex);
             textBox.Focus();
            
        }
       
        private void MoveCursor(object sender, KeyEventArgs e)
        {
            var pressedKey = e.KeyCode;
            var senderTextBox = sender as TextBox;
            var currentTextBoxIndex = int.Parse(senderTextBox.Name.Replace("textBox", ""));
            if (ShouldGoToLeftTextBox(pressedKey, currentTextBoxIndex))
            {
                currentTextBoxIndex--;
            }
            else if (ShouldGoToRichtTextBox(pressedKey, currentTextBoxIndex))
            {
                currentTextBoxIndex++;
            }

            var textBox = GetTextBox(currentTextBoxIndex);
            textBox.Focus();
        }
        private TextBox GetTextBox(int index)
        {
            string textBoxName = string.Format($"textBox{index}", index);
            return this.Controls[textBoxName] as TextBox;
        }
        private void StartNewGame()
        {
            var wordList = GetAllWords();

            var random = new Random();

            currentWord = wordList[random.Next(wordList.Count)];

            btnSubmit1.Enabled = true;
            btnHint.Enabled = true;
        }
         public WordleForm()
         {
             InitializeComponent();
             StartNewGame();
             foreach(TextBox tb in this.Controls.OfType<TextBox>())
             {
                 tb.MouseClick += this.FocusTextBox;
                 tb.keyDown += this.MoveCursor;
             }
         }
        private List<string> GetAllWords()
        {
            var allWords = new List<string>();
            using (StreamReader reader = new StreamReader(WordsTextFile))
            {
                while (!reader.EndOfStream)
                {
                    var nextLine = reader.ReadLine();
                    allWords.Add(nextLine);
                }
            }
            return allWords;
        }
        private void Submit(object sender, EventArgs e)
        {
            var userWord = GetInput();
            if (!IsInputValid(userWord))
            {
                DisplayInvalidWordMessage();
                return;
            }

            ColorBoxes();

            if (IsWordGuessed(userWord))
            {
                FinalizeWinGame();
                return;
            }
            if (IsCurrentRowLast())
            {
                FinalizeLostGame();
                return;
            }
            ModifyTextBoxesAvailability(false);
            previousRow++;
            ModifyTextBoxesAvailability(true);
        }
        private string GetInput()
        {
            this.currentBoxes = new List<TextBox>();
            string tempString = string.Empty;

            int firstTextBoxIndexOnRow = GetFirstTextBoxIndexOnRow();

            for (int i = 0; i < RowLength; i++)
            {
                var textBox = GetTextBox(firstTextBoxIndexOnRow + i);
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    return textBox.Text;
                }
                tempString += textBox.Text[0];
                this.currentBoxes.Add(textBox);
            }
            return tempString;
        }
        private int GetFirstTextBoxIndexOnRow() => this.previousRow * RowLength + 1;
        private bool IsInputValid(string input)
        {
            if (input.All(char.IsLetter) && input.Length == RowLength)
            {
                return true;
            }
            return false;
        }
        private void DisplayInvalidWordMessage()
        {
            MessageBox.Show("Plase enter a valid five-letter word.");
        }
        private void ColorBoxes()
        {
            for (int i = 0; i < this.currentBoxes.Count(); i++)
            {
                var textBox = this.currentBoxes[i];
                var currentTextBoxChar = textBox.Text.ToLower().FirstOrDefault();

                if (!WordContainsChar(currentTextBoxChar))
                {
                    textBox.BackColor = Color.Gray;
                }
                else if (!IsCharOnCorrectIndex(i, currentTextBoxChar))
                {
                    textBox.BackColor = Color.Yellow;
                }
                else
                {
                    textBox.BackColor = Color.LightGreen;
                }
            }
        }
        private bool WordContainsChar(char ch)
        {
            return this.currentWord.IndexOf(ch.ToString(), StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool IsCharOnCorrectIndex(int index, char ch) => this.currentWord[index] == ch;
        private bool IsCharOnCorrectIndex(int index, char ch) => this.currentWord[index] == ch;

        private bool IsWordGuessed(string attempt)
        {
            if (this.currentWord.Equals(attempt, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        private void FinalizeWinGame()
        {
            MessageBox.Show("Congratulations, you win!");

            this.btnSubmit1.Enabled = false;
            this.btnHint.Enabled = false;

            this.btnReset.Text = PlayAgainMessage;

            ModifyTextBoxesAvailability(false);
        }
        private void ModifyTextBoxesAvailability(bool shouldBeEnabled)
        {
            var firstTextBoxIndexOnRow = GetFirstTextBoxIndexOnRow();
            for (int i = 0; i < RowLength; i++)
            {
                var textBox = GetTextBox(firstTextBoxIndexOnRow + i);

                if (shouldBeEnabled)
                {
                    textBox.Enabled = true;
                    if (i == 0)
                    {
                        textBox.Focus();
                    }
                }
                else
                {
                    textBox.ReadOnly = true;
                    textBox.TabStop = false;
                }
            }
        }
        private bool IsCurrentRowLast()
        {
            var columnsCout = 6;
            return this.previousRow == columnsCout - 1;
        }
        private void FinalizeLostGame()
        {
            MessageBox.Show($"Sorry you didn't win this time!" + $" The correct word was: {this.currentWord}");
            btnSubmit1.Enabled = false;
            btnHint.Enabled = false;
            btnReset.Text = PlayAgainMessage;
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            this.previousRow = 0;
            this.hintsCount = 0;

            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;
                tb.BackColor = SystemColors.Window;
                tb.ReadOnly = false;
                tb.Enabled = true;
            }

            btnSubmit1.Enabled = true;
            btnHint.Enabled = true;

            btnReset.Text = "Reset";

            StartNewGame();
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var userWord = GetInput();
            if (!IsInputValid(userWord))
            {
                DisplayInvalidWordMessage();
                return;
            }

            ColorBoxes();

            if (IsWordGuessed(userWord))
            {
                FinalizeWinGame();
                return;
            }

            if (IsCurrentRowLast())
            {
                FinalizeLostGame();
                return;
            }

            ModifyTextBoxesAvailability(false);
            previousRow++;
            ModifyTextBoxesAvailability(true);
        }

        private void btnHint_Click(object sender, EventArgs e)
        {
            var unavailablePositions = GetUnavailablePositions();
            if (unavailablePositions.Count == RowLength)
            {
                ShowInvalidUseOfHintMessage();
                return;
            }
            RevealRandomWordLetter(unavailablePositions);
        }
        private void GameRestart(object sender, EventArgs e)
        {
            Application.Restart();
        }
        private void GiveHint(object sender, EventArgs e)
        {
            var unavailablePositions = GetUnavailablePositions();
            if (unavailablePositions.Count == RowLength)
            {
                ShowInvalidUseOfHintMessage();
                return;
            }
            RevealRandomWordLetter(unavailablePositions);
        }
        private List<int> GetUnavailablePositions()
        {
            var firstIndexOnRow = GetFirstTextBoxIndexOnRow();
            var positions = new List<int>();
            for (int i = 0; i < RowLength; i++)
            {
                var textBoxIndex = firstIndexOnRow + i;
                var textBox = GetTextBox(textBoxIndex);
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    positions.Add(textBoxIndex);
                }
            }
            return positions;
        }
        private void ShowInvalidUseOfHintMessage()
        {
            MessageBox.Show("Free up a space for a hint.");
            this.btnSubmit1.Focus();
            this.hintsCount -= 1;
        }
        private void RevealRandomWordLetter(List<int> unavailablePositions)
        {
            var random = new Random();
            while (true)
            {
                var randomIndex = random.Next(1, RowLength + 1);
                var randomTexBoxIndex = this.previousRow * RowLength + randomIndex;
                var textBox = GetTextBox(randomTexBoxIndex);
                if (textBox.Text != String.Empty)
                {
                    continue;
                }
                var hintLetter = this.currentWord[randomIndex - 1].ToString();
                textBox.Text = hintLetter;
                unavailablePositions.Add(randomTexBoxIndex);

                break;
            }
        }

        private void HintCounterMouseClick(object sender, MouseEventArgs e)
        {
            this.hintsCount++;
            if (this.hintsCount >= 3)
            {
                this.btnHint.Enabled = false;
            }
        }
    }
}





    

