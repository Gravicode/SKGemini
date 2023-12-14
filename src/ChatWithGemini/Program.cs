namespace ChatWithGemini
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("--CHAT WITH Gemini WITH Semantic Kernel--");
            var persona = "you are cute assistant with friendly attitude, you can answer anything with funny way.";
            Console.WriteLine($"ASSISTANT PERSONA: {persona}");
            Console.WriteLine($"----------------------------");
            Console.WriteLine($"-- type 'exit' to quit !! --");
            Console.WriteLine($"----------------------------");
            var chat = new ChatService();
            chat.SetupSkill(persona);
            while (true) {
                Console.Write("Q: ");
                var question = Console.ReadLine();
                if(question == "exit")
                {
                    Console.WriteLine("System: bye, thank you..");
                    break;
                }
                if (question != null)
                {
                    var answer = await chat.Chat(question);
                    Console.WriteLine($"A: {answer}");
                }
            }
        }
    }
}