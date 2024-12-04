using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void RunPenaltyCalculation()
    {
        DateTime currentDate = new DateTime(2023, 4, 29);

        List<Tagihan> tagihanList = new List<Tagihan>
        {
            new Tagihan { NoTagihan = "Tagihan#1", DueDate = new DateTime(2023, 1, 10), TotalTagihan = 165000 },
            new Tagihan { NoTagihan = "Tagihan#3", DueDate = new DateTime(2023, 1, 20), TotalTagihan = 130000 },
            new Tagihan { NoTagihan = "Tagihan#5", DueDate = new DateTime(2023, 2, 10), TotalTagihan = 95500 },
            new Tagihan { NoTagihan = "Tagihan#2", DueDate = new DateTime(2023, 2, 15), TotalTagihan = 80000 },
            new Tagihan { NoTagihan = "Tagihan#4", DueDate = new DateTime(2023, 3, 30), TotalTagihan = 416000 }
        };

        List<Pembayaran> pembayaranList = new List<Pembayaran>
        {
            new Pembayaran { NoTagihan = "Tagihan#1", PmtDate = new DateTime(2023, 1, 10), PmtAmount = 165000 },
            new Pembayaran { NoTagihan = "Tagihan#3", PmtDate = new DateTime(2023, 2, 20), PmtAmount = 130000 },
            new Pembayaran { NoTagihan = "Tagihan#5", PmtDate = new DateTime(2023, 2, 20), PmtAmount = 95500 },
            new Pembayaran { NoTagihan = "Tagihan#2", PmtDate = new DateTime(2023, 2, 25), PmtAmount = 30000 },
            new Pembayaran { NoTagihan = "Tagihan#2", PmtDate = new DateTime(2023, 3, 30), PmtAmount = 50000 },
            new Pembayaran { NoTagihan = "Tagihan#4", PmtDate = new DateTime(2023, 3, 30), PmtAmount = 50000 }
        };

        var penalties = new List<Penalty>();

        foreach (var tagihan in tagihanList)
        {
            decimal totalPaid = 0;
            int penaltyNo = 1;

            var pembayaranTagihan = pembayaranList.Where(p => p.NoTagihan == tagihan.NoTagihan).OrderBy(p => p.PmtDate).ToList();

            foreach (var pembayaran in pembayaranTagihan)
            {
                if (pembayaran.PmtDate > tagihan.DueDate)
                {
                    int daysLate = (pembayaran.PmtDate - tagihan.DueDate).Days;
                    decimal overdueAmount = tagihan.TotalTagihan - totalPaid;
                    decimal penaltyAmount = (overdueAmount * 2m / 1000) * daysLate;

                    penalties.Add(new Penalty
                    {
                        NoTagihan = tagihan.NoTagihan,
                        PenaltyNo = penaltyNo++,
                        OverdueAmount = overdueAmount,
                        DaysLate = daysLate,
                        PenaltyAmount = penaltyAmount
                    });
                }

                totalPaid += pembayaran.PmtAmount;
                if (totalPaid >= tagihan.TotalTagihan)
                    break;
            }
        }

        Console.WriteLine("No Tagihan | No Penalty | Tagihan Overdue | Hari Keterlambatan | Amount Penalty");
        foreach (var penalty in penalties)
        {
            Console.WriteLine($"{penalty.NoTagihan} | {penalty.PenaltyNo} | {penalty.OverdueAmount} | {penalty.DaysLate} | {penalty.PenaltyAmount:F2}");
        }
    }

    public static void RunSortingAndAllocation()
    {
        var invoices = new List<Invoice>
        {
            new Invoice { InvoiceId = "Tagihan#1", DueDate = new DateTime(2023, 1, 10), TotalAmount = 165000 },
            new Invoice { InvoiceId = "Tagihan#2", DueDate = new DateTime(2023, 2, 15), TotalAmount = 80000 },
            new Invoice { InvoiceId = "Tagihan#3", DueDate = new DateTime(2023, 1, 20), TotalAmount = 130000 },
            new Invoice { InvoiceId = "Tagihan#4", DueDate = new DateTime(2023, 3, 25), TotalAmount = 416000 },
            new Invoice { InvoiceId = "Tagihan#5", DueDate = new DateTime(2023, 2, 10), TotalAmount = 95500 },
            new Invoice { InvoiceId = "Tagihan#6", DueDate = new DateTime(2023, 8, 17), TotalAmount = 523000 },
        };

        Console.Write("Enter payment amount: ");
        string input = Console.ReadLine();
        if (!decimal.TryParse(input, out decimal paymentAmount))
        {
            Console.WriteLine("Error: Invalid input. Please enter a numeric value.");
            return;
        }

        AllocatePayment(paymentAmount, invoices);
    }

    public static void RunAlgo1()
    {
        Console.WriteLine("Enter the integers separated by spaces:");

        var input = Console.ReadLine();
        var numbers = input.Split(' ').Select(int.Parse).ToArray();
        
        Console.WriteLine("Calculated score: " + CalculateScore(numbers));
    }

    public static void RunAlgo2()
    {

        int n = 5; 

        Console.WriteLine("Pola 1:");
        PrintPattern(n, 1);

        Console.WriteLine("Pola 2:");
        PrintPattern(n, 2);

        Console.WriteLine("Pola 3:");
        PrintPattern(n, 3);

        Console.WriteLine("Pola 4:");
        PrintPattern(n, 4);
    }

    public static void Main()
    {
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Algoritma Perhitungan Penalty");
        Console.WriteLine("2. Sorting dan Alokasi");
        Console.WriteLine("3. Algoritma 1");
        Console.Write("Enter your choice (1,2,3, or 4): ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                RunPenaltyCalculation();
                break;
            case "2":
                RunSortingAndAllocation();
                break;
            case "3":
                RunAlgo1();
                break;
            case "4":
                RunAlgo2();
                break;
            default:
                Console.WriteLine("Invalid choice. Please enter 1, 2, 3, or 4.");
                break;
        }
    }

    public static void AllocatePayment(decimal paymentAmount, List<Invoice> invoices)
    {
        var sortedInvoices = invoices.OrderBy(i => i.DueDate).ToList();
        Console.WriteLine($"Input Payment = {paymentAmount:N0}");

        foreach (var invoice in sortedInvoices)
        {
            if (paymentAmount <= 0) break;

            if (paymentAmount >= invoice.TotalAmount)
            {
                invoice.OutstandingAmount = invoice.TotalAmount;
                paymentAmount -= invoice.TotalAmount;
            }
            else
            {
                invoice.OutstandingAmount = paymentAmount;
                paymentAmount = 0;
            }
        }

        foreach (var invoice in sortedInvoices)
        {
            Console.WriteLine($"{invoice.InvoiceId} Due: {invoice.DueDate:dd MMM yy} {invoice.TotalAmount:N0} {invoice.OutstandingAmount:N0}");
        }

        if (paymentAmount > 0)
        {
            Console.WriteLine($"Remaining Payment: {paymentAmount:N0}");
        }
    }

    static int CalculateScore(int[] numbers)
    {
        int score = 0;

        foreach (var num in numbers)
        {
            if (num == 8)
            {
                score += 5;
            }
            else if (num % 2 == 0) 
            {
                score += 1;
            }
            else 
            {
                score += 3;
            }
        }

        return score;
    }

    static void PrintPattern(int n, int patternType)
    {
        if (patternType == 1)
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= i; j++)
                {
                    Console.Write(i);
                }
                Console.WriteLine();
            }
        }
        else if (patternType == 2)
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = i; j >= 1; j--)
                {
                    Console.Write(j);
                }
                Console.WriteLine();
            }
        }
        else if (patternType == 3)
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= i; j++)
                {
                    Console.Write(j);
                }
                for (int j = i - 1; j >= 1; j--)
                {
                    Console.Write(j);
                }
                Console.WriteLine();
            }
        }
        else if (patternType == 4) 
        {
            int[,] grid = new int[n, n];
            int number = 1;

            for (int col = 0; col < n; col++)
            {
                for (int row = 0; row < n; row++)
                {
                    grid[row, col] = number++;
                }
            }

            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    Console.Write(grid[row, col] + " ");
                }
                Console.WriteLine();
            }
        }
    }

    public class Invoice
    {
        public string InvoiceId { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
    }

    public class Tagihan
    {
        public string NoTagihan { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalTagihan { get; set; }
    }

    public class Pembayaran
    {
        public string NoTagihan { get; set; }
        public DateTime PmtDate { get; set; }
        public decimal PmtAmount { get; set; }
    }

    public class Penalty
    {
        public string NoTagihan { get; set; }
        public int PenaltyNo { get; set; }
        public decimal OverdueAmount { get; set; }
        public int DaysLate { get; set; }
        public decimal PenaltyAmount { get; set; }
    }


}
