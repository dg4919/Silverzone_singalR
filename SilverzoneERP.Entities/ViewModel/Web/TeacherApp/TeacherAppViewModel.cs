using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.ViewModel.TeacherApp
{
    public class EventViewModel
    {
        [Required, MaxLength(20)]
        public string EventCode { get; set; }

        [Required, MaxLength(20)]
        public string EventName { get; set; }
    }

    public class UserDetailViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string SchoolName { get; set; }

        [MaxLength(200)]
        public string SchoolAddress { get; set; }

        [Required]
        public int PinCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public int ProfileId { get; set; }

        [Required]
        public genderType gender { get; set; }

        [Required, Column(TypeName = "Date")]
        public DateTime Age { get; set; }

        [Required]
        public IEnumerable<int> Events { get; set; }
        
    }
    

}