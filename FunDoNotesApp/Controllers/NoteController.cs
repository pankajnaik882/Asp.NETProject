using Autofac.Builder;
using CommonLayer;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FunDoNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteManager noteManager;
        private readonly ILogger<NoteController> _logger;

        public NoteController(INoteManager noteManager,
            ILogger<NoteController> logger)
        {
            this.noteManager = noteManager;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Route("AddNote")]
        public IActionResult AddNote(NoteModel noteModel)
        {
            try
            {
                int UserID = Convert.ToInt32(this.User.FindFirst("UserID").Value);

                NoteModel addnoteData = this.noteManager.AddNote(noteModel, UserID);

                if (addnoteData != null)
                {
                    return this.Ok(new { Success = true, message = "Note Added Successfully", result = addnoteData });
                }


                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("GetAllNote")]
        public IActionResult GetAllNotes()
        {
            try
            {

                List<NoteModel> notemodellist = (List<NoteModel>)this.noteManager.GetAllNotes();

                if (notemodellist != null)
                {
                    return this.Ok(new { Success = true, message = "Fetched All Notes Successfully", result = notemodellist});
                }

                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetNoteByUserID")]
        public IActionResult GetNoteByUserID()
        {
            try
            {
                int UserID = Convert.ToInt32(this.User.FindFirst("UserID").Value);

                List<NoteModel> notemodellist2 = (List<NoteModel>)this.noteManager.GetNoteWithUserID(UserID);


                if (notemodellist2 != null)
                {
                    return this.Ok(new { Success = true, message = "Note For ID "+UserID+" Fetched successfully" , result = notemodellist2 });
                }


                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateNote")]
        public IActionResult UpdateNote(NoteModel noteModel )
        {
            try
            {
                int UserID = Convert.ToInt32(this.User.FindFirst("UserID").Value);

                int NoteID = noteModel.NoteID;

                NoteModel UpdatenoteData = this.noteManager.UpdateNote(noteModel, UserID , NoteID);

                if (UpdatenoteData != null)
                {
                    return this.Ok(new { Success = true, message = NoteID + " : Note Updated Successfully", result = UpdatenoteData });
                }


                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPatch]
        [Route("ArchieveNote")]
        public IActionResult ArchiveNote(NoteArchiveModel noteArchiveModel)
        {
            try
            {
                int UserID = Convert.ToInt32(this.User.FindFirst("UserID").Value);

                
                    bool ArchieveData = this.noteManager.ArchiveNote(noteArchiveModel, UserID);

                if (ArchieveData == true)
                {
                    return this.Ok(new { Success = true, message = noteArchiveModel.NoteID + " : Note archieved Successfully", result = ArchieveData });
                }


                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }


        [Authorize]
        [HttpPatch]
        [Route("PinNote")]
        public IActionResult PinNote(NoteArchiveModel noteArchiveModel)
        {
            try
            {
                int UserID = Convert.ToInt32(this.User.FindFirst("UserID").Value);


                bool PinNoteData = this.noteManager.PinNote(noteArchiveModel, UserID);

                if (PinNoteData == true)
                {
                    return this.Ok(new { Success = true, message = noteArchiveModel.NoteID + " : Note Pinned Successfully", result = PinNoteData });
                }


                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPatch]
        [Route("TrashNote")]
        public IActionResult TrashNote(NoteArchiveModel noteArchiveModel)
        {
            try
            {
                int UserID = Convert.ToInt32(this.User.FindFirst("UserID").Value);


                bool TrashData = this.noteManager.TrashNote(noteArchiveModel, UserID);

                if (TrashData == true)
                {
                    return this.Ok(new { Success = true, message = noteArchiveModel.NoteID + " : Note Trashed Successfully", result = TrashData });
                }


                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPatch]
        [Route("ChangeNoteColor")]
        public IActionResult ChangeColorNote(string color , int NoteID)
        {
            try
            {
                int UserID = Convert.ToInt32(this.User.FindFirst("UserID").Value);


                //throw new Exception("Error Occured");
                _logger.LogInformation("The process Started");

                NoteModel ColorData = this.noteManager.ChangeColorNote(color, UserID ,NoteID);
                

                if (ColorData != null)
                {
                    return this.Ok(new { Success = true, message = NoteID + " : Note Color changed Successfully", result = ColorData });
                }


                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                //_logger.LogError(ex.ToString());
                return BadRequest(ex.ToString()); /*this.NotFound(new { success = false, message = ex.Message });*/
            }
        }

    }
}
