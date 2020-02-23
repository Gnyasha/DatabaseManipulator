namespace DatabaseManipulator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblReceivedMessage
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public DateTime? ReadDate { get; set; }

        public string Status { get; set; }

        public bool? Success { get; set; }
    }
}
