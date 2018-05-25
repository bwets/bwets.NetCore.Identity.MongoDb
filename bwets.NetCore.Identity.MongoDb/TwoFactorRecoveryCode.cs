namespace bwets.NetCore.Identity.MongoDb
{
    public class TwoFactorRecoveryCode
    {
        public string Code { get; set; }

        public bool Redeemed { get; set; }
    }
}
