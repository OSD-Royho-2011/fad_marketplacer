using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCB.Core.Models
{
    public class BaseViewModel
    {
        public BaseViewModel(BaseEntity entity)
        {
            RecordOrder = entity.RecordOrder;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
        }

        public int RecordOrder { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
