using System;

namespace Mexbt.Api
{
	internal class TestUtils
	{
		internal static void AssertAccepted(MexbtResponse response)
		{
			NUnit.Framework.Assert.IsTrue (response.IsAccepted, response.RejectReason);
		}
	}
}