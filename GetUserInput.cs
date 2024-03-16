using System.Globalization;
namespace CodingTracker
{
    internal class GetUserInput
    {
        CodingController codingController = new();
        internal void MainMenu()
        {
            bool closeApp = false;
            while (!closeApp)
            {
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("----------");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. View all records");
                Console.WriteLine("2. Insert a record");
                Console.WriteLine("3. Update a record");
                Console.WriteLine("4. Delete a record");

                string? userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "0":
                        Console.WriteLine("Goodbye!");
                        closeApp = true;
                        break;
                    case "1":
                        codingController.Get();
                        break;
                    case "2":
                        ProcessAdd();
                        break;
                    case "3":
                        ProcessUpdate();
                        break;
                    case "4":
                        ProcessDelete();
                        break;

                    default:
                        Console.WriteLine("Invalid Command. Please type a number from 0 to 4.");
                        break;
                }
            }
        }

        internal void ProcessAdd()
        {
            var date = GetDateInput();
            string[] info = CalculateDuration();
            CodingSession coding = new();

            coding.Date = date;
            coding.Duration = info[2];

            codingController.Post(coding);
        }

        internal void ProcessDelete()
        {
            codingController.Get();
            int id = GetNumInput("Type the ID of the session you want to delete. Type 0 to go back to Main Menu");
            
            var coding = codingController.Delete(id);

            while (coding == 0)
            {
                Console.WriteLine($"Record with id {id} doesn't exist");
                Console.ReadLine();

                codingController.Get();
                id = GetNumInput("Type the ID of the session you want to delete. Type 0 to go back to Main Menu");

                coding = codingController.Delete(id);
            }
        }

        internal void ProcessUpdate()
        {   
            codingController.Get();
            int id = GetNumInput("Type the ID of the session you want to update. Type 0 to go back to Main Menu");

            codingController.Update(id);
        

        }

        internal int GetNumInput(string message)
        {
            bool validNumber;
            int userNum;
            do
            {
                Console.WriteLine(message);
                string? userInput = Console.ReadLine();

                if (userInput == "0") MainMenu();

                validNumber = int.TryParse(userInput, out userNum);
            } while (!validNumber);

            return userNum;
        }

        internal string GetDateInput()
        {
            Console.WriteLine("Enter the date you want to record(dd-mm-yy): ");
            Console.WriteLine("Type 0 to go back to Main Menu");
            string? userInput = Console.ReadLine();
            if (userInput == "0") MainMenu();

            while (!DateTime.TryParseExact(userInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Wrong Format. Enter the date with the correct format(dd-mm-yy):");
                userInput = Console.ReadLine();
            }

            return userInput;
        }

        internal string[] CalculateDuration()
        {
            string[] codingInfo = new string[3];

            Console.WriteLine("Type the time you started coding (hh:mm): ");
            Console.WriteLine("Type 0 to go back to Main Menu.");
            string? userInput = Console.ReadLine();
            TimeSpan startTime;
            while (!TimeSpan.TryParseExact(userInput, "h\\:mm", CultureInfo.InvariantCulture, TimeSpanStyles.None, out startTime))
            {
                Console.WriteLine("Wrong format. Type the starting time again (hh:mm): ");
                userInput = Console.ReadLine();
            }

            Console.WriteLine("Type the time you stopped coding (hh:mm): ");
            userInput = Console.ReadLine();
            TimeSpan endTime;
            while (!TimeSpan.TryParseExact(userInput, "h\\:mm", CultureInfo.InvariantCulture, TimeSpanStyles.None, out endTime))
            {
                Console.WriteLine("Wrong format. Type the starting time again (hh:mm): ");
                userInput = Console.ReadLine();
            }

            TimeSpan duration = endTime - startTime;

            codingInfo[0] = startTime.ToString();
            codingInfo[1] = endTime.ToString();
            codingInfo[2] = duration.ToString();
            return codingInfo;
        }
    }
}