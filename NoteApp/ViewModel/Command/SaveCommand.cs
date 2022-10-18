using NoteApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoteApp.ViewModel.Command
{
    public class SaveCommand: ICommand
    {
        public NotesVM VM { get; set; }

        public SaveCommand(NotesVM VM)
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
            NotesWindow notesWindow = parameter as NotesWindow;
            VM.SaveRTB(notesWindow);
        }
    }
}
