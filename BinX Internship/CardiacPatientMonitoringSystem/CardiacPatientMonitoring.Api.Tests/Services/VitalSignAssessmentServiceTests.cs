using CardiacPatientMonitoring.Api.Services;

namespace CardiacPatientMonitoring.Api.Tests.Services;

public class VitalSignAssessmentServiceTests
{
    [Fact]
    public void GetHeartRateStatus_ReturnsLow_WhenHeartRateIsBelow60()
    {
        // Arrange
        var service = new VitalSignAssessmentService();

        // Act
        var result = service.GetHeartRateStatus(55);

        // Assert
        Assert.Equal("Low", result);
    }

    [Fact]
    public void GetHeartRateStatus_ReturnsNormal_WhenHeartRateIsBetween60And100()
    {
        // Arrange
        var service = new VitalSignAssessmentService();

        // Act
        var result = service.GetHeartRateStatus(80);

        // Assert
        Assert.Equal("Normal", result);
    }

    [Fact]
    public void GetHeartRateStatus_ReturnsHigh_WhenHeartRateIsAbove100()
    {
        // Arrange
        var service = new VitalSignAssessmentService();

        // Act
        var result = service.GetHeartRateStatus(110);

        // Assert
        Assert.Equal("High", result);
    }

    [Theory]
    [InlineData(120, 80, 40)]
    [InlineData(135, 85, 50)]
    [InlineData(110, 70, 40)]
    public void CalculatePulsePressure_ReturnsDifference(
        int systolicBloodPressure,
        int diastolicBloodPressure,
        int expected)
    {
        // Arrange
        var service = new VitalSignAssessmentService();

        // Act
        var result = service.CalculatePulsePressure(
            systolicBloodPressure,
            diastolicBloodPressure);

        // Assert
        Assert.Equal(expected, result);
    }
}
