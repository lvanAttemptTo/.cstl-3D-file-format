import struct
file = open("cube.stl", "rb")
file.read(80)
faucet_count = struct.unpack("i", file.read(4))[0]
normal_list = []
verticies = []
for i in range(faucet_count):
    normal = list(struct.unpack("fff", file.read(12)))
    vertex1 = list(struct.unpack("fff", file.read(12)))
    vertex2 = list(struct.unpack("fff", file.read(12)))
    vertex3 = list(struct.unpack("fff", file.read(12)))
    ecc = file.read(2)
    if ecc != b"\x00\x00":
        print("File Is Corrupted")
        break
    for i in range(3):
        normal[i] = float("{:f}".format(normal[i]))
        vertex1[i] = float("{:f}".format(normal[i]))
        vertex2[i] = float("{:f}".format(normal[i]))
        vertex3[i] = float("{:f}".format(normal[i]))
    normal_list.append(normal)
    verticies.append(vertex1)
    verticies.append(vertex2)
    verticies.append(vertex3)
print(verticies)
print(normal_list)
file.close()
    
