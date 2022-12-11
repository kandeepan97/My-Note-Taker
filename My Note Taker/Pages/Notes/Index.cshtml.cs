using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace My_Note_Taker.Pages.Notes
{
    public class IndexModel : PageModel
    {
        public List<NoteInfo> listNotes = new List<NoteInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Mynotesmaker;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection (connectionString) )
                { 
                   connection.Open();
                    String sql = "SELECT * FROM notes";
                    using ( SqlCommand command =new SqlCommand(sql,connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader ())
                        {
                            while (reader.Read ())
                            { 
                              NoteInfo noteInfo = new NoteInfo ();
                                noteInfo.id = "" + reader.GetInt32(0);
                                noteInfo.title =  reader.GetString(1);
                                noteInfo.notes = reader.GetString(2);
                                noteInfo.created_at =  reader.GetDateTime(3).ToString();


                                listNotes.Add(noteInfo);
                            }
                        }
                    }
                }

            }
            catch(Exception ex)
            {
              Console.WriteLine ("Exception: " + ex.ToString());
            }
        }
    }
    public class NoteInfo
    {
        public string id;
        public string title;
        public string notes;
        public string created_at;
    }
}
