using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models
{
    public class Truck
    {

        public Truck()
        {
            this.ClientsTrucks = new List<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [RegularExpression(@"^([A-Z]{2})(\d{4})([A-Z]{2})$")]
        public string RegistrationNumber { get; set; }

        [Required]
        [MaxLength(17)]
        public string VinNumber { get; set; }

        [Range(950, 1420, ErrorMessage = "Tank capacity must be > 950 && < 1420")]
        public int TankCapacity { get; set; }

        [Range(5000, 29000, ErrorMessage = "Cargo capacity must be > 5000 && < 29000")]
        public int CargoCapacity { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        [Required]
        public MakeType MakeType { get; set; }

        [Required]
        [ForeignKey(nameof(Despatcher))]
        public int DespatcherId { get; set; }

        [Required]
        public Despatcher Despatcher { get; set; }

        public ICollection<ClientTruck> ClientsTrucks { get; set; }


    }
}
