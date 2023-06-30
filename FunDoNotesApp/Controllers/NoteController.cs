using Autofac.Builder;
using CommonLayer;
using GreenPipes.Caching;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;

namespace FunDoNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteManager noteManager;
        private readonly ILogger<NoteController> logger;
        private readonly IDistributedCache distributedCache;

        public NoteController(INoteManager noteManager,
            ILogger<NoteController> logger , IDistributedCache distributedCache)
        {
            this.noteManager = noteManager;
            this.logger = logger;
            this.distributedCache = distributedCache;
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

        [HttpGet]
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
        [HttpGet]
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
        [HttpPut]
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
                logger.LogInformation("The process Started");

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

        [Authorize]
        [HttpPost]
        [Route("UploadImage")]
        public IActionResult UploadImage(string ImgPath,int NoteID)
        {
            try
            {
                int UserID = Convert.ToInt32(this.User.FindFirst("UserID").Value);

                

                string ImageData = this.noteManager.UploadImages(ImgPath, UserID, NoteID);


                if (ImageData != null)
                {
                    return this.Ok(new { Success = true, message = NoteID + " : Image Uploaded Successfully", result = ImageData });
                }


                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetAllNotesUsingRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNotesUsingRedis()
        {
            try
            {
                var CatcheKey = "NoteList";

                List<NoteModel> NoteList;

                byte[] RedisNoteList = await distributedCache.GetAsync(CatcheKey);
                if (RedisNoteList != null) 
                {
                    logger.LogDebug("Getting List from redis cache");
                    var SerializedNoteList = Encoding.UTF8.GetString(RedisNoteList);
                    NoteList = JsonConvert.DeserializeObject<List<NoteModel>>(SerializedNoteList);
                }
                else
                {
                    logger.LogDebug("Setting list to cache for the first time");
                    NoteList = (List<NoteModel>)this.noteManager.GetAllNotes();
                    var SerializedNoteList = JsonConvert.SerializeObject(NoteList);
                    var redisNoteList = Encoding.UTF8.GetBytes(SerializedNoteList);
                    var option = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(5));
                    await distributedCache.SetAsync(CatcheKey,redisNoteList,option);
                }
                logger.LogInformation("Fetched all Notes using redis Successfully");
                return Ok(NoteList);
            }

            catch (Exception ex)
            {
                logger.LogCritical(ex, "Exception Thrown...");
                return BadRequest(new {success = false , message = ex.Message});
            }
            
        }

    }
}

