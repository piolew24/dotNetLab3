using System;
using System.Threading;
using System.Threading.Tasks;

public class MatrixCalculator
{
    public static int[,] GenerateRandomMatrix(int rows, int cols)
    {
        Random rand = new Random();
        int[,] matrix = new int[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                matrix[i, j] = rand.Next(1, 10);
        return matrix;
    }
    
    public static int[,] MultiplySequential(int[,] A, int[,] B)
    {
        int rowsA = A.GetLength(0);
        int colsA = A.GetLength(1);
        int colsB = B.GetLength(1);
        int[,] result = new int[rowsA, colsB];

        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                int sum = 0;
                for (int k = 0; k < colsA; k++)
                    sum += A[i, k] * B[k, j];
                result[i, j] = sum;
            }
        }
        return result;
    }
    
    public static int[,] MultiplyParallel(int[,] A, int[,] B, int maxThreads)
    {
        int rowsA = A.GetLength(0);
        int colsA = A.GetLength(1);
        int colsB = B.GetLength(1);
        int[,] result = new int[rowsA, colsB];

        ParallelOptions opt = new ParallelOptions { MaxDegreeOfParallelism = maxThreads };

        Parallel.For(0, rowsA, opt, i =>
        {
            for (int j = 0; j < colsB; j++)
            {
                int sum = 0;
                for (int k = 0; k < colsA; k++)
                    sum += A[i, k] * B[k, j];
                result[i, j] = sum;
            }
        });

        return result;
    }
    
    public static int[,] MultiplyThreads(int[,] A, int[,] B, int threadCount)
    {
        int rowsA = A.GetLength(0);
        int colsA = A.GetLength(1);
        int colsB = B.GetLength(1);
        int[,] result = new int[rowsA, colsB];

        Thread[] threads = new Thread[threadCount];
        int rowsPerThread = rowsA / threadCount;

        for (int t = 0; t < threadCount; t++)
        {
            int startRow = t * rowsPerThread;
            int endRow = (t == threadCount - 1) ? rowsA : startRow + rowsPerThread;

            threads[t] = new Thread(() =>
            {
                for (int i = startRow; i < endRow; i++)
                {
                    for (int j = 0; j < colsB; j++)
                    {
                        int sum = 0;
                        for (int k = 0; k < colsA; k++)
                            sum += A[i, k] * B[k, j];
                        result[i, j] = sum;
                    }
                }
            });
            threads[t].Start();
        }
        
        foreach (var thread in threads)
        {
            thread.Join();
        }

        return result;
    }

    public static void ShowMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }
}