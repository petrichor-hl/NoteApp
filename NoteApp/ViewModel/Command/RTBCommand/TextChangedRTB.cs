using NoteApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace NoteApp.ViewModel.Command
{
    public class TextChangedRTB: ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            NotesWindow notesWindow = parameter as NotesWindow;
            int amountCharacter = (new TextRange(notesWindow.contentRichTextBox.Document.ContentStart, notesWindow.contentRichTextBox.Document.ContentEnd)).Text.TrimEnd().Length;
            notesWindow.statusTextBlock.Text = $"{amountCharacter} characters";
        }
    }
}
