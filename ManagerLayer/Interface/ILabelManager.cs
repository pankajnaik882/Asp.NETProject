using CommonLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interface
{
    public interface ILabelManager
    {

        public string AddLabel(String LabelName, int UserID, int NoteID);

        public IEnumerable<LabelModel> GetLabelByUserID(int UserID);

        public IEnumerable<LabelModel> GetLabelByLabelName(string LabelName);

        public string UpdateLabel(string fromLabelName, string changeToLabelName, int UserID, int NoteID);

        public string DeleteLabel(string LabelName, int UserID, int NoteID);
    }
}
