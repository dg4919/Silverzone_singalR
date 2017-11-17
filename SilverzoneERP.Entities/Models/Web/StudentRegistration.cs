using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class StudentRegistration:Entity<long>
    {
        [MaxLength(40)]
        public string EventCode { get; set; }
        [MaxLength(40)]
        public string SchCode { get; set; }
        public int RegSrlNo { get; set; }
        public int ExamDateOpted { get; set; }
        public int Class1E { get; set; }
        public int Class2E { get; set; }
        public int Class3E { get; set; }
        public int Class4E { get; set; }
        public int Class5E { get; set; }
        public int Class6E { get; set; }
        public int Class7E { get; set; }
        public int Class8E { get; set; }
        public int Class9E { get; set; }
        public int Class10E { get; set; }
        public int Class11EN { get; set; }
        public int Class12EN { get; set; }
        public  int TotalE { get; set; }
        public int Book1 { get; set; }
        public int Book2 { get; set; }
        public int Book3 { get; set; }
        public int Book4 { get; set; }
        public int Book5 { get; set; }
        public int Book6 { get; set; }
        public int Book7 { get; set; }
        public int Book8 { get; set; }
        public int Book9 { get; set; }
        public int Book10 { get; set; }
        public int Book11 { get; set; }
        public int Book12 { get; set; }
        public  int BookTot { get; set; }
        public int PreYearQP1 { get; set; }
        public int PreYearQP2 { get; set; }
        public int PreYearQP3 { get; set; }
        public int PreYearQP4 { get; set; }
        public int PreYearQP5 { get; set; }
        public int PreYearQP6 { get; set; }
        public int PreYearQP7 { get; set; }
        public int PreYearQP8 { get; set; }
        public int PreYearQP9 { get; set; }
        public int PreYearQP10 { get; set; }
        public int PreYearQP11 { get; set; }
        public int PreYearQP12 { get; set; }
        public int PreYearQPTot { get; set; }
        public int GenXBook1 { get; set; }
        public int GenXBook2 { get; set; }
        public int GenXBook3 { get; set; }
        public int GenXBook4 { get; set; }
        public int GenXBook5 { get; set; }
        public int GenXBook6 { get; set; }
        public int GenXBook7 { get; set; }
        public int GenXBook8 { get; set; }
        public int GenXBook9 { get; set; }
        public int GenXBook10 { get; set; }
        public int GenXBook11 { get; set; }
        public int GenXBook12 { get; set; }
        public int GenXBookTot { get; set; }
    }
}
