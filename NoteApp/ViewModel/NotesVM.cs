using NoteApp.Model;
using NoteApp.ViewModel.Command;
using NoteApp.ViewModel.Command.EditCommand;
using NoteApp.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                SelectedNoteChanged?.Invoke(this, new EventArgs());
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

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler SelectedNoteChanged;

        public NotesVM()
        {
            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            StartEditCommand = new StartEditCommand(this);
            EndEditCommand = new EndEditCommand(this);

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

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
