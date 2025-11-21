namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");
            string account_number = "";
            string name = "";
            string date_of_birth = "";
            float Amount = 0;
            bool accountExist = false;


            void Create_new_bank_account()
            {
                Console.Write("Enter Your Account Number : ");
                account_number = Console.ReadLine();
                Console.Write("Enter Your Name: ");
                name = Console.ReadLine();
                Console.Write("Enter Your Data of Birth : ");
                date_of_birth = Console.ReadLine();
                Console.Write("Enter amount : ");
                Amount = Convert.ToInt32(Console.ReadLine());
                Console.Write("Your account created successfully ");
                Console.WriteLine();

            }
            void Deposite_money(string acc_no)
            {
                //Console.Write("Enter Your Account Number: ");
                //string acc_no = Console.ReadLine(); 
                if(acc_no == account_number)
                {
                    Console.Write("Enter Amount : ");
                    float amt = Convert.ToInt32(Console.ReadLine());
                    Amount = Amount + amt;
                    Console.WriteLine("Your amount added to your account");
                    Console.WriteLine($"Your current balance : {Amount}");
                }
                else
                {
                    Console.WriteLine("Invalide Account Number");
                }

            }

            void display_account_details()
            {
                Console.WriteLine($"Account Number : {account_number}");
                Console.WriteLine($"Account Holder : {name}");
                Console.WriteLine($"DOB : {date_of_birth}");
                Console.WriteLine($"Available Amount : {Amount}");
            }


            void withdraw_money(string acc_no)
            {
                if (acc_no == account_number)
                {
                    Console.Write("Enter Amount you want Withdraw :");
                    float amt = Convert.ToInt32(Console.ReadLine());
                    Amount -= amt;
                    Console.WriteLine($"{amt} withdrawn ...");
                    Console.WriteLine($"Available Balance : {Amount}");

                }
                else {
                    Console.WriteLine("Invalide Account Number");
                }
            }


            while (true)
            {
                Console.WriteLine("1. Create a new bank account ");
                Console.WriteLine("2. Deposit money ");
                Console.WriteLine("3. Withdraw money ");
                Console.WriteLine("4. Display account details");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice : ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        if (accountExist)
                        {
                            Console.WriteLine("You already have an account ");
                        }
                        else
                        {
                            Create_new_bank_account();
                            accountExist = true;
                        }

                        break;

                    case 2:

                        {
                            Console.Write("Enter your account number :");
                            string acc_number1 = Console.ReadLine();

                            Deposite_money(acc_number1);

                            break;
                        }

                    case 3:

                        Console.Write("Enter your account number :");
                        string acc_number2 = Console.ReadLine();
                        withdraw_money(acc_number2);

                        break;

                    case 4:

                        Console.Write("Enter your account number :");
                        string acc_number = Console.ReadLine();
                        if (acc_number == account_number)
                        {

                            display_account_details();
                        }

                        break;

                    case 5:

                        return;

                    default:

                        Console.WriteLine("Invalid choice");
                        break;

                }
            }
        }
    }
}
