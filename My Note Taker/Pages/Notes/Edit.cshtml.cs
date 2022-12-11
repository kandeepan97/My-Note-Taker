using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace My_Note_Taker.Pages.Notes
{
    public class EditModel : PageModel
    {
        public NoteInfo noteInfo = new NoteInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Mynotesmaker;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM notes WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                 
                                noteInfo.id = "" + reader.GetInt32(0);
                                noteInfo.title = reader.GetString(1);
                                noteInfo.notes = reader.GetString(2);


                            }
                        }
                    }
                }

            }
            catch(Exception ex)
            {
               errorMessage = ex.Message;
            }
        }
        public void OnPost()
        {
            noteInfo.id = Request.Form["id"]; 
            noteInfo.title = Request.Form["title"];
            noteInfo.notes = Request.Form["notes"];

            if (noteInfo.title.Length == 0 || noteInfo.notes.Length == 0)
            {
                errorMessage = "Reqiured all fields";
                return;

            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Mynotesmaker;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE notes SET title=@title, notes=@notes WHERE id=@id";
                    using (SqlCommand command = new(sql, connection))
                    {
                        command.Parameters.AddWithValue("title", noteInfo.title);
                        command.Parameters.AddWithValue("notes", noteInfo.notes);
                        command.Parameters.AddWithValue("id", noteInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Notes/Index");
        }
    }
}
