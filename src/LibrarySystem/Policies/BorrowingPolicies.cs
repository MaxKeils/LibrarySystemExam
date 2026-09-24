namespace LibrarySystem;

public static class BorrowingPolicies
{
    public static BorrowingPolicy MaxConcurrentBooks(int limit)
    {
        if (limit < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(limit));
        }

        return (_, _, activeLoanCount) => activeLoanCount < limit;
    }
}
