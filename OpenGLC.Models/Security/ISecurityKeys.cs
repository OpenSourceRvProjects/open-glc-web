namespace OpenGLC.Models.Security
{
	public interface ISecurityKeys
	{
		public string passwordPrivateKey { get; set; }
		public string JWT_PrivateKey { get; set; }
		public string issuer { get; set; }
	}
}
