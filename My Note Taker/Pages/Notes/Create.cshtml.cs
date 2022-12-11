using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace My_Note_Taker.Pages.Notes
{
    public class CreateModel : PageModel
    {
        public NoteInfo noteInfo = new NoteInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            noteInfo.title = Request.Form["title"];
            noteInfo.notes = Request.Form["notes"];

            if (noteInfo.title.Length == 0 || noteInfo.notes.Length == 0)
            {
                errorMessage = "Reqiured all fields";
                return;

            }
            // save the Notes to database

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Mynotesmaker;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO notes " +
                        "(title, notes) VALUES" +
                        "(@title, @notes);";
                    using (SqlCommand command = new(sql, connection))
                    {
                        command.Parameters.AddWithValue("title", noteInfo.title);
                        command.Parameters.AddWithValue("notes", noteInfo.notes);

                        command.ExecuteNonQuery();                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            noteInfo.notes = ""; noteInfo.title = "";
            successMessage = "Note Saved successfully";

            Response.Redirect("/Notes/Index");
        }
    }
}
