namespace Microsoft.Zune.Service;

public class PointsPayment : PaymentInstrument
{
	public PointsPayment()
		: base(null, PaymentType.Points)
	{
	}
}
