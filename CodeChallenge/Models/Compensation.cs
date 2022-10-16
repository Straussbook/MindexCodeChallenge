using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        [Key]
        public String Employee { get; set; }
        /*
         * Float was chosen for memory efficiency, as it is only read/write
         * If later the app were to support salary calculations, (raises, bonuses, etc.)
         * this should be changed to decimal for higher precision.
         */
        public float Salary { get; set; }
        public DateTime EffectiveDate {get; set;}
    }
}