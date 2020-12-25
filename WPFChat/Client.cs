namespace WPFChat
{
    /// <summary>
    /// Грустный клиент
    /// </summary>
    public class Client
    {
        public int ID { get; set; }

        public string Name { get; set; }
        

        public Client()
        {
            
        }

        public Client(string name)
        {
            Name = name;
        }

    }
}
