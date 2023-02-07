// See https://aka.ms/new-console-template for more information
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading.Tasks.Dataflow;

// Gets the path of the program
char sep = System.IO.Path.DirectorySeparatorChar;
string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase
                                        .ToString()
                                        .Split("//")[1]
                                        .Split("bin")[0];
if (path.Contains("/C:"))
{
    path = path.Split("/C:")[1];
}
// Quit if the path is null
if (path == null)
{
    return;
}
Console.WriteLine(path);

List<List<float>> verticies = new List<List<float>>();
List<List<float>> normals = new List<List<float>>();
int faucet_count = 0;

List<float> ListRound (List<float> list, int places)
{
    if (list.Count == 0)
    {
        return list;
    }
    else if (places == 0)
    {
        return list;
    }
    for (int i = 0; i < list.Count; i++)
    {
        list[i] = MathF.Round(list[i], places);
    }
    return list;
}


List<List<List<float>>> ReadBinaryFile (string filename)
{
    using (BinaryReader read = new BinaryReader(File.Open(filename, FileMode.Open)))
    {
        read.ReadBytes(80);
        faucet_count = read.ReadInt32();
        for (int i = 0; i < faucet_count; i++)
        {
            List<float> normal = new List<float>{ read.ReadSingle(), read.ReadSingle(), read.ReadSingle() };
            List<float> vertex1 = new List<float>{ read.ReadSingle(), read.ReadSingle(), read.ReadSingle() };
            List<float> vertex2 = new List<float> { read.ReadSingle(), read.ReadSingle(), read.ReadSingle() };
            List<float> vertex3 = new List<float> { read.ReadSingle(), read.ReadSingle(), read.ReadSingle() };
            normals.Add(ListRound(normal, 6));
            verticies.Add(ListRound(vertex1, 6));
            verticies.Add(ListRound(vertex2, 6));
            verticies.Add(ListRound(vertex3, 6));
            read.ReadBytes(2);
        }
    }
    return new List<List<List<float>>> {normals, verticies};
}


// Function for checking if the input file exists
string CheckInputFile (string inFile)
{
    string outFile = path + "InputFiles"+ sep + inFile;

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
    string outFile = path + "OutputFiles" + sep + inFile;
    

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
    Console.WriteLine(outFile);
    return CheckOutputFile(outFile, conf);
}

// Get the name of the input file
Console.WriteLine("Please type the name of the .stl file.");
string read_file = CheckInputFile(Console.ReadLine());


// Get the name of the output file
Console.WriteLine("Please type the name of the .cstl file.");
string write_file = CheckOutputFile(Console.ReadLine(), false);
Console.WriteLine(write_file);


// Is the input binary or ascii
Console.WriteLine("Is the .stl file binary or ascii? (b/a)");
string input_type = Console.ReadLine().ToLower();

// do they want the output to be binary or ascii?
Console.WriteLine("Do you want the file to output in binary or ascii? (b/a)");
string output_type = Console.ReadLine().ToLower();

// Temporary arrays for unoptimised points
float[] temp_x_array;
float[] temp_y_array;
float[] temp_z_array;

// Lists for final points
List<float> x_list = new List<float>();
List<float> y_list = new List<float>();
List<float> z_list = new List<float>();

// Point dictionary
Dictionary<Vector3, int> point_dictionary = new Dictionary<Vector3, int>();

// Start of processing code

Stopwatch stopwatch = Stopwatch.StartNew();
// Code that reads the stl file
void Read (string InputFile, string Type)
{
    if (Type == "b")
    {   
        // Reads the binary file
        Console.WriteLine("Reading");
        List<List<List<float>>> input_points = ReadBinaryFile(InputFile);
        // Makes lists of the points and normals
        List<List<float>> normal_vectors = input_points[0];
        List<List<float>> points = input_points[1];
        // Defining the temporary point lists
        Console.WriteLine(points.Count);
        temp_x_array = new float[points.Count];
        temp_y_array = new float[points.Count];
        temp_z_array = new float[points.Count];
        if (points.Count > 0)
        {
            for (int i = 0; i < points.Count; i++)
            {
                temp_x_array[i] = points[i][0];
                temp_y_array[i] = points[i][1];
                temp_z_array[i] = points[i][2];
        }
        }
        else
        {
            return;
        }
    }
    else if (Type == "a")
    {
        // Reads file and breaks it into lines
        Console.WriteLine("Reading File");
        string stl_file = File.ReadAllText(InputFile);
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
        Console.WriteLine("Invalid Input File Type. Please Input Type Again");
        string newType = Console.ReadLine();
        Read(InputFile, newType);
    }
}
Read(read_file, input_type);


// Start of conversion code
Console.WriteLine("Converting");

// Keeps track of the index of the list
int list_index = 0;

// Tesselation array
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


void Write (string outputFile, string type)
{
    if (type == "a")
    {
        decimal[] x = new decimal[x_list.Count];
        decimal[] y = new decimal[y_list.Count];
        decimal[] z = new decimal[z_list.Count];
        for (int i = 0; i < x.Length; i++)
        {
            x[i] = Convert.ToDecimal(x_list[i]);
            y[i] = Convert.ToDecimal(y_list[i]);
            z[i] = Convert.ToDecimal(z_list[i]);
        }
        using (StreamWriter file = new StreamWriter(File.Create(outputFile)))
        {
            // Writes the list to the file 
            file.WriteLine("x_list  [" + String.Join(", ", x) + "]");
            file.WriteLine("y_list  [" + String.Join(", ", y) + "]");
            file.WriteLine("z_list  [" + String.Join(", ", z) + "]");
            file.Write("tess_list  [" + String.Join(", ", tesselation_array) + "]");
        };
    }
    else if (type == "b")
    {
        float[] x = x_list.ToArray();
        float[] y = y_list.ToArray();
        float[] z = z_list.ToArray();
        using (BinaryWriter file = new BinaryWriter(File.Create(outputFile)))
        {
            // Writes the bytes in the x list 
            for (int i = 0; i < x.Length; i++)
            {
                file.Write(x[i]);
            }
            file.Write(0xffffffff); // Seperator
                                    // Writes the bytes in the y list
            for (int i = 0; i < y.Length; i++)
            {
                file.Write(y[i]);
            }
            file.Write(0xffffffff); // Seperator
                                    // Writes the z list
            for (int i = 0; i < z.Length; i++)
            {
                file.Write(z[i]);
            }
            file.Write(0xffffffff); // Seperator
                                    // Writes the tesselation list
            for (int i = 0; i < tesselation_array.Length; i++)
            {
                file.Write(tesselation_array[i]);
            }
        };
    }
    else
    {
        Console.WriteLine("Invalid Output Type. Please Input Type Again.");
        string newType = Console.ReadLine();
        Write(write_file, newType);
    }
}
Write(write_file, output_type);
stopwatch.Stop();
Console.WriteLine("Process Finished With a Time of " + stopwatch.Elapsed + ". Press Enter To Close.");
Console.ReadLine();
return;