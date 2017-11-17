using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.ViewModel.School
{
    public class StudentAttendanceViewModel
    {
        public long Id { set; get; }

        [Required]
        public long StudentEntryId { set; get; }

        [Required]
        public string AnswerJSON { set; get; }

        [Required]
        public int Type { set; get; }
    }   
}
