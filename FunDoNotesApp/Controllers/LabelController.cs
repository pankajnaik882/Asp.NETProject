using CommonLayer;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;

namespace FunDoNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelManager labelManager;
        private readonly ILogger<LabelController> logger;
        private readonly IDistributedCache distributedCache;

        public LabelController(ILabelManager labelManager,
            ILogger<LabelController> logger, IDistributedCache distributedCache)
        {
            this.labelManager = labelManager;
            this.logger = logger;
            this.distributedCache = distributedCache;

        }

        [Authorize]
        [HttpPost]
        [Route("AddLabel")]
        public IActionResult AddLabel(string LabelName, int NoteID)
        {
            try
            {
                int UserID = Convert.ToInt32(this.User.FindFirst("UserID").Value);

                string addlabelData = this.labelManager.AddLabel(LabelName, UserID, NoteID);

                if (addlabelData != null)
                {
                    return this.Ok(new { Success = true, message = "Label Added Successfully", result = addlabelData });
                }


                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetLabelByUserID")]
        public IActionResult GetLabelByUserID()
        {
            try
            {
                int UserID = Convert.ToInt32(this.User.FindFirst("UserID").Value);

                List<LabelModel> labelmodellist = (List<LabelModel>)this.labelManager.GetLabelByUserID(UserID);


                if (labelmodellist != null)
                {
                    return this.Ok(new { Success = true, message = "Lables For UserID " + UserID + " Fetched successfully", result = labelmodellist });
                }


                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetLabelByLabelName")]

        public IActionResult GetLabelByLabelName(string LabelName)
        {
            try
            {

                List<LabelModel> labelmodellist = (List<LabelModel>)this.labelManager.GetLabelByLabelName(LabelName);


                if (labelmodellist != null)
                {
                    return this.Ok(new { Success = true, message = "Lables With LabelName '" + LabelName + "' Fetched successfully", result = labelmodellist });
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
        [Route("UpdateLabel")]
        public IActionResult UpdateLabel(string FromLabelName ,string ChangeToLabelName, int NoteID)
        {
            try
            {
                int UserID = Convert.ToInt32(this.User.FindFirst("UserID").Value);

                string updatelabel = this.labelManager.UpdateLabel(FromLabelName,ChangeToLabelName, UserID, NoteID);


                if (updatelabel != null)
                {
                    return this.Ok(new { Success = true, message = "Updated Lable For UserID " + UserID + " Fetched successfully", result = updatelabel });
                }


                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }

        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteLabel")]
        public IActionResult DeleteLabel(string LabelName, int NoteID)
        {
            try
            {
                int UserID = Convert.ToInt32(this.User.FindFirst("UserID").Value);

                string deletelabel = this.labelManager.DeleteLabel(LabelName, UserID, NoteID);


                if (deletelabel != null)
                {
                    return this.Ok(new { Success = true, message = "Deleted Lable For UserID " + UserID + " successfully", result = deletelabel });
                }


                return this.BadRequest(new { success = true, message = "Process Failed" });
            }

            catch (Exception ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }

        }
    }
}
