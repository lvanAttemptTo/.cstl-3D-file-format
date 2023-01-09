# .cstl-3D-file-format
    The .cstl file format is compacted version of the .stl file format and can be used to save space.
    
    The .cstl file format is a 3D file format that can be easily converted from an ASCII .stl format (Binary support will come eventually). It is mostly lossless only losing the faucet normal vectors which will hopefully be added soon or could be calculated afterward. An ascii .cstl file  is roughly 1 fifth to 1 sixth the size of the ASCII .stl file and can be smaller than the binary file. The binary version can be up to 1 tenth of the size of the ascii .stl file. The .cstl file format contains three lists of point coordinates (x, y, z), and a list of indexes that tells the program that loads the file which index to reference to get the desired point for the mesh. This allows for the list size to be smaller because it is able to reuse points for multiple mesh tessellations instead of .stl which doesn’t do this instead defining the tessellations not the points.
    The  .cstl file structure contains just 4 parts, the x_list, y_list, z_list, and the tess_list. This is an example of an ascii .cstl file:
x_list  [0, 1, 2, ...]
y_list  [0, 1, 2, ...]
z_list  [0, 1 , 2...]
tess_list  [0, 1, 2, ...]

    The binary format works by having 4 lists of 4 byte floats. The lists are seperated by two FF bytes:

/x??/x??/x??/x??... // this is an exaple of a float represented by 4 bytes. This would continue for as many bytes as needed for the x_list.
/xFF/xFF // this tells the program that is reading it that the x_list is done
/x??/x??/x??/x??... // this is the y_list
/xFF/xFF // this tells the program that is reading it that the y_list is done
/x??/x??/x??/x?? // this is the list of float values for the z_list
/xFF/xFF this tells the program that is reading it that the z_list is done
/x??/x??/x??/x?? // this is a list of 4 byte ints for the tess_list.
