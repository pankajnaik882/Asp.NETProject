using CommonLayer;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Client;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace RepositoryLayer.Services
{
    public class NoteRepository : INoteRepository
    {
        private readonly IConfiguration config;
        public readonly string connectionString;

        public NoteRepository(IConfiguration config)
        {
            this.connectionString = config.GetConnectionString("FundooDB");
            this.config = config;
        }

        public NoteModel AddNote(NoteModel Note , int UserID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this.connectionString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_Add_Notes", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Title", Note.Title);
                    cmd.Parameters.AddWithValue("@Description", Note.Description);
                    cmd.Parameters.AddWithValue("@Color", Note.Color);
                    cmd.Parameters.AddWithValue("@Reminder", Note.Reminder);
                    cmd.Parameters.AddWithValue("@IsArchive", Note.IsArchive);
                    cmd.Parameters.AddWithValue("@IsPinned", Note.IsPinned);
                    cmd.Parameters.AddWithValue("@IsTrash", Note.IsTrash);
                    cmd.Parameters.AddWithValue("@CreatedAt", Note.CreatedAt);
                    cmd.Parameters.AddWithValue("@ModifiedAt", Note.ModifiedAt);
                    cmd.Parameters.AddWithValue("@UserID", UserID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return Note;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //---------------------------Get All Notes-----------------------------------

        public IEnumerable<NoteModel> GetAllNotes()
        {
            //NoteModel noteModel = new NoteModel();
            List<NoteModel> notemodellist = new List<NoteModel>();

            using (SqlConnection con = new SqlConnection(this.connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_GetAll_Notes", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NoteModel noteModel = new NoteModel()
                        {
                                NoteID = Convert.ToInt32(reader["UserID"]),
                                Title = reader["Title"].ToString()
                        };

                        notemodellist.Add(noteModel);

                    }
                    
                }
                con.Close();
            }
            return notemodellist;
        }

        //-------------------------Get Notes By UserID------------------

        public IEnumerable<NoteModel> GetNoteWithUserID(int UserID)
        {
            List<NoteModel> notemodellist2 = new List<NoteModel>();
            

            using (SqlConnection con = new SqlConnection(this.connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_GetByUserID_Notes", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", UserID);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NoteModel noteModel = new NoteModel();

                        noteModel.NoteID =  reader.GetInt32(0);
                        noteModel.Title = reader.GetString(1);
                        noteModel.Description = reader.GetString(2);
                        noteModel.Color = reader.GetString(3);
                        noteModel.Reminder = reader.GetDateTime(4);
                        noteModel.IsArchive = reader.GetBoolean(5);
                        noteModel.IsPinned = reader.GetBoolean(6);
                        noteModel.IsTrash = reader.GetBoolean(7);
                        noteModel.CreatedAt= reader.GetDateTime(8);
                        noteModel.ModifiedAt= reader.GetDateTime(9);

                        notemodellist2.Add(noteModel);
                    }
                }
                con.Close();
            }
            return notemodellist2;
        }
        
        //--------------------------------Note Update API--------------------------

        public NoteModel UpdateNote(NoteModel Note , int UserID ,int NoteID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this.connectionString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_Update_Notes", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NoteID", NoteID);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@Title", Note.Title);
                    cmd.Parameters.AddWithValue("@Description", Note.Description);
                    cmd.Parameters.AddWithValue("@Color", Note.Color);
                    cmd.Parameters.AddWithValue("@Reminder", Note.Reminder);
                    cmd.Parameters.AddWithValue("@ModifiedAt", Note.ModifiedAt);
                    

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Note.NoteID = reader.GetInt32(0);
                            Note.Title = reader.GetString(1);
                            Note.Description = reader.GetString(2);
                            Note.Color = reader.GetString(3);
                            Note.Reminder = reader.GetDateTime(4);
                            Note.IsArchive = reader.GetBoolean(5);
                            Note.IsPinned = reader.GetBoolean(6);
                            Note.IsTrash = reader.GetBoolean(7);
                            Note.CreatedAt = reader.GetDateTime(8);
                            Note.ModifiedAt = reader.GetDateTime(9);

                        }
                    }
                    con.Close();
                }
                return Note;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ArchiveNote(NoteArchiveModel noteArchiveModel, int UserID )
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this.connectionString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_IsArchived_Notes", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NoteID", noteArchiveModel.NoteID);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("IsArchived",noteArchiveModel.IsArchive);

                    con.Open();
                    int Archive = cmd.ExecuteNonQuery(); 

                    if (Archive >= 1)
                    {
                        return true;
                    }
                    con.Close();
                }
                return false;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool TrashNote(NoteArchiveModel noteArchiveModel, int UserID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this.connectionString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_IsTrash_Notes", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NoteID", noteArchiveModel.NoteID);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@IsTrash", noteArchiveModel.IsTrash);

                    con.Open();
                    int Trash = cmd.ExecuteNonQuery();

                    if (Trash >= 1)
                    {
                        return true;
                    }
                    con.Close();
                }
                return false;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool PinNote(NoteArchiveModel noteArchiveModel, int UserID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this.connectionString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_IsPinned_Notes", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NoteID", noteArchiveModel.NoteID);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@IsPinned", noteArchiveModel.IsPin);

                    con.Open();
                    int Pinned = cmd.ExecuteNonQuery();

                    if (Pinned >= 1)
                    {
                        return true;
                    }
                    con.Close();
                }
                return false;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public NoteModel ChangeColorNote(string Color, int UserID, int NoteID)
        {
            try
            {
                NoteModel Note = new NoteModel();

                using (SqlConnection con = new SqlConnection(this.connectionString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_ColorChange_Notes", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NoteID", NoteID);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@Color", Color);
                    


                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            

                            Note.NoteID = reader.GetInt32(0);
                            Note.Title = reader.GetString(1);
                            Note.Description = reader.GetString(2);
                            Note.Color = reader.GetString(3);
                            Note.Reminder = reader.GetDateTime(4);
                            Note.IsArchive = reader.GetBoolean(5);
                            Note.IsPinned = reader.GetBoolean(6);
                            Note.IsTrash = reader.GetBoolean(7);
                            Note.CreatedAt = reader.GetDateTime(8);
                            Note.ModifiedAt = reader.GetDateTime(9);

                        }
                    }
                    con.Close();
                }
                return Note;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string UploadImages(string filePath, int NoteID , int UserID )
        {
            try
            {
                NoteModel note = new NoteModel();

                    Account account = new Account("dwqxpcokc", "392936136134788", "IsvbO3CYFqPj_QVypjmpvYHFOtU");
                    Cloudinary cloudinary = new Cloudinary(account);
                    ImageUploadParams imageUploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(filePath),
                        PublicId = note.Title
                    };

                    ImageUploadResult imageUploadResult = cloudinary.Upload(imageUploadParams);

                     note.ModifiedAt = DateTime.Now;
                    note.ImageUrl = imageUploadResult.Url.ToString();

                    using (SqlConnection con = new SqlConnection(this.connectionString))
                    {
                        SqlCommand cmd = new SqlCommand("dbo.usp_UploadImage_Notes", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(UserID));
                        cmd.Parameters.AddWithValue("@NoteID",Convert.ToInt32(NoteID));
                        //cmd.Parameters.AddWithValue("@ModifiedAt", note.ModifiedAt);
                        cmd.Parameters.AddWithValue("@ImageUrl", Convert.ToString(note.ImageUrl));

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    return "Upload Successfull";

            }

            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
        }

        //-----------------------LabelMethods---------------------------

        

       



    }
}
