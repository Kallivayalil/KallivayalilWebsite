using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Website.Models.ReferenceData;

namespace Website.Models
{

    public class Constituents : List<Constituent>{}

    public class Constituent : Entity
    {
        public virtual int Id { get; set; }

        public virtual string Gender { get; set; }
        [UIHint("BranchTypes"), DisplayName("Branch Type")]
        public virtual BranchType BranchName { get; set; }
        public virtual string HouseName { get; set; }
        public virtual DateTime BornOn { get; set; }
        public virtual DateTime? DiedOn { get; set; }
        public virtual bool HasExpired { get; set; }
        public virtual int MaritialStatus { get; set; }
        public virtual string MaritialStatusString { get; set; }
        public virtual char IsRegistered { get; set; }
        public virtual ConstituentName Name { get; set; }

        [UIHint("ProfileImage")]
        public virtual string ProfileImage
        {
            get { return ""; }
            set { ProfileImage = value; } 
        }


    }
}