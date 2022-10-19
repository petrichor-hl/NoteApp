using NoteApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace NoteApp.ViewModel.Command.RTBCommand
{
    public class SelectionChangedRTB : ICommand
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
            var selectedWeight = notesWindow.contentRichTextBox.Selection.GetPropertyValue(Control.FontWeightProperty);
            notesWindow.BoldButton.IsChecked = selectedWeight != DependencyProperty.UnsetValue && selectedWeight.Equals(FontWeights.Bold);

            var selectedStyle = notesWindow.contentRichTextBox.Selection.GetPropertyValue(Control.FontStyleProperty);
            notesWindow.ItalicButton.IsChecked = selectedStyle != DependencyProperty.UnsetValue && selectedStyle.Equals(FontStyles.Italic);

            var selectedDecoration = notesWindow.contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            notesWindow.UnderlineButton.IsChecked = selectedDecoration != DependencyProperty.UnsetValue && selectedDecoration.Equals(TextDecorations.Underline);
        }
    }
}
