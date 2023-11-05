using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using TourDePologne.Enums;

namespace TourDePologne
{
    public class Cyclist : EqualityComparer<Cyclist>, ISerializable, ICloneable
    {
        public Guid Id { get; }
        public int Number { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Sex Sex { get; private set; }
        public Nationality Nationality { get; private set; }
        public Experience Experience { get; private set; }

        protected Cyclist(SerializationInfo info, StreamingContext context)
        {
            Id = Guid.Parse(info.GetString(nameof(Id)) ?? throw new SerializationException($"{nameof(Id)} cannot be null"));
            Number = info.GetInt32(nameof(Number));
            FirstName = info.GetString(nameof(FirstName)) ?? throw new SerializationException($"{nameof(FirstName)} cannot be null");
            LastName = info.GetString(nameof(LastName)) ?? throw new SerializationException($"{nameof(LastName)} cannot be null");
            DateOfBirth = DateTime.Parse(info.GetString(nameof(DateOfBirth)) ?? throw new SerializationException($"{nameof(DateOfBirth)} cannot be null"));
            Sex = Enum.Parse<Sex>(info.GetString(nameof(Sex)) ?? throw new SerializationException($"{nameof(Sex)} cannot be null"));
            Nationality = Enum.Parse<Nationality>(info.GetString(nameof(Nationality)) ?? throw new SerializationException($"{nameof(Nationality)} cannot be null"));
            Experience = Enum.Parse<Experience>(info.GetString(nameof(Experience)) ?? throw new SerializationException($"{nameof(Experience)} cannot be null"));
        }

        public Cyclist(int number, string firstName, string lastName, DateTime dateOfBirth, Sex sex, Nationality nationality, Experience experience)
        {
            Id = Guid.NewGuid();

            Number = number;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Sex = sex;
            Nationality = nationality;
            Experience = experience;
        }

        public void Update(string? firstName = null, string? lastName = null, DateTime? dateOfBirth = null, Sex? sex = null, Nationality? nationality = null, Experience? experience = null)
        {
            FirstName = firstName ?? FirstName;
            LastName = lastName ?? LastName;
            DateOfBirth = dateOfBirth ?? DateOfBirth;
            Sex = sex ?? Sex;
            Nationality = nationality ?? Nationality;
            Experience = experience ?? Experience;
        }

        public override string ToString()
        {
            return $"Number: {Number}\nName: {FirstName} {LastName}\nBirth Date: {DateOfBirth.ToLongDateString()}\nSex: {Sex}\nNationality: {Nationality}\nExperience: {Experience}\n";
        }

        public override bool Equals(Cyclist? x, Cyclist? y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id.Equals(y.Id);
        }

        public override int GetHashCode([DisallowNull] Cyclist obj)
        {
            return obj.Id.GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Id), Id.ToString());
            info.AddValue(nameof(Number), Number);
            info.AddValue(nameof(FirstName), FirstName);
            info.AddValue(nameof(LastName), LastName);
            info.AddValue(nameof(DateOfBirth), DateOfBirth.ToString());
            info.AddValue(nameof(Sex), Enum.GetName(Sex));
            info.AddValue(nameof(Nationality), Enum.GetName(Nationality));
            info.AddValue(nameof(Experience), Enum.GetName(Experience));
        }

        public object Clone()
        {
            return (Cyclist)MemberwiseClone();
        }
    }
}