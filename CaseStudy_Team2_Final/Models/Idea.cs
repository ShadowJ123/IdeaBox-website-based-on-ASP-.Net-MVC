using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy_Team2_Final.Models
{
    public class Idea
    {
        public int IdeaID { get; set; }
        //public string UserId { get; set; }
        public string ContributedBy { get; set; }
        private DateTime _returnDate = DateTime.MinValue;
        public DateTime Date
        {
            get
            {
                return (_returnDate == DateTime.MinValue) ? DateTime.Now : _returnDate;
            }
            set { _returnDate = value; }
        }
       // public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public decimal Rating { get; set; }
        public string Status { get; set; }
        //public ContactStatus Status { get; set; }
    }
    public enum ContactStatus
    {
        Submitted,
        Approved,
        Rejected
    }

}
