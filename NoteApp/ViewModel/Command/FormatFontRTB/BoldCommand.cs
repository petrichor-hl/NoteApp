using NoteApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace NoteApp.ViewModel.Command.FormatFontRTB
{
    public class BoldCommand : ICommand
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
            bool isButtonChecked = notesWindow.BoldButton.IsChecked ?? false;
            if (isButtonChecked)
                notesWindow.contentRichTextBox.Selection.ApplyPropertyValue(Control.FontWeightProperty, FontWeights.Bold);
            else
                notesWindow.contentRichTextBox.Selection.ApplyPropertyValue(Control.FontWeightProperty, FontWeights.Normal);
        }
    }
}
