import binascii as ba
import struct
import os

print(struct.unpack("f",b"\xFF\xFF\xFF\xFF"))
binfile = input("name of bin file")

asciifile = input("name of ascii file")

bin_file = open(binfile,"rb")
binary = bin_file.read()
bin_file.close()
bin_list = binary.split(b"\xFF\xFF\xFF\xFF")
print(len(bin_list))


fcount = ""
for i in range(int(len(bin_list[0])/4)):
    fcount += "f"
x_list_bin = list(struct.unpack(fcount, bin_list[0]))
y_list_bin = list(struct.unpack(fcount, bin_list[1]))
z_list_bin = list(struct.unpack(fcount, bin_list[2]))
for i in range(len(x_list_bin)):
    x_list_bin[i] = round(x_list_bin[i],5)
    y_list_bin[i] = round(y_list_bin[i],5)
    z_list_bin[i] = round(z_list_bin[i],5)
lengthx = len(x_list_bin)
lengthy = len(y_list_bin)
lengthz = len(z_list_bin)
    
icount = ""
for i in range(int(len(bin_list[3])/4)):
    icount += "i"

tess_list_bin = list(struct.unpack(icount, bin_list[3]))
lengtht = len(tess_list_bin)


ascii_file = open(asciifile)
ascii_str = ascii_file.read()
ascii_file.close()
ascii_list = ascii_str.split("\n")
for i in range(len(ascii_list)):
    ascii_list[i] = ascii_list[i].split("  ")


x_list_a = ascii_list[0][1].replace("[","").replace("]","").split(", ")
y_list_a = ascii_list[1][1].replace("[","").replace("]","").split(", ")
z_list_a = ascii_list[2][1].replace("[","").replace("]","").split(", ")
tess_list_a = ascii_list[3][1].replace("[","").replace("]","").split(", ")
for i in range(len(x_list_a)):
    x_list_a[i] = float(x_list_a[i])
    y_list_a[i] = float(y_list_a[i])
    z_list_a[i] = float(z_list_a[i])

for i in range(len(x_list_bin)):
    x_list_a[i] = round(x_list_a[i],5)
    y_list_a[i] = round(y_list_a[i],5)
    z_list_a[i] = round(z_list_a[i],5)

for i in range(len(tess_list_a)):
    tess_list_a[i] = int(tess_list_a[i])

if x_list_bin == x_list_a and y_list_bin == y_list_a and z_list_bin == z_list_a and tess_list_bin == tess_list_a:
    print("pass")
else:
    print("fail")
    print(len(x_list_bin),len(x_list_a))
    print(len(y_list_bin),len(y_list_a))
    print(len(z_list_bin),len(z_list_a))
    print(len(tess_list_bin),len(tess_list_a))
    for i in range(len(x_list_a)):
        if x_list_bin[i] != x_list_a[i]:
            print("x ",x_list_bin[i],x_list_a[i])
            
        if y_list_bin[i] != y_list_a[i]:
            print("y ",y_list_bin[i],y_list_a[i])

        if z_list_bin[i] != z_list_a[i]:
            print("z ",z_list_bin[i],z_list_a[i])

        if tess_list_bin[i] != tess_list_a[i]:
            print("t ",tess_list_bin[i],tess_list_a[i])
