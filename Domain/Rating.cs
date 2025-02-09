namespace Domain
{
    public class Rating
    {
        public double Rate { get; }
        public int Count { get; }

        public Rating(double rate, int count)
        {
            if (rate < 0 || rate > 5) throw new ArgumentException("Rate must be between 0 and 5.");
            if (count < 0) throw new ArgumentException("Count cannot be negative.");

            Rate = rate;
            Count = count;
        }

        public Rating UpdateRating(double newRate)
        {
            var totalRating = (Rate * Count) + newRate;
            var newCount = Count + 1;
            var averageRating = totalRating / newCount;

            return new Rating(averageRating, newCount);
        }
    }
}