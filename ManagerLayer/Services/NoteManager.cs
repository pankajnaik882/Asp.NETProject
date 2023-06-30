using CommonLayer;
using ManagerLayer.Interface;
using RepositoryLayer.Interface;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class NoteManager : INoteManager
    {
        private readonly INoteRepository noterepository;

        public NoteManager(INoteRepository noterepository)
        {
            this.noterepository = noterepository;
        }
        public NoteModel AddNote(NoteModel Note, int UserID)
        {
            return noterepository.AddNote(Note, UserID);
        }

        public IEnumerable<NoteModel> GetAllNotes()
        {
            return noterepository.GetAllNotes();
        }

        public IEnumerable<NoteModel> GetNoteWithUserID(int UserID)
        {
            return noterepository.GetNoteWithUserID(UserID);
        }

        public NoteModel UpdateNote(NoteModel Note, int UserID, int NoteID)
        {
            return noterepository.UpdateNote(Note, UserID, NoteID);
        }

        public bool ArchiveNote(NoteArchiveModel noteArchiveModel, int UserID)
        {
            return noterepository.ArchiveNote(noteArchiveModel, UserID);
        }

        public bool TrashNote(NoteArchiveModel noteArchiveModel, int UserID)
        {
            return noterepository.TrashNote(noteArchiveModel, UserID);
        }

        public bool PinNote(NoteArchiveModel noteArchiveModel, int UserID)
        {
            return noterepository.PinNote(noteArchiveModel, UserID);
        }

        public NoteModel ChangeColorNote(string Color, int UserID, int NoteID)
        {
            return noterepository.ChangeColorNote(Color, UserID, NoteID);
        }

        public string UploadImages(string filePath, int NoteID, int UserID)
        {
            return noterepository.UploadImages(filePath, NoteID, UserID);
        }

    }
}
