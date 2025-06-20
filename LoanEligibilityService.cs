public class LoanEligibilityResult
{
    public bool IsEligible { get; set; }
    public string Reason { get; set; }
    public decimal MaxEligibleAmount { get; set; }
}

public class LoanEligibilityService
{
    /// <summary>
    /// Checks loan eligibility based on applicant's financial and personal information.
    /// </summary>
    /// <param name="annualIncome">Applicant's annual income. Must be positive.</param>
    /// <param name="creditScore">Applicant's credit score (300-850).</param>
    /// <param name="age">Applicant's age (18-70).</param>
    /// <param name="existingDebt">Applicant's existing debt.</param>
    /// <param name="requestedAmount">Requested loan amount. Must be positive and within eligibility.</param>
    /// <param name="employmentYears">Years of employment. Must be at least 1.</param>
    /// <param name="isBankrupt">Whether the applicant is currently bankrupt.</param>
    /// <param name="isCitizen">Whether the applicant is a citizen.</param>
    /// <param name="hasCriminalRecord">Whether the applicant has a criminal record.</param>
    /// <param name="dependents">Number of dependents. Must not exceed 10.</param>
    /// <returns>
    /// LoanEligibilityResult indicating eligibility, reasons, and max eligible amount.
    /// 
    /// <para>Edge cases handled:</para>
    /// <list type="bullet">
    /// <item>Negative or zero income</item>
    /// <item>Age below 18 or above 70</item>
    /// <item>Credit score out of range (below 300 or above 850)</item>
    /// <item>Applicant is bankrupt</item>
    /// <item>Applicant is not a citizen</item>
    /// <item>Applicant has a criminal record</item>
    /// <item>Too many dependents (more than 10)</item>
    /// <item>Insufficient employment history (less than 1 year)</item>
    /// <item>Debt-to-income ratio too high (over 40%)</item>
    /// <item>Requested amount exceeds eligibility</item>
    /// <item>Requested amount is negative or zero</item>
    /// </list>
    /// </returns>
    public LoanEligibilityResult CheckEligibility(
        decimal annualIncome,
        int creditScore,
        int age,
        decimal existingDebt,
        decimal requestedAmount,
        int employmentYears,
        bool isBankrupt,
        bool isCitizen,
        bool hasCriminalRecord,
        int dependents)
    {
        // Edge Case 1: Negative or zero income
        if (annualIncome <= 0)
            return new LoanEligibilityResult { IsEligible = false, Reason = "Invalid income." };

        // Edge Case 2: Age below 18 or above 70
        if (age < 18 || age > 70)
            return new LoanEligibilityResult { IsEligible = false, Reason = "Age not eligible." };

        // Edge Case 3: Credit score out of range
        if (creditScore < 300 || creditScore > 850)
            return new LoanEligibilityResult { IsEligible = false, Reason = "Invalid credit score." };

        // Edge Case 4: Bankruptcy
        if (isBankrupt)
            return new LoanEligibilityResult { IsEligible = false, Reason = "Applicant is bankrupt." };

        // Edge Case 5: Non-citizen
        if (!isCitizen)
            return new LoanEligibilityResult { IsEligible = false, Reason = "Applicant is not a citizen." };

        // Edge Case 6: Criminal record
        if (hasCriminalRecord)
            return new LoanEligibilityResult { IsEligible = false, Reason = "Applicant has a criminal record." };

        // Edge Case 7: Too many dependents
        if (dependents > 10)
            return new LoanEligibilityResult { IsEligible = false, Reason = "Too many dependents." };

        // Edge Case 8: Employment years too low
        if (employmentYears < 1)
            return new LoanEligibilityResult { IsEligible = false, Reason = "Insufficient employment history." };

        // Edge Case 9: Debt-to-income ratio too high
        decimal dti = existingDebt / annualIncome;
        if (dti > 0.4m)
            return new LoanEligibilityResult { IsEligible = false, Reason = "Debt-to-income ratio too high." };

        // Edge Case 10: Requested amount too high
        decimal maxEligibleAmount = annualIncome * 0.5m - existingDebt;
        if (requestedAmount > maxEligibleAmount)
            return new LoanEligibilityResult
            {
                IsEligible = false,
                Reason = "Requested amount exceeds eligibility.",
                MaxEligibleAmount = maxEligibleAmount > 0 ? maxEligibleAmount : 0
            };

        // Edge Case 11: Requested amount negative or zero
        if (requestedAmount <= 0)
            return new LoanEligibilityResult { IsEligible = false, Reason = "Invalid requested amount." };

        // Passed all checks
        return new LoanEligibilityResult
        {
            IsEligible = true,
            Reason = "Eligible for loan.",
            MaxEligibleAmount = maxEligibleAmount
        };
    }
} 