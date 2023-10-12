using System.Text.Json.Serialization;

namespace Business.Models
{
    public class DogModel
    {
        public string Name { get; set; }
        public string Color { get; set; }
        [JsonPropertyName("tail_length")]
        public int TailLength { get; set; }
        public int Weight { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            DogModel other = (DogModel)obj;
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
