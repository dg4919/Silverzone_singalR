using System;

namespace SilverzoneERP.Entities.ViewModel.TeacherApp
{
    public class QueryViewModel
    {
        public int id { get; set; }
        public string Subject { get; set; }
        public string QueryDetail { get; set; }
        public int OldRef { get; set; }
        public DateTime QueryDate { get; set; }
              
        public string QueryStatus { get; set; }
        public DateTime CloseDate { get; set; }
        public int UserId { get; set; }

        
    }
}