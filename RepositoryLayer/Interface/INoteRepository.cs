using CommonLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface INoteRepository
    {
        public NoteModel AddNote(NoteModel Note , int UserID);
        public IEnumerable<NoteModel> GetAllNotes();

        public IEnumerable<NoteModel> GetNoteWithUserID(int UserID);

        public NoteModel UpdateNote(NoteModel Note, int UserID, int NoteID);

        public bool ArchiveNote(NoteArchiveModel noteArchiveModel, int UserID);

        public bool TrashNote(NoteArchiveModel noteArchiveModel, int UserID);

        public bool PinNote(NoteArchiveModel noteArchiveModel, int UserID);

        public  NoteModel ChangeColorNote(string Color, int UserID, int NoteID);

        public string UploadImages(string filePath, int NoteID, int UserID);

        
    }
}
