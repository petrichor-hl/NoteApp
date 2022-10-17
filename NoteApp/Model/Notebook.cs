using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApp.Model
{
    public class Notebook
    {
        public int NotebookId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
    }
}
