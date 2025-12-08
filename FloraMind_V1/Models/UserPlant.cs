using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FloraMind_V1.Models
{
    public class UserPlant
    {
        [Key]
        public int UserPlantID { get; set; }

        public DateTime DateAdopted { get; set; }

        public DateTime LastWatered { get; set; }

        //Foreign Keys


        // Hangi Kullanıcıya ait ?
        public int UserID { get; set; }

        public User User { get; set; }

        // Katalogdaki hangi türden geldi ?

        public int PlantID { get; set; }

        [ValidateNever]
        public Plant Plant { get; set; }




    }
}
