using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Dog
    {
        [Key]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Color { get; set; }
        [Required]
        [Column("tail_length")]
        public int TailLength { get; set; }
        [Required]
        public int Weight { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            Dog other = (Dog)obj;
            return Name == other.Name
                && Color == other.Color
                && TailLength == other.TailLength
                && Weight == other.Weight;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Name != null ? Name.GetHashCode() : 0);
                hash = hash * 23 + (Color != null ? Color.GetHashCode() : 0);
                hash = hash * 23 + TailLength;
                hash = hash * 23 + Weight;
                return hash;
            }
        }
    }
}
