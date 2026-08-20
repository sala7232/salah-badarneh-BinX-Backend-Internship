namespace CardiacPatientMonitoring.Api.Services;

public class VitalSignAssessmentService
{
    public string GetHeartRateStatus(int heartRate)
    {
        if (heartRate < 60)
        {
            return "Low";
        }

        if (heartRate > 100)
        {
            return "High";
        }

        return "Normal";
    }

    public int CalculatePulsePressure(
        int systolicBloodPressure,
        int diastolicBloodPressure)
    {
        return systolicBloodPressure - diastolicBloodPressure;
    }
}
