using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalApi.Entities
{
    public class Camp
    {
        public int Id { get; set; }
        public string Moniker { get; set; }
        public string Name { get; set; }
        public DateTime EventDate { get; set; } = DateTime.MinValue;
        public int Length { get; set; }
        public string Description { get; set; }

        //Navigation
        [ForeignKey("LocationId")]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        public ICollection<Speaker> Speakers { get; set; }

        public byte [ ] RowVersion { get; set; }
    }
}
