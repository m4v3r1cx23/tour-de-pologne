using System.Collections.ObjectModel;

namespace TourDePologne
{
    public class Race
    {
        private List<Cyclist> _cyclists;

        public ReadOnlyCollection<Cyclist> Cyclists => _cyclists.AsReadOnly();

        public Race(List<Cyclist> cyclists)
        {
            _cyclists = cyclists;
        }

        public Cyclist? GetCyclist(int number)
        {
            return (Cyclist?)(_cyclists.FirstOrDefault(c => c.Number == number)?.Clone());
        }

        public void AddCyclist(Cyclist cyclist)
        {
            if (_cyclists.Any(c => c.Number == cyclist.Number))
            {
                throw new Exception("Cyclist with this number already exists");
            }

            _cyclists.Add(cyclist);
        }

        public void UpdateCyclist(Cyclist cyclist)
        {
            var index = _cyclists.FindIndex(c => c.Number == cyclist.Number);

            if (index < 0)
            {
                throw new Exception("Cyclist with this number does not exist");
            }

            _cyclists[index] = cyclist;
        }

        public void RemoveCyclist(int number)
        {
            int index = _cyclists.FindIndex(c => c.Number == number);

            if (index >= 0)
            {
                _cyclists.RemoveAt(index);
            }
        }

        public void SwapCyclists(int position1, int position2)
        {
            if (Math.Abs(position1 - position2) != 1)
            {
                throw new Exception("Positions must be adjacent");
            }

            var index1 = _cyclists.FindIndex(c => c.Number == position1);
            var index2 = _cyclists.FindIndex(c => c.Number == position2);

            if (index1 < 0 || index2 < 0)
            {
                return;
            }

            var cyclist1 = _cyclists[index1];
            var cyclist2 = _cyclists[index2];

            _cyclists[index1] = cyclist2;
            _cyclists[index2] = cyclist1;
        }

        public override string ToString()
        {
            return string.Join("\n", _cyclists.Select(c => c.ToString()));
        }
    }
}