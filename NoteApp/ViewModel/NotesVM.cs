using NoteApp.Model;
using NoteApp.View;
using NoteApp.ViewModel.Command;
using NoteApp.ViewModel.Command.EditCommand;
using NoteApp.ViewModel.Command.FormatFontRTB;
using NoteApp.ViewModel.Command.RTBCommand;
using NoteApp.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace NoteApp.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        // Property
        public ObservableCollection<Notebook> Notebooks { get; set; }

        public ObservableCollection<Note> Notes { get; set; }

        private Notebook selectedNotebook;

        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                OnPropertyChanged("SelectedNotebook");

                GetNotes();
            }
        }

        private Note selectedNote;

        public Note SelectedNote
        {
            get { return selectedNote; }
            set 
            { 
                selectedNote = value;
                OnPropertyChanged("SelectedNote");
                // SelectedNoteChanged?.Invoke(this, new EventArgs());
            }
        }


        private Visibility isVisible;

        public Visibility IsVisible
        {
            get { return isVisible; }
            set {
                isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }


        // Command
        public NewNotebookCommand NewNotebookCommand { get; set; }

        public NewNoteCommand NewNoteCommand { get; set; }

        public StartEditCommand StartEditCommand { get; set; }

        public EndEditCommand EndEditCommand { get; set; }

        public CloseAppCommand CloseAppCommand { get; set; }

        public BoldCommand BoldCommand { get; set; }

        public UnderlineCommand UnderlineCommand { get; set; }

        public ItalicCommand ItalicCommand { get; set; }

        public SaveCommand SaveCommand { get; set; }

        public TextChangedRTB TextChangedRTB { get; set; }

        public SelectionChangedRTB SelectionChangedRTB { get; set; }

        public SelectedNoteChanged SelectedNoteChanged { get; set; }

        // public event EventHandler SelectedNoteChanged;

        public NotesVM()
        {
            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            StartEditCommand = new StartEditCommand(this);
            EndEditCommand = new EndEditCommand(this);
            CloseAppCommand = new CloseAppCommand();
            BoldCommand = new BoldCommand();
            ItalicCommand = new ItalicCommand();
            UnderlineCommand = new UnderlineCommand();
            SaveCommand = new SaveCommand(this);
            TextChangedRTB = new TextChangedRTB();
            SelectionChangedRTB = new SelectionChangedRTB();
            SelectedNoteChanged = new SelectedNoteChanged(this);

            IsVisible = Visibility.Collapsed;

            GetNotebooks();
        }

        void GetNotebooks()
        {
            var notebooksData = DatabaseHelper.FetchNotebooksData();

            Notebooks.Clear();
            foreach (var notebook in notebooksData)
            {
                Notebooks.Add(notebook);
            }
        }

        void GetNotes()
        {
            if (selectedNotebook != null)
            {
                var notesData = DatabaseHelper.FetchNotesData(selectedNotebook.NotebookId);

                Notes.Clear();
                foreach (var note in notesData)
                {
                    Notes.Add(note);
                }
            }
        }

        public void CreateNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New notebook",
            };

            DatabaseHelper.Insert(newNotebook);
            GetNotebooks();
        }

        public void CreateNote(int notebookId)
        {
            Note newNote = new Note()
            {
                NotebookId = notebookId,
                Title = "New note",
                Created = DateTime.Now,
                Updated = DateTime.Now,
            };

            DatabaseHelper.Insert(newNote);
            GetNotes();
        }

        public void StartEditing()
        {
            IsVisible = Visibility.Visible;
        }

        public void EndEditing(Notebook notebook)
        {
            IsVisible = Visibility.Collapsed;
            DatabaseHelper.Update(notebook);
            GetNotebooks();
        }

        public void SaveRTB(NotesWindow notesWindow)
        {
            var content = new TextRange(notesWindow.contentRichTextBox.Document.ContentStart, notesWindow.contentRichTextBox.Document.ContentEnd);

            string filePath = string.Format(@"../../NoteFiles/{0}_{1}.rtf", SelectedNote.NoteId, SelectedNote.Title);
            var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            content.Save(fs, DataFormats.Rtf);

            SelectedNote.FileLocation = filePath;
            DatabaseHelper.Update(SelectedNote);
        }

        public void FetchNoteDataToContentRTB(RichTextBox richTextBox)
        {
            if (SelectedNote != null)
            {
                if (SelectedNote.FileLocation != "Unknow")
                {
                    var fs = new FileStream(SelectedNote.FileLocation, FileMode.Open, FileAccess.Read);
                    var content = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    content.Load(fs, DataFormats.Rtf);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
