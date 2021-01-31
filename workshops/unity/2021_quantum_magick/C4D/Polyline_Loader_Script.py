import c4d
#Welcome to the world of Python

spline_types = [ c4d.SPLINETYPE_LINEAR,  c4d.SPLINETYPE_CUBIC,  c4d.SPLINETYPE_BSPLINE,  c4d.SPLINETYPE_BEZIER]

def main():
    # Read the current path set in UserData
    path = op[c4d.ID_USERDATA,1]

    #Read the current Spline Type set in User Data
    spline_type = op[c4d.ID_USERDATA,2]

    #Read the current custom Scale set in User Data
    scale = op[c4d.ID_USERDATA,3]


    #Read the current custom Center boolean set in User Data
    center = op[c4d.ID_USERDATA,4]
    
    # Open file and read lines
    f = open(path, "r")
    lines = f.readlines()
    points = []
    
    avg_x = 0
    avg_y = 0
    avg_z = 0
    
    # Parse lines and construct points
    for line in lines:
       parts = line.split(",")
       x = float(parts[0])*scale
       y = float(parts[1])*scale
       z = float(parts[2])*scale
       
       avg_x += x
       avg_y += y
       avg_z += z
       points.append(c4d.Vector(x,y,z))

    centroid = c4d.Vector(avg_x / len(lines), avg_y/len(lines), avg_z/len(lines))
    
    if center:
        points = [p -centroid for p in points]
    
    # Construct Spline Object and set its points
    polyline = c4d.SplineObject(len(points), spline_types[spline_type])
    polyline.SetAllPoints(points)


    return polyline