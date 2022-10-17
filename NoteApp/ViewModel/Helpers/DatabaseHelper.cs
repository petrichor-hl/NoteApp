using NoteApp.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApp.ViewModel.Helpers
{
    public class DatabaseHelper
    {
        static readonly string strCon = @"Data Source=LAPTOP-JF6O07NR\SQLEXPRESS;Initial Catalog=notesDB;Integrated Security=True";
        public static SqlConnection sqlCon = new SqlConnection(strCon);

        static readonly string insertNote = "INSERT Note VALUES ({0}, '{1}', '{2}', '{3}', 'Unknow')";
        static readonly string insertNotebook = "INSERT Notebook VALUES ({0}, '{1}')";

        static readonly string queryNotebook = "SELECT * FROM Notebook";
        static readonly string queryNote = "SELECT * FROM Note WHERE Note.NotebookId = {0}";

        static readonly string updateNotebook = "update Notebook set NotebookName = '{0}' where NotebookId = {1}";
        static readonly string updateNote = "update Note set FileLocation = '{0}' where NoteId = {1}";


        public static List<Notebook> FetchNotebooksData()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(queryNotebook, sqlCon);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Notebook> notebooksData = new List<Notebook>();

            while (reader.Read())
            {
                notebooksData.Add(new Notebook()
                {
                    NotebookId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                });
            }

            reader.Close();
            sqlCon.Close();

            return notebooksData;
        }

        public static List<Note> FetchNotesData(int notebookId)
        {
            sqlCon.Open();
            var strCmd = string.Format(queryNote, notebookId);
            var cmd = new SqlCommand(strCmd, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();

            List<Note> notesData = new List<Note>();

            while (reader.Read())
            {
                notesData.Add(new Note()
                {
                    NoteId = reader.GetInt32(0),
                    NotebookId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Created = reader.GetDateTime(3),
                    Updated = reader.GetDateTime(4),
                    FileLocation = reader.GetString(5),
                });
            }

            reader.Close();
            sqlCon.Close();

            return notesData;
        }

        public static void Insert(object T)
        {
            sqlCon.Open();
            if (T is Note note)
            {
                var strCmd = string.Format(insertNote, note.NotebookId, note.Title, note.Created, note.Updated);
                var cmd = new SqlCommand(strCmd, sqlCon);
                cmd.ExecuteNonQuery();
            }
            if (T is Notebook notebook)
            {
                var strCmd = string.Format(insertNotebook, notebook.UserId, notebook.Name);
                var cmd = new SqlCommand(strCmd, sqlCon);
                cmd.ExecuteNonQuery();
            }
            sqlCon.Close();
        }

        public static void Update(object T)
        {
            sqlCon.Open();

            if (T is Note note)
            {
                var strCmd = string.Format(updateNote, note.FileLocation, note.NoteId);
                var cmd = new SqlCommand(strCmd, sqlCon);
                cmd.ExecuteNonQuery();
            }
            if (T is Notebook notebook)
            {
                var strCmd = string.Format(updateNotebook, notebook.Name, notebook.NotebookId);
                var cmd = new SqlCommand(strCmd, sqlCon);
                cmd.ExecuteNonQuery();
            }

            sqlCon.Close();
        }

    }
}
