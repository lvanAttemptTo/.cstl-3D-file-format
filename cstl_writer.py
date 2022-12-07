# set to True if the .stl file is binary, False for ASCII
in_binary = False

# set to True if the output should be binary, False for ASCII
out_binary = False

# intermidiate lists for the tessalation
temp_x_list = [] 
temp_y_list = []
temp_z_list = []

# 2D list of points
point_list = []

# tessalation list
tess_list = []

# final output x and y coordinates
x_list = []
y_list = []
z_list = []

# input binary or ASCII settings
in_binary = input("Input 'True' for binary input, input 'False' for ASCII input: ").lower()
out_binary = input("Input 'True' for binary output, input 'False' for ASCII output: ").lower()

# input file name
input_file = input("Input the name of the file: ")

# output file name
output_file = input("Input desired name of the .cstl file: ")

# opens the desired stl file
stl_file = open(input_file, "r")

if in_binary == "true":
    print("Binary .stl files are currently not supported")
else:
    # prep the stl file
    stlmod = stl_file.read()
    stlmod = stlmod.splitlines()
    stl_file.close()
     
    # code for geting the virticies from the ascii
    for i in range(len(stlmod)):
        stlmod[i] = stlmod[i].strip().split(" ")
        if stlmod[i][0] == "vertex":
            temp_x_list.append(float(str("{:f}".format(float(stlmod[i][1]))).rstrip("0")))
            temp_y_list.append(float(str("{:f}".format(float(stlmod[i][2]))).rstrip("0")))
            temp_z_list.append(float(str("{:f}".format(float(stlmod[i][3]))).rstrip("0")))
if out_binary == "true":
    print("Binary .cstl files are currently not supported")
else:
    # adds all unique points to the points list
    for i in range(len(temp_x_list)):
        if [temp_x_list[i], temp_y_list[i], temp_z_list[i]] not in point_list:
            point_list.append([temp_x_list[i], temp_y_list[i], temp_z_list[i]])

    # find the index of all the points and add them to the mesh list
    for i in range(len(temp_x_list)):
        tess_list.append(point_list.index([temp_x_list[i], temp_y_list[i], temp_z_list[i]]))

    # add all the points to the coordinate lists
    for i in range(3):
        for j in range(len(point_list)):
            if i == 0:
                x_list.append(point_list[j][0])
            if i == 1:
                y_list.append(point_list[j][1])
            if i == 2:
                z_list.append(point_list[j][2])

    # opens the output file
    cstl_file = open(output_file, "w")
    print("Finished")
    cstl_file.write("x_list " + str(x_list) + "\ny_list " + str(y_list) + "\nz_list " + str(z_list) + "\ntess_list " + str(tess_list))
    cstl_file.close()
