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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = BoldButton.IsChecked ?? false;
            if (isButtonChecked)
                contentRichTextBox.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
            else
                contentRichTextBox.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Normal);
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = ItalicButton.IsChecked ?? false;
            if (isButtonChecked)
                contentRichTextBox.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
            else
                contentRichTextBox.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Normal);
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = UnderlineButton.IsChecked ?? false;
            if (isButtonChecked)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
            {
                TextDecorationCollection textDecorations;
                (contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out textDecorations);
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
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
            UnderlineButton.IsChecked = selectedStyle != DependencyProperty.UnsetValue && selectedDecoration.Equals(TextDecorations.Underline);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var content = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);

            string filePath = string.Format(@"../../NoteFiles/{0}_{1}.rtf", notesVM.SelectedNote.NoteId, notesVM.SelectedNote.Title);
            var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            content.Save(fs, DataFormats.Rtf);

            notesVM.SelectedNote.FileLocation = filePath;
            DatabaseHelper.Update(notesVM.SelectedNote);
        }
    }
}
