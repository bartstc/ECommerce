namespace ProductCatalog.Domain;

public class Rating : ValueObject<Rating>
{
    public double Rate { get; }
    public int Count { get; }

    public static Rating Of(double rate, int count)
    {
        if (rate < 0 || rate > 5)
            throw new BusinessValidationException("Rate must be between 0 and 5.");
        if (count < 0)
            throw new BusinessValidationException("Count cannot be negative.");

        return new Rating(rate, count);
    }

    public Rating Recalculate(double newRate)
    {
        var totalRating = (Rate * Count) + newRate;
        var newCount = Count + 1;
        var averageRating = totalRating / newCount;

        return new Rating(averageRating, newCount);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Rate;
        yield return Count;
    }

    private Rating(double rate, int count)
    {
        Rate = rate;
        Count = count;
    }

    private Rating() { }
}