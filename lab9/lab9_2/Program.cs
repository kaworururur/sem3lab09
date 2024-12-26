namespace lab9_2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}