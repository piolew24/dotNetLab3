using System;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        bool showMatrix = false;
        int size = 500;
        int trials = 10;  
        int[] threadCounts = { 2, 4, 8, 16 };

        Console.WriteLine($"Generowanie macierzy {size}x{size}...");
        int[,] A = MatrixCalculator.GenerateRandomMatrix(size, size);
        int[,] B = MatrixCalculator.GenerateRandomMatrix(size, size);
        if (showMatrix) MatrixCalculator.ShowMatrix(A);
        
        MatrixCalculator.MultiplySequential(A, B);

        Console.WriteLine($"--- SREDNIA Z {trials} PROB ---");

        Console.WriteLine("\n--- OBLICZENIA SEKWENCYJNE ---");
        double avgSeq = MeasureAverageMs(trials, () => MatrixCalculator.MultiplySequential(A, B), out var result1);
        Console.WriteLine($"Sredni czas: {avgSeq:F2} ms");
        if (showMatrix) MatrixCalculator.ShowMatrix(result1);

        Console.WriteLine("\n--- OBLICZENIA PARALLEL (Zadanie 1) ---");
        foreach (int t in threadCounts)
        {
            double avgPar = MeasureAverageMs(trials, () => MatrixCalculator.MultiplyParallel(A, B, t), out var result2);
            Console.WriteLine($"Watki: {t} | Sredni czas: {avgPar:F2} ms");
            if (showMatrix) MatrixCalculator.ShowMatrix(result2);
        }

        Console.WriteLine("\n--- OBLICZENIA THREAD (Zadanie 2) ---");
        foreach (int t in threadCounts)
        {
            double avgThr = MeasureAverageMs(trials, () => MatrixCalculator.MultiplyThreads(A, B, t), out var result3);
            Console.WriteLine($"Watki: {t} | Sredni czas: {avgThr:F2} ms");
            if (showMatrix) MatrixCalculator.ShowMatrix(result3);
        }
    }

    static double MeasureAverageMs(int trials, Func<int[,]> operation, out int[,] lastResult)
    {
        var sw = new Stopwatch();
        long totalTicks = 0;
        lastResult = null!;

        for (int i = 0; i < trials; i++)
        {
            sw.Restart();
            lastResult = operation();
            sw.Stop();
            totalTicks += sw.ElapsedTicks;
        }

        return (totalTicks * 1000.0 / Stopwatch.Frequency) / trials;
    }
}
