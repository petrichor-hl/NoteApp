using NoteApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows;
using System.IO;

namespace NoteApp.ViewModel.Command.RTBCommand
{
    public class SelectedNoteChanged: ICommand
    {
        public NotesVM VM;

        public SelectedNoteChanged(NotesVM VM)
        {
            this.VM = VM;
        }


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
            RichTextBox richTextBox = parameter as RichTextBox;
            richTextBox.Document.Blocks.Clear();

            VM.FetchNoteDataToContentRTB(richTextBox);
        }
    }
}
