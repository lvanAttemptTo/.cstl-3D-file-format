# .cstl-3D-file-format
    The .cstl file format is compacted version of the .stl file format and can be used to save space.
    
    The .cstl file format is a 3D file format that can be easily converted from an ASCII .stl format (Binary support will come eventually). It is mostly lossless only losing the faucet normal vectors which will hopefully be added soon. The .cstl file  is roughly 1 fifth to 1 sixth the size of the ASCII .stl file and can be smaller than the binary file. Right now .cstl only supports ASCII but eventually will be available in a binary format as well. The .cstl file format contains three lists of point coordinates (x, y, z), and a list of indexes that tells the program that loads the file which index to reference to get the desired point for the mesh. This allows for the list size to be smaller because it is able to reuse points for multiple mesh tessellations instead of .stl which doesn’t do this instead defining the tessellations not the points.
    The  .cstl file structure contains just 4 parts, the x_list, y_list, z_list, and the tessellation_list in this format for the ASCII version:
x_list [...]
y_list [...]
z_list [...]
tess_list [...]

In the binary version the format is this:
00000000  // This byte tells the program how many bytes the x_list is
00000000 00000000  
