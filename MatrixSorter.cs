using System;

// I didn't write any of the following actual code

public class MatrixSorter
{
    // Hate me all you want
    public static readonly string SOURCE_CODE = @"<color=#DCDCDC>#include <stdio.h></color>
<color=#DCDCDC>#include <stdlib.h></color>
<color=#DCDCDC>#include <time.h></color>
<color=#DCDCDC>#include <unistd.h></color>

<color=#FF6347>const</color> <color=#D3D3D3>int</color> SHELL_GAPS[3] = {7, 3, 1};

<color=#FF6347>void</color> swap(<color=#D3D3D3>int</color> *a, <color=#D3D3D3>int</color> *b) {
    <color=#D3D3D3>int</color> temp = *a;
    *a = *b;
    *b = temp;
}

<color=#FF6347>void</color> shell_sort(<color=#D3D3D3>int</color> size, <color=#D3D3D3>int</color> **array) {
    <color=#FF6347>for</color> (<color=#D3D3D3>int</color> g = 0; g < sizeof(SHELL_GAPS) / sizeof(<color=#D3D3D3>int</color>); g++) {
        <color=#D3D3D3>int</color> gap = SHELL_GAPS[g];
        <color=#FF6347>for</color> (<color=#D3D3D3>int</color> i = gap; i < size; i++) { <color=#808080>// if gap is greater than size, we skip it</color>
            <color=#FF6347>for</color> (<color=#D3D3D3>int</color> j = i - gap; j >= 0; j -= gap) {
                <color=#D3D3D3>int</color> *diagonal_j      = &array[j][size - j - 1];
                <color=#D3D3D3>int</color> *diagonal_j_next = &array[j + gap][size - (j + gap) - 1];
                
                <color=#FF6347>if</color> (*diagonal_j > *diagonal_j_next) { <color=#808080>// Condition that defines order</color>
                    swap(diagonal_j, diagonal_j_next);
                }
            }
        }
    }
}

<color=#FF6347>void</color> print_matrix(<color=#D3D3D3>int</color> size, <color=#D3D3D3>int</color> **array) {
    <color=#FF6347>for</color> (<color=#D3D3D3>int</color> y = 0; y < size; y++) {
        <color=#FF6347>for</color> (<color=#D3D3D3>int</color> x = 0; x < size; x++) {
            <color=#FF6347>if</color> (x == size - y - 1) {
                <color=#D3D3D3>printf</color>(<color=#FFFF00>""\033[31m%3d\033[0m ""</color>, array[y][x]); <color=#808080>// print in red</color>
            } <color=#FF6347>else</color> {
                <color=#D3D3D3>printf</color>(<color=#FFFF00>""%3d ""</color>, array[y][x]);
            }
        }
        <color=#D3D3D3>printf</color>(<color=#FFFF00>""\n""</color>);
    }
}

<color=#FF6347>int</color> main(<color=#D3D3D3>int</color> argc, <color=#D3D3D3>char</color> **argv) {
    <color=#D3D3D3>srand</color>(clock());

    <color=#D3D3D3>int</color> size;
    <color=#D3D3D3>int</color> **matrix;

    <color=#FF6347>if</color> (argc < 3) {
        <color=#FF6347>if</color> (argc == 1) {
            <color=#D3D3D3>printf</color>(<color=#FFFF00>""Enter the size of the matrix: ""</color>);
            <color=#D3D3D3>scanf</color>(<color=#FFFF00>""%d""</color>, &size);
        } <color=#FF6347>else</color> {
            size = <color=#D3D3D3>atoi</color>(argv[1]);
        }
        
        matrix = <color=#D3D3D3>malloc</color>(sizeof(<color=#D3D3D3>int</color> *) * size);
        <color=#FF6347>for</color> (<color=#D3D3D3>int</color> y = 0; y < size; y++) {
            matrix[y] = <color=#D3D3D3>malloc</color>(size * sizeof(<color=#D3D3D3>int</color>));
            <color=#FF6347>for</color> (<color=#D3D3D3>int</color> x = 0; x < size; x++) {
                matrix[y][x] = rand() % 100 - 50;
            }
        }
    } <color=#FF6347>else</color> {
        size = <color=#D3D3D3>atoi</color>(argv[1]);
        <color=#FF6347>if</color> (argc < size * size + 2) {
            <color=#D3D3D3>printf</color>(<color=#FFFF00>""Invalid argument count for matrix size %d\n""</color>, size);
            <color=#D3D3D3>return</color> 0;
        }
        
        matrix = <color=#D3D3D3>malloc</color>(sizeof(<color=#D3D3D3>int</color> *) * size);
        <color=#FF6347>for</color> (<color=#D3D3D3>int</color> i = 0; i < size*size; i++) {
            <color=#D3D3D3>int</color> y = i / size;
            <color=#D3D3D3>int</color> x = i % size;
            <color=#FF6347>if</color> (x == 0) {
                matrix[y] = <color=#D3D3D3>malloc</color>(size * sizeof(<color=#D3D3D3>int</color>));
            }
            matrix[y][x] = <color=#D3D3D3>atoi</color>(argv[i + 2]);
        }
    }

    <color=#D3D3D3>printf</color>(<color=#FFFF00>""\nInput matrix:\n""</color>);
    print_matrix(size, matrix);
    
    shell_sort(size, matrix);
    
    <color=#D3D3D3>printf</color>(<color=#FFFF00>""\nSorted matrix:\n""</color>);
    print_matrix(size, matrix);
    
    <color=#D3D3D3>free</color>(matrix);
    <color=#D3D3D3>return</color> 0;
}

// <color=#808080>vim: ts=4: sw=4:</color>
";
    private static readonly int[] ShellGaps = [7, 3, 1];
    public string Run(int size = 10, int[,]? initialMatrix = null)
    {
        int[,] matrix;

        if (initialMatrix != null)
        {
            matrix = initialMatrix;
            size = initialMatrix.GetLength(0);
        }
        else
        {
            matrix = GenerateRandomMatrix(size);
        }

        var result = $"Input matrix:\n{MatrixToString(matrix)}\n";
        
        ShellSortAntidiagonal(matrix, size);
        
        result += $"Sorted matrix:\n{MatrixToString(matrix)}\n";
        return result;
    }

    public string RunFromString(string? input = null)
    {
        if (input == null) {
            return Run();
        }

        var args = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        // Check if there are valid arguments
        if (args.Length == 0)
        {
            return Run();
        }

        if (args.Length == 1)
        {
            // Only size is provided
            int size;
            if (!int.TryParse(args[0], out size))
            {
                return "<color=#FF0000>Error: Invalid size argument.</color>\n\n";
            }
            return Run(size);
        }
        else
        {
            int size;
            if (!int.TryParse(args[0], out size)) {
                return "<color=#FF0000>Error: Invalid size argument.</color>\n\n";
            }
            if (args.Length < size*size + 1)
            {
                return $"<color=#FF0000>Error: The number of matrix elements does not match the expected size. Expected {size}x{size} matrix, but got {args.Length - 1} elements.</color>\n\n";
            }

            var matrix = new int[size, size];
            for (int i = 0; i < size*size; i++)
            {
                    if (!int.TryParse(args[i + 1], out matrix[i / size, i % size]))
                    {
                        return $"<color=#FF0000>Error: Invalid matrix element '{args[i + 1]}'.</color>\n\n";
                    }
            }

            return Run(size, matrix);
        }
    }

    private void ShellSortAntidiagonal(int[,] matrix, int size)
    {
        foreach (var gap in ShellGaps)
        {
            if (gap >= size) continue;

            for (int i = gap; i < size; i++)
            {
                for (int j = i - gap; j >= 0; j -= gap)
                {
                    int currentIndex = j;
                    int nextIndex = j + gap;

                    int currentValue = matrix[currentIndex, size - currentIndex - 1];
                    int nextValue = matrix[nextIndex, size - nextIndex - 1];

                    if (currentValue > nextValue)
                    {
                        matrix[currentIndex, size - currentIndex - 1] = nextValue;
                        matrix[nextIndex, size - nextIndex - 1] = currentValue;
                    }
                }
            }
        }
    }

    private static int[,] GenerateRandomMatrix(int size)
    {
        var random = new Random();
        var matrix = new int[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                matrix[y, x] = random.Next(-50, 50);
            }
        }
        return matrix;
    }

    private static string MatrixToString(int[,] matrix)
    {
        int size = matrix.GetLength(0);
        var result = string.Empty;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (x == size - y - 1)
                {
                    result += $"<color=#FF0000>{matrix[y, x],3}</color> ";
                }
                else
                {
                    result += $"{matrix[y, x],3} ";
                }
            }
            result += "\n";
        }
        return result;
    }
}