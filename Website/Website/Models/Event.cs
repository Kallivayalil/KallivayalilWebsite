﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Website.Models.ReferenceData;

namespace Website.Models
{
    [KnownType(typeof (Event))]
    public class Event : Entity
    {
        [UIHint("EventTypes"),DisplayName("Event Type")]
        public virtual EventType Type { get; set; }

        [DisplayName("Title")]
        public virtual string EventTitle { get; set; }

        [DisplayName("Description")]
        public virtual string EventDescription { get; set; }

        public virtual bool IsApproved { get; set; }

        [DisplayName("Contact Person")]
        public virtual string ContactPerson { get; set; }

        [DisplayName("Contact Number")]
        public virtual string ContactNumber { get; set; }

        [UIHint("StartDate"), DisplayName("Start Displaying From")]
        public virtual DateTime StartDate { get; set; }

        [UIHint("EndDate"), DisplayName("Date of Event")]
        public virtual DateTime EndDate { get; set; }

        [DisplayName("Constituent")]
        public virtual Constituent Constituent { get; set; }
    }

    public class Events : List<Event> {}
}

