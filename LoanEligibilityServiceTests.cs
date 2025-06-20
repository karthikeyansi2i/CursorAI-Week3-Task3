using Xunit;

public class LoanEligibilityServiceTests
{
    private readonly LoanEligibilityService _service = new LoanEligibilityService();

    [Fact]
    public void EdgeCase1_NegativeOrZeroIncome()
    {
        var result = _service.CheckEligibility(0, 700, 30, 1000, 5000, 5, false, true, false, 2);
        Assert.False(result.IsEligible);
        Assert.Equal("Invalid income.", result.Reason);
    }

    [Theory]
    [InlineData(17)]
    [InlineData(71)]
    public void EdgeCase2_AgeBelow18OrAbove70(int age)
    {
        var result = _service.CheckEligibility(50000, 700, age, 1000, 5000, 5, false, true, false, 2);
        Assert.False(result.IsEligible);
        Assert.Equal("Age not eligible.", result.Reason);
    }

    [Theory]
    [InlineData(299)]
    [InlineData(851)]
    public void EdgeCase3_CreditScoreOutOfRange(int creditScore)
    {
        var result = _service.CheckEligibility(50000, creditScore, 30, 1000, 5000, 5, false, true, false, 2);
        Assert.False(result.IsEligible);
        Assert.Equal("Invalid credit score.", result.Reason);
    }

    [Fact]
    public void EdgeCase4_Bankruptcy()
    {
        var result = _service.CheckEligibility(50000, 700, 30, 1000, 5000, 5, true, true, false, 2);
        Assert.False(result.IsEligible);
        Assert.Equal("Applicant is bankrupt.", result.Reason);
    }

    [Fact]
    public void EdgeCase5_NonCitizen()
    {
        var result = _service.CheckEligibility(50000, 700, 30, 1000, 5000, 5, false, false, false, 2);
        Assert.False(result.IsEligible);
        Assert.Equal("Applicant is not a citizen.", result.Reason);
    }

    [Fact]
    public void EdgeCase6_CriminalRecord()
    {
        var result = _service.CheckEligibility(50000, 700, 30, 1000, 5000, 5, false, true, true, 2);
        Assert.False(result.IsEligible);
        Assert.Equal("Applicant has a criminal record.", result.Reason);
    }

    [Fact]
    public void EdgeCase7_TooManyDependents()
    {
        var result = _service.CheckEligibility(50000, 700, 30, 1000, 5000, 5, false, true, false, 11);
        Assert.False(result.IsEligible);
        Assert.Equal("Too many dependents.", result.Reason);
    }

    [Fact]
    public void EdgeCase8_EmploymentYearsTooLow()
    {
        var result = _service.CheckEligibility(50000, 700, 30, 1000, 5000, 0, false, true, false, 2);
        Assert.False(result.IsEligible);
        Assert.Equal("Insufficient employment history.", result.Reason);
    }

    [Fact]
    public void EdgeCase9_DebtToIncomeRatioTooHigh()
    {
        var result = _service.CheckEligibility(50000, 700, 30, 25000, 5000, 5, false, true, false, 2);
        Assert.False(result.IsEligible);
        Assert.Equal("Debt-to-income ratio too high.", result.Reason);
    }

    [Fact]
    public void EdgeCase10_RequestedAmountTooHigh()
    {
        var result = _service.CheckEligibility(50000, 700, 30, 1000, 30000, 5, false, true, false, 2);
        Assert.False(result.IsEligible);
        Assert.Equal("Requested amount exceeds eligibility.", result.Reason);
        Assert.True(result.MaxEligibleAmount > 0);
    }

    [Fact]
    public void EdgeCase11_RequestedAmountNegativeOrZero()
    {
        var result = _service.CheckEligibility(50000, 700, 30, 1000, 0, 5, false, true, false, 2);
        Assert.False(result.IsEligible);
        Assert.Equal("Invalid requested amount.", result.Reason);
    }

    [Fact]
    public void EligibleCase_AllValid()
    {
        var result = _service.CheckEligibility(50000, 700, 30, 1000, 5000, 5, false, true, false, 2);
        Assert.True(result.IsEligible);
        Assert.Equal("Eligible for loan.", result.Reason);
        Assert.True(result.MaxEligibleAmount > 0);
    }
} 