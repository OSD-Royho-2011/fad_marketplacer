using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NCB.Core.DataAccess.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            var now = DateTime.UtcNow;

            Id = new Guid();
            CreatedAt = now;
            UpdatedAt = now;
            RecordDeleted = false;
            RecordActive = true;
        }

        [Key]
        public Guid Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordOrder { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool RecordActive { get; set; }

        public bool RecordDeleted { get; set; }
    }
}
