using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.ViewModel.School
{
    public class QuestionAnswer
    {
        public string Question { set; get; }
        public string Answer { set; get; }        
    }
    public class LoginViewModel
    {
        [Required]
        public string EmailID { set; get; }
        [Required]
        public string Password { get; set; }       
    }

    public class UserViewModel
    {
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string EmailID { set; get; }
        public string ProfilePic { get; set; }
        [Required]
        public int GenderType { get; set; }
        public string UserAddress { get; set; }
        public string MobileNumber { get; set; } 
        [Required] 
        public long RoleId { set; get; }

        [DataType(DataType.Date)]
        public DateTime DOB { set; get; } 
    } 
    
    public class RolePermissionViewModel
    {
        public long Id { set; get; }
        public string RoleName { set; get; }
        public string RoleDescription { set; get; }
        public List<Forms> Forms { set; get; }
    }

    public class Forms
    {
        public long FormId { set; get; }       
        public Permission Permission { set; get; }
    }

    public class UserRoleViewModel
    {
        public long Id { set; get; }
        public List<multiSelect> Users { set; get; }
        public int RoleId { set; get; }
    }

    public class UserPermissionViewModel
    {
        public long UserId { set; get; }
        public List<Forms> Forms { set; get; }                
    }  

    public class multiSelect
    {
        public long Id { set; get; }
        public bool ticked { set; get; }
    }

    public class SchMngtViewModel
    {

        public long SchId { set; get; }
        [Required]
        [MaxLength(50)]
        public string SchName { set; get; }
        
        [MaxLength(50)]
        public string SchEmail { set; get; }
        [Required]
        [MaxLength(150)]
        public string SchAddress { set; get; }
        [MaxLength(150)]
        public string SchAltAddress { set; get; }
        public long SchPinCode { set; get; }
        public long CityId { set; get; }
        public string CityName { set; get; }
        public long DistrictId { set; get; }
        public long StateId { set; get; }
        public long ZoneId { set; get; }
        public long CountryId { set; get; }
        public Nullable<long> SchPhoneNo { set; get; }
        public Nullable<long> SchFaxNo { set; get; }
        public string SchWebSite { set; get; }
        [MaxLength(50)]
        public string SchBoard { set; get; }
        [MaxLength(50)]
        public string SchAffiliationNo { set; get; }
        public City NewCity { set; get; }
        public BlackListedViewModel BlackListed { set; get; }      
        public bool IsOtherContact { set; get; }
        public ContactViewModel OtherContact { set; get; }
        public List<ContactViewModel> Contact_List { set; get; }
        //public List<EventCoOrdViewModel> Events { set; get; }

       
        public Nullable<long> SchCategoryId { set; get; }
        
        public Nullable<long> SchGroupId { set; get; }
    }

    public class ContactViewModel
    {
        public long ContactId { set; get; }
        public long DesgId { set; get; }
        public long TitleId { set; get; }
        [Required]
        public string ContactName { set; get; }
        
        public long ContactMobile { set; get; }
        public Nullable<long> ContactAltMobile1 { set; get; }
        public Nullable<long> ContactAltMobile2 { set; get; }

        
        [MaxLength(50)]
        public string ContactEmail { set; get; }

        [MaxLength(50)]
        public string ContactAltEmail1 { set; get; }

        [MaxLength(50)]
        public string ContactAltEmail2 { set; get; }
        public short ContactType { set; get; }                
        public bool AddressTo { set; get; }
    }
  
    public class BlackListedViewModel
    {
        public bool IsBlocked { set; get; }
        [MaxLength(200)]
        public string BlackListedRemarks { set; get; }     
    }

    public class EnrollmentOrderViewModel
    {                
        public long Id { set; get; }
        public long EventManagementId { set; get; }       
        public long TotlaEnrollment { set; get; }
        public List<EnrollmentOrderSummary> EnrollmentOrderDetail { set; get; }
        public Nullable<long> ExaminationDateId { set; get; }
       
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ChangeExamDate { set; get; }
    }


    public class EnrollmentOrderSummary
    {
        public long ClassId{ set; get; }
        public string ClassName { set; get; }
        public long No_Of_Student { set; get; }
    }
}