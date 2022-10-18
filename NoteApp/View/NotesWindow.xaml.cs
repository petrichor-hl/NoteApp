using NoteApp.ViewModel;
using NoteApp.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NoteApp.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        NotesVM notesVM;
        public NotesWindow()
        {
            InitializeComponent();

            notesVM = Resources["NotesVM"] as NotesVM;
            notesVM.SelectedNoteChanged += NotesVM_SelectedNoteChanged;
        }

        private void NotesVM_SelectedNoteChanged(object sender, EventArgs e)
        {
            contentRichTextBox.Document.Blocks.Clear();
            if (notesVM.SelectedNote != null)
            {
                if (notesVM.SelectedNote.FileLocation != "Unknow")
                {
                    var fs = new FileStream(notesVM.SelectedNote.FileLocation, FileMode.Open, FileAccess.Read);
                    var content = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
                    content.Load(fs, DataFormats.Rtf);
                }
            }
        }

        private void RichTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            int amountCharacter = (new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd)).Text.TrimEnd().Length;
            statusTextBlock.Text = $"{amountCharacter} characters";
        }

        private void ContentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedWeight = contentRichTextBox.Selection.GetPropertyValue(FontWeightProperty);
            BoldButton.IsChecked = selectedWeight != DependencyProperty.UnsetValue &&  selectedWeight.Equals(FontWeights.Bold);
        
            var selectedStyle = contentRichTextBox.Selection.GetPropertyValue(FontStyleProperty);
            ItalicButton.IsChecked = selectedStyle != DependencyProperty.UnsetValue && selectedStyle.Equals(FontStyles.Italic);

            var selectedDecoration = contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UnderlineButton.IsChecked = selectedDecoration != DependencyProperty.UnsetValue && selectedDecoration.Equals(TextDecorations.Underline);
        }

    }
}
