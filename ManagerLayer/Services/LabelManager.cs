using CommonLayer;
using ManagerLayer.Interface;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;

namespace ManagerLayer.Services
{
    public class LabelManager : ILabelManager
    {
        private readonly ILabelRepository labelrepository;

        public LabelManager(ILabelRepository labelrepository)
        {
            this.labelrepository = labelrepository;
        }
        public string AddLabel(string LabelName, int UserID, int NoteID)
        {
            return labelrepository.AddLabel(LabelName, UserID, NoteID);
        }

        public IEnumerable<LabelModel> GetLabelByUserID(int UserID)
        {
            return labelrepository.GetLabelByUserID(UserID);
        }

        public IEnumerable<LabelModel> GetLabelByLabelName(string LabelName)
        {
            return labelrepository.GetLabelByLabelName(LabelName);
        }

        public string UpdateLabel(string fromLabelName, string changeToLabelName, int UserID, int NoteID)
        {
            return labelrepository.UpdateLabel(fromLabelName,changeToLabelName,UserID,NoteID);
        }

        public string DeleteLabel(string LabelName, int UserID, int NoteID)
        {
            return labelrepository.DeleteLabel(LabelName,UserID,NoteID);   
        }
    }
}
