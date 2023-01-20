// See https://aka.ms/new-console-template for more information
using System;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using System.Threading.Tasks.Dataflow;

// Gets the path of the program
string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Split("bin\\Debug\\net6.0")[0].Split("file:\\")[1];


// Function for checking if the input file exists
string CheckInputFile (string inFile)
{
    string outFile = path + "InputFiles\\" + inFile;

    // Checks if the .stl file exists in the "InputFiles" folder
    if (File.Exists(outFile))
    {
        // If it exists return the file adress
        return outFile;
    }
    else
    {
        // If the file does not exist ask for the name of the file again
        Console.WriteLine("File does not exist. Please type the name of the file in again.");
        outFile = Console.ReadLine ();
    }
    // Recursion to make sure the new file exists
    return CheckInputFile(outFile);
}

// Function to check if the output file name is taken or not
string CheckOutputFile (string inFile, bool conf)
{
    string prompt = string.Empty;
    string outFile = path + "OutputFiles\\" + inFile;
    

    // Checks if the file name is avalible and if the user has already confirmed that they want to delete the old file already
    if (File.Exists(outFile) && conf == false)
    {

        // If it is taken ask if they want to delete the old one or not
        Console.WriteLine("The file allready exists. Would you like to overwrite it?(Y/N)");
        prompt = Console.ReadLine().ToLower();
        if (prompt == "y") 
        {
            // If they say yes delete the old one and create a new one and set the confirmation variable to true
            conf = true;
            File.Delete(outFile);
            
            
            // Return the file
            outFile = inFile;
            return outFile;
        }
        else
        {   
            // If they say no to delete the old one it ask for a new file name
            Console.WriteLine("Type a new file name.");

            outFile = Console.ReadLine();
            
        }
    }
    else
    {
        
        // If the file name is avalible make a file with that file name
        
            
        return outFile;
        
    }
    // Recursion to check if new file name is taken or not.
    return CheckOutputFile(outFile, conf);
}

// Get the name of the input file
Console.WriteLine("Please type the name of the .stl file.");
string read_file = CheckInputFile(Console.ReadLine());


// Get the name of the output file
Console.WriteLine("Please type the name of the .cstl file.");
string write_file = CheckOutputFile(Console.ReadLine(), false);


// Is the input binary or ascii
Console.WriteLine("Is the .stl file binary or ascii? (b/a)");
string input_type = Console.ReadLine().ToLower();

// do they want the output to be binary or ascii?
Console.WriteLine("Do you want the file to output in binary or ascii? (b/a)");
string output_type = Console.ReadLine().ToLower();

float[] temp_x_array;
float[] temp_y_array;
float[] temp_z_array;


// Start of processing code
if (input_type == "b")
{
    Console.WriteLine("Sorry binary .stl files are currently unsupported");
    return;
}
else if (input_type == "a")
{
    // Reads file and breaks it into lines
    Console.WriteLine("Reading File");
    string stl_file = File.ReadAllText(read_file);
    char[] delims = new[] { '\r', '\n' };
    string[] preprocessed_lines = stl_file.Split(delims, StringSplitOptions.TrimEntries);
    List<string[]> postprocessed_lines = new List<string[]>();

    Console.WriteLine("Processing File");
    // Checks if the argument in the .stl file is "vertex" or not and if it is it adds the line to the postprocessed list
    for (int i = 0; i < preprocessed_lines.Length; i++)
    {
        string[] line = preprocessed_lines[i].Split(" ", StringSplitOptions.TrimEntries);
        if (line[0] == "vertex")
        {
            postprocessed_lines.Add(line);
        }
    }

    // Arrays of floats for the values of the verticies
    temp_x_array = new float[postprocessed_lines.Count];
    temp_y_array = new float[postprocessed_lines.Count];
    temp_z_array = new float[postprocessed_lines.Count];

    // Adds the vertex values to the arrays
    for (int i = 0; i < postprocessed_lines.Count; i++)
    {
        string[] line = postprocessed_lines[i];
        temp_x_array[i] = float.Parse(line[1]);
        temp_y_array[i] = float.Parse(line[2]);
        temp_z_array[i] = float.Parse(line[3]);
    }


}
else
{
    Console.WriteLine("Invalid Input Type");
    return;
}

// Start of conversion code
Console.WriteLine("Converting");

int list_index = 0;

// 2D point list
List<float> x_list = new List<float>();
List<float> y_list = new List<float>();
List<float> z_list = new List<float>();

// Point dictionary
Dictionary<Vector3, int> point_dictionary = new Dictionary<Vector3, int>();

// Tesselation list
int[] tesselation_array = new int[temp_x_array.Length];

// Adds all unique points to a list and makes a list of pointers for the tesselations
for (int i = 0; i < temp_x_array.Length; i++)
{
    Vector3 point = new Vector3(temp_x_array[i], temp_y_array[i], temp_z_array[i]);
    if (!point_dictionary.ContainsKey(point))
    {

        x_list.Add(temp_x_array[i]);
        y_list.Add(temp_y_array[i]);
        z_list.Add(temp_z_array[i]);
        point_dictionary.Add(point, list_index);
        list_index++;
    }
    tesselation_array[i] = point_dictionary[point];

}

// Code to  write the files
Console.WriteLine("Writing");
float[] x = x_list.ToArray();
float[] y = y_list.ToArray();
float[] z = z_list.ToArray();
if (output_type == "a")
{
    
    using (StreamWriter file = new StreamWriter(File.Create(write_file)))
    {
        file.WriteLine("x_list  [" + String.Join(", ",x) + "]");
        file.WriteLine("y_list  [" + String.Join(", ", y) + "]");
        file.WriteLine("z_list  [" + String.Join(", ", z) + "]");
        file.WriteLine("tess_list  [" + String.Join(", ", tesselation_array) + "]");
    };
}
else if (output_type == "b")
{
    using (BinaryWriter file = new BinaryWriter(File.Create(write_file)))
    {
        for (int i = 0; i < x.Length; i++)
        {
            file.Write(x[i]);
        }
        file.Write(0xffffffff);
        for (int i = 0; i < y.Length; i++)
        {
            file.Write(y[i]);
        }
        file.Write(0xffffffff);
        for (int i = 0; i < z.Length; i++)
        {
            file.Write(z[i]);
        }
        file.Write(0xffffffff);
        for (int i = 0; i < tesselation_array.Length; i++)
        {
            file.Write(tesselation_array[i]);
        }
    };
}
else
{
    Console.WriteLine("Invalid Output Type");
    return;
}