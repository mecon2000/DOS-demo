namespace DOSAttacker
{
    static public class Consts
    {
        public const int minNextCallIntervalMs = 500;
        public const int maxNextCallIntervalMs = 1000;
        public const string baseDomain = "https://localhost";
        public const int port = 44368;
        public static readonly string fullUrlTemplate = $"{baseDomain}:{port}?clientId={{0}}";
    }
}
